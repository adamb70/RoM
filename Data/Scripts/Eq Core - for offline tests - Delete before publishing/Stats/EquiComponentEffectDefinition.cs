﻿using System.Xml.Serialization;
using Medieval.ObjectBuilders.Components;
using Sandbox.Definitions.Components.Entity.Stats.Effects;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Logging;
using VRage.ObjectBuilders;
using VRage.ObjectBuilders.Components.Entity.Stats.Definitions;

namespace Equinox76561198048419394.Core.Stats
{
    [MyDefinitionType(typeof(MyObjectBuilder_EquiComponentEffectDefinition), null)]
    public class EquiComponentEffectDefinition : MyEntityEffectDefinition
    {
        public MyDefinitionId AddedComponent { get; private set; }

        protected override void Init(MyObjectBuilder_DefinitionBase builder)
        {
            base.Init(builder);
            var b = (MyObjectBuilder_EquiComponentEffectDefinition) builder;

            if (!b.AddedComponent.HasValue)
                MyDefinitionErrors.Add(Package, $"{Id} has AppliedEffect == null", LogSeverity.Error);
//            else if (!typeof(MyObjectBuilder_EntityComponent).IsAssignableFrom(b.AddedComponent.Value.TypeId))
//                MyDefinitionErrors.Add(Package, $"{Id} has AppliedEffect that isn't an entity component", LogSeverity.Error);
        }
    }

    [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
    [MyObjectBuilderDefinition]
    public class MyObjectBuilder_EquiComponentEffectDefinition : MyObjectBuilder_EntityEffectDefinition
    {
        public SerializableDefinitionId? AddedComponent;
    }
}