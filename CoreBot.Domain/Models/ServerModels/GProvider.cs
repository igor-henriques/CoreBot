namespace CoreBot.Domain.Models.ServerModels;

public class GProvider : IPwDaemonConfig
{
    [JsonProperty("HOST")]
    public string Host { get; set; }

    [JsonProperty("PORT")]
    public int Port { get; set; }

    public GProvider(string host, int port)
    {
        Host = host;
        Port = port;
    }
}
