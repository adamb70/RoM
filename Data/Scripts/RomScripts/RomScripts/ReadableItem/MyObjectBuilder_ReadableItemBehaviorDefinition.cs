using System.Xml.Serialization;

using VRage.ObjectBuilders;

using VRage.ObjectBuilders.Definitions.Equipment;

using ObjectBuilders.Definitions.Tools;
using Sandbox.Game.EntityComponents;

namespace RomScripts.ReadableItem

{
    [MyObjectBuilderDefinition]
    [XmlSerializerAssembly("MedievalEngineers.ObjectBuilders.XmlSerializers")]

    public class MyObjectBuilder_ReadableItemBehaviorDefinition : MyObjectBuilder_ToolBehaviorDefinition
    {
        [XmlArrayItem("Page")]
        public System.Collections.Generic.List<string> Pages;

        public string Title;

    }

}

