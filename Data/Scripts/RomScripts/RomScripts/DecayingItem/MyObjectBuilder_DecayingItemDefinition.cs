﻿using System;
using System.ComponentModel;
using System.Xml.Serialization;
using VRage.ObjectBuilders;
using VRage.ObjectBuilders.Definitions.Inventory;

namespace RomScripts76561197972467544.DecayingItem
{
    [MyObjectBuilderDefinition]
    [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
    public class MyObjectBuilder_DecayingItemDefinition : MyObjectBuilder_DurableItemDefinition
    {
        [XmlElement("DurabilityLossPerSecond")]
        public int? DurabilityLossPerSecond;
    }
}
