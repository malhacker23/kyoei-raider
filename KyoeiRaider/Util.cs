using KyoeiRaider.Browser;
using KyoeiRaider.Misc;
using Leaf.xNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace KyoeiRaider
{
    internal class Util
    {
        private static Random _rnd = new Random();

        public static int GetRandom(int start, int end)
        {
            return _rnd.Next(start, end);
        }

        public static string GetRandomString(int len)
        {
            const string characters = "abcdefABCDEF1234567890";

            var builder = new StringBuilder();
            for (int i = 0; i < len; i++)
                builder.Append(characters[Util.GetRandom(0, characters.Length)]);

            return builder.ToString();
        }

        public static BrowserAgent GetRandomBrowser()
        {
            return StaticBrowsers.BROWSERS[GetRandom(0, StaticBrowsers.BROWSERS.Length)];
        }

        public static ConsoleColor GetDefaultColor()
        {
            return ConsoleColor.DarkGray;
        }

        public static CookieStorage RetrieveCookies()
        {
            var request = new HttpRequest();

            request["authority"] = "discord.com";
            request["accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7";
            request["accept-language"] = "ja";
            request["sec-ch-ua"] = "\"Opera Crypto\";v=\"105\", \"Chromium\";v=\"119\", \"Not?A_Brand\";v=\"24\"";
            request["sec-ch-ua-mobile"] = "?0";
            request["sec-ch-ua-platform"] = "\"Windows\"";
            request["sec-fetch-dest"] = "document";
            request["sec-fetch-mode"] = "navigate";
            request["sec-fetch-site"] = "none";
            request["sec-fetch-user"] = "?1";
            request["upgrade-insecure-requests"] = "1";
            request["user-agent"] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.6045.199 Safari/537.36";

            request.IgnoreProtocolErrors = false;

            var response = request.Get("https://discord.com/");
            return response.Cookies;
        }

        public static List<DiscordToken> OpenTokens()
        {
            var ofd = new OpenFileDialog();
            ofd.FileName = "tokens.txt";
            ofd.Filter = ".txtファイル|*.txt|*.*|*.*";
            ofd.CheckFileExists = true;
            ofd.ShowDialog();

            var tokens = new List<DiscordToken>();
            foreach (var token in File.ReadAllLines(ofd.FileName))
            {
                tokens.Add(new DiscordToken
                {
                    Token = token.Trim(),
                    Cookies = Util.RetrieveCookies(),
                    Agent = Util.GetRandomBrowser()
                });
                Thread.Sleep(200);
            }

            return tokens;
        }
    }
}
