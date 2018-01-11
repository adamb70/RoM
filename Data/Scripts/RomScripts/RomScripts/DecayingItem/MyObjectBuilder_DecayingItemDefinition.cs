using System;
using System.ComponentModel;
using System.Xml.Serialization;
using VRage.ObjectBuilders;
using VRage.ObjectBuilders.Definitions.Inventory;

namespace RomScripts
{
    [MyObjectBuilderDefinition]
    [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
    public class MyObjectBuilder_DecayingItemDefinition : MyObjectBuilder_DurableItemDefinition
    {
        [XmlElement("DurabilityLossPerSecond")]
        public int? DurabilityLossPerSecond;
    }
}
