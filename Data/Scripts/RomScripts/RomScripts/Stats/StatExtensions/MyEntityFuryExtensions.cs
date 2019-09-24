using System;
using VRage.Utils;
using Sandbox.Game.Entities.Entity.Stats;

namespace RomScripts76561197972467544.StatExtensions
{
    public static class MyEntityFuryExtensions
    {
        public static readonly MyStringHash DefaultStatFury = MyStringHash.GetOrCompute("Fury");

        public static readonly MyStringHash DefaulEffectCategoryFury = MyStringHash.GetOrCompute("Fury");

        public static MyEntityStat GetFury(this MyEntityStatComponent statComponent)
        {
            MyEntityStat result = null;
            if (statComponent.TryGetStat(MyEntityFuryExtensions.DefaultStatFury, out result))
            {
                return result;
            }
            return null;
        }
    }
}
