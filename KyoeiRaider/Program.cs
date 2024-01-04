using KyoeiRaider.Logging;
using System;
using System.Runtime.InteropServices;

namespace KyoeiRaider
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Logger.Initialize();
            Console.ForegroundColor = Util.GetDefaultColor();
            Console.Title = "Kyoei Raider v1.0";

            var menu = new Menu();
            menu.Initialize();
            while (true)
            {
                menu.Display();
                Console.Clear();
            }
        }
    }
}
