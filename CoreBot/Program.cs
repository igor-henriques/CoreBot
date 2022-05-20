await Host.CreateDefaultBuilder(args)
    .ConfigureServices((services) =>
    {
        services.AddSingleton<IBackgroundTaskQueue>(_ =>
        {
            return new BackgroundTaskQueue(10_000);
        });

        services.AddSingleton<Definitions>();
        services.AddSingleton<CoreLicense>();
        services.AddSingleton(_ => JsonConvert.DeserializeObject<ServerConnection>(File.ReadAllText("./Configurations/ServerConnection.json")));

        services.AddScoped<IServerRepository, ServerRepository>();

        services.AddHostedService<LicenseControl>();

        services.AddHostedService<FormatlogProducer>();
        services.AddHostedService<LogProducer>();

        services.AddHostedService<FormatlogConsumer>();
        services.AddHostedService<LogWorker>();       
    })
    .Build()
    .RunAsync();