using BattleTech.UI;
using static SpeedMod.Control;

namespace SpeedMod.Patches;

[HarmonyPatch(typeof(CombatSelectionHandler), nameof(CombatSelectionHandler.ProcessInput))]
internal static class CombatSelectionHandler_ProcessInput_Patch
{
    [HarmonyPrefix]
    [HarmonyWrapSafe]
    internal static void Prefix(ref bool __runOriginal)
    {
        if (!__runOriginal)
        {
            return;
        }

        if (!Settings.FastForwardKeyIsToggle)
        {
            return;
        }

        if (SpeedUpAction == null || !SpeedUpAction.HasChanged || !SpeedUpAction.IsPressed)
        {
            return;
        }

        SpeedToggled = !SpeedToggled;

        Log.Log("toggled speed " + SpeedToggled);
    }
}