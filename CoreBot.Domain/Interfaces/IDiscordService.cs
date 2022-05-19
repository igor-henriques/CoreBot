namespace CoreBot.Domain.Interfaces;

public interface IDiscordService
{
    Task SendMessageAsync(string webhook, string message);
}
