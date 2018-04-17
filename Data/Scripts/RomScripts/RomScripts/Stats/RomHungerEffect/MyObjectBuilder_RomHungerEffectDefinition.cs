using System;
using System.Xml.Serialization;
using VRage.ObjectBuilders;
using VRage.ObjectBuilders.Components.Entity.Stats.Definitions;

namespace RomScripts.RomHungerEffect
{
    [MyObjectBuilderDefinition]
    [XmlSerializerAssembly("MedievalEngineers.ObjectBuilders.XmlSerializers")]
    public class MyObjectBuilder_RomHungerEffectDefinition : MyObjectBuilder_EntityStatEffectDefinition
    {
        /// <summary>
        /// Effect that is activated when hunger level gets over 30.
        /// Not applied if left blank.
        /// </summary>
        public SerializableDefinitionId? WellFedEffect;

        /// <summary>
        /// Effect that is activated when hunger level gets at or under 30.
        /// If blank, will try to maintain WellFedEffect instead.
        /// </summary>
        public SerializableDefinitionId? HungryEffect;

        /// <summary>
        /// Effect that is activated when hunger level to 0.
        /// If blank, will first try to maintain VeryHungryEffect, if not possible, try to maintain HungryEffect.
        /// </summary>
        public SerializableDefinitionId? StarvationEffect;

        public SerializableDefinitionId? OverstuffedEffect;
    }
}
