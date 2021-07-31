using PWToolKit;

namespace CoreBot.Server
{
    public record ServerConnection
    {
        public GDeliveryd GDeliveryd { get; init; }
        public Gamedbd Gamedbd{ get; init; }
        public PwVersion PwVersion { get; init; }
        public string LogsPath { get; init; }
        public string Webhook { get; init; }

        public ServerConnection(string GDeliverydHost, int GDeliverydPort, string GamedbdHost, int GamedbdPort, PwVersion _PwVersion, string _LogsPath, string _Webhook)
        {
            this.GDeliveryd = new(GDeliverydHost, GDeliverydPort);
            this.Gamedbd = new(GamedbdHost, GamedbdPort);
            this.PwVersion = _PwVersion;
            this.LogsPath = _LogsPath;
            this.Webhook = _Webhook;
        }
    }
}
