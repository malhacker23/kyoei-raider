using KyoeiRaider.Browser;
using KyoeiRaider.Logging;
using KyoeiRaider.Misc;
using Leaf.xNet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace KyoeiRaider.Module.Modules
{
    internal class SpammerModule : Module
    {
        private string _latestNonce = "";

        public string GetName()
        {
            return "Spammer";
        }

        public void Run()
        {
            var tokens = Util.OpenTokens();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Tokens: {0}\n\r", tokens.Count);

            Console.ForegroundColor = Util.GetDefaultColor();

            var leftPos = Console.CursorLeft;
            var topPos = Console.CursorTop;
            Console.Write("[Guild Id] > ");
            var inputGuildId = Console.ReadLine();
            if (!ulong.TryParse(inputGuildId, out ulong guildId))
            {
                Logger.Errer("Invalid Guild Id!");
                return;
            }
            Console.CursorLeft = leftPos;
            Console.CursorTop = topPos;
            Console.WriteLine("Guild Id: {0}", guildId);

            leftPos = Console.CursorLeft;
            topPos = Console.CursorTop;
            Console.Write("[Channel Id] > ");
            var inputChannelId = Console.ReadLine();
            if (!ulong.TryParse(inputChannelId, out ulong channelId))
            {
                Logger.Errer("Invalid Channel Id!");
                return;
            }
            Console.CursorLeft = leftPos;
            Console.CursorTop = topPos;
            Console.WriteLine("Channel Id: {0}", channelId);

            leftPos = Console.CursorLeft;
            topPos = Console.CursorTop;
            Console.Write("[Messages] > ");
            var ofd = new OpenFileDialog();
            ofd.FileName = "messages.txt";
            ofd.Filter = ".txtファイル|*.txt|*.*|*.*";
            ofd.CheckFileExists = true;
            ofd.ShowDialog();
            var messages = File.ReadAllLines(ofd.FileName);
            Console.CursorLeft = leftPos;
            Console.CursorTop = topPos;
            Console.WriteLine("Messages: {0} => {1} Lines", Path.GetFileName(ofd.FileName), messages.Length);

            leftPos = Console.CursorLeft;
            topPos = Console.CursorTop;
            Console.Write("[Delay (sec)] > ");
            var inputDelay = Console.ReadLine();
            if (!float.TryParse(inputDelay, out float delay))
            {
                Logger.Errer("Invalid Value!");
                return;
            }
            Console.CursorLeft = leftPos;
            Console.CursorTop = topPos;
            Console.WriteLine("Delay: {0}", inputDelay);

            var running = true;
            void RunSpammer()
            {
                while (running)
                {
                    var token = tokens[Util.GetRandom(0, tokens.Count)];
                    var message = messages[Util.GetRandom(0, messages.Length)];
                    Task.Run(() => Spam(token, guildId, channelId, message, true));
                    Thread.Sleep((int)(delay * 1000));
                }
            }

            Task.Run(RunSpammer);
            Console.WriteLine("\n\r<<<Press any key to stop>>>");
            Console.ReadKey();

            running = false;
        }

        private void Spam(DiscordToken token, ulong guildId, ulong channelId, string message, bool suffix)
        {
            var request = new HttpRequest();

            request.IgnoreProtocolErrors = true;
            request.Cookies = token.Cookies;
            request.UserAgent = token.Agent.GetUserAgent();

            request["authority"] = "discord.com";
            request["accept"] = "*/*";
            request["accept-language"] = "ja,en-US;q=0.9,en;q=0.8";
            request["authorization"] = token.Token;
            request["origin"] = "https://discord.com";
            request["referer"] = $"https://discord.com/channels/{guildId}/{channelId}";
            request["sec-fetch-dest"] = "empty";
            request["sec-fetch-mode"] = "cors";
            request["sec-fetch-site"] = "same-origin";
            request["x-debug-options"] = "bugReporterEnabled";
            request["x-discord-locale"] = "ja";
            request["x-discord-timezone"] = "Asia/Tokyo";
            request["x-super-properties"] = token.Agent.GetSuperProperties();

            var json = new
            {
                mobile_network_type = "unknown",
                content = message + (suffix ? " " + Util.GetRandomString(8) : string.Empty),
                nonce = Util.GetRandom(100000000, 999999999),
                flags = "0",
                tts = false

            };
            var resposne = 
                request.Post($"https://discord.com/api/v9/channels/{channelId}/messages", JsonConvert.SerializeObject(json), "application/json");
        }
    }

}
