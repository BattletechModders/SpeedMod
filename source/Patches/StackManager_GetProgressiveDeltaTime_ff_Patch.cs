using BattleTech;
using static SpeedMod.Control;

namespace SpeedMod.Patches;

[HarmonyPatch(typeof(StackManager), nameof(StackManager.GetProgressiveDeltaTime), typeof(float), typeof(float))]
internal static class StackManager_GetProgressiveDeltaTime_ff_Patch
{
    [HarmonyPrefix]
    internal static void Prefix(ref bool __runOriginal, StackManager __instance, ref float multiplier, ref float __result)
    {
        if (!__runOriginal)
        {
            return;
        }

        multiplier *= Settings.SpeedBaseFactor;

        if (!Settings.SpeedUpIsConstant)
        {
            return;
        }

        __result = __instance.deltaTime * multiplier;

        __runOriginal = false;
    }
}