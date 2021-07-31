using PWToolKit.Packets;

namespace CoreBot.Server
{
    public class Gamedbd : IPwDaemonConfig
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public Gamedbd(string host, int port)
        {
            Host = host;
            Port = port;
        }
    }
}
