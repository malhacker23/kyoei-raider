using KyoeiRaider.Module.Modules;
using System.Collections.Generic;

namespace KyoeiRaider.Module
{
    internal class ModuleRegistry
    {
        private List<Module> _modules = new List<Module>();

        public void Initialize()
        {
            _modules.Add(new SpammerModule());
            _modules.Add(new ReactorModule());
            _modules.Add(new CheckerModule());
        }

        public List<Module> GetModules()
        {
            return _modules;
        }
    }
}
