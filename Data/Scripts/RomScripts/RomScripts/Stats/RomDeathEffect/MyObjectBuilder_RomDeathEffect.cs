using System;
using System.Xml.Serialization;
using VRage.ObjectBuilders;
using VRage.ObjectBuilders.Components.Entity.Stats;

namespace RomScripts.RomDeathEffect
{
    [MyObjectBuilderDefinition]
    [XmlSerializerAssembly("MedievalEngineers.ObjectBuilders.XmlSerializers")]
    public class MyObjectBuilder_RomDeathEffect : MyObjectBuilder_EntityStatEffect
    {
    }
}
