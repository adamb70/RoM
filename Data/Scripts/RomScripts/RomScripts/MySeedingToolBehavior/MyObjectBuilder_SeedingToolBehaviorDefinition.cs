using System;
using System.Xml.Serialization;
using VRage.ObjectBuilders;
using VRage.ObjectBuilders.Components.Entity.Stats.Definitions;
using ObjectBuilders.Definitions.Tools;

namespace Romscripts.SeedingToolBehavior
{
    [MyObjectBuilderDefinition]
    [XmlSerializerAssembly("MedievalEngineers.ObjectBuilders.XmlSerializers")]
    public class MyObjectBuilder_RomSeedingToolBehaviorDefinition : MyObjectBuilder_SeedingToolBehaviorDefinition
    {
        [XmlElement("MinAltitudePercentage")]
        public float? MinAltitudePercentage;

        [XmlElement("MaxAltitudePercentage")]
        public float? MaxAltitudePercentage;

        [XmlElement("MaxNorthPercentage")]
        public float? MaxNorthPercentage;

        [XmlElement("MinNorthPercentage")]
        public float? MinNorthPercentage;
    }
}
