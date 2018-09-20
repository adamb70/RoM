using Medieval.Constants;
using Medieval.Definitions.Tools;
using ObjectBuilders.Definitions.Tools;
using Sandbox.Definitions;
using Sandbox.Definitions.Equipment;
using Sandbox.Definitions.Inventory;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Voxels;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Inventory;
using Sandbox.Game.EntityComponents.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.Inventory;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Players;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Components;
using VRage.Components.Entity.Camera;
using VRage.Definitions;
using VRage.Definitions.Engine;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.Voxels;
using VRage.Systems;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;
using Medieval.GameSystems.Tools;
using Medieval.GameSystems;
using VRage.Session;
using Sandbox.ModAPI;
using VRage.Game.ModAPI;
using VRage.Collections;
using VRage.ObjectBuilders.Definitions.Inventory;
using VRage.Library.Utils;
using VRage.Definitions.Inventory;

namespace RomScripts.VoxelMining
{
    [MyHandItemBehavior(typeof(MyObjectBuilder_RomMinerToolBehaviorDefinition), true)]
    public class MyRomMinerToolBehavior : MyMinerToolBehavior
    {
        private static MyStorageData StorageData;

        protected MyVoxelMiningLootTableDefinition m_oreMiningLootTableDefinition;
        

        public override void Init(MyEntity holder, MyHandItem item, MyHandItemBehaviorDefinition definition)
        {
            base.Init(holder, item, definition);

            MyRomMinerToolBehaviorDefinition MyRomMinerToolBehaviorDefinition = (MyRomMinerToolBehaviorDefinition)definition;
            this.m_oreMiningLootTableDefinition = MyDefinitionManager.Get<MyVoxelMiningLootTableDefinition>(MyRomMinerToolBehaviorDefinition.MiningLootTable);
            base.m_oreMiningDefinition = (MyVoxelMiningDefinition)MyDefinitionManager.Get<MyVoxelMiningLootTableDefinition>(MyRomMinerToolBehaviorDefinition.MiningLootTable);
        }


        private MyVoxelMaterialDefinition GetVoxelMaterial(MyVoxelBase voxelBase, Vector3D hitPos)
        {
            MatrixD worldMatrixInvScaled = voxelBase.PositionComp.WorldMatrixInvScaled;
            Vector3D xyz;
            Vector3D.TransformNoProjection(ref hitPos, ref worldMatrixInvScaled, out xyz);
            Vector3I vector3I = voxelBase.StorageMin + new Vector3I(xyz) + (voxelBase.Size >> 1);
            MyRomMinerToolBehavior.StorageData.Resize(Vector3I.One);
            voxelBase.Storage.ReadRange(MyRomMinerToolBehavior.StorageData, MyStorageDataTypeFlags.Material, 0, vector3I, vector3I);
            byte index = MyRomMinerToolBehavior.StorageData.Material(0);
            return MyVoxelMaterialDefinition.Get((int)index);
        }


        protected override void Hit()
        {
            base.Hit();

            if (this.Target.Entity == null || !(this.Target.Entity is MyVoxelBase))
            {
                return;
            }
            
            MyVoxelBase myVoxelBase = (MyVoxelBase)this.Target.Entity;
            myVoxelBase = myVoxelBase.RootVoxel;
            if (!myVoxelBase.MarkedForClose)
            {
                MyVoxelMaterialDefinition voxelMaterial = this.GetVoxelMaterial(myVoxelBase, this.Target.Position);
                MyVoxelMiningLootTableDefinition.MiningEntry miningEntry;

                if (voxelMaterial != null && this.m_oreMiningLootTableDefinition.MiningEntries.TryGetValue((int)voxelMaterial.Index, out miningEntry))
                {
                    if (this.Holder == MySession.Static.PlayerEntity)
                    {
                        this.GenerateLoot(voxelMaterial.Index);
                    }
                }
            }
        }


        private void GenerateLoot(int voxelMaterial)
        {
            //((IMyUtilities)MyAPIUtilities.Static).ShowNotification(this.m_oreMiningLootTableDefinition.MiningEntries.Count.ToString(), 1000, null, Color.Black);
            MyVoxelMiningLootTableDefinition.MiningEntry miningEntry;
            if (this.m_oreMiningLootTableDefinition.MiningEntries.TryGetValue(voxelMaterial, out miningEntry))
            {
                CachingHashSet<MyVoxelMiningLootTableDefinition.MinedItem> cachingHashSet = new CachingHashSet<MyVoxelMiningLootTableDefinition.MinedItem>();
                foreach (MyVoxelMiningLootTableDefinition.MinedItem current in miningEntry.MinedItems)
                {
                    cachingHashSet.Add(current);
                }
                cachingHashSet.ApplyChanges();
                float num = 0f;
                
                foreach (MyVoxelMiningLootTableDefinition.MinedItem current in cachingHashSet)
                {
                    if (current.AlwaysDrops && current.ItemDefinition.HasValue)
                    {
                        // The "AlwaysDrop" items are being handled by the base method in the vanilla way, don't add them to the inventory again.
                        //this.AddItemsFuzzy(this.m_holdersInventory, current.ItemDefinition.Value, current.Amount);

                        if (current.IsUnique)
                        {
                            cachingHashSet.Remove(current, false);
                            continue;
                        }
                    }
                    num += current.Weight;
                }

                for (int j = 0; j < miningEntry.Rolls; j++)
                {
                    float num2 = MyRandom.Instance.NextFloat(0f, num);
                    cachingHashSet.ApplyChanges();
                    foreach (MyVoxelMiningLootTableDefinition.MinedItem current2 in cachingHashSet)
                    {
                        num2 -= current2.Weight;
                        if (num2 <= 0f && current2.ItemDefinition.HasValue)
                        {
                            this.AddItemsFuzzy(this.m_holdersInventory, current2.ItemDefinition.Value, current2.Amount);
                            
                            if (current2.IsUnique)
                            {
                                cachingHashSet.Remove(current2, false);
                                num -= current2.Weight;
                                break;
                            }
                            break;
                        }
                    }
                }
            }
        }


        private void AddItemsFuzzy(MyInventoryBase inventory, MyDefinitionId itemDefinition, int amount)
        {
            if (itemDefinition.TypeId == typeof(MyObjectBuilder_ItemTagDefinition))
            {
                inventory.AddItemsWithTag(itemDefinition.SubtypeId, amount, true);
                return;
            }
            inventory.AddOrSpawnItem(itemDefinition, amount);
        }
        

        static MyRomMinerToolBehavior()
        {
            MyRomMinerToolBehavior.StorageData = new MyStorageData();
        }
    }
}
