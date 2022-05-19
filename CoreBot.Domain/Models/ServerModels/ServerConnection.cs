namespace CoreBot.Domain.Models.ServerModels;

public record ServerConnection
{
    [JsonProperty("LOGS_PATH")]
    public string LogsPath { get; init; }
}
