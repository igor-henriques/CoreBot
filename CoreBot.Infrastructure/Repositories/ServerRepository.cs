namespace CoreBot.Infrastructure.Repositories;

public class ServerRepository : IServerRepository
{
    private readonly ILogger<ServerRepository> logger;
    private readonly ServerConnection _server;
    public ServerRepository(ILogger<ServerRepository> logger, ServerConnection server)
    {
        this.logger = logger;
        this._server = server;

        PWGlobal.UsedPwVersion = _server.PwVersion;
    }

    public async Task<List<int>> GetOnlineAccountId()
    {
        var onlinePlayers = await Task.Run(() => GMListOnlineUser.Get(_server.GDeliveryd));

        return onlinePlayers?.Select(x => x.UserId)?.ToList();
    }

    public async Task<GRoleData> GetRoleByID(int roleId)
    {
        try
        {
            GRoleData roleData = await Task.Run(() => GetRoleData.Get(_server.Gamedbd, roleId));
            return roleData;
        }
        catch (Exception ex)
        {
            logger.Write(ex.ToString());
        }

        return default;
    }

    public async Task<int> GetUserIDByRoleID(int roleId)
    {
        return await Task.Run(() => GetRoleData.Get(_server.Gamedbd, roleId).GRoleBase.UserId);
    }

    public async Task<GRoleData> GetRoleByName(string characterName)
    {
        try
        {
            GRoleData roleData = await GetRoleByID(GetRoleId.Get(_server.Gamedbd, characterName));
            return roleData;
        }
        catch (Exception ex)
        {
            logger.Write(ex.ToString());
        }

        return default;
    }

    public async Task SendPrivateMessage(int roleId, string message)
    {
        try
        {
            await Task.Run(() => PrivateChat.Send(_server.GDeliveryd, roleId, message));
        }
        catch (Exception e)
        {
            logger.Write(e.ToString());
        }
    }

    public async Task SendMessage(BroadcastChannel channel, string message, int roleId = 0)
    {
        try
        {
            await Task.Run(() => ChatBroadcast.Send(_server.GProvider, channel, message));            
        }
        catch (Exception e)
        {
            logger.Write(e.ToString());
        }
    }

    public async Task<List<GRoleData>> GetRolesFromAccount(int userId)
    {
        try
        {
            List<int> idRoles = await Task.Run(() => GetUserRoles.Get(_server.Gamedbd, userId).Select(x => x.Item1).ToList());

            List<GRoleData> roles = new List<GRoleData>();

            idRoles.ForEach(x => roles.Add(GetRoleData.Get(_server.Gamedbd, x)));

            return roles;
        }
        catch (Exception e)
        {
            logger.Write($"Erro para userID {userId}: {e}");
        }

        return default;
    }

    public async Task<bool> SendMail(int roleId, string title, string message, GRoleInventory item, int coinCount = 0)
    {
        try
        {
            return await Task.Run(() => SysSendMail.Send(_server.GDeliveryd, roleId, title, message, item, coinCount));
        }
        catch (Exception e)
        {
            logger.Write(e.ToString());
        }

        return false;
    }

    public async Task<int> GetRoleIdByName(string characterName)
    {
        try
        {
            return await Task.Run(() => GetRoleId.Get(_server.Gamedbd, characterName));
        }
        catch (Exception e)
        {
            logger.Write(e.ToString());
        }

        return -1;
    }

    public async Task<string> GetRoleNameByID(int roleId)
    {
        return await Task.Run(() => GetRoleBase.Get(_server.Gamedbd, roleId)?.Name);
    }

    public async Task<bool> GiveCash(int accountId, int cashAmount)
    {
        try
        {
            return await Task.Run(() => DebugAddCash.Add(_server.Gamedbd, accountId, cashAmount * 100));
        }
        catch (Exception e)
        {
            logger.Write(e.ToString());
        }

        return false;
    }

    public GRoleInventory GenerateItem(int itemId, int itemCount)
        => new GRoleInventory
        {
            Id = itemId,
            Count = itemCount,
            MaxCount = itemCount,
            Proctype = 0,
        };

    public async Task<int> GetWorldTag(int roleId)
        => await Task.Run(() => GetRoleStatus.Get(_server.Gamedbd, roleId).WorldTag);
}
