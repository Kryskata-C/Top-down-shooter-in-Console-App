using MyApp;
using System;
using System.Threading;
using TopDownView_Console_App;
namespace TopDownView_Console_App
{

    public static class DArray
    {
        public static void UpdateArray(ref string[][] array)
        {
            Console.Clear();
            while (true)
            {
                Console.SetCursorPosition(0, 0);
                lock (array)
                {
                    foreach (var row in array)
                    {
                        Console.WriteLine(string.Join(" ", row ?? Array.Empty<string>()));
                    }
                }
                Thread.Sleep(50);
            }
        }
    }
}
