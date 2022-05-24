namespace CoreBot.Workers;

internal class LogConsumer : BackgroundService
{
    private readonly IBackgroundTaskQueue _taskQueue;
    private readonly ILogger<LogConsumer> _logger;

    public LogConsumer(IServiceProvider services, ILogger<LogConsumer> logger)
    {
        this._taskQueue = GetTaskQueue<LogTaskQueue>(services);
        this._logger = logger;
    }
    private IBackgroundTaskQueue GetTaskQueue<T>(IServiceProvider services) where T : IBackgroundTaskQueue
    {
        foreach (var task in services.GetServices(typeof(IBackgroundTaskQueue)))
        {
            if (task.GetType() == typeof(T))
                return (T)task;
        }

        return null;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.Write($"{nameof(LogConsumer)} inicializado");

        while (!stoppingToken.IsCancellationRequested)
        {
            var workItem = await _taskQueue.DequeueAsync(stoppingToken);

            await workItem();

            await Task.Delay(1000);
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"{nameof(LogConsumer)} finalizado.");

        await base.StopAsync(stoppingToken);
    }
    public override Task StartAsync(System.Threading.CancellationToken cancellationToken)
    {
        return base.StartAsync(cancellationToken);
    }
}
