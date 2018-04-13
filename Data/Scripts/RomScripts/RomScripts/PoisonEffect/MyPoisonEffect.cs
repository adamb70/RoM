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

namespace RomScripts.PoisonEffect
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
            this.m_poisonStat = owner.GetPoison();
            if (this.m_poisonStat == null)
            {
                MyLog.Default.Error("Poison effect '{0}' applied to an entity '{1}' without poison stat!", new object[]
                {
                    base.Definition.Id,
                    base.Owner.Entity.Definition.Id
                });
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

        private MyDefinitionId? GetAppropriatePoisonEffect(float poisonValue)
        {
            int num = (int)poisonValue;
            MyPoisonEffectDefinition myPoisonEffectDefinition = base.Definition as MyPoisonEffectDefinition;

            if (num >= 70)
            {
                if (myPoisonEffectDefinition.PoisonedEffect.HasValue)
                {
                    return myPoisonEffectDefinition.PoisonedEffect;
                }
                return null;
            }
            else
            {
                return null;
            }
        }

        private void poisonStat_OnValueChanged(MyEntityStat stat, float oldValue, float newValue)
        {
            MyDefinitionId? appropriatePoisonEffect = this.GetAppropriatePoisonEffect(newValue);
            if (appropriatePoisonEffect.HasValue)
            {
                if (!base.Owner.HasEffect(appropriatePoisonEffect.Value))
                {
                    base.Owner.AddEffect(appropriatePoisonEffect.Value, 0L);
                }
            }
            else
            {
                MyEntityEffectDefinition arg_41_0 = base.Definition;
                MyStringHash orCompute = MyStringHash.GetOrCompute("Poison");
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
