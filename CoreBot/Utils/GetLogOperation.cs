namespace CoreBot.Utils;

public static class GetLogOperation
{
    public static ELogOperation GetOperation(this string log)
    {
        return log switch
        {
            string _log when _log.Contains("丢弃包裹") => ELogOperation.PickupItem,
            _ => ELogOperation.None
        };
    }
}
