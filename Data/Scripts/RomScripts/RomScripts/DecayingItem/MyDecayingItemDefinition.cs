using System;
using VRage.Definitions.Inventory;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.ObjectBuilders.Definitions.Inventory;
using Sandbox.Definitions.Inventory;

namespace RomScripts76561197972467544.DecayingItem
{
    [MyDefinitionType(typeof(MyObjectBuilder_DecayingItemDefinition))]
    public class MyDecayingItemDefinition : MyDurableItemDefinition
    {

        public int DurabilityLossPerSecond;

        protected override void Init(MyObjectBuilder_DefinitionBase builder)
        {
            base.Init(builder);
            MyObjectBuilder_DecayingItemDefinition ob = (MyObjectBuilder_DecayingItemDefinition)builder;
            if (ob == null) return;

            this.DurabilityLossPerSecond = ob.DurabilityLossPerSecond ?? 0;
        }

        public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
        {
            MyObjectBuilder_DecayingItemDefinition myObjectBuilder_DecayingItemDefinition = (MyObjectBuilder_DecayingItemDefinition)base.GetObjectBuilder();
            myObjectBuilder_DecayingItemDefinition.DurabilityLossPerSecond = new int?(this.DurabilityLossPerSecond);
            return myObjectBuilder_DecayingItemDefinition;
        }

    }
}
