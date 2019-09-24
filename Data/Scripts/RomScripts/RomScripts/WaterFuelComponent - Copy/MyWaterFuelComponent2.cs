//using Medieval.Definitions.Crafting;
//using Medieval.ObjectBuilders.Components.Crafting;
//using Sandbox.Engine.Multiplayer;
//using Sandbox.Game;
//using Sandbox.Game.Entities.Inventory;
//using Sandbox.Game.Entities.Inventory.Constraints;
//using Sandbox.Game.Multiplayer;
//using Sandbox.Game.Replication;
//using System;
//using System.Collections.Generic;
//using VRage.Components;
//using VRage.Definitions.Inventory;
//using VRage.Game;
//using VRage.Game.Components;
//using VRage.Game.Entity;
//using VRage.Game.ObjectBuilders.ComponentSystem;
//using VRage.Network;
//using VRage.ObjectBuilders;
//using VRage.ObjectBuilders.Definitions.Inventory;
//using VRage.Session;
//using VRage.Systems;
//using VRage.Utils;

//using Medieval.Entities.Components.Crafting;
//using Sandbox.ModAPI;
//using VRage.Game.ModAPI;
//using VRage.ModAPI;
//using VRageMath;

//namespace RomScripts76561197972467544
//{
//    [ReplicatedComponent]
//    [MyComponent(typeof(MyObjectBuilder_WaterFuelComponent2))]
//    public class MyWaterFuelComponent2 : MyEntityComponent, IMyEventOwner, IMyEventProxy, IMyComponentEventProvider, IMyPowerProvider
//    {
//        private MyWaterFuelComponent2Definition m_definition = null;
//        private IMyInventory m_fuelInventory = null;
//        private MyInventoryBase myInventoryBase = null;

//        private long m_maxFuelTime;
//        private long m_remainingFuelTime;
//        private double m_timeAtFuelDepletion;
//        private bool m_isReady;

//        private MyComponentEventBus m_eventBus = null;
//        public event System.Action<MyWaterFuelComponent2> TurnedOn;
//        public event System.Action<MyWaterFuelComponent2> TurnedOff;
//        public event System.Action<MyWaterFuelComponent2> FuelConsumed;
//        public event System.Action<MyWaterFuelComponent2> MissingFuel;

//        public event Action<IMyPowerProvider, bool> PowerStateChanged;
//        public event Action<IMyPowerProvider, bool> ReadyStateChanged;

//        public const string TURN_ON_EVENT = "FuelOn";
//        public const string TURN_OFF_EVENT = "FuelOff";

//        public enum FuelChangeStatus
//        {
//            FuelConsumed,
//            MissingFuel
//        }

//        public override string ComponentTypeDebugString
//        {
//            get
//            {
//                return "Fuel Component";
//            }
//        }

//        public bool Enabled
//        {
//            get;
//            private set;
//        }

//        public MyDefinitionId? CurrentFuel
//        {
//            get;
//            private set;
//        }

//        public IMyInventory Inventory
//        {
//            get
//            {
//                return this.m_fuelInventory;
//            }
//        }

//        public float FuelProgress
//        {
//            get
//            {
//                if (this.m_maxFuelTime == 0L)
//                {
//                    return 0f;
//                }
//                if (this.m_timeAtFuelDepletion > MyAPIGateway.Session.ElapsedPlayTime.TotalMilliseconds)
//                {
//                    double num = this.m_timeAtFuelDepletion - MyAPIGateway.Session.ElapsedPlayTime.TotalMilliseconds;
//                    return (float)num / (float)this.m_maxFuelTime;
//                }
//                if (this.m_remainingFuelTime > 0L)
//                {
//                    return (float)this.m_remainingFuelTime / (float)this.m_maxFuelTime;
//                }
//                return 0f;
//            }
//        }

//        /// <summary>
//        /// The amount of time the current fuel has at max capacity, in seconds.
//        /// </summary>
//        public float MaxFuelTime
//        {
//            get
//            {
//                return (float)this.m_maxFuelTime / 1000f;
//            }
//        }

//        public override bool IsSerialized
//        {
//            get
//            {
//                return true;
//            }
//        }

//        public bool IsPowered
//        {
//            get
//            {
//                return this.Enabled;
//            }
//        }

