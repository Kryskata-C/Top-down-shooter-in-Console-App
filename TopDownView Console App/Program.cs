using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using TopDownView_Console_App;

namespace MyApp
{
    public class Program
    {
       public int updateCounter = 50;

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
            Enemy enemy = new Enemy();
            enemy.Create(array);
            int eRow = enemy.enemyR; 
            int eCol = enemy.enemyC;
            int countForEndScreen = 0;
            //enemy checkup
            for (int i = 0; i < array.Length; i++)
            {
                for (int j = 0; j < array[i].Length; j++)
                {
                    if (array[i][j] == "&")
                    {
                        eCol = j;
                        eRow = i;
                        break;
                    }
                }
            }
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

                    if (true)
                    {
                        array[0][0] = $"{pRow}";
                        array[0][1] = $"{eRow}";
                        if (eRow > pRow)
                        {

                            array[eRow][eCol] = ".";
                            array[eRow - 1][eCol] = "&";
                            eRow--;
                        }
                        else if (eRow < pRow)
                        {

                            array[eRow][eCol] = ".";
                            array[eRow + 1][eCol] = "&";
                            eRow++;
                        }

                        if (eCol > pCol)
                        {
                            array[eRow][eCol] = ".";
                            array[eRow][eCol - 1] = "&";
                            eCol--;
                        }
                        else if (eCol < pCol)
                        {
                            array[eRow][eCol] = ".";
                            array[eRow][eCol + 1] = "&";
                            eCol++;
                        }

                        if (eRow == pRow && eCol == pCol)
                        {
                            for (int i = 0; i < array.Length;)
                            {
                                if (countForEndScreen == 0)
                                {
                                    array[0][array.Length / 2] = "L";
                                    countForEndScreen++;
                                }
                                if (countForEndScreen == 1)
                                {
                                    array[0][array.Length / 2 + 1] = "O";
                                    countForEndScreen++;
                                }
                                if (countForEndScreen == 2)
                                {
                                    array[0][array.Length / 2 + 2] = "O";
                                    countForEndScreen++;
                                }
                                if (countForEndScreen == 3)
                                {
                                    array[0][array.Length / 2 + 3] = "S";
                                    countForEndScreen++;
                                }
                                if (countForEndScreen == 4)
                                {
                                    array[0][array.Length / 2 + 4] = "E";
                                    countForEndScreen++;
                                }
                                if (countForEndScreen == 5)
                                {
                                    array[0][array.Length / 2 + 5] = "R";
                                    countForEndScreen = 0;
                                }


                            }
                        }
                        Debug.WriteLine(eCol + pCol);
                    }

                }

                

            }
            Thread.Sleep(50);
        }
                
            }
        }
    

