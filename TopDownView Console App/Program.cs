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

            // Initialize the array
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = new string[20];
                for (int j = 0; j < array[i].Length; j++)
                {
                    array[i][j] = ".";
                }
            }

            // Start updating the array asynchronously
            _ = Task.Run(() => DArray.UpdateArray(ref array));
            Thread.Sleep(50);

            lock (array)
            {
                Array.Resize(ref array, array.Length);
            }

            string PlayerChar = "B";
            string BulletChar = "^";
            string EnemyChar = "&";
            string EmptyChar = ".";
            string WinMessage = "Winner";


            int pRow = 0;
            int pCol = 0;
            int BulletR = 0;
            int BulletC = 0;

            Enemy enemy = new Enemy();
            enemy.Create(array);
            int eRow = enemy.enemyR;
            int eCol = enemy.enemyC;

            int countForEndScreen = 0;
            int moveCount = 0; // Later make it so the enemy can move every second turn

            string lookSide = null;
            bool foundBullet = false;
            bool enemyActive = true;
            bool bulletIsBeingShot = false;

            // Enemy checkup
            for (int i = 0; i < array.Length; i++)
            {
                for (int j = 0; j < array[i].Length; j++)
                {
                    if (array[i][j] == EnemyChar)
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

                // Player checkup
                for (int i = 0; i < array.Length; i++)
                {
                    for (int j = 0; j < array[i].Length; j++)
                    {
                        if (array[i][j] == PlayerChar)
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
                    // Find bullet position
                    for (int i = 0; i < array.Length; i++)
                    {
                        for (int j = 0; j < array.Length; j++)
                        {
                            if (array[j][i] == BulletChar)
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

                    // Find player position
                    for (int i = 0; i < array.Length; i++)
                    {
                        for (int j = 0; j < array.Length; j++)
                        {
                            if (array[i][j] == PlayerChar)
                            {
                                pRow = i;
                                pCol = j;
                            }
                        }
                    }

                    array[pRow][pCol] = EmptyChar;

                    // Player logic
                    if (key == ConsoleKey.W)
                    {
                        array[pRow - 1][pCol] = PlayerChar;
                        pRow--;
                        lookSide = "forward";
                    }
                    else if (key == ConsoleKey.S)
                    {
                        array[pRow + 1][pCol] = PlayerChar;
                        pRow++;
                        lookSide = "backward";
                    }
                    else if (key == ConsoleKey.A)
                    {
                        array[pRow][pCol - 1] = PlayerChar;
                        pCol--;
                        lookSide = "left";
                    }
                    else if (key == ConsoleKey.D)
                    {
                        array[pRow][pCol + 1] = PlayerChar;
                        pCol++;
                        lookSide = "right";
                    }
                    else if (key == ConsoleKey.Q && !bulletIsBeingShot)
                    {
                        bulletIsBeingShot = true;
                        int bulletR = 0, bulletC = 0;
                        int dRow = 0, dCol = 0;

                        lock (array)
                        {
                            if (lookSide == "forward")
                            {
                                array[pRow - 1][pCol] = BulletChar;
                                array[pRow][pCol] = PlayerChar;
                                bulletR = pRow - 1;
                                bulletC = pCol;
                                dRow = -1;
                                dCol = 0;
                            }
                            else if (lookSide == "backward")
                            {
                                array[pRow + 1][pCol] = BulletChar;
                                array[pRow][pCol] = PlayerChar;
                                bulletR = pRow + 1;
                                bulletC = pCol;
                                dRow = 1;
                                dCol = 0;
                            }
                            else if (lookSide == "left")
                            {
                                array[pRow][pCol - 1] = BulletChar;
                                array[pRow][pCol] = PlayerChar;
                                bulletR = pRow;
                                bulletC = pCol - 1;
                                dRow = 0;
                                dCol = -1;
                            }
                            else if (lookSide == "right")
                            {
                                array[pRow][pCol + 1] = BulletChar;
                                array[pRow][pCol] = PlayerChar;
                                bulletR = pRow;
                                bulletC = pCol + 1;
                                dRow = 0;
                                dCol = 1;
                            }
                        }

                        Task.Run(() =>
                        {
                            try
                            {
                                while (bulletR >= 0 && bulletR < array.Length && bulletC >= 0 && bulletC < array[0].Length)
                                {
                                    Thread.Sleep(50);

                                    lock (array)
                                    {
                                        if (bulletR == eRow && bulletC == eCol)
                                        {
                                            array[0][3] = "Winner";
                                            break;
                                        }

                                        array[bulletR][bulletC] = EmptyChar;
                                        bulletR += dRow;
                                        bulletC += dCol;

                                        if (bulletR >= 0 && bulletR < array.Length && bulletC >= 0 && bulletC < array[0].Length)
                                        {
                                            array[bulletR][bulletC] = BulletChar;
                                        }
                                    }
                                }
                                lock (array)
                                {
                                    if (bulletR >= 0 && bulletR < array.Length && bulletC >= 0 && bulletC < array[0].Length)
                                    {
                                        array[bulletR][bulletC] = EmptyChar;
                                    }
                                    array[pRow][pCol] = PlayerChar;
                                }
                            }
                            finally
                            {
                                bulletIsBeingShot = false;
                            }
                        });
                    }

                    // Enemy logic
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
                                array[eRow][eCol] = EmptyChar;
                                array[eRow - 1][eCol] = EnemyChar;
                                eRow--;
                            }
                            else if (eRow < pRow)
                            {
                                array[eRow][eCol] = EmptyChar;
                                array[eRow + 1][eCol] = EnemyChar;
                                eRow++;
                            }

                            if (eCol > pCol)
                            {
                                array[eRow][eCol] = EmptyChar;
                                array[eRow][eCol - 1] = EnemyChar;
                                eCol--;
                            }
                            else if (eCol < pCol)
                            {
                                array[eRow][eCol] = EmptyChar;
                                array[eRow][eCol + 1] = EnemyChar;
                                eCol++;
                            }
                        }

                        if (eRow == pRow && eCol == pCol)
                        {
                            array[10][10] = "Enemy wins";
                        }
                    }
                }

                Thread.Sleep(50);
            }
        }
    }
}
