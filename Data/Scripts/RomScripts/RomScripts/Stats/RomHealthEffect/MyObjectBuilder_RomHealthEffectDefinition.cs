using System;
using System.Xml.Serialization;
using VRage.ObjectBuilders;
using VRage.ObjectBuilders.Components.Entity.Stats.Definitions;
using VRage.ObjectBuilders.Definitions;

namespace RomScripts76561197972467544.RomHealthEffect
{
    [MyObjectBuilderDefinition]
    [XmlSerializerAssembly("MedievalEngineers.ObjectBuilders.XmlSerializers")]
    public class MyObjectBuilder_RomHealthEffectDefinition : MyObjectBuilder_EntityStatEffectDefinition
    {
        public SerializableDefinitionId LowHealthEffect;

        /// <summary>
        /// The health threshold that has to be crossed for low health to trigger.
        /// </summary>
        public float? LowHealthThreshold;

        /// <summary>
		/// The ratio of food consumed for every 1 health regenerated.
		/// </summary>
		public float? HealthToFoodRatio;

        /// <summary>
        /// Cooldown of no regen after taking damage.
        /// </summary>
        public TimeDefinition? RegenCooldown;
    }
}
