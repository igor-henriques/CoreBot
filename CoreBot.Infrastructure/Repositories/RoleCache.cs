namespace CoreBot.Infrastructure.Repositories;

public class RoleCache : BaseCacheRecord<Role>, IBaseCache
{
    private readonly Dictionary<long, RoleCache> roleCache;

    private readonly Timer timeoutVerifier;

    public RoleCache()
    {
        this.roleCache = new Dictionary<long, RoleCache>();
        this.timeoutVerifier = new Timer((obj) => TimeoutVerifier(), null, 0, 60_000);
    }

    private void TimeoutVerifier()
    {
        foreach (var item in roleCache)
        {
            if (item.Value.ExpireDate >= DateTime.Now)
                roleCache.Remove(item.Key);
        }
    }

    public void Insert(Role role)
    {
        if (role is null)
            return;

        if (!roleCache.TryGetValue(role.Id, out RoleCache cachedRole))
        {
            this.roleCache.Add(role.Id, cachedRole = BuildCacheModel(role));
        }

        cachedRole.ExpireDate = DateTime.Now;
    }

    public void Remove(Role role)
    {
        if (role is null)
            return;

        if (roleCache.TryGetValue(role.Id, out RoleCache cacheRole))
        {
            roleCache.Remove(role.Id);
        }
    }

    private RoleCache BuildCacheModel(Role role)
    {
        return new RoleCache
        {
            ExpireDate = DateTime.Now.AddDays(1),
            Record = role
        };
    }

    public Role Get(long id)
    {
        if (!roleCache.TryGetValue(id, out RoleCache cacheRole))
        {
            return null;
        }

        return cacheRole.Record;
    }
}
