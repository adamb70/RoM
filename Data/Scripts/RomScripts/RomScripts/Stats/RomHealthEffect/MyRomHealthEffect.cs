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
using VRage.Game.Components;
using VRage.Session;
using VRage.Components.Interfaces;
using RomScripts76561197972467544.StatExtensions;


namespace RomScripts76561197972467544.RomHealthEffect
{
    [MyEntityEffect(typeof(MyObjectBuilder_RomHealthEffect))]
    public class MyRomHealthEffect : MyEntityStatEffect
    {
        private MyEntityStat m_healthStat;

        private IMyDamageReceiver m_damageReceiver;

        private TimeSpan m_damageRegenCooldown;

        private TimeSpan? m_healthRegenBlockedUntil;

        public override void Activate(MyEntityStatComponent owner)
        {
            base.Activate(owner);
            MyRomHealthEffectDefinition myHealthEffectDefinition = base.Definition as MyRomHealthEffectDefinition;
            this.m_damageRegenCooldown = myHealthEffectDefinition.RegenCooldown;
            if (!owner.TryGetStat(myHealthEffectDefinition.Stat, out this.m_healthStat))
            {
                this.m_healthStat = owner.GetHealth();
            }
            if (this.m_healthStat == null)
            {
                //MyLog.Default.Error("Health effect '{0}' applied to an entity '{1}' without source stat!", new object[]
                //{
                //    base.Definition.Id,
                //    base.Owner.Entity.Definition.Id
                //});
                return;
            }
            this.m_damageReceiver = base.Owner.Entity.Components.Get<IMyDamageReceiver>();
            if (this.m_damageReceiver != null)
            {
                this.m_damageReceiver.DamageTaken += new DamageTakenDelegate(this.OnDamageTaken);
            }
        }

        public override void Deactivate()
        {
            if (this.m_damageReceiver != null)
            {
                this.m_damageReceiver.DamageTaken -= new DamageTakenDelegate(this.OnDamageTaken);
                this.m_damageReceiver = null;
            }
            base.Deactivate();
        }

        public override void Tick(float timeSinceUpdate)
        {
            if (this.m_healthStat == null)
            {
                return;
            }
            float c = this.m_healthStat;
            if (this.m_healthRegenBlockedUntil.HasValue && this.m_healthRegenBlockedUntil.Value > MySession.Static.ElapsedGameTime)
            {
                return;
            }
            this.m_healthRegenBlockedUntil = null;
            base.Tick(timeSinceUpdate);
            float num = this.m_healthStat - c;
            if (num > 0f)
            {
                MyEntityStat food = base.Owner.GetFood();
                if (food != null)
                {
                    MyRomHealthEffectDefinition myHealthEffectDefinition = base.Definition as MyRomHealthEffectDefinition;
                    food.Current -= num * myHealthEffectDefinition.HealthToFoodRatio;
                }
            }
        }

        private void OnDamageTaken(MyDamageInformation damageInformation)
        {
            if (base.Owner == null)
            {
                this.m_damageReceiver.DamageTaken -= new DamageTakenDelegate(this.OnDamageTaken);
                this.m_damageReceiver = null;
                return;
            }
            if (damageInformation.DamagedEntity != base.Owner.Entity)
            {
                return;
            }
            this.m_healthRegenBlockedUntil = new TimeSpan?(MySession.Static.ElapsedGameTime + this.m_damageRegenCooldown);

            MyRomHealthEffectDefinition myHealthEffectDefinition = base.Definition as MyRomHealthEffectDefinition;
            if (base.Owner.GetHealth() <= myHealthEffectDefinition.LowHealthThreshold)
            {
                base.Owner.AddEffect(myHealthEffectDefinition.LowHealthEffect, 0L);
            }
        }
    }
}
