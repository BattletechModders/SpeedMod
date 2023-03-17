using System.Collections.Generic;
using BattleTech;
using InControl;
using static SpeedMod.Control;

namespace SpeedMod.Patches;

[HarmonyPatch(typeof(OrderSequence), nameof(OrderSequence.OnUpdate))]
internal static class OrderSequence_OnUpdate_Patch
{
    internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return instructions
            .MethodReplacer(
                AccessTools.Property(typeof(OneAxisInputControl), nameof(OneAxisInputControl.WasReleased)).GetMethod,
                AccessTools.Method(typeof(OrderSequence_OnUpdate_Patch), nameof(get_WasReleased))
            );
    }

    internal static bool get_WasReleased(OneAxisInputControl @this)
    {
        if (Settings.FastForwardKeyIsToggle)
        {
            return false;
        }

        return SpeedUpAction.WasReleased;
    }
}