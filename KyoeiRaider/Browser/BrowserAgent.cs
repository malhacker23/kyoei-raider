using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KyoeiRaider.Browser
{
    internal interface BrowserAgent
    {
        string GetSuperProperties();

        string GetUserAgent();
    }
}
