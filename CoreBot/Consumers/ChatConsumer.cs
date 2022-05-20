namespace CoreBot.Consumers;

internal class ChatConsumer : BackgroundService
{    
    private readonly IBackgroundTaskQueue _taskQueue;
    private readonly ILogger<ChatConsumer> _logger;
    private readonly ILogModelsFactory logModelFactory;
    private static IDiscordService discordService;

    public ChatConsumer(IServiceProvider services, ILogger<ChatConsumer> logger, ILogModelsFactory logModelFactory)
    {
        this._taskQueue = (IBackgroundTaskQueue)services.GetService(typeof(LogTaskQueue));
        this._logger = logger;
        this.logModelFactory = logModelFactory;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return ProcessTaskQueueAsync(stoppingToken);
    }

    private async Task ProcessTaskQueueAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var workItem = await _taskQueue.DequeueAsync(stoppingToken);

            await workItem();

            await Task.Delay(1000);
        }
    }

    public static async ValueTask Process(string log)
    {
        try
        {
            discordService = discordService ?? new DiscordService();

            if (!log.GetOperation().Equals(ELogOperation.Chat))
                return;

            var logParseResult = await logModelFactory.BuildChatModel(log);

            if (logParseResult is null | logParseResult is default(IBaseLogModel))
                return;

            await discordService.SendMessageAsync(WebHooks.Feedback, logParseResult.ToString());
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"{nameof(ChatConsumer)} finalizado.");

        await base.StopAsync(stoppingToken);
    }
}
