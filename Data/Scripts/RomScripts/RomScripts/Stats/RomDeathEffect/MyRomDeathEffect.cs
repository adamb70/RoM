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

namespace RomScripts.RomDeathEffect
{
    [MyEntityEffect(typeof(MyObjectBuilder_RomDeathEffect))]
    public class MyRomDeathEffect : MyEntityStatEffect
    {
        private MyEntityStat m_DeathStat;

        public override void Activate(MyEntityStatComponent owner)
        {
            base.Activate(owner);
            if (!MyAPIGateway.Multiplayer?.IsServer ?? false)
            {
                return;
            }
            MyRomDeathEffectDefinition myRomDeathEffectDefinition = base.Definition as MyRomDeathEffectDefinition;
            if (!owner.TryGetStat(myRomDeathEffectDefinition.Stat, out this.m_DeathStat))
            {
                this.m_DeathStat = owner.GetFood();
            }
            if (this.m_DeathStat == null)
            {
                MyLog.Default.Error("Death effect '{0}' applied to an entity '{1}' without source stat!", new object[]
                {
                    base.Definition.Id,
                    base.Owner.Entity.Definition.Id
                });
                return;
            }
            this.m_DeathStat.OnValueChanged += new MyEntityStat.ValueChangedDelegate(this.DeathStat_OnValueChanged);
        }

        public override void Deactivate()
        {
            if (this.m_DeathStat != null)
            {
                this.m_DeathStat.OnValueChanged -= new MyEntityStat.ValueChangedDelegate(this.DeathStat_OnValueChanged);
            }
            base.Deactivate();
        }
        
        private void DeathStat_OnValueChanged(MyEntityStat stat, float oldValue, float newValue)
        {
            MyRomDeathEffectDefinition myRomDeathEffectDefinition = base.Definition as MyRomDeathEffectDefinition;
            int num = 0;
            MyDefinitionId? myDefinitionId = null;
            foreach (MyRomDeathEffectDefinition.Trigger current in myRomDeathEffectDefinition.Triggers)
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
