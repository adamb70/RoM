using System;
using System.Xml.Serialization;
using VRage.ObjectBuilders;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ObjectBuilders.Inventory;
using VRage.Game;

namespace RomScripts
{
    [MyObjectBuilderDefinition]
    [XmlSerializerAssembly("MedievalEngineers.ObjectBuilders.XmlSerializers")]
    public class MyObjectBuilder_DecayHandlerComponent : MyObjectBuilder_EntityComponent
    {
    }
}
