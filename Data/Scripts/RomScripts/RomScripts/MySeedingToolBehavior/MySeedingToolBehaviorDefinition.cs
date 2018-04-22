using Medieval.ObjectBuilders.Definitions.Stats;
using Sandbox.Definitions.Components.Entity.Stats.Effects;
using System;
using VRage.Game;
using VRage.Game.Definitions;
using Medieval.Definitions.Tools;

namespace Romscripts.SeedingToolBehavior
{
    [MyDefinitionType(typeof(MyObjectBuilder_RomSeedingToolBehaviorDefinition))]
    public class MyRomSeedingToolBehaviorDefinition : MySeedingToolBehaviorDefinition
    {

        public float? MinAltitudePercentage;
        public float? MaxAltitudePercentage;
        public float? MaxNorthPercentage;
        public float? MinNorthPercentage;


        protected override void Init(MyObjectBuilder_DefinitionBase builder)
        {
            base.Init(builder);
            MyObjectBuilder_RomSeedingToolBehaviorDefinition ob = builder as MyObjectBuilder_RomSeedingToolBehaviorDefinition;
            
            this.MinAltitudePercentage = ob.MinAltitudePercentage;
            this.MaxAltitudePercentage = ob.MaxAltitudePercentage;
            this.MaxNorthPercentage = ob.MaxNorthPercentage;
            this.MinNorthPercentage = ob.MinNorthPercentage;

        }
    }
}
