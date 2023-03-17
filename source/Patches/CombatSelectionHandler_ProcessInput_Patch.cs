using System;
using BattleTech.UI;
using static SpeedMod.Control;

namespace SpeedMod.Patches;

[HarmonyPatch(typeof(CombatSelectionHandler), nameof(CombatSelectionHandler.ProcessInput))]
internal static class CombatSelectionHandler_ProcessInput_Patch
{
    internal static void Prefix()
    {
        try
        {
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
        catch (Exception e)
        {
            Log.LogError(e);
        }
    }
}