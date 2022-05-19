await Host.CreateDefaultBuilder(args)
    .ConfigureServices((services) =>
    {
        services.AddSingleton<IBackgroundTaskQueue>(_ =>
        {
            return new BackgroundTaskQueue(10_000);
        });

        //用户1090丢弃包裹1个60007

        services.AddSingleton<Definitions>();
        services.AddSingleton<CoreLicense>();
        services.AddSingleton(_ => JsonConvert.DeserializeObject<ServerConnection>(File.ReadAllText("./Configurations/ServerConnection.json")));

        services.AddHostedService<LicenseControl>();

        services.AddHostedService<FormatlogProducer>();
        services.AddHostedService<LogProducer>();

        services.AddHostedService<FormatlogConsumer>();
        services.AddHostedService<LogWorker>();       
    })
    .Build()
    .RunAsync();