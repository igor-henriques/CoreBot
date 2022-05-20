namespace CoreBot.Infrastructure.Interfaces;

public interface IServerRepository
{
    Task<int> GetWorldTag(int roleId);
    Task SendMessage(BroadcastChannel channel, string message, int roleId = 0);
    Task SendPrivateMessage(int roleId, string message);
    Task<GRoleData> GetRoleByID(int roleId);
    Task<GRoleData> GetRoleByName(string characterName);
    Task<int> GetRoleIdByName(string characterName);
    Task<List<GRoleData>> GetRolesFromAccount(int userId);
    Task<bool> SendMail(int roleId, string title, string message, GRoleInventory item, int coinCount);
    Task<string> GetRoleNameByID(int roleId);
    Task<bool> GiveCash(int accountId, int cashAmount);
    Task<List<int>> GetOnlineAccountId();
    GRoleInventory GenerateItem(int itemId, int itemCount);
    Task<int> GetUserIDByRoleID(int roleId);
}
