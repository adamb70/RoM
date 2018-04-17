using System;
using System.Xml.Serialization;
using VRage.ObjectBuilders;
using VRage.ObjectBuilders.Components.Entity.Stats.Definitions;

namespace RomScripts.MoodEffect
{
    [MyObjectBuilderDefinition]
    [XmlSerializerAssembly("MedievalEngineers.ObjectBuilders.XmlSerializers")]
    public class MyObjectBuilder_MoodEffectDefinition : MyObjectBuilder_EntityStatEffectDefinition
    {
        /// <summary>
        /// Effect that is activated when hunger level gets at or under 30.
        /// If blank, will try to maintain WellFedEffect instead.
        /// </summary>
        public SerializableDefinitionId? HappyEffect;

        public SerializableDefinitionId? UnhappyEffect;
    }
}
