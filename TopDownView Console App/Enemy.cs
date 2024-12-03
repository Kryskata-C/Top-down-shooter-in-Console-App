using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopDownView_Console_App
{
    public class Enemy()
    {
        public int enemyR;
        public int enemyC;

        public void Create(string[][] array)
        {
            Random rand = new Random();
            int randRow = rand.Next(0, array.Length);
            int randCol = rand.Next(0, array.Length);

            if (array[randCol][randRow] != "*")
            {
                array[randCol][randRow] = "*";
            }

            array[randCol][randRow] = "&";
        }
    }
}
