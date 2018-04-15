using System;
using VRage.Utils;
using Sandbox.Game.Entities.Entity.Stats;

namespace RomScripts.StatExtensions
{
    public static class MyEntityPoisonExtensions
    {
        public static readonly MyStringHash DefaultStatPoison = MyStringHash.GetOrCompute("Poison");

        public static readonly MyStringHash DefaulEffectCategoryPoison = MyStringHash.GetOrCompute("Poison");

        public static MyEntityStat GetPoison(this MyEntityStatComponent statComponent)
        {
            MyEntityStat result = null;
            if (statComponent.TryGetStat(MyEntityPoisonExtensions.DefaultStatPoison, out result))
            {
                return result;
            }
            return null;
        }
    }
}
