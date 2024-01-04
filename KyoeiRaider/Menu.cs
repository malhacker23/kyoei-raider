using KyoeiRaider.Logging;
using KyoeiRaider.Module;
using System;

namespace KyoeiRaider
{
    internal class Menu
    {
        private static string KYOEI_LOGO = " ____  __.                    .__  __________        .__    .___            \r\n|    |/ _|___.__. ____   ____ |__| \\______   \\_____  |__| __| _/___________ \r\n|      < <   |  |/  _ \\_/ __ \\|  |  |       _/\\__  \\ |  |/ __ |/ __ \\_  __ \\\r\n|    |  \\ \\___  (  <_> )  ___/|  |  |    |   \\ / __ \\|  / /_/ \\  ___/|  | \\/\r\n|____|__ \\/ ____|\\____/ \\___  >__|  |____|_  /(____  /__\\____ |\\___  >__|   \r\n        \\/\\/                \\/             \\/      \\/        \\/    \\/       ";
        private static int CURSOR_SIZE = 20;

        private ModuleRegistry _moduleRegistry;

        public void Initialize()
        {
            _moduleRegistry = new ModuleRegistry();

            _moduleRegistry.Initialize();
        }

        public void Display()
        {
            DisplayLogo();

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("========= <Select Raider> =========");
            for (int i = 0; i < _moduleRegistry.GetModules().Count; i++)
            {
                Console.WriteLine("| [{0}] {1}", i, _moduleRegistry.GetModules()[i].GetName());
            }
            Console.WriteLine("===================================");
            Console.Write("\n\r> ");
            var result = Console.ReadLine();
            Console.ForegroundColor = Util.GetDefaultColor();
            
            if (!int.TryParse(result, out int index) || index < 0 || index >= _moduleRegistry.GetModules().Count)
            {
                Logger.Errer("Invalid Index!");
                return;
            }

            Console.Clear();
            DisplayLogo();

            try
            {
                _moduleRegistry.GetModules()[index].Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.WriteLine();
            Logger.Info("Press any key to continue...");
            Console.ReadKey();
        }

        private void DisplayLogo()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.CursorSize = 10;
            Console.WriteLine(KYOEI_LOGO);
            Console.CursorSize = CURSOR_SIZE;
            Console.ForegroundColor = Util.GetDefaultColor();
            Console.WriteLine();
        }
    }
}
