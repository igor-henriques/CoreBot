﻿using CoreBot.Model;
using CoreBot.Server;
using System.Collections.Specialized;
using System.Net;

namespace CoreBot
{
    public class Discord
    {
        public delegate void TextToPlayer(int roleId);

        /// <summary>
        /// Envia mensagem a um determinador canal do Discord.
        /// </summary>
        /// <param name="message">Mensagem a ser enviada ao canal especificado em arquivo externo</param>
        public static void SendMessage(Message message, TextToPlayer textToPlayer, ServerConnection _server)
        {
            using (WebClient wc = new())
            {
                wc.UploadValuesAsync(new(_server.Webhook), new NameValueCollection()
                {
                    { "username", "CoreBot" },
                    { "content", $"{message.RoleName} enviou uma mensagem!\n{message.Content.Substring(_server.Trigger.Length).Trim()}" }
                });

                textToPlayer(message.RoleID);
            };
        }
    }
}
