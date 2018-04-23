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

        private MyDefinitionId? GetAppropriateHungerEffect(float hungerValue)
        {
            int num = (int)hungerValue;
            MyRomHungerEffectDefinition myHungerEffectDefinition = base.Definition as MyRomHungerEffectDefinition;
            if (num >= 125)
            {
                if (myHungerEffectDefinition.OverstuffedEffect.HasValue)
                {
                    return myHungerEffectDefinition.OverstuffedEffect;
                }
                return null;
            }
            if (num >= 30)
            {
                if (myHungerEffectDefinition.WellFedEffect.HasValue)
                {
                    return myHungerEffectDefinition.WellFedEffect;
                }
                return null;
            }
            if (num >= 1)
            {
                if (myHungerEffectDefinition.HungryEffect.HasValue)
                {
                    return myHungerEffectDefinition.HungryEffect;
                }
                return null;
            }
            else
            {
                if (myHungerEffectDefinition.StarvationEffect.HasValue)
                {
                    return myHungerEffectDefinition.StarvationEffect;
                }
                if (myHungerEffectDefinition.HungryEffect.HasValue)
                {
                    return myHungerEffectDefinition.HungryEffect;
                }
                return null;
            }
        }

        private void hungerStat_OnValueChanged(MyEntityStat stat, float oldValue, float newValue)
        {
            MyDefinitionId? appropriateHungerEffect = this.GetAppropriateHungerEffect(newValue);
            if (appropriateHungerEffect.HasValue)
            {
                if (!base.Owner.HasEffect(appropriateHungerEffect.Value))
                {
                    base.Owner.AddEffect(appropriateHungerEffect.Value, 0L);
                }
            }
            else
            {
                MyEntityEffectDefinition arg_41_0 = base.Definition;
                MyStringHash orCompute = MyStringHash.GetOrCompute("Hunger");
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