//        public bool IsReady
//        {
//            get
//            {
//                return this.m_isReady;
//            }
//            private set
//            {
//                if (this.m_isReady == value)
//                {
//                    return;
//                }
//                this.m_isReady = value;
//                if (this.ReadyStateChanged != null)
//                {
//                    this.ReadyStateChanged(this, value);
//                }
//            }
//        }


//        public override void Init(MyEntityComponentDefinition definition)
//        {
//            m_definition = definition as MyWaterFuelComponent2Definition;
//        }

//        public override void OnAddedToContainer()
//        {
//            base.OnAddedToContainer();
//            this.m_eventBus = base.Container.Get<MyComponentEventBus>();
//        }

//        public override void OnAddedToScene()
//        {
//            base.OnAddedToScene();

//            ((IMyUtilities)MyAPIUtilities.Static).ShowNotification(m_definition.FuelInventory.ToString(), 3000, null, Color.Red);

//            foreach (var comp in Entity.Components)
//            {
//                MyInventoryBase inventory = comp as MyInventoryBase;
//                if (inventory == null)
//                    continue;

//                if (inventory.SubtypeId == m_definition.FuelInventory)
//                    this.myInventoryBase = inventory;
//            }

//            IMyInventory myInventory = base.Entity.GetInventory(this.m_definition.FuelInventory) as IMyInventory;
//            if (myInventory == null)
//            {
//                return;
//            }
//            this.m_fuelInventory = myInventory;

//        }

//        public override void OnBeforeRemovedFromContainer()
//        {
//            base.OnBeforeRemovedFromContainer();

//            this.m_fuelInventory = null;
//        }

//        public override MyObjectBuilder_EntityComponent Serialize(bool copy = false)
//        {
//            MyObjectBuilder_WaterFuelComponent2 myObjectBuilder_WaterFuelComponent2 = base.Serialize(copy) as MyObjectBuilder_WaterFuelComponent2;
//            long num = this.m_remainingFuelTime;
//            if (this.Enabled)
//            {
//                num = (long)(this.m_timeAtFuelDepletion - MyAPIGateway.Session.ElapsedPlayTime.TotalMilliseconds);
//            }
//            if (num < 0L)
//            {
//                num = 0L;
//            }
//            myObjectBuilder_WaterFuelComponent2.Enabled = this.Enabled;
//            MyObjectBuilder_WaterFuelComponent2 arg_79_0 = myObjectBuilder_WaterFuelComponent2;
//            MyDefinitionId? currentFuel = this.CurrentFuel;
//            arg_79_0.CurrentFuelId = (currentFuel.HasValue ? new SerializableDefinitionId?(currentFuel.GetValueOrDefault()) : null);
//            myObjectBuilder_WaterFuelComponent2.MaxFuelTime = this.m_maxFuelTime;
//            myObjectBuilder_WaterFuelComponent2.RemainingFuelTime = num;
//            return myObjectBuilder_WaterFuelComponent2;
//        }

//        public override void Deserialize(MyObjectBuilder_EntityComponent builder)
//        {
//            base.Deserialize(builder);
//            MyObjectBuilder_WaterFuelComponent2 myObjectBuilder_WaterFuelComponent2 = builder as MyObjectBuilder_WaterFuelComponent2;
//            SerializableDefinitionId? currentFuelId = myObjectBuilder_WaterFuelComponent2.CurrentFuelId;
//            this.CurrentFuel = (currentFuelId.HasValue ? new MyDefinitionId?(currentFuelId.GetValueOrDefault()) : null);
//            this.m_maxFuelTime = myObjectBuilder_WaterFuelComponent2.MaxFuelTime;
//            this.m_remainingFuelTime = myObjectBuilder_WaterFuelComponent2.RemainingFuelTime;
//            if (myObjectBuilder_WaterFuelComponent2.Enabled)
//            {
//                MyUpdateComponent.Static.Schedule(new MyTimedUpdate(this.TurnOnDelayed), 0L);
//            }
//        }

