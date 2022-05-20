namespace CoreBot.Domain.Models.ServerModels;

public class Gamedbd : IPwDaemonConfig
{
    [JsonProperty("HOST")]
    public string Host { get; set; }

    [JsonProperty("PORT")]
    public int Port { get; set; }

    public Gamedbd(string host, int port)
    {
        Host = host;
        Port = port;
    }
}
