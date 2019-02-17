using System;
using VRage.Utils;
using Sandbox.Game.Entities.Entity.Stats;

namespace RomScripts76561197972467544.StatExtensions
{
    public static class MyEntityThirstExtensions
    {
        public static readonly MyStringHash DefaultStatThirst = MyStringHash.GetOrCompute("Thirst");

        public static readonly MyStringHash DefaulEffectCategoryThirst = MyStringHash.GetOrCompute("Thirst");

        public static MyEntityStat GetThirst(this MyEntityStatComponent statComponent)
        {
            MyEntityStat result = null;
            if (statComponent.TryGetStat(MyEntityThirstExtensions.DefaultStatThirst, out result))
            {
                return result;
            }
            return null;
        }
    }
}
