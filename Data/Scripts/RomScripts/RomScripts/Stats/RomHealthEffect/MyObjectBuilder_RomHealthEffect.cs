using System;
using System.Xml.Serialization;
using VRage.ObjectBuilders;
using VRage.ObjectBuilders.Components.Entity.Stats;

namespace RomScripts76561197972467544.RomHealthEffect
{
    [MyObjectBuilderDefinition]
    [XmlSerializerAssembly("MedievalEngineers.ObjectBuilders.XmlSerializers")]
    public class MyObjectBuilder_RomHealthEffect : MyObjectBuilder_EntityStatEffect
    {
    }
}
