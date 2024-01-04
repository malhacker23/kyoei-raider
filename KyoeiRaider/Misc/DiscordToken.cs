using KyoeiRaider.Browser;
using Leaf.xNet;

namespace KyoeiRaider.Misc
{
    internal class DiscordToken
    {
        public string Token { get; set; }

        public CookieStorage Cookies { get; set; }

        public BrowserAgent Agent { get; set; }
    }
}