//        [Update(20, false)]
//        public void Update(long deltaFrames)
//        {
//            if (this.TryConsumeFuel())
//            {
//                if (this.FuelConsumed != null)
//                {
//                    this.FuelConsumed(this);
//                    return;
//                }
//            }
//            else
//            {
//                this.CurrentFuel = null;
//                if (MyAPIGateway.Multiplayer.IsServer)
//                {
//                    Func<MyWaterFuelComponent2, System.Action<SerializableDefinitionId?>> arg_8C_1 = (MyWaterFuelComponent2 x) => new System.Action<SerializableDefinitionId?>(x.TurnOff_Notify);
//                    MyDefinitionId? currentFuel = this.CurrentFuel;
//                    MyAPIGateway.Multiplayer.RaiseEvent<MyWaterFuelComponent2, SerializableDefinitionId?>(this, arg_8C_1, currentFuel.HasValue ? new SerializableDefinitionId?(currentFuel.GetValueOrDefault()) : null, default(EndpointId));
//                    MyDefinitionId? currentFuel2 = this.CurrentFuel;
//                    this.TurnOff_Notify(currentFuel2.HasValue ? new SerializableDefinitionId?(currentFuel2.GetValueOrDefault()) : null);
//                }
//            }
//        }

//        private void TurnOnDelayed(long deltaFrames)
//        {
//            if (MyAPIGateway.Multiplayer.IsServer)
//            {
//                this.TurnOn();
//                return;
//            }
//            if (this.m_remainingFuelTime > 0L && this.CurrentFuel.HasValue)
//            {
//                this.m_timeAtFuelDepletion = MyAPIGateway.Session.ElapsedPlayTime.TotalMilliseconds + (double)this.m_remainingFuelTime;
//                this.m_remainingFuelTime = 0L;
//            }
//            this.TurnOn_Succeeded();
//        }

//        public void TurnOn()
//        {
//            if (!MyAPIGateway.Multiplayer.IsServer)
//            {
//                MyAPIGateway.Multiplayer.RaiseEvent<MyWaterFuelComponent2>(this, (MyWaterFuelComponent2 x) => new Action(x.TurnOn_Server), default(EndpointId));
//                return;
//            }
//            if (this.Enabled)
//            {
//                return;
//            }
//            if (this.TryConsumeFuel())
//            {
//                if (this.FuelConsumed != null)
//                {
//                    this.FuelConsumed(this);
//                }
//                this.Enabled = true;
//                if (this.TurnedOn != null)
//                {
//                    this.TurnedOn(this);
//                }
//                if (this.PowerStateChanged != null)
//                {
//                    this.PowerStateChanged(this, this.IsPowered);
//                }
//                if (this.m_eventBus != null)
//                {
//                    this.m_eventBus.Invoke(MyWaterFuelComponent2.TURN_ON_EVENT);
//                }
//                MyAPIGateway.Multiplayer.RaiseEvent<MyWaterFuelComponent2>(this, (MyWaterFuelComponent2 x) => new Action(x.TurnOn_Succeeded), default(EndpointId));
//                return;
//            }
//            if (this.MissingFuel != null)
//            {
//                this.MissingFuel(this);
//            }
//            MyAPIGateway.Multiplayer.RaiseEvent<MyWaterFuelComponent2, MyWaterFuelComponent2.FuelChangeStatus>(this, (MyWaterFuelComponent2 x) => new System.Action<MyWaterFuelComponent2.FuelChangeStatus>(x.TurnOn_Failed), MyWaterFuelComponent2.FuelChangeStatus.MissingFuel, default(EndpointId));
//        }

//        public void TurnOff()
//        {
//            if (!MyAPIGateway.Multiplayer.IsServer)
//            {
//                MyAPIGateway.Multiplayer.RaiseEvent<MyWaterFuelComponent2>(this, (MyWaterFuelComponent2 x) => new Action(x.TurnOff_Server), default(EndpointId));
//                return;
//            }
//            if (!this.Enabled)
//            {
//                return;
//            }
//            this.Enabled = false;
//            this.m_remainingFuelTime = (long)(this.m_timeAtFuelDepletion - MyAPIGateway.Session.ElapsedPlayTime.TotalMilliseconds);
//            if (this.m_remainingFuelTime < 0L)
//            {
//                this.m_remainingFuelTime = 0L;
//            }
//            this.m_timeAtFuelDepletion = 0.0;
//            if (this.TurnedOff != null)
//            {
//                this.TurnedOff(this);
//            }
//            if (this.PowerStateChanged != null)
//            {
//                this.PowerStateChanged(this, this.IsPowered);
//            }
//            if (this.m_eventBus != null)
//            {
//                this.m_eventBus.Invoke(MyWaterFuelComponent2.TURN_OFF_EVENT);
//            }
//            MyUpdateComponent.Static.RemoveFromUpdate(new MyTimedUpdate(this.Update));
//            Func<MyWaterFuelComponent2, System.Action<SerializableDefinitionId?>> arg_136_1 = (MyWaterFuelComponent2 x) => new System.Action<SerializableDefinitionId?>(x.TurnOff_Notify);
//            MyDefinitionId? currentFuel = this.CurrentFuel;
//            MyAPIGateway.Multiplayer.RaiseEvent<MyWaterFuelComponent2, SerializableDefinitionId?>(this, arg_136_1, currentFuel.HasValue ? new SerializableDefinitionId?(currentFuel.GetValueOrDefault()) : null, default(EndpointId));
//        }

