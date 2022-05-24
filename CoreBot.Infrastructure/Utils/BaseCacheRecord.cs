namespace CoreBot.Infrastructure.Utils;

public abstract class BaseCacheRecord<T>
{
    public DateTime ExpireDate { get; set; }
    public T Record { get; set; }
}
