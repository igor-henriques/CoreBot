namespace CoreBot.Infrastructure.Interfaces;

public interface IBaseCache
{
    void Insert(Role role);
    void Remove(Role role);
    Role Get(long id);
}