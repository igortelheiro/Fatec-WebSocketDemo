using System;

namespace WebSocketDemo.Common
{
    public static class Logger
    {
        public static void Log(string text)
        {
            Console.WriteLine(DateTime.Now + $" {text}");
        }
    }
}
