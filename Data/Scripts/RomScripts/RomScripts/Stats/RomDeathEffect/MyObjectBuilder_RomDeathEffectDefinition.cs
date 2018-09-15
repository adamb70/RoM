using System;
using System.Xml.Serialization;
using VRage.ObjectBuilders;
using VRage.ObjectBuilders.Components.Entity.Stats.Definitions;

namespace RomScripts.RomDeathEffect
{
    [MyObjectBuilderDefinition]
    [XmlSerializerAssembly("MedievalEngineers.ObjectBuilders.XmlSerializers")]
    public class MyObjectBuilder_RomDeathEffectDefinition : MyObjectBuilder_EntityStatEffectDefinition
    {
        public class Trigger
        {
            /// <summary>
            /// At which value does this trigger fire?
            /// </summary>
            [XmlAttribute]
            public int Threshold;

            /// <summary>
            /// Is there a matching sound effect?
            /// </summary>
            [XmlAttribute]
            public string CueId;

            /// <summary>
            /// Is there a matching effect?
            /// </summary>
            public SerializableDefinitionId? Effect;
        }

        [XmlElement("Trigger")]
        public MyObjectBuilder_RomDeathEffectDefinition.Trigger[] Triggers;



    }
}
