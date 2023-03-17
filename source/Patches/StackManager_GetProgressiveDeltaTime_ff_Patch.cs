using BattleTech;
using static SpeedMod.Control;

namespace SpeedMod.Patches;

[HarmonyPatch(typeof(StackManager), nameof(StackManager.GetProgressiveDeltaTime), typeof(float), typeof(float))]
internal static class StackManager_GetProgressiveDeltaTime_ff_Patch
{
    internal static bool Prefix(StackManager __instance, ref float multiplier, ref float __result)
    {
        multiplier *= Settings.SpeedBaseFactor;

        if (!Settings.SpeedUpIsConstant)
        {
            return true;
        }

        __result = __instance.deltaTime * multiplier;

        return false;
    }
}