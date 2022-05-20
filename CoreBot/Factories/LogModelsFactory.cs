namespace CoreBot.Domain.Factories;

public class LogModelsFactory
{
    private readonly ILogger<LogModelsFactory> logger;
    private readonly IServerRepository serverContext;

    public LogModelsFactory(ILogger<LogModelsFactory> logger, IServerRepository serverContext)
    {
        this.logger = logger;
        this.serverContext = serverContext;
    }    

    public async Task<ItemDropped> BuildItemDroppedModel(string log)
    {
        return new ItemDropped();
    }
}
