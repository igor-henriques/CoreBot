using PWToolKit.Packets;

namespace CoreBot.Server
{
    public class GDeliveryd : IPwDaemonConfig
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public GDeliveryd(string host, int port)
        {
            Host = host;
            Port = port;
        }
    }
}
