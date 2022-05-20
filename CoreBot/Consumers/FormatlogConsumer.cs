namespace CoreBot.Consumers;

internal class FormatlogConsumer : BackgroundService
{    
    private readonly IBackgroundTaskQueue _taskQueue;
    private readonly ILogger<FormatlogConsumer> _logger;
    private readonly IServerRepository serverContext;
    private static IDiscordService discordService;

    public FormatlogConsumer(IServiceProvider services, ILogger<FormatlogConsumer> logger, IServerRepository serverContext)
    {
        this._taskQueue = (IBackgroundTaskQueue)services.GetService(typeof(LogTaskQueue));
        this._logger = logger;
        this.serverContext = serverContext;
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

            await workItem(stoppingToken);

            await Task.Delay(1000);
        }
    }

    public static async ValueTask Process(CancellationToken cancellationToken, string log)
    {
        try
        {
            discordService = discordService ?? new DiscordService();



            await discordService.SendMessageAsync(WebHooks.Feedback, log);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"{nameof(FormatlogConsumer)} finalizado.");

        await base.StopAsync(stoppingToken);
    }
}
