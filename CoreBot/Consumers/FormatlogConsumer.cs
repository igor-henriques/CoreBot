namespace CoreBot.Consumers;

internal class FormatlogConsumer : BackgroundService
{    
    private readonly IBackgroundTaskQueue _taskQueue;
    private readonly ILogger<FormatlogConsumer> _logger;

    public FormatlogConsumer(IServiceProvider services, ILogger<FormatlogConsumer> logger)
    {
        this._taskQueue = (IBackgroundTaskQueue)services.GetService(typeof(LogTaskQueue));
        this._logger = logger;
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

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"{nameof(FormatlogConsumer)} finalizado.");

        await base.StopAsync(stoppingToken);
    }
}
