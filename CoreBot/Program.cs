using CoreBot.License;
using CoreBot.Server;
using CoreBot.Watcher;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PWToolKit;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;


namespace CoreBot
{
    public class Program
    {
        private static ManualResetEvent quitEvent = new ManualResetEvent(false);
        static ServerConnection ServerConnection;
        static LicenseControl license = new LicenseControl();

        public static async Task Main() => await Run();

        private static async Task Run()
        {
            await InitializePrefs();

            _ = new WorldChatWatch(ServerConnection);            

            Stop();
        }

        private static async Task InitializePrefs()
        {
            Console.WriteLine("CHECANDO PROCESSOS PRÉ-EXISTENTES");
            var count = CheckProcess();
            Console.WriteLine($"Foram encontrados e finalizados {count} processos\n");

            Console.WriteLine("INICIALIZANDO SISTEMA DE LICENÇA\n");
            CoreLicense licenseConfigs = JsonConvert.DeserializeObject<CoreLicense>(await File.ReadAllTextAsync("./Configurations/License.json"));
            await license.Start(licenseConfigs.User, licenseConfigs.Licensekey, licenseConfigs.Product);

            Console.WriteLine("INICIALIZANDO CONFIGURAÇÕES DE SERVIDOR\n\n");
            JObject jsonServerConfig = (JObject)JsonConvert.DeserializeObject(await File.ReadAllTextAsync("./Configurations/ServerConnection.json"));
            ServerConnection = LoadServerConfig(jsonServerConfig);

            Console.Write("MÓDULOS INICIALIZADOS COM SUCESSO\n");
            Console.WriteLine("REPORT BUG: Ironside#3862");
        }

        private static ServerConnection LoadServerConfig(JObject jsonNodes)
        {
            ServerConnection ServerConnection = new ServerConnection
            (                
                jsonNodes["GDELIVERYD"]["HOST"].ToObject<string>(),
                jsonNodes["GDELIVERYD"]["PORT"].ToObject<int>(),
                jsonNodes["GAMEDBD"]["HOST"].ToObject<string>(),
                jsonNodes["GAMEDBD"]["PORT"].ToObject<int>(),
                (PwVersion)jsonNodes["PW_VERSION"].ToObject<int>(),
                jsonNodes["LOGS_PATH"].ToObject<string>(),
                jsonNodes["WEBHOOK"].ToObject<string>()
            );

            return ServerConnection;
        }
        private static int CheckProcess()
        {
            Process p = Process.GetCurrentProcess();
            var ProcessesList = Process.GetProcessesByName(p.ProcessName);
            int pCount = 0;

            for (int i = 0; i < ProcessesList.Length - 1; i++)
            {
                if (!ProcessesList[i].Equals(p))
                {
                    ProcessesList[i].Kill();
                    pCount++;
                }
            }

            return pCount;
        }
        private static void Stop()
        {
            Console.CancelKeyPress += (sender, eArgs) =>
            {
                quitEvent.Set();
                eArgs.Cancel = true;
            };

            quitEvent.WaitOne();
        }
    }
}