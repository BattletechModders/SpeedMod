using Newtonsoft.Json;

namespace SpeedMod;

internal class SpeedSettings
{
    [JsonProperty]
    internal bool FastForwardKeyIsToggle = true;
    [JsonProperty]
    internal bool SpeedUpIsConstant = true;
    [JsonProperty]
    internal float SpeedBaseFactor = 1.0f;
}