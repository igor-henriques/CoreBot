namespace CoreBot.Interfaces;

public interface ILogModelsFactory
{
    Task<ItemDropped> BuildItemDroppedModel(string log);
    Task<ItemPickedup> BuildItemPickedupModel(string log);
    Task<Chat> BuildChatModel(string log);
}
