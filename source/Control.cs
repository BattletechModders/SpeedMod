
using System;
using HBS.Logging;
using Harmony;
using System.Reflection;
using BattleTech;
using BattleTech.UI;
using DynModLib;
using InControl;
using UnityEngine;
using Logger = HBS.Logging.Logger;

namespace SpeedMod
{
    public static class Control
    {
        public static Mod mod;

        public static SpeedSettings settings = new SpeedSettings();

        public static void Start(string modDirectory, string json)
        {
            mod = new Mod(modDirectory);
            mod.LoadSettings(settings);
			
			var harmony = HarmonyInstance.Create(mod.Name);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            // logging output can be found under BATTLETECH\BattleTech_Data\output_log.txt
            // or also under yourmod/log.txt
            mod.Logger.Log("Loaded " + mod.Name);
        }

        public static PlayerAction SpeedUpAction;
        public static bool speedToggled;


        public class SpeedSettings : ModSettings
        {
            public float speedFactor = 5.0f;
        }

        [HarmonyPatch(typeof(DynamicActions), "CreateWithDefaultBindings")]
        public static class DynamicActionsCreateWithDefaultBindingsPatch
        {
            public static void Postfix(DynamicActions __result)
            {
                try
                {
                    var adapter = new DynamicActionsAdapter(__result);
                    SpeedUpAction = adapter.CreatePlayerAction("Toggle Speed Mod");
                    SpeedUpAction.AddDefaultBinding(Key.P);
                    mod.Logger.Log("added dynamic action and default key binding");
                }
                catch (Exception e)
                {
                    mod.Logger.LogError(e);
                }
            }
        }

        internal class DynamicActionsAdapter : Adapter<DynamicActions>
        {
            internal DynamicActionsAdapter(DynamicActions instance) : base(instance)
            {
            }

            internal PlayerAction CreatePlayerAction(string name)
            {
                return traverse.Method("CreatePlayerAction", name).GetValue<PlayerAction>(name); ;
            }
        }

        [HarmonyPatch(typeof(CombatSelectionHandler), "ProcessInput")]
        public static class CombatSelectionHandlerProcessInputPatch
        {
            public static void Prefix(CombatSelectionHandler __instance)
            {
                try
                {
                    if (SpeedUpAction == null || !SpeedUpAction.HasChanged || !SpeedUpAction.IsPressed)
                    {
                        return;
                    }

                    speedToggled = !speedToggled;
                    
                    mod.Logger.Log("toggled speed " + speedToggled);
                }
                catch (Exception e)
                {
                    Control.mod.Logger.LogError(e);
                }
            }
        }

        [HarmonyPatch(typeof(StackManager), "get_deltaTime")]
        public static class StackManagerget_deltaTimePatch
        {
            public static bool Prefix(StackManager __instance, ref float __result)
            {
                if (speedToggled)
                {
                    __result = Time.deltaTime * settings.speedFactor;
                    return false;
                }

                return true;
            }
        }
    }
}