//        public long GetFuelTime(MyDefinitionId id)
//        {
//            if (this.m_definition == null)
//            {
//                return 0L;
//            }
//            int arg_18_0 = this.m_definition.FuelTimes.Count;
//            long result;
//            if (this.m_definition.FuelTimes.TryGetValue(id, out result))
//            {
//                return result;
//            }
//            MyInventoryItemDefinition myInventoryItemDefinition = MyDefinitionManager.Get<MyInventoryItemDefinition>(id);
//            if (myInventoryItemDefinition == null || myInventoryItemDefinition.Tags == null || myInventoryItemDefinition.Tags.Count == 0)
//            {
//                return 0L;
//            }
//            foreach (MyStringHash current in myInventoryItemDefinition.Tags)
//            {
//                if (this.m_definition.FuelTimes.TryGetValue(new MyDefinitionId(typeof(MyObjectBuilder_ItemTagDefinition), current), out result))
//                {
//                    return result;
//                }
//            }
//            return 0L;
//        }

//        [Event(null, 377), Reliable, Server]
//        private void TurnOn_Server()
//        {
//            this.TurnOn();
//        }

//        [Broadcast, Event(null, 383), Reliable]
//        private void TurnOn_Succeeded()
//        {
//            this.Enabled = true;
//            if (this.TurnedOn != null)
//            {
//                this.TurnedOn(this);
//            }
//            if (this.PowerStateChanged != null)
//            {
//                this.PowerStateChanged(this, this.IsPowered);
//            }
//            if (this.m_eventBus != null)
//            {
//                this.m_eventBus.Invoke(MyWaterFuelComponent2.TURN_ON_EVENT);
//            }
//        }

//        [Broadcast, Event(null, 398), Reliable]
//        private void ConsumeFuel_Notify(float fuelProgress, SerializableDefinitionId currentFuel)
//        {
//            this.CurrentFuel = new MyDefinitionId?(currentFuel);
//            this.m_maxFuelTime = this.GetFuelTime(currentFuel);
//            this.m_timeAtFuelDepletion = MyAPIGateway.Session.ElapsedPlayTime.TotalMilliseconds + (double)(fuelProgress * (float)this.m_maxFuelTime);
//            if (this.FuelConsumed != null)
//            {
//                this.FuelConsumed(this);
//            }
//        }

//        [Broadcast, Event(null, 409), Reliable]
//        private void TurnOn_Failed(MyWaterFuelComponent2.FuelChangeStatus change)
//        {
//            if (change != MyWaterFuelComponent2.FuelChangeStatus.MissingFuel)
//            {
//                return;
//            }
//            if (this.MissingFuel != null)
//            {
//                this.MissingFuel(this);
//            }
//        }

//        [Event(null, 421), Reliable, Server]
//        private void TurnOff_Server()
//        {
//            this.TurnOff();
//        }

//        [Broadcast, Event(null, 427), Reliable]
//        private void TurnOff_Notify(SerializableDefinitionId? remainingFuel)
//        {
//            this.Enabled = false;
//            SerializableDefinitionId? serializableDefinitionId = remainingFuel;
//            this.CurrentFuel = (serializableDefinitionId.HasValue ? new MyDefinitionId?(serializableDefinitionId.GetValueOrDefault()) : null);
//            this.m_remainingFuelTime = (long)(this.FuelProgress * (float)this.m_maxFuelTime);
//            if (this.m_remainingFuelTime < 0L)
//            {
//                this.m_remainingFuelTime = 0L;
//                this.m_maxFuelTime = 0L;
//            }
//            this.m_timeAtFuelDepletion = 0.0;
//            if (this.TurnedOff != null)
//            {
//                this.TurnedOff(this);
//            }
//            if (this.PowerStateChanged != null)
//            {
//                this.PowerStateChanged(this, this.IsPowered);
//            }
//            if (this.m_eventBus != null)
//            {
//                this.m_eventBus.Invoke(MyWaterFuelComponent2.TURN_OFF_EVENT);
//            }
//            MyUpdateComponent.Static.RemoveFromUpdate(new MyTimedUpdate(this.Update));
//        }


