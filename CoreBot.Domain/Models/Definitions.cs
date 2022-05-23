namespace CoreBot.Domain.Models;

public record Definitions
{
    [JsonProperty("CHAT_WEBHOOK")]
    public string ChatWebhook { get; init; }

    [JsonProperty("LOG_WEBHOOK")]
    public string LogWebhook { get; init; }
}
