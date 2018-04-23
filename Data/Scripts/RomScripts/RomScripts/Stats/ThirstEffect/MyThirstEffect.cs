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

namespace RomScripts.ThirstEffect
{
    [MyEntityEffect(typeof(MyObjectBuilder_ThirstEffect))]
    public class MyThirstEffect : MyEntityStatEffect
    {
        private MyEntityStat m_thirstStat;

        public override void Activate(MyEntityStatComponent owner)
        {
            base.Activate(owner);

            if (!MyAPIGateway.Multiplayer?.IsServer ?? false)
            {
                return;
            }
            this.m_thirstStat = owner.GetThirst();
            if (this.m_thirstStat == null)
            {
                MyLog.Default.Error("Thirst effect '{0}' applied to an entity '{1}' without thirst stat!", new object[]
                {
                    base.Definition.Id,
                    base.Owner.Entity.Definition.Id
                });
                return;
            }
            this.m_thirstStat.OnValueChanged += new MyEntityStat.ValueChangedDelegate(this.thirstStat_OnValueChanged);
        }

        public override void Deactivate()
        {
            if (this.m_thirstStat != null)
            {
                this.m_thirstStat.OnValueChanged -= new MyEntityStat.ValueChangedDelegate(this.thirstStat_OnValueChanged);
            }
            base.Deactivate();
        }

        private MyDefinitionId? GetAppropriateThirstEffect(float thirstValue)
        {
            int num = (int)thirstValue;
            MyThirstEffectDefinition myThirstEffectDefinition = base.Definition as MyThirstEffectDefinition;

            if (num >= 30)
            {
                // sufficiently hydrated
                return null;
            }
            if (num >= 1)
            {
                if (myThirstEffectDefinition.ThirstyEffect.HasValue)
                {
                    return myThirstEffectDefinition.ThirstyEffect;
                }
                return null;
            }
            else
            {
                if (myThirstEffectDefinition.DehydrationEffect.HasValue)
                {
                    return myThirstEffectDefinition.DehydrationEffect;
                }
                if (myThirstEffectDefinition.ThirstyEffect.HasValue)
                {
                    return myThirstEffectDefinition.ThirstyEffect;
                }
                return null;
            }
        }

        private void thirstStat_OnValueChanged(MyEntityStat stat, float oldValue, float newValue)
        {
            MyDefinitionId? appropriateThirstEffect = this.GetAppropriateThirstEffect(newValue);
            if (appropriateThirstEffect.HasValue)
            {
                if (!base.Owner.HasEffect(appropriateThirstEffect.Value))
                {
                    base.Owner.AddEffect(appropriateThirstEffect.Value, 0L);
                }
            }
            else
            {
                MyEntityEffectDefinition arg_41_0 = base.Definition;
                MyStringHash orCompute = MyStringHash.GetOrCompute("Thirst");
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
