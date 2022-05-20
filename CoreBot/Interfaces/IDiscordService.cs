namespace CoreBot.Interfaces;

public interface IDiscordService
{
    Task SendMessageAsync(string webhook, string message);
}
