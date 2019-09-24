//using Medieval.ObjectBuilders.Definitions.Crafting;
//using System;
//using System.Collections.Generic;
//using VRage.Game;
//using VRage.Game.Definitions;
//using VRage.Utils;

//namespace RomScripts76561197972467544
//{
//    [MyDefinitionType(typeof(MyObjectBuilder_WaterFuelComponent2Definition))]
//    public class MyWaterFuelComponent2Definition : MyEntityComponentDefinition
//    {
//        public readonly System.Collections.Generic.Dictionary<MyDefinitionId, long> FuelTimes = new System.Collections.Generic.Dictionary<MyDefinitionId, long>(MyDefinitionId.Comparer);

//        public MyStringHash FuelInventory { get; private set; }

//        protected override void Init(MyObjectBuilder_DefinitionBase builder)
//        {
//            base.Init(builder);

//            var ob = (MyObjectBuilder_WaterFuelComponent2Definition)builder;

//            if (!string.IsNullOrWhiteSpace(ob.FuelInventory))
//                FuelInventory = MyStringHash.GetOrCompute(ob.FuelInventory);

//            if (ob.FuelTimes == null || ob.FuelTimes.Count == 0)
//            {
//                throw new MyDefinitionException("Crafting recipe must have prerequisites.");
//            }
//            foreach (MyObjectBuilder_WaterFuelComponent2Definition.FuelTimeDef current in ob.FuelTimes)
//            {
//                TimeSpan val = current.Time.Value;
//                this.FuelTimes[current.Id] = (long)val.TotalMilliseconds;
//            }
//        }
//    }
//}
