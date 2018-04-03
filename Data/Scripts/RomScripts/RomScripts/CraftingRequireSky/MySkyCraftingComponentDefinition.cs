using System;
using ObjectBuilders.Definitions.Tools;
using Sandbox.Definitions.Equipment;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Utils;

using Medieval.Definitions.Tools;

namespace RomScripts.SkyCraftingComponent
{
    [MyDefinitionType(typeof(MyObjectBuilder_SkyCraftingComponentDefinition))]
    public class MySkyCraftingComponentDefinition : MyEntityComponentDefinition
    {
        public int CheckIntervalMS { get; private set; }
        public float PhysicsCheckDistance { get; private set; }
        public bool IgnoreVoxels { get; private set; }
        public bool IgnoreBlocks { get; private set; }
        public bool IgnoreOther { get; private set; }

        public MyStringHash DestructionEffect { get; private set; }

        public int NotifactionDurationMS { get; private set; }
        public int NotifactionDurationLongMS { get; private set; }

        public MyStringId Notification_SkyBlocked { get; private set; }
        public MyStringId Notification_SkyBlockedFinalWarning { get; private set; }
        public MyStringId Notification_SkyUnblocked { get; private set; }
        public MyStringId Notification_Destroyed { get; private set; }

        protected override void Init(MyObjectBuilder_DefinitionBase builder)
        {
            base.Init(builder);

            var ob = (MyObjectBuilder_SkyCraftingComponentDefinition)builder;

            CheckIntervalMS = ob.CheckIntervalMS;
            PhysicsCheckDistance = ob.PhysicsCheckDistance;
            IgnoreVoxels = ob.IgnoreVoxels;
            IgnoreBlocks = ob.IgnoreBlocks;
            IgnoreOther = ob.IgnoreOther;

            DestructionEffect = MyStringHash.GetOrCompute(ob.DestructionEffect);

            NotifactionDurationMS = ob.NotifactionDurationMS;
            NotifactionDurationLongMS = ob.NotifactionDurationLongMS;

            Notification_SkyBlocked = MyStringId.GetOrCompute(ob.Notification_SkyBlocked);
            Notification_SkyBlockedFinalWarning = MyStringId.GetOrCompute(ob.Notification_SkyBlockedFinalWarning);
            Notification_SkyUnblocked = MyStringId.GetOrCompute(ob.Notification_SkyUnblocked);
            Notification_Destroyed = MyStringId.GetOrCompute(ob.Notification_Destroyed);
        }
    }
}
