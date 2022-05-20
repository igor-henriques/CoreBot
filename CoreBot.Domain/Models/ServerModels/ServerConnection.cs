namespace CoreBot.Domain.Models.ServerModels;

public record ServerConnection
{
    [JsonProperty("GAMEDBD")]
    public Gamedbd Gamedbd { get; init; }

    [JsonProperty("GDELIVERYD")]
    public GDeliveryd GDeliveryd { get; init; }

    [JsonProperty("GPROVIDER")]
    public GProvider GProvider { get; init; }

    [JsonProperty("LOGS_PATH")]
    public string LogsPath { get; init; }

    [JsonProperty("PW_VERSION")]
    public PwVersion PwVersion { get; init; }
}
