using Medieval.Definitions.Stats;
using Medieval.ObjectBuilders.Components.Stats;
using Sandbox.Definitions.Components.Entity.Stats.Effects;
using Sandbox.Game.Entities.Entity.Stats;
using Sandbox.Game.Entities.Entity.Stats.Effects;
using Sandbox.Game.Entities.Entity.Stats.Extensions;
using Sandbox.Game.GUI;
using Sandbox.Game.Multiplayer;
using System;
using VRage.Audio;
using VRage.Game;
using VRage.Library.Logging;
using VRage.Utils;
using Sandbox.ModAPI;
using VRage.Game.ModAPI;
using VRageMath;
using RomScripts.StatExtensions;

namespace RomScripts.MoodEffect
{
    [MyEntityEffect(typeof(MyObjectBuilder_MoodEffect))]
    public class MyMoodEffect : MyEntityStatEffect
    {
        private MyEntityStat m_moodStat;


        public override void Activate(MyEntityStatComponent owner)
        {
            base.Activate(owner);

            if (!MyAPIGateway.Multiplayer?.IsServer ?? false)
            {
                return;
            }
            this.m_moodStat = owner.GetMood();
            if (this.m_moodStat == null)
            {
                MyLog.Default.Error("Mood effect '{0}' applied to an entity '{1}' without mood stat!", new object[]
                {
                    base.Definition.Id,
                    base.Owner.Entity.Definition.Id
                });
                return;
            }
            this.m_moodStat.OnValueChanged += new MyEntityStat.ValueChangedDelegate(this.moodStat_OnValueChanged);
        }


        public override void Tick(float timeSinceUpdate)
        {
            if (base.Owner.IsDead())
            {
                return;
            }
            MyEntityStatEffectDefinition myEntityStatEffectDefinition = base.Definition as MyEntityStatEffectDefinition;
            MyEntityStat myEntityStat = null;
            if (!base.Owner.TryGetStat(myEntityStatEffectDefinition.Stat, out myEntityStat))
            {
                return;
            }

            if (this.RegenLimit == null)
            {
                return;
            }
            if (this.Regen == 0f)
            {
                return;
            }
            if (myEntityStat == this.RegenLimit)
            {
                return;
            }
            

            float num = myEntityStat.Current;
            
            if (num > this.RegenLimit)
            {
                num -= Math.Abs(this.Regen) * timeSinceUpdate;
                if (num < this.RegenLimit)
                {
                    num = this.RegenLimit;
                }
            }
            else if (num < this.RegenLimit)
            {
                num += Math.Abs(this.Regen) * timeSinceUpdate;
                if (num > this.RegenLimit)
                {
                    num = this.RegenLimit;
                }
            }
            
            myEntityStat.Current = num;
        }


        public override void Deactivate()
        {
            if (this.m_moodStat != null)
            {
                this.m_moodStat.OnValueChanged -= new MyEntityStat.ValueChangedDelegate(this.moodStat_OnValueChanged);
            }
            base.Deactivate();
        }


        private MyDefinitionId? GetAppropriateMoodEffect(float moodValue)
        {
            int num = (int)moodValue;
            MyMoodEffectDefinition myMoodEffectDefinition = base.Definition as MyMoodEffectDefinition;

            if (num >= 75)
            {
                if (myMoodEffectDefinition.HappyEffect.HasValue)
                {
                    return myMoodEffectDefinition.HappyEffect;
                }
                return null;
            }
            else if (num <= 25)
            {
                if (myMoodEffectDefinition.HappyEffect.HasValue)
                {
                    return myMoodEffectDefinition.UnhappyEffect;
                }
                return null;
            }
            else
            {
                return null;
            }
        }


        private void moodStat_OnValueChanged(MyEntityStat stat, float oldValue, float newValue)
        {
            MyDefinitionId? appropriateMoodEffect = this.GetAppropriateMoodEffect(newValue);
            if (appropriateMoodEffect.HasValue)
            {
                if (!base.Owner.HasEffect(appropriateMoodEffect.Value))
                {
                    base.Owner.AddEffect(appropriateMoodEffect.Value, 0L);
                }
            }
            else
            {
                MyEntityEffectDefinition arg_41_0 = base.Definition;
                MyStringHash orCompute = MyStringHash.GetOrCompute("Mood");
                if (base.Owner.HasEffect(orCompute))
                {
                    base.Owner.RemoveEffect(orCompute);
                }
            }
            //if ((oldValue >= 50f && newValue < 50f) || (oldValue >= 25f && newValue < 25f) || (oldValue >= 20f && newValue < 20f) || (oldValue >= 15f && newValue < 15f) || (oldValue >= 10f && newValue < 10f) || (oldValue >= 5f && newValue < 5f))
            //{
            //    MyCueId soundId = new MyCueId("Hunger");
            //    MyGuiAudio.PlaySound(soundId);
            //}
        }
    }
}
