using Sandbox.Definitions.Inventory;
using System;
using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI;
using VRage.Game.ModAPI;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Library.Collections;
using VRage.ObjectBuilders.Inventory;
using VRageMath;
using Sandbox.Game.Inventory;

namespace RomScripts.DecayingItem
{
    [MyInventoryItemType(typeof(MyObjectBuilder_DecayingItem))]
    public class MyDecayingItem : MyDurableItem
    {
        public new MyDecayingItemDefinition GetDefinition()
        {
            return (MyDecayingItemDefinition)base.GetDefinition();
        }

        public override void Init(MyDefinitionId defId, int amount)
        {
            base.Init(defId, amount);
            MyDecayingItemDefinition definition = this.GetDefinition();
        }

        
    }
}
