namespace CoreBot.Domain.Interfaces;

public interface ILogModelsFactory
{
    Task<Chat> BuildChatModel(string log);
    Task<ItemDropped> BuildItemDroppedModel(string log);
    Task<ItemPickedup> BuildItemPickedupModel(string log);
}
