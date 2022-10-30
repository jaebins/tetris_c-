using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tetris
{
    static class Blocks
    {
        // I미노
        internal static Point[,] Imino = new Point[,]
        {
            {
                new Point(0, 0),  new Point(0, 1) , new Point(0, 2), new Point(0, 3)
            },
            {
                new Point(0, 0) , new Point(1, 0) , new Point(2, 0), new Point(3, 0)
            }
        };

        internal static Point[,] Omino = new Point[,]
        {
            {
                new Point(0, 0),  new Point(0, 1) , new Point(1, 0), new Point(1, 1)
            },
        };

        internal static Point[,] Tmino = new Point[,]
        {
            {
                new Point(0, 1) , new Point(1, 1) , new Point(2, 1), new Point(1, 0)
            },
            {
                new Point(1, 2) , new Point(1, 1) , new Point(2, 1), new Point(1, 0)
            },
            {
                new Point(1, 2) , new Point(1, 1) , new Point(2, 1), new Point(0, 1)
            },
            {
                new Point(1, 2) , new Point(1, 1) , new Point(1, 0), new Point(0, 1)
            },
        };

        internal static Point[,] Zmino = new Point[,]
        {
            {
                new Point(0, 0),  new Point(1, 0) , new Point(1, 1), new Point(2, 1)
            },
            {
                new Point(0, 1) , new Point(0, 2) , new Point(1, 0), new Point(1, 1)
            }
        };

        internal static Point[,] Zmino_Reverse = new Point[,]
        {
            {
                new Point(0, 1),  new Point(1, 1) , new Point(1, 0), new Point(2, 0)
            },
            {
                new Point(0, 0) , new Point(0, 1) , new Point(1, 1), new Point(1, 2)
            }
        };

        internal static Point[,] Lmino = new Point[,]
        {
            {
                new Point(2, 0), new Point(0, 1), new Point(1, 1), new Point(2, 1)
            },
            {
                new Point(1, 0), new Point(1, 1), new Point(1, 2), new Point(2, 2)
            },
            {
                new Point(0, 1), new Point(1, 1), new Point(2, 1), new Point(0, 2)
            },
            {
                new Point(1, 0), new Point(1, 1), new Point(1, 2), new Point(0, 0)
            },
        };

        internal static Point[,] Lmino_Reverse = new Point[,]
        {
            {
                new Point(0, 0), new Point(0, 1), new Point(1, 1), new Point(2, 1)
            },
            {
                new Point(1, 0), new Point(1, 1), new Point(1, 2), new Point(2, 0)
            },
            {
                new Point(0, 1), new Point(1, 1), new Point(2, 1), new Point(2, 2)
            },
            {
                new Point(1, 0), new Point(1, 1), new Point(1, 2), new Point(0, 2)
            },
        };
    }
}
