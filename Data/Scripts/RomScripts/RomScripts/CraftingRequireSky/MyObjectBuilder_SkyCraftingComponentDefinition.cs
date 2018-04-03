using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using VRage.Game;
using VRage.ObjectBuilders;

namespace RomScripts.SkyCraftingComponent
{
    [MyObjectBuilderDefinition]
    [XmlSerializerAssembly("MedievalEngineers.ObjectBuilders.XmlSerializers")]
    public class MyObjectBuilder_SkyCraftingComponentDefinition : MyObjectBuilder_EntityComponentDefinition
    {
        public int CheckIntervalMS = 60000;
        public float PhysicsCheckDistance = 50.0f;
        public bool IgnoreVoxels = false;
        public bool IgnoreBlocks = false;
        public bool IgnoreOther = false;

        public string DestructionEffect = "Prefab_DestructionSmoke";

        public int NotifactionDurationMS = 3000;
        public int NotifactionDurationLongMS = 4000;

        public string Notification_SkyBlocked = "Sky visibility is blocked!";
        public string Notification_SkyBlockedFinalWarning = "Sky visibility is blocked! Final warning!";
        public string Notification_SkyUnblocked = "Sky visibility is no longer blocked!";
        public string Notification_Destroyed = "Sky visibiltiy was blocked and an entity was destroyed!";
    }
}
