using System;
using System.Linq;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Game.Components;
using VRage.Utils;
using VRageMath;
using Sandbox.ModAPI;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders.Definitions.Inventory;
using VRage.Systems;
using Sandbox.Game.EntityComponents;
using Medieval.ObjectBuilders.Components;
using Sandbox.Game.Inventory;
using VRage.Components;
using Medieval.Inventory;


namespace RomScripts76561197972467544.DecayingItem
{
    [MyComponent(typeof(MyObjectBuilder_DecayHandlerComponent))]
    public class MyDecayHandlerComponent : MyEntityComponent
    {
        MyDecayHandlerComponentDefinition ComponentDefinition = null;
        
        IEnumerable<MyInventoryBase> ComponentInventories = null;
        HashSet<MyInventoryBase> InventoriesWithDecayingItems = new HashSet<MyInventoryBase>();
        bool RegisteredForUpdate = false;
        long TickInterval;
        MyInventoryBase OutputInventory;
        bool ticking = false;


        public override void Init(MyEntityComponentDefinition definition)
        {
            ComponentDefinition = definition as MyDecayHandlerComponentDefinition;
            this.TickInterval = ComponentDefinition.TickIntervalMs;
        }

        public override void OnAddedToScene()
        {
            base.OnAddedToScene();
            if (ComponentDefinition == null) return;

            this.OutputInventory = Entity.Components.Get<MyInventoryBase>(ComponentDefinition.OutputInventory);
            
            ComponentInventories = Entity.Components.GetComponents<MyInventoryBase>();
            foreach (MyInventoryBase inv in ComponentInventories)
            {
                inv.ContentsChanged += new System.Action<MyInventoryBase>(this.OnInventoryChanged);

                inv.RaiseContentsChanged(); // Used to trigger countdown when loading save, otherwise will not tick until inventory changed another way.

                //((IMyUtilities)MyAPIUtilities.Static).ShowNotification(inv.ItemCount.ToString(), 3000, null, Color.Red);
            }
        }

        private void OnInventoryChanged(MyInventoryBase inventory)
        {
            if (ticking) return;

            //((IMyUtilities)MyAPIUtilities.Static).ShowNotification("updating!", 900, null, Color.Green);
            bool found = false;
            foreach (var item in inventory.Items)
            {
                // Look for a MyDurableItem in the inventory
                if (item.GetType() == typeof(MyDurableItem))
                {
                    // Add inventory to the list
                    InventoriesWithDecayingItems.Add(inventory);
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                InventoriesWithDecayingItems.Remove(inventory);
            }

            if (InventoriesWithDecayingItems.Count() > 0 && !RegisteredForUpdate)
            {
                RegisteredForUpdate = true;
                MyUpdateComponent.Static.AddForUpdate(Tick, this.TickInterval);
            }
            else if (InventoriesWithDecayingItems.Count() == 0)
            {
                MyUpdateComponent.Static.RemoveFromUpdate(Tick);
                RegisteredForUpdate = false;
            }
        }


        [Update(false)]
        private void Tick(long ms)
        {
            ((IMyUtilities)MyAPIUtilities.Static).ShowNotification("TICKING", 900, null, Color.Green);
            foreach (var inventory in InventoriesWithDecayingItems.ToArray< MyInventoryBase>())
            {
                ticking = true;

                List<MyInventoryItem> durableItems = new List<MyInventoryItem>();
                foreach (MyInventoryItem item in inventory.Items)
                {
                    //((IMyUtilities)MyAPIUtilities.Static).ShowNotification("looping items", 900, null, Color.Green);
                    var decayingItem = item as MyDurableItem;
                    if (decayingItem == null)
                        continue;

                    durableItems.Add(item);
                }


                foreach (var item in durableItems)
                {
                    var decayingItem = item as MyDurableItem;

                    decayingItem.Durability -= 1;
                    //((IMyUtilities)MyAPIUtilities.Static).ShowNotification(decayingItem.Durability.ToString(), 900, null, Color.Aqua);

                    if (decayingItem.Durability <= 0)
                    {
                        if (MyAPIGateway.Multiplayer.IsServer)
                        {
                            if (item.Amount > 1)
                            {
                                // Lower stack count
                                item.Amount -= 1;
                                // Reset Durability of stack
                                decayingItem.Durability = decayingItem.GetDefinition().MaxDurability;
                            }
                            else
                            {
                                inventory.Remove(item);
                            }

                            if (decayingItem.GetDefinition().BrokenItem.HasValue)
                            {
                                if (this.OutputInventory != null)
                                {
                                    this.OutputInventory.AddItems(decayingItem.GetDefinition().BrokenItem.Value, 1);
                                }
                                else
                                {
                                    inventory.AddItems(decayingItem.GetDefinition().BrokenItem.Value, 1);
                                }
                            }
                        }
                        else
                        {
                            decayingItem.Durability = 0;
                        }
                    }

                    continue;

                }


                // Check if inventory still has DecayingItems
                ticking = false;
                OnInventoryChanged(inventory);

            }
        }

        
    }
}
