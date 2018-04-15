using Medieval.ObjectBuilders.Definitions.Stats;
using Sandbox.Definitions.Components.Entity.Stats.Effects;
using System;
using VRage.Game;
using VRage.Game.Definitions;

namespace RomScripts.MoodEffect
{
    [MyDefinitionType(typeof(MyObjectBuilder_MoodEffectDefinition))]
    public class MyMoodEffectDefinition : MyEntityStatEffectDefinition
    {
        
        /// <summary>
        /// Effect that is activated when mood level gets at or over 75.
        /// If blank, will try to maintain WellFedEffect instead.
        /// </summary>
        public MyDefinitionId? HappyEffect
        {
            get;
            private set;
        }
        

        protected override void Init(MyObjectBuilder_DefinitionBase builder)
        {
            base.Init(builder);
            MyObjectBuilder_MoodEffectDefinition myObjectBuilder_MoodEffectDefinition = builder as MyObjectBuilder_MoodEffectDefinition;
            
            this.HappyEffect = null;
            if (myObjectBuilder_MoodEffectDefinition.HappyEffect.HasValue)
            {
                this.HappyEffect = new MyDefinitionId?(myObjectBuilder_MoodEffectDefinition.HappyEffect.Value);
            }

        }
    }
}
