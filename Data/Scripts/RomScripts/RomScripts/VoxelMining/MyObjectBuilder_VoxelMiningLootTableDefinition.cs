using System;
using System.ComponentModel;
using System.Xml.Serialization;
using VRage.Game;
using VRage.ObjectBuilder.Merging;
using VRage.ObjectBuilders.Definitions;
using VRage.ObjectBuilders;
using VRage.ObjectBuilders.Definitions.Inventory;

namespace RomScripts.VoxelMining
{
    [MyObjectBuilderDefinition]
    [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
    public class MyObjectBuilder_VoxelMiningLootTableDefinition : MyObjectBuilder_DefinitionBase
    {
        public class MinedItem
        {
            [XmlIgnore]
            public DefinitionTagId DefinitionId;

            [XmlIgnore]
            public bool IsEmpty = true;
            
            [XmlAttribute]
            public int Amount = 1;

            [XmlAttribute]
            public float Weight = 1f;
            
            [XmlAttribute]
            public bool IsUnique;

            [XmlAttribute]
            public bool AlwaysDrops;

            [XmlAttribute]
            public string Type
            {
                get
                {
                    return this.DefinitionId.Type;
                }
                set
                {
                    this.DefinitionId.Type = value;
                    this.IsEmpty = false;
                }
            }

            [XmlAttribute]
            public string Subtype
            {
                get
                {
                    return this.DefinitionId.Subtype;
                }
                set
                {
                    this.DefinitionId.Subtype = value;
                    this.IsEmpty = false;
                }
            }

            public override bool Equals(object obj)
            {
                return obj != null && !(obj.GetType() != base.GetType()) && this.DefinitionId == ((MyObjectBuilder_LootTableDefinition.Row)obj).DefinitionId;
            }

            protected bool Equals(MyObjectBuilder_LootTableDefinition.Row other)
            {
                return this.DefinitionId.Equals(other.DefinitionId);
            }

            public override int GetHashCode()
            {
                return this.DefinitionId.GetHashCode();
            }
        }

        public class MiningDef
        {
            [XmlAttribute]
            public string VoxelMaterial;

            [XmlAttribute]
            public int Rolls = 1;

            [XmlIgnore]
            public int? Volume;

            [DefaultValue(null), XmlElement("MinedItem")]
            public System.Collections.Generic.List<MyObjectBuilder_VoxelMiningLootTableDefinition.MinedItem> MinedItems = new System.Collections.Generic.List<MyObjectBuilder_VoxelMiningLootTableDefinition.MinedItem>();

            [XmlAttribute("Volume")]
            public int VolumeAttribute
            {
                get
                {
                    int? volume = this.Volume;
                    if (!volume.HasValue)
                    {
                        return 64;
                    }
                    return volume.GetValueOrDefault();
                }
                set
                {
                    this.Volume = new int?(value);
                }
            }
            
            protected bool Equals(MyObjectBuilder_VoxelMiningLootTableDefinition.MiningDef other)
            {
                return string.Equals(this.VoxelMaterial, other.VoxelMaterial);
            }

            public override bool Equals(object obj)
            {
                return !object.ReferenceEquals(null, obj) && (object.ReferenceEquals(this, obj) || (!(obj.GetType() != base.GetType()) && this.Equals((MyObjectBuilder_VoxelMiningLootTableDefinition.MiningDef)obj)));
            }

            public override int GetHashCode()
            {
                if (this.VoxelMaterial == null)
                {
                    return 0;
                }
                return this.VoxelMaterial.GetHashCode();
            }
        }

        [XmlElement("Entry")]
        public System.Collections.Generic.List<MyObjectBuilder_VoxelMiningLootTableDefinition.MiningDef> Entries = new System.Collections.Generic.List<MyObjectBuilder_VoxelMiningLootTableDefinition.MiningDef>();
    }
}
