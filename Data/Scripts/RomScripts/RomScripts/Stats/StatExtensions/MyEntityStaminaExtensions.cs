using System;
using VRage.Utils;
using Sandbox.Game.Entities.Entity.Stats;

namespace RomScripts76561197972467544.StatExtensions
{
    public static class MyEntityRomStaminaExtensions
    {
        public const float StaminaCostSwingTool = 8f;

        public static readonly MyStringHash DefaultStatStamina = MyStringHash.GetOrCompute("Stamina");

        public static MyEntityStat GetRomStamina(this MyEntityStatComponent statComponent)
        {
            MyEntityStat result = null;
            if (statComponent.TryGetStat(MyEntityRomStaminaExtensions.DefaultStatStamina, out result))
            {
                return result;
            }
            return null;
        }

        public static bool HasEnoughStamina(this MyEntityStatComponent statComponent, float requiredStamina)
        {
            MyEntityStat stamina = statComponent.GetRomStamina();
            return stamina == null || stamina > requiredStamina;
        }
    }
}
