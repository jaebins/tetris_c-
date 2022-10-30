using Microsoft.VisualBasic.Logging;
using System.Windows.Forms.Design.Behavior;
using static System.Reflection.Metadata.BlobBuilder;

namespace tetris
{
    public partial class Form1 : Form
    {
        Graphics gameScreen_graphic; // ����� �׷��� ȭ�� �׷���
        Thread downBlockThread;

        List<Point[,]> blocks = new List<Point[,]>();
        Point[,] nowBlock; // ���� ��ġ���ִ� ���
        List<Point> nowBlocks_Loc = new List<Point>(); // ���� ��ġ���ִ� ����� ��ġ
        List<Point> nowBlocks_index = new List<Point>();

        Point incBlcokLoc = new Point(0, 0); // ����� �̵��� ��
        Point mostX = new Point(0, 0); // �����ڸ� X
        Point mostY = new Point(0, 0); // �����ڸ� Y
        int blockTransCount = 0; // �� ���

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // ��� ��ġ ��ǥ ����, x�� 10����, y�� 20����
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

        private void Form1_KeyUp(object sender, KeyEventArgs e) // Ű �̺�Ʈ
        {
            if (e.KeyCode == Keys.Left) // ���� Ű �̺�Ʈ
            {
                for(int i = 0; i < nowBlocks_Loc.Count; i++)
                {
                    if (nowBlocks_Loc[i].X > 0)
                        incBlcokLoc.X -= 1;
                        ResettingMino(true);
                        break;
                }
            }

            if (e.KeyCode == Keys.Right && mostX.X < 9) // ������ Ű �̺�Ʈ
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

        void SelectMino() // �� �̱�, �ʱ�ȭ
        {
            //Random random = new Random();
            //nowBlock = blocks[random.Next(0, blocks.Count)];
            nowBlock = Blocks.Lmino_Reverse;
            incBlcokLoc = new Point(0, 0);
            blockTransCount = 0;
            nowBlocks_Loc.Clear();
            nowBlocks_index.Clear();
            DrawMino(nowBlock, false, false); // ����������� �ٲٱ�
        }

        void ResettingMino(bool isUserMove) // �� ����� �ٽ� �׸��� (�� �̵�)
        {
            if (mostY.Y >= (GameRule.game_Height / GameRule.game_blockY) - 1 ||
                GameRule.game_blockLocs[mostY.X, mostY.Y + 1].IsFill) // ���� ���� �ٴڿ� ��ų� ���� ��´ٸ�
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

        void DrawMino(Point[,] tarBlocks, bool isUserMove, bool isBuild) // �� �����ϰ� �׸���
        {
            mostX = new Point(0, 0);
            mostY = new Point(0, 0);

            for (int i = 0; i < 4; i++)
            {
                int x_inArray = tarBlocks[blockTransCount, i].X + incBlcokLoc.X; 
                int y_inArray = tarBlocks[blockTransCount, i].Y + incBlcokLoc.Y;

                BlocksLocs blocksLocs = GameRule.game_blockLocs[x_inArray, y_inArray]; // �������� ����
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

                // �� �׸���
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

        void ClearMino() // �� �����
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

        void DownBlocks() // �� �������� ������
        {
            Thread.Sleep(1000);
            downBlockThread.Interrupt(); // ���� �������鼭 �����尡 ��ġ�� �ʰ��ϱ� ���� ������ ����

            incBlcokLoc.Y += 1;
            ResettingMino(false);
        }
    }
}