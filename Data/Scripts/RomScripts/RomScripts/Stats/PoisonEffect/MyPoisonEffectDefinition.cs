using Medieval.ObjectBuilders.Definitions.Stats;
using Sandbox.Definitions.Components.Entity.Stats.Effects;
using System;
using VRage.Game;
using VRage.Game.Definitions;

namespace RomScripts.PoisonEffect
{
    [MyDefinitionType(typeof(MyObjectBuilder_PoisonEffectDefinition))]
    public class MyPoisonEffectDefinition : MyEntityStatEffectDefinition
    {
        
        /// <summary>
        /// Effect that is activated when poison level gets at or above 50.
        /// If blank, will try to maintain WellFedEffect instead.
        /// </summary>
        public MyDefinitionId? PoisonedEffect
        {
            get;
            private set;
        }
        

        protected override void Init(MyObjectBuilder_DefinitionBase builder)
        {
            base.Init(builder);
            MyObjectBuilder_PoisonEffectDefinition myObjectBuilder_PoisonEffectDefinition = builder as MyObjectBuilder_PoisonEffectDefinition;
            
            this.PoisonedEffect = null;
            if (myObjectBuilder_PoisonEffectDefinition.PoisonedEffect.HasValue)
            {
                this.PoisonedEffect = new MyDefinitionId?(myObjectBuilder_PoisonEffectDefinition.PoisonedEffect.Value);
            }

        }
    }
}
