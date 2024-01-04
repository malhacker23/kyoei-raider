using KyoeiRaider.Logging;
using KyoeiRaider.Misc;
using Leaf.xNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KyoeiRaider.Module.Modules
{
    internal class ReactorModule : Module
    {
        public string GetName()
        {
            return "Reaction";
        }

        public void Run()
        {
            Console.WriteLine("注意：リアクションを指定する際は、まずDiscordにて絵文字の先頭に\"\\\"をつけて送信し、");
            Console.WriteLine("絵文字のユニコードを https://tech-unlimited.com/urlencode.html などで変換をしてから指定をしてください");
            Console.WriteLine("例: %F0%9F%98%AD (ユニコード絵文字)\n\r");

            var tokens = Util.OpenTokens();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Tokens: {0}\n\r", tokens.Count);

            Console.ForegroundColor = Util.GetDefaultColor();

            var leftPos = Console.CursorLeft;
            var topPos = Console.CursorTop;
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
            Console.Write("[Message Id] > ");
            var inputmessageId = Console.ReadLine();
            if (!ulong.TryParse(inputmessageId, out ulong messageId))
            {
                Logger.Errer("Invalid Message Id!");
                return;
            }
            Console.CursorLeft = leftPos;
            Console.CursorTop = topPos;
            Console.WriteLine("Message Id: {0}", messageId);

            leftPos = Console.CursorLeft;
            topPos = Console.CursorTop;
            Console.Write("[Reaction] > ");
            var reaction = Console.ReadLine();
            Console.CursorLeft = leftPos;
            Console.CursorTop = topPos;
            Console.WriteLine("Reaction: {0}\n\r", reaction);

            Logger.Info("Running...");
            var tasks = new List<Task>();
            foreach (var token in tokens)
            {
                var task = Task.Run(() => React(token, channelId, messageId, reaction));
                tasks.Add(task);
                Thread.Sleep(300);
            }
            Task.WaitAll(tasks.ToArray());

            Logger.Info("Done!");
        }

        private void React(DiscordToken token, ulong channelId, ulong messageId, string reaction)
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
            request["referer"] = $"https://discord.com/channels/@me";
            request["sec-fetch-dest"] = "empty";
            request["sec-fetch-mode"] = "cors";
            request["sec-fetch-site"] = "same-origin";
            request["x-debug-options"] = "bugReporterEnabled";
            request["x-discord-locale"] = "ja";
            request["x-discord-timezone"] = "Asia/Tokyo";
            request["x-super-properties"] = token.Agent.GetSuperProperties();

            var param = new RequestParams();
            param["location"] = "Message";
            param["type"] = 0;

            var response = request.Put($"https://discord.com/api/v9/channels/{channelId}/messages/{messageId}/reactions/{reaction}/@me", param);
            Console.WriteLine(response.ToString());
        }
    }
}
