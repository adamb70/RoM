using System;
using VRage.Utils;
using Sandbox.Game.Entities.Entity.Stats;

namespace RomScripts.StatExtensions
{
    public static class MyEntityMoodExtensions
    {
        public static readonly MyStringHash DefaultStatMood = MyStringHash.GetOrCompute("Mood");

        public static readonly MyStringHash DefaulEffectCategoryMood = MyStringHash.GetOrCompute("Mood");

        public static MyEntityStat GetMood(this MyEntityStatComponent statComponent)
        {
            MyEntityStat result = null;
            if (statComponent.TryGetStat(MyEntityMoodExtensions.DefaultStatMood, out result))
            {
                return result;
            }
            return null;
        }
    }
}
