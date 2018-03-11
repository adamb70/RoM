using System;
using System.Runtime.InteropServices;

namespace RomScripts
{
    [System.Runtime.InteropServices.StructLayout(LayoutKind.Sequential, Size = 1)]
    internal struct RomEffectConstants
    {
        public static float HealthTick = 1f;

        public static float HealthInterval = 5f;

        public static float HealthTickOutOfFood = -2f;

        public static float HealthIntervalOutOfFood = 4f;

        public static float StaminaTick = 10f;

        public static float StaminaInterval = 0.75f;

        public static float StaminaTickCrouchIdle = 10f;

        public static float StaminaIntervalCrouchIdle = 0.5f;

        public static float StaminaTickCrouchWalk = 3f;

        public static float StaminaIntervalCrouchWalk = 1f;

        public static float StaminaTickRun = 4.6f;

        public static float StaminaIntervalRun = 1f;

        public static float StaminaTickSprint = -4f;

        public static float StaminaIntervalLowFood = 0.75f;

        public static float StaminaIntervalDuringSecondBreath = 0.75f;

        public static float StaminaCostJump = 4f;

        public static float StaminaCostSwingTool = 6f;

        public static float StaminaCostWork = 0f;

        public static float StaminaCostBlock = 8f;

        public static float FoodTick = -0.5f;

        public static float FoodInterval = 12f;

        public static float FoodIntervalSecondBreath = 12f;
    }
}
