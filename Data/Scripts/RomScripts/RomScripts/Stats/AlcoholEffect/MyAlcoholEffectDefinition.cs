using Medieval.ObjectBuilders.Definitions.Stats;
using Sandbox.Definitions.Components.Entity.Stats.Effects;
using System;
using VRage.Game;
using VRage.Game.Definitions;

namespace RomScripts.AlcoholEffect
{
    [MyDefinitionType(typeof(MyObjectBuilder_AlcoholEffectDefinition))]
    public class MyAlcoholEffectDefinition : MyEntityStatEffectDefinition
    {
        
        /// <summary>
        /// Effect that is activated when hunger level gets at or under 30.
        /// If blank, will try to maintain WellFedEffect instead.
        /// </summary>
        public MyDefinitionId? DrunkEffect
        {
            get;
            private set;
        }
        

        protected override void Init(MyObjectBuilder_DefinitionBase builder)
        {
            base.Init(builder);
            MyObjectBuilder_AlcoholEffectDefinition myObjectBuilder_AlcoholEffectDefinition = builder as MyObjectBuilder_AlcoholEffectDefinition;
            
            this.DrunkEffect = null;
            if (myObjectBuilder_AlcoholEffectDefinition.DrunkEffect.HasValue)
            {
                this.DrunkEffect = new MyDefinitionId?(myObjectBuilder_AlcoholEffectDefinition.DrunkEffect.Value);
            }

        }
    }
}
