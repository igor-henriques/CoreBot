using CoreBot.Data;
using CoreBot.Model;
using CoreBot.Server;
using PWToolKit;
using PWToolKit.API.Gamedbd;
using PWToolKit.API.GDeliveryd;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;

namespace CoreBot.Watcher
{
    public class WorldChatWatch
    {
        static private long lastSize;
        private static string path;
        private static ServerConnection _server;
        private static Timer chatWatch;

        public WorldChatWatch(ServerConnection serverConnection)
        {
            _server = serverConnection;
            PWGlobal.UsedPwVersion = _server.PwVersion;

            path = Path.Combine(_server.LogsPath, "world2.chat");
            lastSize = GetFileSize(path);

            chatWatch = new Timer(500);
            chatWatch.Elapsed += chatWatch_Elapsed;
            chatWatch.Start();
        }

        private void chatWatch_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                long fileSize = GetFileSize(path);

                if (fileSize > lastSize)
                {
                    var messages = ReadTail(path, UpdateLastFileSize(fileSize));
                    messages.ForEach(message => Discord.SendMessage(message, TextToPlayer, _server));
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write(ex.ToString());
            }
        }      
        
        private static List<Message> ReadTail(string filename, long offset)
        {
            List<Message> messages = new List<Message>();

            byte[] bytes;
            using (FileStream fs = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                fs.Seek(offset * -1, SeekOrigin.End);

                bytes = new byte[offset];
                fs.Read(bytes, 0, (int)offset);
            }

            List<string> logs = Encoding.Default.GetString(bytes).Split(new string[] { "\n" }[0]).Where(x => !string.IsNullOrEmpty(x.Trim())).ToList();

            logs.ForEach(log => messages.Add(ProcessLog(log)));

            return messages.Where(x => x is not null).ToList();
        }

        private static void TextToPlayer(int roleId)
        {
            PrivateChat.Send(_server.GDeliveryd, roleId, "Seu feedback foi enviado ao nosso canal do Discord e será analisado assim que possível. Obrigado por contribuir!");
        }

        private static Message ProcessLog(string encodedMessage)
        {
            if (!encodedMessage.Contains("src=-1"))
            {
                string message = Encoding.Unicode.GetString(Convert.FromBase64String(System.Text.RegularExpressions.Regex.Match(encodedMessage, @"msg=([\s\S]*)").Value.Replace("msg=", "")));

                //Gera registro se a mensagem contiver a key "corebot" e se o conteúdo da mensagem (excluindo a key) for maior que 3 caracteres.
                if (message.ToLower().Trim().Contains(_server.Trigger) && message.ToLower().Trim().Replace(_server.Trigger, string.Empty).Length > 3)
                {
                    int roleId = int.Parse(System.Text.RegularExpressions.Regex.Match(encodedMessage, @"src=([0-9]*)").Value.Replace("src=", "").Trim());
                    string roleName = GetRoleData.Get(_server.Gamedbd, roleId).GRoleBase.Name;

                    return new Message
                    {
                        RoleID = roleId,
                        RoleName = roleName,
                        Content = message
                    };
                }                
            }

            return null;
        }

        private static long UpdateLastFileSize(long fileSize)
        {
            long difference = fileSize - lastSize;
            lastSize = fileSize;

            return difference;
        }

        private static long GetFileSize(string fileName)
        {
            return new System.IO.FileInfo(fileName).Length;
        }
    }
}