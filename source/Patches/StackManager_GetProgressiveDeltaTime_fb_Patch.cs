using BattleTech;
using static SpeedMod.Control;

namespace SpeedMod.Patches;

[HarmonyPatch(typeof(StackManager), nameof(StackManager.GetProgressiveDeltaTime), typeof(float), typeof(bool))]
internal static class StackManager_GetProgressiveDeltaTime_fb_Patch
{
    internal static void Prefix(ref bool isSpedUp)
    {
        if (SpeedToggled)
        {
            isSpedUp = true;
        }
    }
}