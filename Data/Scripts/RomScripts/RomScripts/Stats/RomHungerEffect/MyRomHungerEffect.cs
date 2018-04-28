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

namespace RomScripts.RomHungerEffect
{
    [MyEntityEffect(typeof(MyObjectBuilder_RomHungerEffect))]
    public class MyRomHungerEffect : MyEntityStatEffect
    {
        private MyEntityStat m_hungerStat;

        public override void Activate(MyEntityStatComponent owner)
        {
            base.Activate(owner);
            if (!MyAPIGateway.Multiplayer?.IsServer ?? false)
            {
                return;
            }
            MyRomHungerEffectDefinition myRomHungerEffectDefinition = base.Definition as MyRomHungerEffectDefinition;
            if (!owner.TryGetStat(myRomHungerEffectDefinition.Stat, out this.m_hungerStat))
            {
                this.m_hungerStat = owner.GetFood();
            }
            if (this.m_hungerStat == null)
            {
                MyLog.Default.Error("Hunger effect '{0}' applied to an entity '{1}' without source stat!", new object[]
                {
                    base.Definition.Id,
                    base.Owner.Entity.Definition.Id
                });
                return;
            }
            this.m_hungerStat.OnValueChanged += new MyEntityStat.ValueChangedDelegate(this.hungerStat_OnValueChanged);
        }

        public override void Deactivate()
        {
            if (this.m_hungerStat != null)
            {
                this.m_hungerStat.OnValueChanged -= new MyEntityStat.ValueChangedDelegate(this.hungerStat_OnValueChanged);
            }
            base.Deactivate();
        }
        
        private void hungerStat_OnValueChanged(MyEntityStat stat, float oldValue, float newValue)
        {
            MyRomHungerEffectDefinition myRomHungerEffectDefinition = base.Definition as MyRomHungerEffectDefinition;
            int num = 0;
            MyDefinitionId? myDefinitionId = null;
            foreach (MyRomHungerEffectDefinition.Trigger current in myRomHungerEffectDefinition.Triggers)
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
