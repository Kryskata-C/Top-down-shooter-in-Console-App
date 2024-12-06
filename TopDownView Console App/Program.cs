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
            Console.ForegroundColor = ConsoleColor.Red;
            string BulletChar = "^";
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Green;
            string EnemyChar = "&";
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Yellow;
            string EmptyChar = ".";
            string WinMessage = "Winner";
            Console.ResetColor();
            string Door = "D";


            int pRow = 10;
            int pCol = 10;
            int BulletR = 0;
            int BulletC = 0;

            Enemy enemy = new Enemy();
            enemy.Create(array);
            int eRow = enemy.enemyR;
            int eCol = enemy.enemyC;
            int dorRow = new int();
            int dorCol = new int(); 


            int moveCount = 0;

            string lookSide = null;
            bool foundBullet = false;
            bool enemyActive = new bool(); 
            bool bulletIsBeingShot = false;
            bool enemyDead = false;

            array[rnd.Next(0,array.Length-1)][rnd.Next(0,array.Length-1)] = Door;
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
            //Door checkup
            for (int i = 0; i < array.Length; i++)
            {
                for (int j = 0; j < array[i].Length; j++)
                {
                    if (array[i][j] == Door)
                    {
                        dorRow = i; 
                        dorCol = j; 
                        break;
                    }
                }
            }

            while (true)
            {
                if (!enemyDead) 
                {
                    array[dorRow][dorCol] = "D";
                }
                array[0][0] = $"{enemyDead}";
                // Player checkup
                for (int i = 0; i < array.Length; i++)
                {
                    for (int j = 0; j < array[i].Length; j++)
                    {
                        if (array[i][j] == PlayerChar)
                        {
                            pCol = j;
                            pRow = i;
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

                    // Player movement logic
                    // Player movement logic
                    if (key == ConsoleKey.W && pRow > 0 && !(pRow - 1 == dorRow && pCol == dorCol && !enemyDead))
                    {
                        array[pRow][pCol] = EmptyChar;
                        array[pRow - 1][pCol] = PlayerChar;
                        pRow--;
                        lookSide = "forward";
                    }
                    else if (key == ConsoleKey.S && pRow < array.Length - 1 && !(pRow + 1 == dorRow && pCol == dorCol && !enemyDead))
                    {
                        array[pRow][pCol] = EmptyChar;
                        array[pRow + 1][pCol] = PlayerChar;
                        pRow++;
                        lookSide = "backward";
                    }
                    else if (key == ConsoleKey.A && pCol > 0 && !(pRow == dorRow && pCol - 1 == dorCol && !enemyDead))
                    {
                        array[pRow][pCol] = EmptyChar;
                        array[pRow][pCol - 1] = PlayerChar;
                        pCol--;
                        lookSide = "left";
                    }
                    else if (key == ConsoleKey.D && pCol < array[pRow].Length - 1 && !(pRow == dorRow && pCol + 1 == dorCol && !enemyDead))
                    {
                        array[pRow][pCol] = EmptyChar;
                        array[pRow][pCol + 1] = PlayerChar;
                        pCol++;
                        lookSide = "right";
                    }
                    else if (key == ConsoleKey.W || key == ConsoleKey.S || key == ConsoleKey.A || key == ConsoleKey.D)
                    {
                        // Prevent movement and reset player position to avoid "invisibility"
                        array[pRow][pCol] = PlayerChar;
                    }






                    //Bullet keybind shit
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

                        //bullet logic
                        Task.Run(() =>
                        {
                            try
                            {
                                while (bulletR >= 0 && bulletR < array.Length && bulletC >= 0 && bulletC < array[0].Length)
                                {
                                    //Bullet loc are broken again but somehow the check if its at the same spot as the enemy works which i cant explain to myself so fuck it

                                    Thread.Sleep(50);

                                    lock (array)
                                    {

                                        if (bulletR == eRow && bulletC == eCol)
                                        {
                                            enemyDead = true;
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



                    //Room logic
                    if (pRow == dorRow && pCol == dorCol)
                    {
                        if (enemyDead)
                        {
                            throw new Exception("New level entered, the change of rooms still not coded in");
                        }
                        else
                        {
                            Console.SetCursorPosition(0, 22);
                            Console.WriteLine("You need to defeat the enemy before entering!");
                        }
                    }



                }

                Thread.Sleep(50);
            }
        }
    }
}
