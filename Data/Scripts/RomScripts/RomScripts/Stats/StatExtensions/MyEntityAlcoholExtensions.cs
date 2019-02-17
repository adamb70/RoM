using System;
using VRage.Utils;
using Sandbox.Game.Entities.Entity.Stats;

namespace RomScripts76561197972467544.StatExtensions
{
    public static class MyEntityAlcoholExtensions
    {
        public static readonly MyStringHash DefaultStatAlcohol = MyStringHash.GetOrCompute("Alcohol");

        public static readonly MyStringHash DefaulEffectCategoryAlcohol = MyStringHash.GetOrCompute("Alcohol");

        public static MyEntityStat GetAlcohol(this MyEntityStatComponent statComponent)
        {
            MyEntityStat result = null;
            if (statComponent.TryGetStat(MyEntityAlcoholExtensions.DefaultStatAlcohol, out result))
            {
                return result;
            }
            return null;
        }
    }
}
