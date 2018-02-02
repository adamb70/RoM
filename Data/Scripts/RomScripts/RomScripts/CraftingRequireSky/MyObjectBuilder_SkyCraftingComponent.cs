using System.Xml.Serialization;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ObjectBuilders;

namespace RomScripts.SkyCraftingComponent
{
    [MyObjectBuilderDefinition]
    [XmlSerializerAssembly("MedievalEngineers.ObjectBuilders.XmlSerializers")]
    public class MyObjectBuilder_SkyCraftingComponent : MyObjectBuilder_EntityComponent
    {
    }
}
