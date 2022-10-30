using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tetris
{
    static class GameRule
    {
        internal const int game_Width = 400;
        internal const int game_Height = 800;
        internal const int game_blockX = 40;
        internal const int game_blockY = 40;
        internal static BlocksLocs[,] game_blockLocs;
    }

    class BlocksLocs
    {
        private bool isFill;
        private Point loc;

        public bool IsFill { get; set; }
        public Point Loc { get; set; }
    }
}
