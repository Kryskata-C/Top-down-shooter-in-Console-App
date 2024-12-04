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
            bool bulletIsBeingShot = false;

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

                //player checkup
                for (int i = 0; i < array.Length; i++)
                {
                    for (int j = 0; j < array[i].Length; j++)
                    {
                        if (array[i][j] == "B")
                        {
                            pCol = j;
                            pRow = i;
                            array[0][0] = $"{pRow}"; 
                            break;
                        }
                    }
                }

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
                        array[0][2] = "NAN";
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

                    if (key == ConsoleKey.W) { array[pRow - 1][pCol] = "B"; pRow--; }
                    else if (key == ConsoleKey.S) { array[pRow + 1][pCol] = "B"; pRow++; }
                    else if (key == ConsoleKey.A) { array[pRow][pCol - 1] = "B"; pCol--;  }
                    else if (key == ConsoleKey.D) { array[pRow][pCol + 1] = "B"; pCol++; }
                    else if (key == ConsoleKey.Q && !bulletIsBeingShot)
                    {
                        bulletIsBeingShot = true;

                        lock (array)
                        {
                            array[pRow - 1][pCol] = "^";
                            array[pRow][pCol] = "B"; 
                        }

                        int bulletR = pRow - 1;
                        int bulletC = pCol;

                        Task.Run(() =>
                        {
                            try
                            {
                                while (bulletR > 0)
                                {
                                    Thread.Sleep(50); 

                                    lock (array)
                                    {
                                        array[0][2] = $"{bulletR}";

                                        if (bulletR == eRow && bulletC == eCol)
                                        {
                                            array[0][3] = "Winner";
                                            break; 
                                        }

                                        array[bulletR][bulletC] = ".";
                                        bulletR--;

                                        if (bulletR >= 0)
                                        {
                                            array[bulletR][bulletC] = "^";
                                        }
                                    }
                                }
                                lock (array)
                                {
                                    if (bulletR >= 0)
                                    {
                                        array[bulletR][bulletC] = ".";
                                    }
                                    array[pRow][pCol] = "B";
                                }
                            }
                            finally {bulletIsBeingShot = false;}
                        });
                    }


                    enemyActive = true;  
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
                            array[10][10] = "Enemy wins";
                        }
                    }

                }

                

            }
            Thread.Sleep(50);
        }
                
            }
        }
    

