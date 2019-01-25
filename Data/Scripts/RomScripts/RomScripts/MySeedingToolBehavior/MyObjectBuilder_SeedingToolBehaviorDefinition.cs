using System;
using System.Xml.Serialization;
using VRage.ObjectBuilders;
using VRage.ObjectBuilders.Components.Entity.Stats.Definitions;
using ObjectBuilders.Definitions.Tools;
using VRage.ObjectBuilder.Merging;
using System.Collections.Generic;
using System.ComponentModel;

namespace Romscripts.SeedingToolBehavior
{
    [MyObjectBuilderDefinition]
    [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
    public class MyObjectBuilder_RomSeedingToolBehaviorDefinition : MyObjectBuilder_SeedingToolBehaviorDefinition
    {
        public class Limit
        {
            [XmlAttribute]
            public float Lower;

            [XmlAttribute]
            public float Upper;
        }

        public List<Limit> AltitudeLimits;
        
        public List<Limit> LatitudeLimits;
        
        public List<Limit> LongitudeLimits;

        [DefaultValue(false)]
        public bool Debug;
    }
}
