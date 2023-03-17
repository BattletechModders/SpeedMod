using BattleTech;
using InControl;
using System.IO;
using System.Reflection;
using HBS.Logging;
using Newtonsoft.Json;

namespace SpeedMod;

public static class Control
{
    internal static readonly ILog Log = HBS.Logging.Logger.GetLogger(nameof(SpeedMod), LogLevel.Debug);
    internal static readonly SpeedSettings Settings = new();

    internal static PlayerAction SpeedUpAction => BTInput.Instance.Combat_FFCurrentMove();
    internal static bool SpeedToggled;

    public static void Start(string modDirectory, string json)
    {
        JsonConvert.PopulateObject(
            File.ReadAllText(Path.Combine(modDirectory, "Settings.json")),
            Settings,
            new()
            {
                ObjectCreationHandling = ObjectCreationHandling.Replace,
                NullValueHandling = NullValueHandling.Ignore
            }
        );

        var harmony = HarmonyInstance.Create(nameof(SpeedMod));
        harmony.PatchAll(Assembly.GetExecutingAssembly());
        Log.Log("initialized");
    }
}