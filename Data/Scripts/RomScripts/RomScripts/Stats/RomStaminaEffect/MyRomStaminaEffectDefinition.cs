using Medieval.ObjectBuilders.Definitions.Stats;
using Sandbox.Definitions.Components.Entity.Stats.Effects;
using System;
using VRage.Game;
using VRage.Game.Definitions;

namespace RomScripts.RomStaminaEffect
{
    [MyDefinitionType(typeof(MyObjectBuilder_RomStaminaEffectDefinition))]
    public class MyRomStaminaEffectDefinition : MyEntityStatEffectDefinition
    {

        public MyDefinitionId ExhaustionEffect
        {
            get;
            private set;
        }

        public MyDefinitionId RestingEffect
        {
            get;
            private set;
        }

        public System.TimeSpan RestingTime
        {
            get;
            private set;
        }

        /// <summary>
        /// The ratio of food consumed for every 1 health regenerated.
        /// </summary>
        public float StaminaToFoodRatio
        {
            get;
            private set;
        }

        /// <summary>
        /// The ratio of thirst consumed for every 1 health regenerated.
        /// </summary>
        public float StaminaToThirstRatio
        {
            get;
            private set;
        }
        

        protected override void Init(MyObjectBuilder_DefinitionBase builder)
        {
            base.Init(builder);
            MyObjectBuilder_RomStaminaEffectDefinition myObjectBuilder_StaminaEffectDefinition = builder as MyObjectBuilder_RomStaminaEffectDefinition;
            this.ExhaustionEffect = myObjectBuilder_StaminaEffectDefinition.ExhaustionEffect;
            this.RestingEffect = myObjectBuilder_StaminaEffectDefinition.RestingEffect;
            this.RestingTime = new System.TimeSpan(0, 0, 10);
            if (myObjectBuilder_StaminaEffectDefinition.RestingTime.HasValue)
            {
                this.RestingTime = myObjectBuilder_StaminaEffectDefinition.RestingTime.Value;
            }
            this.StaminaToFoodRatio = 0f;
            if (myObjectBuilder_StaminaEffectDefinition.StaminaToFoodRatio.HasValue)
            {
                this.StaminaToFoodRatio = myObjectBuilder_StaminaEffectDefinition.StaminaToFoodRatio.Value;
            }
            this.StaminaToThirstRatio = 0f;
            if (myObjectBuilder_StaminaEffectDefinition.StaminaToThirstRatio.HasValue)
            {
                this.StaminaToThirstRatio = myObjectBuilder_StaminaEffectDefinition.StaminaToThirstRatio.Value;
            }
        }
    }
}
