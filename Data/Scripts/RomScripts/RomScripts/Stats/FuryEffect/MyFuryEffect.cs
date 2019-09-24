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

namespace RomScripts76561197972467544.FuryEffect
{
    [MyEntityEffect(typeof(MyObjectBuilder_FuryEffect))]
    public class MyFuryEffect : MyEntityStatEffect
    {
        private MyEntityStat m_furyStat;

        public override void Activate(MyEntityStatComponent owner)
        {
            base.Activate(owner);

            if (!MyAPIGateway.Multiplayer?.IsServer ?? false)
            {
                return;
            }
            this.m_furyStat = owner.GetFury();
            if (this.m_furyStat == null)
            {
                //MyLog.Default.Error("Fury effect '{0}' applied to an entity '{1}' without fury stat!", new object[]
                //{
                //    base.Definition.Id,
                //    base.Owner.Entity.Definition.Id
                //});
                return;
            }
            this.m_furyStat.OnValueChanged += new MyEntityStat.ValueChangedDelegate(this.furyStat_OnValueChanged);
        }

        public override void Deactivate()
        {
            if (this.m_furyStat != null)
            {
                this.m_furyStat.OnValueChanged -= new MyEntityStat.ValueChangedDelegate(this.furyStat_OnValueChanged);
            }
            base.Deactivate();
        }

        private MyDefinitionId? GetAppropriateFuryEffect(float furyValue)
        {
            int num = (int)furyValue;
            MyFuryEffectDefinition myFuryEffectDefinition = base.Definition as MyFuryEffectDefinition;

            if (num >= 50)
            {
                if (myFuryEffectDefinition.RageEffect.HasValue)
                {
                    return myFuryEffectDefinition.RageEffect;
                }
                return null;
            }
            else
            {
                return null;
            }
        }

        private void furyStat_OnValueChanged(MyEntityStat stat, float oldValue, float newValue)
        {
            MyDefinitionId? appropriateFuryEffect = this.GetAppropriateFuryEffect(newValue);
            if (appropriateFuryEffect.HasValue)
            {
                if (!base.Owner.HasEffect(appropriateFuryEffect.Value))
                {
                    base.Owner.AddEffect(appropriateFuryEffect.Value, 0L);
                }
            }
            else
            {
                MyEntityEffectDefinition arg_41_0 = base.Definition;
                MyStringHash orCompute = MyStringHash.GetOrCompute("Fury");
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
