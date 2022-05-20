﻿namespace CoreBot.Workers;

internal class LogWorker : BackgroundService
{
    private readonly IBackgroundTaskQueue _taskQueue;
    private readonly ILogger<LogWorker> _logger;
    private readonly IServerRepository serverContext;
    private static IDiscordService discordService;

    public LogWorker(IBackgroundTaskQueue taskQueue, ILogger<LogWorker> logger, IServerRepository serverContext)
    {
        this._taskQueue = taskQueue;
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

            var logParseFunc = log.GetOperation() switch
            {
                ELogOperation.PickupItem => TranslateToModel.GeneratePickupItemLog(log),
                _ => ValueTask.FromResult(default(IBaseLogModel))
            };

            var logParseResult = await logParseFunc;

            if (logParseResult is not null or not default(IBaseLogModel))
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
        _logger.LogInformation($"{nameof(FormatlogConsumer)} finalizado.");

        await base.StopAsync(stoppingToken);
    }
}
