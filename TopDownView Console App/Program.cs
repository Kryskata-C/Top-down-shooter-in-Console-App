using System;
using System.Threading;
using System.Threading.Tasks;
using TopDownView_Console_App;

namespace MyApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random();

            string[][] array = new string[20][];

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = new string[20];
                for (int j = 0; j < array[i].Length; j++)
                {
                    array[i][j] = ".";
                }
            }

            _ = Task.Run(() => DArray.UpdateArray(ref array));

            Thread.Sleep(50);

            lock (array)
            {
                Array.Resize(ref array, array.Length);
            }
            int pRow = new int();
            int pCol = new int();
            int BulletR = new int();
            int BulletC = new int();
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    int test = rnd.Next(0, array.Length);
                    int test2 = rnd.Next(0, array.Length);


                    var key = Console.ReadKey(intercept: true).Key;

                    if (key == ConsoleKey.Backspace)
                        break;
                    else if (key == ConsoleKey.Enter)
                    {
                        lock (array)
                        {

                            array[array.Length / 2][array.Length / 2] = "B";
                            
                            for (int i = 0; i < array.Length; i++)
                            {
                                for (int j = 0; j < array.Length; j++)
                                {
                                    if (array[i][j] == "B")
                                    {
                                        pRow = i;
                                        pCol = j;
                                    }
                                    else if (array[i][j] == "^")
                                    {
                                        BulletR = i;
                                        BulletC = j;
                                    }
                                }
                            }

                        }
                    }
                    for (int i = 0; i < array.Length; i++)
                        for (int j = 0; j < array.Length; j++)
                            if (array[i][j] == "B") { pRow = i; pCol = j; }

                    array[pRow][pCol] = ".";

                    if (key == ConsoleKey.W) array[pRow - 1][pCol] = "B";
                    else if (key == ConsoleKey.S) array[pRow + 1][pCol] = "B";
                    else if (key == ConsoleKey.A) array[pRow][pCol - 1] = "B";
                    else if (key == ConsoleKey.D) array[pRow][pCol + 1] = "B";
                    else if (key == ConsoleKey.Q)
                    {
                        array[pRow - 1][pCol] = "^";
                        array[pRow][pCol] = "B";
                        BulletR = pRow - 1;
                        BulletC = pCol;

                        while (BulletR > 0)
                        {
                            Thread.Sleep(50); 
                            lock (array)
                            {
                                array[BulletR][BulletC] = "."; 
                                BulletR--; 
                                if (BulletR >= 0) array[BulletR][BulletC] = "^"; 
                            }
                        }
                        array[BulletR][BulletC] = "."; 
                    }

                }

            }
            Thread.Sleep(50);
        }
                
            }
        }
    

