namespace CoreBot.Infrastructure.Utils;

public static class GetLogOperation
{
    public static ELogOperation GetOperation(this string log)
    {
        return log switch
        {
            string _log when _log.Contains("丢弃包裹") => ELogOperation.DropItem,
            string _log when _log.Contains("丢弃]") => ELogOperation.PickupItem,
            string _log when _log.Contains("Chat:") & _log.Contains("chl=1") => ELogOperation.Chat,
            _ => ELogOperation.None
        };
    }
}
