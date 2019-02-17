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
using VRage.Logging;
using VRage.Utils;
using Sandbox.ModAPI;
using VRage.Game.ModAPI;
using VRageMath;
using RomScripts76561197972467544.StatExtensions;

namespace RomScripts76561197972467544.PoisonEffect
{
    [MyEntityEffect(typeof(MyObjectBuilder_PoisonEffect))]
    public class MyPoisonEffect : MyEntityStatEffect
    {
        private MyEntityStat m_poisonStat;

        public override void Activate(MyEntityStatComponent owner)
        {
            base.Activate(owner);
            if (!MyAPIGateway.Multiplayer?.IsServer ?? false)
            {
                return;
            }
            MyPoisonEffectDefinition myPoisonEffectDefinition = base.Definition as MyPoisonEffectDefinition;
            if (!owner.TryGetStat(myPoisonEffectDefinition.Stat, out this.m_poisonStat))
            {
                this.m_poisonStat = owner.GetPoison();
            }
            
            if (this.m_poisonStat == null)
            {
                //MyLog.Default.Error("Poison effect '{0}' applied to an entity '{1}' without poison stat!", new object[]
                //{
                //    base.Definition.Id,
                //    base.Owner.Entity.Definition.Id
                //});
                return;
            }
            this.m_poisonStat.OnValueChanged += new MyEntityStat.ValueChangedDelegate(this.poisonStat_OnValueChanged);
        }

        public override void Deactivate()
        {
            if (this.m_poisonStat != null)
            {
                this.m_poisonStat.OnValueChanged -= new MyEntityStat.ValueChangedDelegate(this.poisonStat_OnValueChanged);
            }
            base.Deactivate();
        }
        
        private void poisonStat_OnValueChanged(MyEntityStat stat, float oldValue, float newValue)
        {
            MyPoisonEffectDefinition myPoisonEffectDefinition = base.Definition as MyPoisonEffectDefinition;
            int num = 0;
            MyDefinitionId? myDefinitionId = null;
            foreach (MyPoisonEffectDefinition.Trigger current in myPoisonEffectDefinition.Triggers)
            {
                if (current.Threshold >= num && (float)current.Threshold < newValue)
                {
                    if (current.Effect.HasValue)
                    {
                        myDefinitionId = current.Effect;
                    }
                    num = current.Threshold;
                }
                //if (oldValue >= (float)current.Threshold && newValue < (float)current.Threshold && !current.CueId.IsNull)
                //{
                //    MyGuiAudio.PlaySound(current.CueId);
                //}
            }
            if (myDefinitionId.HasValue && !base.Owner.HasEffect(myDefinitionId.Value))
            {
                base.Owner.AddEffect(myDefinitionId.Value, 0L);
            }
        }
    }
}
