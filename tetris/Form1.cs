using Microsoft.VisualBasic.Logging;
using System.Windows.Forms.Design.Behavior;
using static System.Reflection.Metadata.BlobBuilder;

namespace tetris
{
    public partial class Form1 : Form
    {
        Graphics gameScreen_graphic; // 블록을 그려낼 화면 그래픽
        Thread downBlockThread;

        List<Point[,]> blocks = new List<Point[,]>();
        Point[,] nowBlock; // 현재 설치되있는 블록
        List<Point> nowBlocks_Loc = new List<Point>(); // 현재 설치되있는 블록의 위치
        List<Point> nowBlocks_index = new List<Point>();

        Point incBlcokLoc = new Point(0, 0); // 블록이 이동한 값
        Point mostX = new Point(0, 0); // 가장자리 X
        Point mostY = new Point(0, 0); // 가장자리 Y
        int blockTransCount = 0; // 블럭 모양

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 블록 위치 좌표 저장, x는 10까지, y는 20까지
            GameRule.game_blockLocs = new BlocksLocs[GameRule.game_Width / GameRule.game_blockX, GameRule.game_Height / GameRule.game_blockY];
            for (int i = 0; i < GameRule.game_blockLocs.GetLength(0); i++)
            {
                for (int j = 0; j < GameRule.game_blockLocs.GetLength(1); j++)
                {
                    BlocksLocs blocksLocs = new BlocksLocs();
                    blocksLocs.IsFill = false;
                    blocksLocs.Loc = new Point(GameRule.game_blockX * i, GameRule.game_blockY * j);
                    GameRule.game_blockLocs[i, j] = blocksLocs;
                }
            }

            blocks.Add(Blocks.Imino);
            blocks.Add(Blocks.Tmino);
            blocks.Add(Blocks.Zmino);
            blocks.Add(Blocks.Zmino_Reverse);
            blocks.Add(Blocks.Lmino);
            blocks.Add(Blocks.Lmino_Reverse);

            SetClientSizeCore(GameRule.game_Width, GameRule.game_Height);
            gameScreen_graphic = CreateGraphics();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e) // 키 이벤트
        {
            if (e.KeyCode == Keys.Left) // 왼쪽 키 이벤트
            {
                for(int i = 0; i < nowBlocks_Loc.Count; i++)
                {
                    if (nowBlocks_Loc[i].X > 0)
                        incBlcokLoc.X -= 1;
                        ResettingMino(true);
                        break;
                }
            }

            if (e.KeyCode == Keys.Right && mostX.X < 9) // 오른쪽 키 이벤트
            {
                incBlcokLoc.X += 1;
                ResettingMino(true);
            }

            if(e.KeyCode == Keys.Up)
            {
                blockTransCount++;
                if (blockTransCount == nowBlock.GetLength(0))
                {
                    blockTransCount = 0;
                }
                ResettingMino(true);
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, GameRule.game_Width, GameRule.game_Height));
            for (int i = 0; i < GameRule.game_Height / GameRule.game_blockY; i++)
            {
                Pen pen = new Pen(Color.Black);
                gameScreen_graphic.DrawLine(pen, new Point(0, GameRule.game_blockY * i), new Point(GameRule.game_Width, GameRule.game_blockY * i));
            }

            for (int i = 0; i < GameRule.game_Width / GameRule.game_blockX; i++)
            {
                Pen pen = new Pen(Color.Black);
                gameScreen_graphic.DrawLine(pen, new Point(GameRule.game_blockX * i, 0), new Point(GameRule.game_blockX * i, GameRule.game_Height));
            }

            SelectMino();
        }

        void SelectMino() // 블럭 뽑기, 초기화
        {
            Random random = new Random();
            nowBlock = blocks[random.Next(0, blocks.Count)];
            incBlcokLoc = new Point(0, 0);
            blockTransCount = 0;
            nowBlocks_Loc.Clear();
            nowBlocks_index.Clear();
            DrawMino(nowBlock, false, false); // 랜덤블록으로 바꾸기
        }

        void ResettingMino(bool isUserMove) // 블럭 지우고 다시 그리기 (블럭 이동)
        {
            if (mostY.Y >= (GameRule.game_Height / GameRule.game_blockY) - 1 ||
                GameRule.game_blockLocs[mostY.X, mostY.Y + 1].IsFill) // 만약 블럭이 바닥에 닿거나 블럭에 닿는다면
            {
                for(int i = 0; i < 4; i++)
                {
                    GameRule.game_blockLocs[nowBlocks_index[i].X, nowBlocks_index[i].Y].IsFill = true;
                }
                SelectMino();
            }
            else
            {
                ClearMino();
                DrawMino(nowBlock, isUserMove, false);
            }
        }

        void DrawMino(Point[,] tarBlocks, bool isUserMove, bool isBuild) // 블럭 선택하고 그리기
        {
            mostX = new Point(0, 0);
            mostY = new Point(0, 0);

            for (int i = 0; i < 4; i++)
            {
                int x_inArray = tarBlocks[blockTransCount, i].X + incBlcokLoc.X; 
                int y_inArray = tarBlocks[blockTransCount, i].Y + incBlcokLoc.Y;

                BlocksLocs blocksLocs = GameRule.game_blockLocs[x_inArray, y_inArray]; // 실질적인 블럭값
                Point loc = blocksLocs.Loc;

                if (!isBuild)
                {
                    if (x_inArray > mostX.X)
                        mostX = new Point(x_inArray, y_inArray);
                    if (y_inArray > mostY.Y)
                        mostY = new Point(x_inArray, y_inArray);

                    nowBlocks_Loc.Add(loc);
                    nowBlocks_index.Add(new Point(x_inArray, y_inArray));
                }

                // 블럭 그리기
                Rectangle rect = new Rectangle(loc, new Size(GameRule.game_blockX, GameRule.game_blockY));
                Rectangle rect2 = new Rectangle(loc, new Size(GameRule.game_blockX, GameRule.game_blockY));
                gameScreen_graphic.DrawRectangle(Pens.Black, rect);
                gameScreen_graphic.FillRectangle(Brushes.Red, rect2);
            }

            if (!isUserMove)
            {
                downBlockThread = new Thread(DownBlocks);
                downBlockThread.IsBackground = true;
                downBlockThread.Start();
            }
        }

        void ClearMino() // 블럭 지우기
        {
            for (int i = 0; i < 4; i++)
            {
                Rectangle rect1 = new Rectangle(nowBlocks_Loc[i], new Size(GameRule.game_blockX, GameRule.game_blockY));
                gameScreen_graphic.FillRectangle(Brushes.White, rect1);
                Rectangle rect2 = new Rectangle(nowBlocks_Loc[i], new Size(GameRule.game_blockX, GameRule.game_blockY));
                gameScreen_graphic.DrawRectangle(Pens.Black, rect2);
            }
            nowBlocks_Loc.Clear();
            nowBlocks_index.Clear();
        }

        void DownBlocks() // 블럭 내려오는 쓰레드
        {
            Thread.Sleep(1000);
            downBlockThread.Interrupt(); // 블럭이 내려오면서 쓰레드가 겹치지 않게하기 위해 쓰레드 중지

            incBlcokLoc.Y += 1;
            ResettingMino(false);
        }
    }
}
