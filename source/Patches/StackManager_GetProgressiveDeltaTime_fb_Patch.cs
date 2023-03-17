using BattleTech;
using static SpeedMod.Control;

namespace SpeedMod.Patches;

[HarmonyPatch(typeof(StackManager), nameof(StackManager.GetProgressiveDeltaTime), typeof(float), typeof(bool))]
internal static class StackManager_GetProgressiveDeltaTime_fb_Patch
{
    [HarmonyPrefix]
    internal static void Prefix(ref bool __runOriginal, ref bool isSpedUp)
    {
        if (!__runOriginal)
        {
            return;
        }

        if (SpeedToggled)
        {
            isSpedUp = true;
        }
    }
}