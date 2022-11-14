using BattleTech;
using BattleTech.UI;
using Harmony;
using InControl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using HBS.Logging;
using HBS.Util;
using UnityEngine;

namespace SpeedMod;

public static class Control
{
    private static readonly ILog _log = HBS.Logging.Logger.GetLogger(nameof(SpeedMod), LogLevel.Debug);
    private static readonly SpeedSettings _settings = new();

    public static void Start(string modDirectory, string json)
    {
        JSONSerializationUtility.FromJSON(
            _settings,
            File.ReadAllText(
                Path.Combine(modDirectory, "Settings.json")
            )
        );
        var harmony = HarmonyInstance.Create(nameof(SpeedMod));
        harmony.PatchAll(Assembly.GetExecutingAssembly());
        _log.Log("Loaded");
    }

    private class SpeedSettings
    {
        public bool FastForwardKeyIsToggle = true;
        public bool SpeedUpIsConstant = true;
        public float SpeedBaseFactor = 1.0f;
    }

    private static PlayerAction SpeedUpAction => BTInput.Instance.Combat_FFCurrentMove();

    private static bool speedToggled;

    [HarmonyPatch(typeof(CombatSelectionHandler), "ProcessInput")]
    public static class CombatSelectionHandlerProcessInputPatch
    {
        public static void Prefix()
        {
            try
            {
                if (!_settings.FastForwardKeyIsToggle)
                {
                    return;
                }

                if (SpeedUpAction == null || !SpeedUpAction.HasChanged || !SpeedUpAction.IsPressed)
                {
                    return;
                }

                speedToggled = !speedToggled;

                _log.Log("toggled speed " + speedToggled);
            }
            catch (Exception e)
            {
                _log.LogError(e);
            }
        }
    }

    [HarmonyPatch(typeof(OrderSequence), "OnUpdate")]
    public static class OrderSequenceOnUpdatePatch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return instructions
                .MethodReplacer(
                    AccessTools.Method(typeof(OneAxisInputControl), "get_WasReleased"),
                    AccessTools.Method(typeof(OrderSequenceOnUpdatePatch), "get_WasReleased")
                );
        }

        public static bool get_WasReleased(OneAxisInputControl @this)
        {
            if (_settings.FastForwardKeyIsToggle)
            {
                return false;
            }

            return SpeedUpAction.WasReleased;
        }
    }

    [HarmonyPatch(typeof(StackManager), "GetProgressiveDeltaTime", typeof(float), typeof(bool))]
    public static class StackManagerGetProgressiveDeltaTime1Patch
    {
        public static void Prefix(ref bool isSpedUp)
        {
            if (speedToggled)
            {
                isSpedUp = true;
            }
        }
    }

    [HarmonyPatch(typeof(StackManager), "GetProgressiveAttackDeltaTime")]
    public static class StackManagerGetProgressiveAttackDeltaTimePatch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return instructions
                .MethodReplacer(
                    AccessTools.Method(typeof(StackManager), "get_AttackTimeMultiplier"),
                    AccessTools.Method(typeof(StackManagerGetProgressiveAttackDeltaTimePatch), "get_AttackTimeMultiplier")
                );
        }

        public static float get_AttackTimeMultiplier(StackManager @this)
        {
            var num = @this.AttackTimeMultiplier;
            if (speedToggled)
            {
                if (Mathf.Approximately(num, 1f))
                {
                    return @this.Combat.Constants.CombatUIConstants.AttackSpeedUpFactor;
                }
            }
            return num;
        }
    }

    [HarmonyPatch(typeof(StackManager), "GetProgressiveDeltaTime", typeof(float), typeof(float))]
    public static class StackManagerGetProgressiveDeltaTime2Patch
    {
        public static bool Prefix(StackManager __instance, ref float multiplier, ref float __result)
        {
            multiplier *= _settings.SpeedBaseFactor;

            if (!_settings.SpeedUpIsConstant)
            {
                return true;
            }

            __result = __instance.deltaTime * multiplier;

            return false;
        }
    }
}