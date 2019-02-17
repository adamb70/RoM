﻿using System;
using System.Xml.Serialization;
using VRage.ObjectBuilders;
using VRage.ObjectBuilders.Definitions.Equipment;
using ObjectBuilders.Definitions.Tools;
using System.ComponentModel;

namespace RomScripts76561197972467544.VoxelMining
{
    [XmlSerializerAssembly("MedievalEngineers.ObjectBuilders.XmlSerializers")]
    [MyObjectBuilderDefinition(null)]
    public class MyObjectBuilder_RomMinerToolBehaviorDefinition : MyObjectBuilder_MinerToolBehaviorDefinition
    {
        public SerializableDefinitionId MiningLootTable;
    }
}