﻿namespace CoreBot.Domain.Factories;

public class LogModelsFactory : ILogModelsFactory
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
        if (!int.TryParse(Regex.Match(log, @"用户([0-9]*)").Value.Replace("用户", ""), out int roleId))
        {
            return null;
        }

        var droppedBy = await serverContext.GetRoleByID(roleId);
        int.TryParse(Regex.Match(log, @"个([0-9]*)").Value.Replace("个", ""), out int itemId);
        int.TryParse(Regex.Match(log, @"丢弃包裹([0-9]*)").Value.Replace("丢弃包裹", ""), out int itemAmount);

        var model = new ItemDropped
        {
            Amount = itemAmount,
            DroppedBy = droppedBy is null ? null : Role.ToRole(droppedBy),
            Date = DateTime.Now,
            ItemId = itemId
        };

        logger.Write($"Log de item jogado no chão construído: {model}");

        return model;
    }

    public async Task<ItemPickedup> BuildItemPickedupModel(string log)
    {
        if (!int.TryParse(Regex.Match(log, @"用户([0-9]*)").Value.Replace("用户", ""), out int pickedupByRoleId))
        {
            return null;
        }

        var pickedupBy = await serverContext.GetRoleByID(pickedupByRoleId);

        if (!int.TryParse(Regex.Match(log, @"(\[用户([0-9]*))").Value.Replace("[用户", ""), out int droppedByRoleId))
        {
            return null;
        }

        var droppedBy = await serverContext.GetRoleByID(droppedByRoleId);

        int.TryParse(Regex.Match(log, @"个([0-9]*)").Value.Replace("个", ""), out int itemId);
        int.TryParse(Regex.Match(log, @"拣起([0-9]*)").Value.Replace("拣起", ""), out int itemAmount);

        var model = new ItemPickedup
        {
            Amount = itemAmount,
            PickedupBy = pickedupBy is null ? null : Role.ToRole(pickedupBy),
            DroppedBy = droppedBy is null ? null : Role.ToRole(droppedBy),
            Date = DateTime.Now,
            ItemId = itemId
        };

        logger.Write($"Log de item coletado do chão construído: {model}");

        return model;
    }

    public async Task<Chat> BuildChatModel(string log)
    {
        if (!int.TryParse(Regex.Match(log, @"src=([0-9]*)").Value.Replace("src=", ""), out int roleId))
        {
            return null;
        }

        var role = await serverContext.GetRoleByID(roleId);

        var content = Regex.Match(log, @"msg=(.*)").Value.Replace("msg=", "");

        var model = new Chat
        {
            SentFrom = role is null ? null : Role.ToRole(role),
            Content = content.DecodeBase64(),
            Date = DateTime.Now,
        };

        logger.Write($"Log de chat construído: {model}");

        return model;
    }
}
