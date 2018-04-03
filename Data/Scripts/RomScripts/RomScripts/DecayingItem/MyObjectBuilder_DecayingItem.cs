using System;
using System.Xml.Serialization;
using VRage.ObjectBuilders;
using VRage.ObjectBuilders.Inventory;

namespace RomScripts.DecayingItem
{
    [MyObjectBuilderDefinition]
    [XmlSerializerAssembly("MedievalEngineers.ObjectBuilders.XmlSerializers")]
    public class MyObjectBuilder_DecayingItem : MyObjectBuilder_DurableItem
    {
    }
}
