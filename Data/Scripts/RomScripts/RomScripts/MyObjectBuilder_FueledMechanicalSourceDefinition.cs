using VRage.Game;
using VRage.ObjectBuilders;
using System.Xml.Serialization;
using Medieval.ObjectBuilders.Definitions.MechanicalPower;

namespace RomScripts
{
    [MyObjectBuilderDefinition]
    [XmlSerializerAssembly("MedievalEngineers.ObjectBuilders.XmlSerializers")]
    public class MyObjectBuilder_FueledMechanicalSourceComponentDefinition : MyObjectBuilder_MechanicalSourceComponentDefinition
    {
    }
}
