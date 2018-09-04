using System;
using System.ComponentModel;
using System.Xml.Serialization;
using VRage.ObjectBuilders;
using VRage.Game;
using VRage.ObjectBuilders.Definitions.Inventory;

namespace RomScripts.DecayingItem
{
    [MyObjectBuilderDefinition]
    [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
    public class MyObjectBuilder_DecayHandlerComponentDefinition : MyObjectBuilder_EntityComponentDefinition
    {
        [XmlElement("TickInterval")]
        public long? TickInterval;

        [XmlElement("OutputInventory")]
        public string OutputInventory;
    }
}
