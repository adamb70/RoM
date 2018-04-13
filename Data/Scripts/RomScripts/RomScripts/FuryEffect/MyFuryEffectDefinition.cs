using Medieval.ObjectBuilders.Definitions.Stats;
using Sandbox.Definitions.Components.Entity.Stats.Effects;
using System;
using VRage.Game;
using VRage.Game.Definitions;

namespace RomScripts.FuryEffect
{
    [MyDefinitionType(typeof(MyObjectBuilder_FuryEffectDefinition))]
    public class MyFuryEffectDefinition : MyEntityStatEffectDefinition
    {
        
        /// <summary>
        /// Effect that is activated when hunger level gets at or under 30.
        /// If blank, will try to maintain WellFedEffect instead.
        /// </summary>
        public MyDefinitionId? RageEffect
        {
            get;
            private set;
        }
        

        protected override void Init(MyObjectBuilder_DefinitionBase builder)
        {
            base.Init(builder);
            MyObjectBuilder_FuryEffectDefinition myObjectBuilder_FuryEffectDefinition = builder as MyObjectBuilder_FuryEffectDefinition;
            
            this.RageEffect = null;
            if (myObjectBuilder_FuryEffectDefinition.RageEffect.HasValue)
            {
                this.RageEffect = new MyDefinitionId?(myObjectBuilder_FuryEffectDefinition.RageEffect.Value);
            }

        }
    }
}
