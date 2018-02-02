using ProtoBuf;
using Sandbox.Game.Entities;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game.Entity;
using VRage.Utils;

namespace RomScripts
{
    internal class RomStatLogic
    {
        [ProtoContract]
        public struct MyStatAction
        {
            [ProtoMember(57)]
            public MyStringHash StatId;

            [ProtoMember(60)]
            public float Cost;

            [ProtoMember(63)]
            public float AmountToActivate;

            [ProtoMember(66)]
            public bool CanPerformWithout;
        }

        [ProtoContract]
        public struct MyStatRegenModifier
        {
            [ProtoMember(73)]
            public MyStringHash StatId;

            [ProtoMember(76)]
            public float AmountMultiplier;

            [ProtoMember(79)]
            public float Duration;
        }

        [ProtoContract]
        public struct MyStatEfficiencyModifier
        {
            [ProtoMember(86)]
            public MyStringHash StatId;

            [ProtoMember(89)]
            public float Threshold;

            [ProtoMember(92)]
            public float EfficiencyMultiplier;
        }

        public const int STAT_VALUE_TOO_LOW = 4;

        private string m_scriptName;

        private MyEntity m_character;

        protected System.Collections.Generic.Dictionary<MyStringHash, MyEntityStat> m_stats;

        private System.Collections.Generic.Dictionary<string, RomStatLogic.MyStatAction> m_statActions = new System.Collections.Generic.Dictionary<string, RomStatLogic.MyStatAction>();

        private System.Collections.Generic.Dictionary<string, RomStatLogic.MyStatRegenModifier> m_statRegenModifiers = new System.Collections.Generic.Dictionary<string, RomStatLogic.MyStatRegenModifier>();

        private System.Collections.Generic.Dictionary<string, RomStatLogic.MyStatEfficiencyModifier> m_statEfficiencyModifiers = new System.Collections.Generic.Dictionary<string, RomStatLogic.MyStatEfficiencyModifier>();

        public string Name
        {
            get
            {
                return this.m_scriptName;
            }
        }

        public MyEntity Entity
        {
            get
            {
                return this.m_character;
            }
            set
            {
                MyEntity character = this.m_character;
                this.m_character = value;
                this.OnCharacterChanged(character);
            }
        }

        public System.Collections.Generic.Dictionary<string, RomStatLogic.MyStatAction> StatActions
        {
            get
            {
                return this.m_statActions;
            }
        }

        public System.Collections.Generic.Dictionary<string, RomStatLogic.MyStatRegenModifier> StatRegenModifiers
        {
            get
            {
                return this.m_statRegenModifiers;
            }
        }

        public System.Collections.Generic.Dictionary<string, RomStatLogic.MyStatEfficiencyModifier> StatEfficiencyModifiers
        {
            get
            {
                return this.m_statEfficiencyModifiers;
            }
        }

        public virtual void Init(MyEntity character, System.Collections.Generic.Dictionary<MyStringHash, MyEntityStat> stats, string scriptName)
        {
            this.m_scriptName = scriptName;
            this.Entity = character;
            this.m_stats = stats;
        }

        public virtual void Update()
        {
        }

        public virtual void Update10()
        {
        }

        public virtual void Close()
        {
        }

        protected virtual void OnCharacterChanged(MyEntity oldCharacter)
        {
        }

        public void AddAction(string actionId, RomStatLogic.MyStatAction action)
        {
            this.m_statActions.Add(actionId, action);
        }

        public void AddModifier(string modifierId, RomStatLogic.MyStatRegenModifier modifier)
        {
            this.m_statRegenModifiers.Add(modifierId, modifier);
        }

        public void AddEfficiency(string modifierId, RomStatLogic.MyStatEfficiencyModifier modifier)
        {
            this.m_statEfficiencyModifiers.Add(modifierId, modifier);
        }

        public bool CanDoAction(string actionId, bool continuous, out MyTuple<ushort, MyStringHash> message)
        {
            RomStatLogic.MyStatAction myStatAction;
            if (!this.m_statActions.TryGetValue(actionId, out myStatAction))
            {
                message = new MyTuple<ushort, MyStringHash>(0, myStatAction.StatId);
                return true;
            }
            if (myStatAction.CanPerformWithout)
            {
                message = new MyTuple<ushort, MyStringHash>(0, myStatAction.StatId);
                return true;
            }
            MyEntityStat myEntityStat;
            if (!this.m_stats.TryGetValue(myStatAction.StatId, out myEntityStat))
            {
                message = new MyTuple<ushort, MyStringHash>(0, myStatAction.StatId);
                return true;
            }
            if (continuous)
            {
                if (myEntityStat.Value < myStatAction.Cost)
                {
                    message = new MyTuple<ushort, MyStringHash>(4, myStatAction.StatId);
                    return false;
                }
            }
            else if (myEntityStat.Value < myStatAction.Cost || myEntityStat.Value < myStatAction.AmountToActivate)
            {
                message = new MyTuple<ushort, MyStringHash>(4, myStatAction.StatId);
                return false;
            }
            message = new MyTuple<ushort, MyStringHash>(0, myStatAction.StatId);
            return true;
        }

        public bool DoAction(string actionId)
        {
            RomStatLogic.MyStatAction myStatAction;
            if (!this.m_statActions.TryGetValue(actionId, out myStatAction))
            {
                return false;
            }
            MyEntityStat myEntityStat;
            if (!this.m_stats.TryGetValue(myStatAction.StatId, out myEntityStat))
            {
                return false;
            }
            if (myStatAction.CanPerformWithout)
            {
                myEntityStat.Value -= System.Math.Min(myEntityStat.Value, myStatAction.Cost);
                return true;
            }
            if (((myStatAction.Cost >= 0f && myEntityStat.Value >= myStatAction.Cost) || myStatAction.Cost < 0f) && myEntityStat.Value >= myStatAction.AmountToActivate)
            {
                myEntityStat.Value -= myStatAction.Cost;
            }
            return true;
        }

        public void ApplyModifier(string modifierId)
        {
            RomStatLogic.MyStatRegenModifier myStatRegenModifier;
            if (!this.m_statRegenModifiers.TryGetValue(modifierId, out myStatRegenModifier))
            {
                return;
            }
            MyEntityStat myEntityStat;
            if (!this.m_stats.TryGetValue(myStatRegenModifier.StatId, out myEntityStat))
            {
                return;
            }
            myEntityStat.ApplyRegenAmountMultiplier(myStatRegenModifier.AmountMultiplier, myStatRegenModifier.Duration);
        }

        public float GetEfficiencyModifier(string modifierId)
        {
            RomStatLogic.MyStatEfficiencyModifier myStatEfficiencyModifier;
            if (!this.m_statEfficiencyModifiers.TryGetValue(modifierId, out myStatEfficiencyModifier))
            {
                return 1f;
            }
            MyEntityStat myEntityStat;
            if (!this.m_stats.TryGetValue(myStatEfficiencyModifier.StatId, out myEntityStat))
            {
                return 1f;
            }
            return myEntityStat.GetEfficiencyMultiplier(myStatEfficiencyModifier.EfficiencyMultiplier, myStatEfficiencyModifier.Threshold);
        }
    }
}
