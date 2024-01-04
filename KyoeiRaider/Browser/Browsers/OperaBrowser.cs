using Newtonsoft.Json;
using System;
using System.Text;

namespace KyoeiRaider.Browser.Browsers
{
    internal class OperaBrowser : BrowserAgent
    {
        public string GetSuperProperties()
        {
            var json = new
            {
                os = "Windows",
                browser = "Chrome",
                device = "",
                system_locale = "ja",
                browser_user_agent = GetUserAgent(),
                browser_version = "119.0.6045.199",
                os_version = "10",
                referrer = "https://appeals.wickbot.com/",
                referring_domain = "appeals.wickbot.com",
                referrer_current = "",
                referring_domain_current = "",
                release_channel = "stable",
                client_build_number = 256231,
                client_event_source = (string)null
            };

            var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(json));
            return Convert.ToBase64String(bytes);
        }

        public string GetUserAgent()
        {
            return "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.6045.199 Safari/537.36";
        }
    }
}
