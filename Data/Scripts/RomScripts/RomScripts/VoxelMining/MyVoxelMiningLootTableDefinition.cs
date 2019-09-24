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

        public System.Collections.Generic.Dictionary<int, MyVoxelMiningLootTableDefinition.MiningEntry> MiningLootEntries = new System.Collections.Generic.Dictionary<int, MyVoxelMiningLootTableDefinition.MiningEntry>();
        public Dictionary<int, MyVoxelMiningDefinition.MiningEntry> MiningEntries = new Dictionary<int, MyVoxelMiningDefinition.MiningEntry>();

        protected override void Init(MyObjectBuilder_DefinitionBase builder)
        {
            base.Init(builder);
            MyObjectBuilder_VoxelMiningLootTableDefinition ob = (MyObjectBuilder_VoxelMiningLootTableDefinition)builder;
            if (ob.Entries == null)
            {
                return;
            }
            foreach (MyObjectBuilder_VoxelMiningLootTableDefinition.MiningDef miningDef in ob.Entries)
            {
                System.Collections.Generic.List<MyVoxelMiningLootTableDefinition.MinedItem> minedItemList = new System.Collections.Generic.List<MyVoxelMiningLootTableDefinition.MinedItem>();
                Dictionary<MyDefinitionId, int> dictionary = new Dictionary<MyDefinitionId, int>();
                System.Collections.Generic.List<MyObjectBuilder_VoxelMiningLootTableDefinition.MinedItem> minedItems = miningDef.MinedItems;
                if (minedItems != null)
                {
                    foreach (MyObjectBuilder_VoxelMiningLootTableDefinition.MinedItem minedLootItem in minedItems)
                    {
                        MyVoxelMiningLootTableDefinition.MinedItem minedLootItemData = new MyVoxelMiningLootTableDefinition.MinedItem();

                        if (!minedLootItem.IsEmpty)
                        {
                            minedLootItemData.ItemDefinition = new MyDefinitionId?(minedLootItem.DefinitionId);
                        }
                        
                        minedLootItemData.AlwaysDrops = minedLootItem.AlwaysDrops;
                        minedLootItemData.Amount = minedLootItem.Amount;
                        minedLootItemData.IsUnique = minedLootItem.IsUnique;
                        minedLootItemData.Weight = minedLootItem.Weight;

                        minedItemList.Add(minedLootItemData);

                        /////////////// Build a vanilla dictionary too
                        if (minedLootItem.AlwaysDrops) {
                            MyObjectBuilderType type;
                            try
                            {
                                type = MyObjectBuilderType.Parse(minedLootItem.Type);
                            }
                            catch (Exception)
                            {
                                //MyLog.Default.Error("Can not parse defined builder type {0}", new object[]
                                //{
                                //    minedLootItem.Type
                                //});
                                continue;
                            }
                            MyDefinitionId key = new MyDefinitionId(type, MyStringHash.GetOrCompute(minedLootItem.Subtype));
                            dictionary[key] = minedLootItem.Amount;
                        }

                    }
                }
                int volume;
                if (miningDef.Volume == null)
                {
                    volume = 64;
                }
                else
                {
                    volume = System.Math.Max(miningDef.Volume.Value, 1);
                }
                MyVoxelMaterialDefinition myVoxelMaterialDefinition = MyDefinitionManager.Get<MyVoxelMaterialDefinition>(miningDef.VoxelMaterial, false);
                if (myVoxelMaterialDefinition == null || myVoxelMaterialDefinition.Index == 255)
                {
                    //MyLog.Default.Error("Cannot find voxel material {0}", new object[]
                    //{
                    //    miningDef.VoxelMaterial
                    //});
                }
                else
                {
                    this.MiningLootEntries[(int)myVoxelMaterialDefinition.Index] = new MyVoxelMiningLootTableDefinition.MiningEntry
                    {
                        MinedItems = minedItemList,
                        Volume = volume,
                        Rolls = miningDef.Rolls
                        
                    };

                    // Create vanilla mining entries to be handled by the custom behaviour
                    this.MiningEntries[(int)myVoxelMaterialDefinition.Index] = new MyVoxelMiningDefinition.MiningEntry
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
            mining_def.MiningEntries = def.MiningEntries;
            return mining_def;
        }
    }

}
