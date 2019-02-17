using Medieval.ObjectBuilders.Definitions.Stats;
using Sandbox.Definitions.Components.Entity.Stats.Effects;
using System;
using VRage.Game;
using VRage.Game.Definitions;

namespace RomScripts76561197972467544.RomHealthEffect
{
    [MyDefinitionType(typeof(MyObjectBuilder_RomHealthEffectDefinition))]
    public class MyRomHealthEffectDefinition : MyEntityStatEffectDefinition
    {

        public MyDefinitionId LowHealthEffect
        {
            get;
            private set;
        }

        /// <summary>
        /// The stamina threshold that has to be crossed for exhaustion to trigger.
        /// </summary>
        public float LowHealthThreshold
        {
            get;
            private set;
        }

        /// <summary>
		/// The ratio of food consumed for every 1 health regenerated.
		/// </summary>
		public float HealthToFoodRatio
        {
            get;
            private set;
        }

        /// <summary>
        /// Cooldown of no regen after taking damage.
        /// </summary>
        public TimeSpan RegenCooldown
        {
            get;
            private set;
        }

        protected override void Init(MyObjectBuilder_DefinitionBase builder)
        {
            base.Init(builder);
            MyObjectBuilder_RomHealthEffectDefinition myObjectBuilder_HealthEffectDefinition = builder as MyObjectBuilder_RomHealthEffectDefinition;
            this.LowHealthEffect = myObjectBuilder_HealthEffectDefinition.LowHealthEffect;
            this.LowHealthThreshold = (myObjectBuilder_HealthEffectDefinition.LowHealthThreshold ?? 0f);
            this.HealthToFoodRatio = 0f;
            if (myObjectBuilder_HealthEffectDefinition.HealthToFoodRatio.HasValue)
            {
                this.HealthToFoodRatio = myObjectBuilder_HealthEffectDefinition.HealthToFoodRatio.Value;
            }
            this.RegenCooldown = new TimeSpan(0, 0, 0);
            if (myObjectBuilder_HealthEffectDefinition.RegenCooldown.HasValue)
            {
                this.RegenCooldown = myObjectBuilder_HealthEffectDefinition.RegenCooldown.Value;
            }
        }
    }
}
