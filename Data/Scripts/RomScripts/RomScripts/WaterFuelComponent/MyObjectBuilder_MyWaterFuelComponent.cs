using System.Xml.Serialization;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ObjectBuilders;
using VRage.Serialization;

namespace RomScripts.WaterFuelComponent
{
    [MyObjectBuilderDefinition]
    [XmlSerializerAssembly("MedievalEngineers.ObjectBuilders.XmlSerializers")]
    public class MyObjectBuilder_WaterFuelComponent : MyObjectBuilder_EntityComponent
    {
        public bool Enabled;

        public SerializableDefinitionId? CurrentFuelId;

        public long MaxFuelTime;

        public long RemainingFuelTime;
    }
}
