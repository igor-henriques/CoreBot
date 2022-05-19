namespace CoreBot.Services;

internal class DiscordService : IDiscordService
{
    private readonly Dictionary<string, DiscordWebhookClient> webhooks = new Dictionary<string, DiscordWebhookClient>();

    public async Task SendMessageAsync(string webhook, string message)
    {
        if (!webhooks.TryGetValue(webhook, out DiscordWebhookClient client))
        {
            webhooks.Add(webhook, client = new DiscordWebhookClient(webhook));
        }

        await client.SendMessageAsync(message);
    }
}
