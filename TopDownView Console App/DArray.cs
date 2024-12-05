using MyApp;
using System;
using System.Threading;
namespace TopDownView_Console_App
{
    public static class DArray
    {
        public static void UpdateArray(ref string[][] array)
        {
            while (true)
            {
                Console.SetCursorPosition(0, 0);
                lock (array)
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        for (int j = 0; j < array[i].Length; j++)
                        {
                            if (array[i][j] == "B") 
                            {
                                Console.ForegroundColor = ConsoleColor.Blue;
                            }
                            else if (array[i][j] == "^") 
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                            }
                            else if (array[i][j] == "&") 
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                            }
                            else 
                            {
                                Console.ForegroundColor = ConsoleColor.Gray;
                            }

                            Console.Write(array[i][j] + " ");
                        }
                        Console.WriteLine();
                    }
                }
                Console.ResetColor();
                Thread.Sleep(50);
            }
        }
    }
}
