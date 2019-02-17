using Medieval.ObjectBuilders.Definitions.Stats;
using Sandbox.Definitions.Components.Entity.Stats.Effects;
using System;
using VRage.Game;
using VRage.Game.Definitions;
using Medieval.Definitions.Tools;
using VRage.Logging;
using System.Collections.Generic;

namespace RomScripts76561197972467544.SeedingToolBehavior
{
    [MyDefinitionType(typeof(MyObjectBuilder_RomSeedingToolBehaviorDefinition))]
    public class MyRomSeedingToolBehaviorDefinition : MySeedingToolBehaviorDefinition
    {
        public class Limit
        {
            public float Lower
            {
                get;
                internal set;
            }

            public float Upper
            {
                get;
                internal set;
            }

            public Limit(float lower, float upper)
            {
                Lower = lower;
                Upper = upper;
            }
        }

        public List<Limit> AltitudeLimits = new List<Limit>();
        public List<Limit> LatitudeLimits = new List<Limit>();
        public List<Limit> LongitudeLimits = new List<Limit>();

        public bool Debug { get; private set; }

        protected override void Init(MyObjectBuilder_DefinitionBase builder)
        {
            base.Init(builder);
            MyObjectBuilder_RomSeedingToolBehaviorDefinition ob = builder as MyObjectBuilder_RomSeedingToolBehaviorDefinition;

            Debug = ob.Debug;

            if (ob.AltitudeLimits != null)
            {
                foreach (MyObjectBuilder_RomSeedingToolBehaviorDefinition.Limit current in ob.AltitudeLimits)
                {
                    Limit lim = new Limit(current.Lower, current.Upper);
                    AltitudeLimits.Add(lim);
                }
            }
            if (ob.LatitudeLimits != null)
            {
                foreach (MyObjectBuilder_RomSeedingToolBehaviorDefinition.Limit current in ob.LatitudeLimits)
                {
                    Limit lim = new Limit(current.Lower, current.Upper);
                    LatitudeLimits.Add(lim);
                }
            }
            if (ob.LongitudeLimits != null)
            {
                foreach (MyObjectBuilder_RomSeedingToolBehaviorDefinition.Limit current in ob.LongitudeLimits)
                {
                    Limit lim = new Limit(current.Lower, current.Upper);
                    LongitudeLimits.Add(lim);
                }
            }

        }
    }
}
