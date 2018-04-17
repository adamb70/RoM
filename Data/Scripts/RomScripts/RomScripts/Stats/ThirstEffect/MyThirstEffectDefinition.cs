using Medieval.ObjectBuilders.Definitions.Stats;
using Sandbox.Definitions.Components.Entity.Stats.Effects;
using System;
using VRage.Game;
using VRage.Game.Definitions;

namespace RomScripts.ThirstEffect
{
    [MyDefinitionType(typeof(MyObjectBuilder_ThirstEffectDefinition))]
    public class MyThirstEffectDefinition : MyEntityStatEffectDefinition
    {
        
        /// <summary>
        /// Effect that is activated when hunger level gets at or under 30.
        /// If blank, will try to maintain WellFedEffect instead.
        /// </summary>
        public MyDefinitionId? ThirstyEffect
        {
            get;
            private set;
        }
        

        protected override void Init(MyObjectBuilder_DefinitionBase builder)
        {
            base.Init(builder);
            MyObjectBuilder_ThirstEffectDefinition myObjectBuilder_ThirstEffectDefinition = builder as MyObjectBuilder_ThirstEffectDefinition;
            
            this.ThirstyEffect = null;
            if (myObjectBuilder_ThirstEffectDefinition.ThirstyEffect.HasValue)
            {
                this.ThirstyEffect = new MyDefinitionId?(myObjectBuilder_ThirstEffectDefinition.ThirstyEffect.Value);
            }

        }
    }
}
