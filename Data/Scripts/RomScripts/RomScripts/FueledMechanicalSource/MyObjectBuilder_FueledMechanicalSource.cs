using System.Xml.Serialization;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ObjectBuilders;
using Medieval.ObjectBuilders.Components.MechanicalPower;

namespace RomScripts.FueledMechanicalSource
{
    [MyObjectBuilderDefinition]
    [XmlSerializerAssembly("MedievalEngineers.ObjectBuilders.XmlSerializers")]
    public class MyObjectBuilder_FueledMechanicalSourceComponent : MyObjectBuilder_MechanicalSourceComponent
    {
    }
}
