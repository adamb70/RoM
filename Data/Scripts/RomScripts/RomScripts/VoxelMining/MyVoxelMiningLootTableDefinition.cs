using System;
using System.Collections.Generic;
using VRage.Factory;
using VRage.Game;
using VRage.Components;
using VRage.Game.Definitions;
using VRage.ObjectBuilder;
using VRage.ObjectBuilders.Definitions;
using VRage.Utils;
using VRage.Game.ModAPI;
using VRageMath;
using Sandbox.ModAPI;
using Sandbox.Definitions;
using VRage.Definitions;
using VRage.Logging;

namespace RomScripts76561197972467544.VoxelMining
{
    [MyDependency(typeof(MyVoxelMaterialDefinition), Recursive = true), MyDefinitionType(typeof(MyObjectBuilder_VoxelMiningLootTableDefinition), null)]
    public class MyVoxelMiningLootTableDefinition : MyDefinitionBase
    {

        public class MinedItem
        {
            public MyDefinitionId? ItemDefinition
            {
                get;
                internal set;
            }

            public float Weight
            {
                get;
                internal set;
            }
            
            public bool IsUnique
            {
                get;
                internal set;
            }
            
            public bool AlwaysDrops
            {
                get;
                internal set;
            }
            
            public int Amount
            {
                get;
                internal set;
            }
        }


        public struct MiningEntry
        {
            public System.Collections.Generic.List<MyVoxelMiningLootTableDefinition.MinedItem> MinedItems;

            public int Rolls;

            public int Volume;
        }

        public System.Collections.Generic.Dictionary<int, MyVoxelMiningLootTableDefinition.MiningEntry> MiningEntries = new System.Collections.Generic.Dictionary<int, MyVoxelMiningLootTableDefinition.MiningEntry>();
        public Dictionary<int, MyVoxelMiningDefinition.MiningEntry> VanillaMiningEntries = new Dictionary<int, MyVoxelMiningDefinition.MiningEntry>();

        protected override void Init(MyObjectBuilder_DefinitionBase builder)
        {
            base.Init(builder);
            MyObjectBuilder_VoxelMiningLootTableDefinition ob = (MyObjectBuilder_VoxelMiningLootTableDefinition)builder;
            if (ob.Entries == null)
            {
                return;
            }
            foreach (MyObjectBuilder_VoxelMiningLootTableDefinition.MiningDef current in ob.Entries)
            {
                System.Collections.Generic.List<MyVoxelMiningLootTableDefinition.MinedItem> minedItemList = new System.Collections.Generic.List<MyVoxelMiningLootTableDefinition.MinedItem>();
                Dictionary<MyDefinitionId, int> dictionary = new Dictionary<MyDefinitionId, int>();
                System.Collections.Generic.List<MyObjectBuilder_VoxelMiningLootTableDefinition.MinedItem> minedItems = current.MinedItems;
                if (minedItems != null)
                {
                    foreach (MyObjectBuilder_VoxelMiningLootTableDefinition.MinedItem current2 in minedItems)
                    {
                        MyVoxelMiningLootTableDefinition.MinedItem minedItemData = new MyVoxelMiningLootTableDefinition.MinedItem();

                        if (!current2.IsEmpty)
                        {
                            minedItemData.ItemDefinition = new MyDefinitionId?(current2.DefinitionId);
                        }
                        
                        minedItemData.AlwaysDrops = current2.AlwaysDrops;
                        minedItemData.Amount = current2.Amount;
                        minedItemData.IsUnique = current2.IsUnique;
                        minedItemData.Weight = current2.Weight;

                        minedItemList.Add(minedItemData);

                        /////////////// Build a vanilla dictionary too
                        if (current2.AlwaysDrops) {
                            MyObjectBuilderType type;
                            try
                            {
                                type = MyObjectBuilderType.Parse(current2.Type);
                            }
                            catch (Exception)
                            {
                                //MyLog.Default.Error("Can not parse defined builder type {0}", new object[]
                                //{
                                //    current2.Type
                                //});
                                continue;
                            }
                            MyDefinitionId key = new MyDefinitionId(type, MyStringHash.GetOrCompute(current2.Subtype));
                            dictionary[key] = current2.Amount;
                        }

                    }
                }
                int volume;
                if (!current.Volume.HasValue)
                {
                    volume = 64;
                }
                else
                {
                    volume = System.Math.Max(current.Volume.Value, 1);
                }
                MyVoxelMaterialDefinition myVoxelMaterialDefinition = MyDefinitionManager.Get<MyVoxelMaterialDefinition>(current.VoxelMaterial, false);
                if (myVoxelMaterialDefinition == null || myVoxelMaterialDefinition.Index == 255)
                {
                    //MyLog.Default.Error("Cannot find voxel material {0}", new object[]
                    //{
                    //    current.VoxelMaterial
                    //});
                }
                else
                {
                    this.MiningEntries[(int)myVoxelMaterialDefinition.Index] = new MyVoxelMiningLootTableDefinition.MiningEntry
                    {
                        MinedItems = minedItemList,
                        Volume = volume,
                        Rolls = current.Rolls
                        
                    };

                    // Create vanilla mining entries to be handled by the custom behaviour
                    this.VanillaMiningEntries[(int)myVoxelMaterialDefinition.Index] = new MyVoxelMiningDefinition.MiningEntry
                    {
                        MinedItems = dictionary,
                        Volume = volume
                    };
                }
            }
        }

        public static explicit operator MyVoxelMiningDefinition(MyVoxelMiningLootTableDefinition def)
        {
            var mining_def = new MyVoxelMiningDefinition();
            mining_def.MiningEntries = def.VanillaMiningEntries;
            return mining_def;
        }
    }

}
