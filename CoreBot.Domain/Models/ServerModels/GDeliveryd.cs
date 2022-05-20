namespace CoreBot.Domain.Models.ServerModels;

public class GDeliveryd : IPwDaemonConfig
{
    [JsonProperty("HOST")]
    public string Host { get; set; }

    [JsonProperty("PORT")]
    public int Port { get; set; }

    public GDeliveryd(string host, int port)
    {
        Host = host;
        Port = port;
    }
}
