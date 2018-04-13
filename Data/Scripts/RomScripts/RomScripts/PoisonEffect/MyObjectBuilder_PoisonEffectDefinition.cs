using System;
using System.Xml.Serialization;
using VRage.ObjectBuilders;
using VRage.ObjectBuilders.Components.Entity.Stats.Definitions;

namespace RomScripts.PoisonEffect
{
    [MyObjectBuilderDefinition]
    [XmlSerializerAssembly("MedievalEngineers.ObjectBuilders.XmlSerializers")]
    public class MyObjectBuilder_PoisonEffectDefinition : MyObjectBuilder_EntityStatEffectDefinition
    {
        /// <summary>
        /// Effect that is activated when poison level gets at or over 50.
        /// If blank, will try to maintain WellFedEffect instead.
        /// </summary>
        public SerializableDefinitionId? PoisonedEffect;
    }
}
