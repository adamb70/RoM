//using Medieval.ObjectBuilders.Definitions.Crafting;
//using System;
//using System.Collections.Generic;
//using VRage.Game;
//using VRage.Game.Definitions;
//using VRage.Utils;

//namespace RomScripts76561197972467544.WaterFuelComponent
//{
//    [MyDefinitionType(typeof(MyObjectBuilder_WaterFuelComponentDefinition))]
//    public class MyWaterFuelComponentDefinition : MyEntityComponentDefinition
//    {
//        public readonly System.Collections.Generic.Dictionary<MyDefinitionId, long> FuelTimes = new System.Collections.Generic.Dictionary<MyDefinitionId, long>(MyDefinitionId.Comparer);

//        public MyStringHash FuelInventory;

//        protected override void Init(MyObjectBuilder_DefinitionBase builder)
//        {
//            base.Init(builder);

//            var ob = (MyObjectBuilder_WaterFuelComponentDefinition)builder;

//            if (!string.IsNullOrWhiteSpace(ob.FuelInventory))
//            {
//                this.FuelInventory = MyStringHash.GetOrCompute(ob.FuelInventory);
//            }

//            if (ob.FuelTimes == null || ob.FuelTimes.Count == 0)
//            {
//                throw new MyDefinitionException("Crafting recipe must have prerequisites.");
//            }
//            foreach (MyObjectBuilder_WaterFuelComponentDefinition.FuelTimeDef current in ob.FuelTimes)
//            {
//                TimeSpan val = current.Time.Value;
//                this.FuelTimes[current.Id] = (long)val.TotalMilliseconds;
//            }
//        }
//    }
//}
