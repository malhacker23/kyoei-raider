using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KyoeiRaider.Logging
{ 
    internal class Logger
    {
        private static ConcurrentQueue<LoggerTask> _tasks = new ConcurrentQueue<LoggerTask>();

        public static void Initialize()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    while (_tasks.TryDequeue(out var result))
                    {
                        DisplayLevel(result.Level);
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine(result.Message);
                    }
                    Thread.Sleep(10);
                }
            });
        }

        public static void Info(string message, params object[] args)
        {
            Log(LogLevel.Info, message, args);
        }

        public static void Errer(string message, params object[] args)
        {
            Log(LogLevel.Errer, message, args);
        }

        private static void Log(LogLevel level, string message, params object[] args)
        {
            _tasks.Enqueue(new LoggerTask
            {
                Level = level,
                Message = string.Format(message, args)
            });
        }

        private static void DisplayLevel(LogLevel level)
        {
            var color = ConsoleColor.Gray;
            switch (level)
            {
                case LogLevel.Info: color = ConsoleColor.Blue; break;
                case LogLevel.Errer: color = ConsoleColor.DarkRed; break;
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("[");
            Console.ForegroundColor = color;
            Console.Write(level.ToString());
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("] ");
        }
    }
}
