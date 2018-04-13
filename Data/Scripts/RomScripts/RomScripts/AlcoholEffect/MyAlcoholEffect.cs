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

namespace RomScripts.AlcoholEffect
{
    [MyEntityEffect(typeof(MyObjectBuilder_AlcoholEffect))]
    public class MyAlcoholEffect : MyEntityStatEffect
    {
        private MyEntityStat m_alcoholStat;

        public override void Activate(MyEntityStatComponent owner)
        {
            base.Activate(owner);

            if (!MyAPIGateway.Multiplayer?.IsServer ?? false)
            {
                return;
            }
            this.m_alcoholStat = owner.GetAlcohol();
            if (this.m_alcoholStat == null)
            {
                MyLog.Default.Error("Alcohol effect '{0}' applied to an entity '{1}' without alcohol stat!", new object[]
                {
                    base.Definition.Id,
                    base.Owner.Entity.Definition.Id
                });
                return;
            }
            this.m_alcoholStat.OnValueChanged += new MyEntityStat.ValueChangedDelegate(this.alcoholStat_OnValueChanged);
        }

        public override void Deactivate()
        {
            if (this.m_alcoholStat != null)
            {
                this.m_alcoholStat.OnValueChanged -= new MyEntityStat.ValueChangedDelegate(this.alcoholStat_OnValueChanged);
            }
            base.Deactivate();
        }

        private MyDefinitionId? GetAppropriateAlcoholEffect(float alcoholValue)
        {
            int num = (int)alcoholValue;
            MyAlcoholEffectDefinition myAlcoholEffectDefinition = base.Definition as MyAlcoholEffectDefinition;

            if (num >= 70)
            {
                if (myAlcoholEffectDefinition.DrunkEffect.HasValue)
                {
                    return myAlcoholEffectDefinition.DrunkEffect;
                }
                return null;
            }
            else
            {
                return null;
            }
        }

        private void alcoholStat_OnValueChanged(MyEntityStat stat, float oldValue, float newValue)
        {
            MyDefinitionId? appropriateAlcoholEffect = this.GetAppropriateAlcoholEffect(newValue);
            if (appropriateAlcoholEffect.HasValue)
            {
                if (!base.Owner.HasEffect(appropriateAlcoholEffect.Value))
                {
                    base.Owner.AddEffect(appropriateAlcoholEffect.Value, 0L);
                }
            }
            else
            {
                MyEntityEffectDefinition arg_41_0 = base.Definition;
                MyStringHash orCompute = MyStringHash.GetOrCompute("Alcohol");
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
