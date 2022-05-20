namespace CoreBot.Services;

public class LogTaskQueue : IBackgroundTaskQueue
{
    private readonly Channel<Func<ValueTask>> _queue;

    public LogTaskQueue(int capacity)
    {
        BoundedChannelOptions options = new(capacity)
        {
            FullMode = BoundedChannelFullMode.Wait
        };

        _queue = Channel.CreateBounded<Func<ValueTask>>(options);
    }

    public async ValueTask QueueBackgroundWorkItemAsync(Func<ValueTask> workItem)
    {
        if (workItem is null) throw new ArgumentNullException("Tasks nulas não são suportadas");

        await _queue.Writer.WriteAsync(workItem);
    }

    public async ValueTask<Func<ValueTask>> DequeueAsync(CancellationToken cancellationToken)
    {
        var workItem = await _queue.Reader.ReadAsync(cancellationToken);

        return workItem;
    }
}
