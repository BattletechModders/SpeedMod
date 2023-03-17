using System.Collections.Generic;
using BattleTech;
using UnityEngine;
using static SpeedMod.Control;

namespace SpeedMod.Patches;

[HarmonyPatch(typeof(StackManager), nameof(StackManager.GetProgressiveAttackDeltaTime))]
internal static class StackManager_GetProgressiveAttackDeltaTime_Patch
{
    [HarmonyTranspiler]
    internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return instructions
            .MethodReplacer(
                AccessTools.Property(typeof(StackManager), nameof(StackManager.AttackTimeMultiplier)).GetMethod,
                AccessTools.Method(typeof(StackManager_GetProgressiveAttackDeltaTime_Patch), nameof(get_AttackTimeMultiplier))
            );
    }

    internal static float get_AttackTimeMultiplier(StackManager @this)
    {
        var num = @this.AttackTimeMultiplier;
        if (SpeedToggled)
        {
            if (Mathf.Approximately(num, 1f))
            {
                return @this.Combat.Constants.CombatUIConstants.AttackSpeedUpFactor;
            }
        }
        return num;
    }
}