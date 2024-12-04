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
            int moveCount = 0; //later make it so the enemy can moove every seccond turn

            bool foundBullet = false;
            bool enemyActive = true;


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
                array[0][1] = $"{eRow}";

                

                if (Console.KeyAvailable)
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        for (int j = 0; j < array.Length; j++)
                        {
                            if (array[j][i] == "^")
                            {
                                BulletR = j;
                                BulletC = i;
                                foundBullet = true;
                                break;
                            }
                        }
                    }
                    if (!foundBullet)
                    {
                        array[0][2] = "N";
                    }

                    int test = rnd.Next(0, array.Length);
                    int test2 = rnd.Next(0, array.Length);


                    var key = Console.ReadKey(intercept: true).Key;

                    if (key == ConsoleKey.Backspace)
                        throw new Exception("Game stopped with backspace");
                    
                    for (int i = 0; i < array.Length; i++)
                        for (int j = 0; j < array.Length; j++)
                            if (array[i][j] == "B") { pRow = i; pCol = j; }

                    array[pRow][pCol] = ".";

                    if (key == ConsoleKey.W) { array[pRow - 1][pCol] = "B"; array[0][0] = $"{pRow}";}
                    else if (key == ConsoleKey.S) { array[pRow + 1][pCol] = "B"; array[0][0] = $"{pRow}"; }
                    else if (key == ConsoleKey.A) { array[pRow][pCol - 1] = "B"; array[0][0] = $"{pRow}"; }
                    else if (key == ConsoleKey.D) { array[pRow][pCol + 1] = "B"; array[0][0] = $"{pRow}"; }
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
                                array[0][2] = $"{BulletR}";
                                if (BulletR == eRow && BulletC == eCol)
                                {
                                    array[0][3] = "Winner";
                                }
                                array[BulletR][BulletC] = ".";
                                BulletR--;
                                if (BulletR >= 0) array[BulletR][BulletC] = "^";
                            }
                        }
                        array[BulletR][BulletC] = ".";
                    }
                    enemyActive = false;  
                    if (true)
                    {
                       
                        if (enemyActive)
                        {

                            if (BulletR == eRow && BulletC == eCol)
                            {
                                array[3][3] = "Dead";
                            }
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
                    }

                }

                

            }
            Thread.Sleep(50);
        }
                
            }
        }
    

