using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KyoeiRaider.Browser.Browsers
{
    internal class FireFoxBrowser : BrowserAgent
    {
        public string GetSuperProperties()
        {
            var json = new
            {
                os = "Windows",
                browser = "Firefox",
                device = "",
                system_locale = "ja",
                browser_user_agent = GetUserAgent(),
                browser_version = "121.0",
                os_version = "10",
                referrer = "",
                referring_domain = "",
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
            return "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:121.0) Gecko/20100101 Firefox/121.0";
        }
    }
}
