namespace CoreBot.Domain.Interfaces;

public interface IBackgroundTaskQueue
{
    ValueTask QueueBackgroundWorkItemAsync(Func<ValueTask> workItem);

    ValueTask<Func<ValueTask>> DequeueAsync(CancellationToken cancellationToken);
}
