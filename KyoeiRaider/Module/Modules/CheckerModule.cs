using KyoeiRaider.Logging;
using Leaf.xNet;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KyoeiRaider.Module.Modules
{
    internal class CheckerModule : Module
    {
        private ConcurrentBag<string> tokens = new ConcurrentBag<string>();

        public string GetName()
        {
            return "Token Checker";
        }

        public void Run()
        {
            var ofd = new OpenFileDialog();
            ofd.FileName = "tokens.txt";
            ofd.Filter = ".txtファイル|*.txt|*.*|*.*";
            ofd.CheckFileExists = true;
            ofd.ShowDialog();

            var list = new List<Task>();
            var lines = File.ReadAllLines(ofd.FileName);
            foreach (var line in lines)
            {
                var task = Task.Run(() => CheckToken(line.Trim()));
                list.Add(task);
                Thread.Sleep(200);
            }
            Task.WaitAll(list.ToArray());

            var sfd = new SaveFileDialog();
            sfd.FileName = "tokens_checked.txt";
            sfd.Filter = ".txtファイル|*.txt|*.*|*.*";
            sfd.ShowDialog();
            File.WriteAllText(sfd.FileName, string.Join("\n", tokens.ToArray()));
        }

        private void CheckToken(string token)
        {
            var cookies = Util.RetrieveCookies();
            var browser = Util.GetRandomBrowser();

            var request = new HttpRequest();
            request["authority"]  ="discord.com";
            request["accept"] = "*/*";
            request["accept-language"] = "ja,en-US;q=0.9,en;q=0.8";
            request["authorization"] = token;
            request["referer"] = "https://discord.com/channels/@me";
            request["sec-fetch-dest"] = "empty";
            request["sec-fetch-mode"] = "cors";
            request["sec-fetch-site"] = "same-origin";
            request["user-agent"] = browser.GetUserAgent();
            request["x-debug-options"]= "bugReporterEnabled";
            request["x-discord-locale"] ="ja";
            request["x-discord-timezone"] = "Asia/Tokyo";
            request["x-super-properties"] = browser.GetSuperProperties();
            request.Cookies = cookies;

            request.IgnoreProtocolErrors = true;

            var response = request.Get("https://discord.com/api/v9/users/@me/billing/payment-sources");
            if (response.IsOK)
            {
                Logger.Info("Valid Token: {0}", token);
                tokens.Add(token);
            }
            else
            {
                Logger.Errer("Invalid Token: {0}", token);
            }
        }
    }
}
