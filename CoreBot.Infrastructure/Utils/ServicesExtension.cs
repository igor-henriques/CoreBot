namespace CoreBot.Infrastructure.Utils;

public static class ServicesExtension
{
    public static U GetService<U, T>(this IServiceProvider services) where T : U
    {
        foreach (var task in services.GetServices(typeof(U)))
        {
            if (task.GetType() == typeof(T))
                return (T)task;
        }

        return default(U);
    }
}
