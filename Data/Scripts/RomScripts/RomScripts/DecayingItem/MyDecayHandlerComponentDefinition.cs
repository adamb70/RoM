﻿using System;
using VRage.Definitions.Inventory;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.ObjectBuilders.Definitions.Inventory;
using Sandbox.Definitions.Inventory;

namespace RomScripts.DecayingItem
{
    [MyDefinitionType(typeof(MyObjectBuilder_DecayHandlerComponentDefinition))]
    public class MyDecayHandlerComponentDefinition : MyEntityComponentDefinition
    {
        public long TickIntervalMs;

        protected override void Init(MyObjectBuilder_DefinitionBase builder)
        {
            base.Init(builder);
            MyObjectBuilder_DecayHandlerComponentDefinition ob = (MyObjectBuilder_DecayHandlerComponentDefinition)builder;
            this.TickIntervalMs = ob.TickInterval ?? 10000;
        }

        public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
        {
            MyObjectBuilder_DecayHandlerComponentDefinition ob = (MyObjectBuilder_DecayHandlerComponentDefinition)base.GetObjectBuilder();
            ob.TickInterval = this.TickIntervalMs;
            return ob;
        }

    }
}