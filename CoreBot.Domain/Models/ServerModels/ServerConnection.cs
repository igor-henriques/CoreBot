namespace CoreBot.Domain.Models.ServerModels;

public record ServerConnection
{
    [JsonProperty("GAMEDBD")]
    public Gamedbd Gamedbd { get; init; }

    [JsonProperty("GDELIVERYD")]
    public GDeliveryd GDeliveryd { get; init; }

    [JsonProperty("GPROVIDER")]
    public GProvider GProvider { get; init; }

    [JsonProperty("WORLD2.FORMATLOG")]
    public string FormatlogPath { get; init; }

    [JsonProperty("WORLD2.LOG")]
    public string LogPath { get; init; }

    [JsonProperty("WORLD2.CHAT")]
    public string ChatPath { get; init; }

    [JsonProperty("PW_VERSION")]
    public PwVersion PwVersion { get; init; }
}
