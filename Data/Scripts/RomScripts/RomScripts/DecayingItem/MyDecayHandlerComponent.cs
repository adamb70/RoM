using System;
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

namespace RomScripts
{
    [MyComponent(typeof(MyObjectBuilder_DecayHandlerComponent))]
    public class MyDecayHandlerComponent : MyEntityComponent
    {
        MyDecayHandlerComponentDefinition ComponentDefinition = null;
        
        IEnumerable<MyInventoryBase> ComponentInventories = null;
        HashSet<MyInventoryBase> InventoriesWithDecayingItems = new HashSet<MyInventoryBase>();
        bool RegisteredForUpdate = false;
        long TickInterval = 10000; 


        public override void Init(MyEntityComponentDefinition definition)
        {
            ComponentDefinition = definition as MyDecayHandlerComponentDefinition;
        }

        public override void OnAddedToScene()
        {
            base.OnAddedToScene();
            if (ComponentDefinition == null) return;

            ComponentInventories = Entity.Components.GetComponents<MyInventoryBase>();
            foreach (MyInventoryBase inv in ComponentInventories)
            {
                inv.ContentsChanged += new System.Action<MyInventoryBase>(this.OnInventoryChanged);

                ((IMyUtilities)MyAPIUtilities.Static).ShowNotification(inv.ItemCount.ToString(), 3000, null, Color.Red);
            }
        }

        private void OnInventoryChanged(MyInventoryBase inventory)
        {
            ((IMyUtilities)MyAPIUtilities.Static).ShowNotification("updating!", 900, null, Color.Green);
            bool found = false;
            foreach (var item in inventory.Items)
            {
                // Look for a MyDurableItem in the inventory
                if (item is MyDurableItem)
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

            if (InventoriesWithDecayingItems.Count > 0 && !RegisteredForUpdate)
            {
                RegisteredForUpdate = true;
                MyUpdateComponent.Static.AddForUpdate(Tick, this.TickInterval);
            }
            else if (InventoriesWithDecayingItems.Count == 0)
            {
                MyUpdateComponent.Static.RemoveFromUpdate(Tick);
                RegisteredForUpdate = false;
            }
        }


        [Update(false)]
        private void Tick(long ms)
        {
            ((IMyUtilities)MyAPIUtilities.Static).ShowNotification("TICKING", 900, null, Color.Green);
            foreach (var inventory in InventoriesWithDecayingItems)
            {

                foreach (MyInventoryItem item in inventory.Items)
                {
                    ((IMyUtilities)MyAPIUtilities.Static).ShowNotification("looping items", 900, null, Color.Green);
                    var decayingItem = item as MyDecayingItem;
                    if (decayingItem == null)
                        continue;

                    ((IMyUtilities)MyAPIUtilities.Static).ShowNotification(decayingItem.GetDefinition().DurabilityLossPerSecond.ToString(), 900, null, Color.Orange);
                    decayingItem.Durability -= decayingItem.GetDefinition().DurabilityLossPerSecond;

                    ((IMyUtilities)MyAPIUtilities.Static).ShowNotification(decayingItem.Durability.ToString(), 900, null, Color.Aqua);
                    if (decayingItem.Durability <= 0)
                    {
                        if (MyAPIGateway.Multiplayer.IsServer)
                        {
                            inventory.Remove(item);

                            if (decayingItem.GetDefinition().BrokenItem.HasValue)
                            {
                                inventory.AddItems(decayingItem.GetDefinition().BrokenItem.Value, 1);
                            }

                            // Check if inventory still has DecayingItems
                            OnInventoryChanged(inventory);
                            return;
                        }
                        decayingItem.Durability = 0;
                    }
                }

            }
        }

        
    }
}