//        private void OnInventoryChanged(MyInventoryBase inventory)
//        {
//            if (this.CurrentFuel.HasValue && this.m_remainingFuelTime > 0L)
//            {
//                this.IsReady = true;
//                return;
//            }
//            if (this.m_definition == null)
//            {
//                this.IsReady = false;
//                return;
//            }
//            foreach (System.Collections.Generic.KeyValuePair<MyDefinitionId, long> current in this.m_definition.FuelTimes)
//            {
//                if (this.myInventoryBase.FindItemFuzzy(current.Key) != null)
//                {
//                    this.IsReady = true;
//                    return;
//                }
//            }
//            this.IsReady = false;
//        }

//        private bool TryConsumeFuel()
//        {
//            if (this.m_remainingFuelTime > 0L && this.CurrentFuel.HasValue)
//            {
//                this.m_timeAtFuelDepletion = MyAPIGateway.Session.ElapsedPlayTime.TotalMilliseconds + (double)this.m_remainingFuelTime;
//                MyUpdateComponent.Static.Schedule(new MyTimedUpdate(this.Update), this.m_remainingFuelTime);
//                this.m_remainingFuelTime = 0L;
//                MyAPIGateway.Multiplayer.RaiseEvent<MyWaterFuelComponent2, float, SerializableDefinitionId>(this, (MyWaterFuelComponent2 x) => new Action<float, SerializableDefinitionId>(x.ConsumeFuel_Notify), this.FuelProgress, this.CurrentFuel.Value, default(EndpointId));
//                return true;
//            }
//            MyInventoryItem myInventoryItem = null;
//            foreach (System.Collections.Generic.KeyValuePair<MyDefinitionId, long> current in this.m_definition.FuelTimes)
//            {
//                MyDefinitionId key = current.Key;
//                myInventoryItem = this.myInventoryBase.FindItemFuzzy(key);
//                if (myInventoryItem != null)
//                {
//                    this.m_maxFuelTime = current.Value;
//                    this.m_timeAtFuelDepletion = MyAPIGateway.Session.ElapsedPlayTime.TotalMilliseconds + (double)this.m_maxFuelTime;
//                    break;
//                }
//            }
//            if (myInventoryItem != null)
//            {
//                this.CurrentFuel = new MyDefinitionId?(myInventoryItem.DefinitionId);
//                MyUpdateComponent.Static.Schedule(new MyTimedUpdate(this.Update), this.m_maxFuelTime);
//                if (MyAPIGateway.Multiplayer.IsServer)
//                {
//                    this.myInventoryBase.Remove(myInventoryItem, new int?(1));
//                }
//                MyAPIGateway.Multiplayer.RaiseEvent<MyWaterFuelComponent2, float, SerializableDefinitionId>(this, (MyWaterFuelComponent2 x) => new Action<float, SerializableDefinitionId>(x.ConsumeFuel_Notify), this.FuelProgress, this.CurrentFuel.Value, default(EndpointId));
//                return true;
//            }
//            return false;
//        }

//        private MyComponentEventBus EventBus
//        {
//            get
//            {
//                if (m_eventBus != null)
//                    return m_eventBus;

//                m_eventBus = Container.Get<MyComponentEventBus>();
//                return m_eventBus;
//            }
//        }




//        #region IMyComponentEventProvider
//        public void TryStart()
//        {
//            this.TurnOn();
//        }

//        public void TryStop()
//        {
//            this.TurnOff();
//        }

//        public bool HasEvent(string eventName)
//        {
//            switch (eventName)
//            {
//                default: return false;
//                case MyWaterFuelComponent2.TURN_ON_EVENT:
//                case MyWaterFuelComponent2.TURN_OFF_EVENT:
//                    return true;
//            }
//        }
//        #endregion IMyComponentEventProvider
//    }
//}
