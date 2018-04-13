using System;
using System.Xml.Serialization;
using VRage.ObjectBuilders;
using VRage.ObjectBuilders.Components.Entity.Stats.Definitions;

namespace RomScripts.AlcoholEffect
{
    [MyObjectBuilderDefinition]
    [XmlSerializerAssembly("MedievalEngineers.ObjectBuilders.XmlSerializers")]
    public class MyObjectBuilder_AlcoholEffectDefinition : MyObjectBuilder_EntityStatEffectDefinition
    {
        /// <summary>
        /// Effect that is activated when alcohol level gets at or over 70.
        /// If blank, will try to maintain WellFedEffect instead.
        /// </summary>
        public SerializableDefinitionId? DrunkEffect;
    }
}
