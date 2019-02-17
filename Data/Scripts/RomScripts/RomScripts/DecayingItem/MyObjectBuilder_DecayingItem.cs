using System;
using System.Xml.Serialization;
using VRage.ObjectBuilders;
using VRage.ObjectBuilders.Inventory;

namespace RomScripts76561197972467544.DecayingItem
{
    [MyObjectBuilderDefinition]
    [XmlSerializerAssembly("MedievalEngineers.ObjectBuilders.XmlSerializers")]
    public class MyObjectBuilder_DecayingItem : MyObjectBuilder_DurableItem
    {
    }
}
