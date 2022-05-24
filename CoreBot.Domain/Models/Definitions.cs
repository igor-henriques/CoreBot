namespace CoreBot.Domain.Models;

public record Definitions
{
    [JsonProperty("CHAT_WEBHOOK")]
    public string ChatWebhook { get; init; }

    [JsonProperty("LOG_WEBHOOK")]
    public string LogWebhook { get; init; }

    public void Validate()
    {
        if (string.IsNullOrEmpty(this.ChatWebhook) & string.IsNullOrEmpty(this.LogWebhook))
            throw new ArgumentException("Não há webhook preenchido no arquivo Definitions.json33");
    }
}
