namespace Maze
{
    public partial class Form1 : Form
    {
        MazeWall[,] mazeWall;

        enum Direction { Top = 0, Right = 1, Bottom = 2, Left = 3 }
        static Point[] directions = new Point[]
        {
            new Point(0, -1), // Top
            new Point(1, 0),  // Right
            new Point(0, 1),  // Bottom
            new Point(-1, 0), // Left
        };
        public Form1()
        {
            InitializeComponent();
        }

        public void Delay(int ms)
        {
            DateTime dateTimeNow = DateTime.Now;
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, ms);
            DateTime dateTimeAdd = dateTimeNow.Add(duration);
            while (dateTimeAdd >= dateTimeNow)
            {
                System.Windows.Forms.Application.DoEvents();
                dateTimeNow = DateTime.Now;
            }
            return;
        }

        List<Point> StartBFS(Player player, MazeWall[,] mazeWalls, int width, int height)
        {
            Queue<Point> queue = new Queue<Point>();
            HashSet<Point> visited = new HashSet<Point>();

            queue.Enqueue(player.Location);
            visited.Add(player.Location);

            // 큐에 넣은 모든 경로를 저장
            List<Point> bfsMoves = new List<Point>();

            Point[] directions = new Point[]
            {
                new Point(0, -1), // 상
                new Point(1, 0),  // 우
                new Point(0, 1),  // 하
                new Point(-1, 0)  // 좌
            };

            while (queue.Count > 0)
            {
                Point current = queue.Dequeue();
                bfsMoves.Add(current); // 탐색 시도 위치 기록

                if (current == new Point(width - 1, height - 1))
                    break;

                for (int i = 0; i < 4; i++)
                {
                    if (!mazeWalls[current.X, current.Y].isnotConnected[i] && !mazeWalls[current.X, current.Y].closedSides.Contains((MazeWall.Closed)i))
                    {
                        Point next = new Point(current.X + directions[i].X, current.Y + directions[i].Y);
                        if (!visited.Contains(next))
                        {
                            queue.Enqueue(next);
                            visited.Add(next);
                        }
                    }
                }
            }

            // bfsMoves 리스트를 따라 0.5초 간격으로 실제 이동
            return bfsMoves;
        }

        List<Point> StartDFS(Player player, MazeWall[,] mazeWalls, int width, int height)
        {
            Stack<Point> stack = new Stack<Point>();
            HashSet<Point> visited = new HashSet<Point>();

            List<Point> dfsMoves = new List<Point>();

            Point[] directions = new Point[]
            {
                new Point(0, -1), // 상
                new Point(1, 0),  // 우
                new Point(0, 1),  // 하
                new Point(-1, 0)  // 좌
            };

            stack.Push(player.Location);
            visited.Add(player.Location);

            while (stack.Count > 0)
            {
                Point current = stack.Pop();
                dfsMoves.Add(current); // 이동 시도 기록

                if (current == new Point(width - 1, height - 1))
                    break;

                // DFS 특성상 우선순위를 섞고 싶다면 이곳에 Shuffle 도 가능
                for (int i = 0; i < 4; i++)
                {
                    if (!mazeWalls[current.X, current.Y].isnotConnected[i] && !mazeWalls[current.X, current.Y].closedSides.Contains((MazeWall.Closed)i))
                    {
                        Point next = new Point(current.X + directions[i].X, current.Y + directions[i].Y);
                        if (!visited.Contains(next))
                        {
                            stack.Push(next);
                            visited.Add(next);
                        }
                    }
                }
            }

            // DFS 경로 시뮬레이션
            return dfsMoves;
        }


        Player SimulateMovement(Player player, List<Point> moveSequence, MazeWall[,] mazeWalls)
        {
            for (int moveIndex = 0; moveIndex < moveSequence.Count; moveIndex++)
            {
                player.Move(moveSequence[moveIndex], mazeWalls); // 내부적으로 Path가 갱신됨
                player.Location = moveSequence[moveIndex];       // 실제 현재 위치도 갱신
            }
            return player;
        }

        void SimulateMove(List<Player> player, int clock)
        {
            int[] time = new int[player.Count];
            while (true)
            {
                int a = 0;
                for (int i = 0; i < player.Count; i++)
                {
                    if (player[i].Path.Count > 0)
                    {
                        time[i]++; // 시간 증가
                        a++;
                        Point nextLocation = player[i].Path[0];
                        player[i].Path.RemoveAt(0);
                        player[i].Location = nextLocation;
                        mazeWall[nextLocation.X, nextLocation.Y].playerOn(player[i].Color.R, player[i].Color.G, player[i].Color.B);
                    }
                }
                Delay(clock); // 0.5초 대기
                if (a == 0)
                {
                    label1.Text += time[0] * clock / 1000.0 + "s";
                    label2.Text += time[1] * clock / 1000.0 + "s";
                    break; // 모든 플레이어가 이동할 수 없을 때 종료
                }
            }
        }

        void GenerateMaze(ref MazeWall[,] mazeWalls, int width, int height)
        {
            bool[,] visited = new bool[width, height];
            Stack<Point> stack = new Stack<Point>();
            Random rand = new Random();

            // 시작점은 0,0 (또는 랜덤도 가능)
            Point start = new Point(0, 0);
            stack.Push(start);
            visited[start.X, start.Y] = true;

            while (stack.Count > 0)
            {
                Point current = stack.Peek();
                List<int> availableDirections = new List<int>();

                for (int i = 0; i < 4; i++)
                {
                    int nx = current.X + directions[i].X;
                    int ny = current.Y + directions[i].Y;
                    if (nx >= 0 && nx < width && ny >= 0 && ny < height && !visited[nx, ny])
                    {
                        availableDirections.Add(i);
                    }
                }

                if (availableDirections.Count > 0)
                {
                    int dir = availableDirections[rand.Next(availableDirections.Count)];
                    int nx = current.X + directions[dir].X;
                    int ny = current.Y + directions[dir].Y;

                    mazeWalls[current.X, current.Y].removeclosed((MazeWall.Closed)dir);
                    mazeWalls[nx, ny].removeclosed((MazeWall.Closed)((dir + 2) % 4));

                    visited[nx, ny] = true;
                    stack.Push(new Point(nx, ny));
                }
                else
                {
                    stack.Pop();
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (mazeWall != null)
            {
                foreach (var wall in mazeWall)
                {
                    this.Controls.Remove(wall.pictureBox);
                }
            }
            mazeWall = new MazeWall[(int)numericUpDown1.Value, (int)numericUpDown1.Value];
            int size;
            int Heightstart = numericUpDown1.Location.Y + numericUpDown1.Size.Height + Size.Width / ((int)numericUpDown1.Value * 10);
            int Widthstart;

            if (Size.Width < Size.Height)
            {
                size = 4 * Size.Width / ((int)numericUpDown1.Value * 5);
                Heightstart = numericUpDown1.Location.Y + numericUpDown1.Size.Height + size / 8;
                Widthstart = Size.Width / 2 - Convert.ToInt32(size * (int)numericUpDown1.Value / 2.0);
            }
            else
            {
                size = 4 * (Size.Height - numericUpDown1.Location.Y - numericUpDown1.Size.Height) / ((int)numericUpDown1.Value * 5);
                Heightstart = numericUpDown1.Location.Y + numericUpDown1.Size.Height + size / 8;
                Widthstart = Size.Width / 2 - Convert.ToInt32(size * (int)numericUpDown1.Value / 2.0);
            }
            for (int i = 0; i < mazeWall.GetLength(0); i++)
            {
                for (int j = 0; j < mazeWall.GetLength(1); j++)
                {
                    mazeWall[i, j] = new MazeWall();
                    if (i == 0 && j == 0)
                    {
                        mazeWall[i, j].isnotConnected[0] = true; // Top
                        mazeWall[i, j].isnotConnected[3] = true; // Left
                    }
                    else if (i == 0 && j == mazeWall.GetLength(1) - 1)
                    {
                        mazeWall[i, j].isnotConnected[2] = true; // Bottom
                        mazeWall[i, j].isnotConnected[3] = true; // Left
                    }
                    else if (i == mazeWall.GetLength(0) - 1 && j == 0)
                    {
                        mazeWall[i, j].isnotConnected[0] = true; // Top
                        mazeWall[i, j].isnotConnected[1] = true; // Right
                    }
                    else if (i == mazeWall.GetLength(0) - 1 && j == mazeWall.GetLength(1) - 1)
                    {
                        mazeWall[i, j].isnotConnected[2] = true; // Bottom
                        mazeWall[i, j].isnotConnected[1] = true; // Right
                    }
                    else if (i == 0)
                    {
                        mazeWall[i, j].isnotConnected[3] = true; // Left
                    }
                    else if (j == 0)
                    {
                        mazeWall[i, j].isnotConnected[0] = true; // Top
                    }
                    else if (i == mazeWall.GetLength(0) - 1)
                    {
                        mazeWall[i, j].isnotConnected[1] = true; // Right
                    }
                    else if (j == mazeWall.GetLength(1) - 1)
                    {
                        mazeWall[i, j].isnotConnected[2] = true; // Bottom
                    }
                    mazeWall[i, j].Size = new Size(size, size);
                    mazeWall[i, j].Location = new Point(i * size + Widthstart, j * size + Heightstart);
                    mazeWall[i, j].addAllWalls();
                    this.Controls.Add(mazeWall[i, j].pictureBox);
                }
            }
            GenerateMaze(ref mazeWall, (int)numericUpDown1.Value, (int)numericUpDown1.Value);
            button2.Enabled = true;
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (this.Size.Width < 800)
            {
                this.Size = new Size(800, Size.Height);

            }
            if (this.Size.Height < 800)
            {
                this.Size = new Size(Size.Width, 800);

            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown1.Value < 2)
            {
                numericUpDown1.Value = 2;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<Player> players = new List<Player>
            {
                new Player(Color.Red),
                new Player(Color.Blue)
            };
            List<Point> bfs = StartBFS(players[0], mazeWall, (int)numericUpDown1.Value, (int)numericUpDown1.Value);
            List<Point> dfs = StartDFS(players[1], mazeWall, (int)numericUpDown1.Value, (int)numericUpDown1.Value);
            players[0] = SimulateMovement(players[0], bfs, mazeWall);
            players[1] = SimulateMovement(players[1], dfs, mazeWall);
            players[0].Path.RemoveAt(0); // 시작 위치는 제외
            players[1].Path.RemoveAt(0); // 시작 위치는 제외
            SimulateMove(players, 500);
        }
    }

    class Player
    {
        public Point Location { get; set; }
        public Color Color { get; set; }
        public List<Point> Path { get; set; } = new List<Point>();
        public Player(Color color)
        {
            Color = color;
            Location = new Point(0, 0); // 초기 위치 (0, 0)로 설정
        }
        public void Move(Point newLocation, MazeWall[,] mazeWalls)
        {
            Dictionary<Point, Point?> parent = new Dictionary<Point, Point?>();
            Queue<Point> queue = new Queue<Point>();
            HashSet<Point> visited = new HashSet<Point>();

            queue.Enqueue(Location);
            visited.Add(Location);
            parent[Location] = null;

            while (queue.Count > 0)
            {
                Point current = queue.Dequeue();

                if (current == newLocation)
                {
                    break;
                }

                // 상, 우, 하, 좌 순서
                Point[] directions = new Point[]
                {
                    new Point(0, -1), // 상
                    new Point(1, 0),  // 우
                    new Point(0, 1),  // 하
                    new Point(-1, 0)  // 좌
                };

                for (int i = 0; i < 4; i++)
                {
                    if (!mazeWalls[current.X, current.Y].isnotConnected[i] && !mazeWalls[current.X, current.Y].closedSides.Contains((MazeWall.Closed)i))
                    {
                        Point next = new Point(current.X + directions[i].X, current.Y + directions[i].Y);
                        if (!visited.Contains(next))
                        {
                            queue.Enqueue(next);
                            visited.Add(next);
                            parent[next] = current;
                        }
                    }
                }
            }

            // 경로를 역추적
            Point? trace = newLocation;
            Stack<Point> reversedPath = new Stack<Point>();
            while (trace != null)
            {
                reversedPath.Push(trace.Value);
                trace = parent.ContainsKey(trace.Value) ? parent[trace.Value] : null;
            }

            while (reversedPath.Count > 0)
            {
                if(reversedPath.Peek() == Location && reversedPath.Peek() != new Point(0, 0))
                {
                    reversedPath.Pop(); // 현재 위치는 제외
                }
                Path.Add(reversedPath.Pop());
            }
        }
    }
    class MazeWall
    {
        public List<Closed> closedSides;
        public Bitmap bitmap;
        public PictureBox pictureBox;
        public bool[] isnotConnected = { false, false, false, false }; // Top, Right, Bottom, Left

        public enum Closed
        {
            Top,
            Right,
            Bottom,
            Left
        }

        public MazeWall()
        {
            closedSides = new List<Closed>();
            bitmap = new Bitmap(800, 800);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.White); // 배경 흰색
            }
            pictureBox = new PictureBox
            {
                Image = bitmap,
                SizeMode = PictureBoxSizeMode.StretchImage
            };
        }
        public Point Location
        {
            get { return pictureBox.Location; }
            set { pictureBox.Location = value; }
        }
        public Size Size
        {
            get { return pictureBox.Size; }
            set { pictureBox.Size = value; }
        }
        public void addAllWalls()
        {
            addclosed(Closed.Top);
            addclosed(Closed.Right);
            addclosed(Closed.Bottom);
            addclosed(Closed.Left);
        }
        public bool addclosed(Closed closed)
        {
            if (closedSides.Contains(closed))
            {
                return false;
            }
            closedSides.Add(closed);
            using (Graphics g = Graphics.FromImage(bitmap))
            {


                // 2. 두꺼운 선 그리기
                using (Pen thickPen = new Pen(Color.Gray, 40)) // 회색, 40픽셀 두께
                {
                    if (closed == Closed.Top)
                        g.DrawLine(thickPen, new Point(0, 20), new Point(800, 20));
                    else if (closed == Closed.Right)
                        g.DrawLine(thickPen, new Point(780, 0), new Point(780, 800));
                    else if (closed == Closed.Bottom)
                        g.DrawLine(thickPen, new Point(0, 780), new Point(800, 780));
                    else if (closed == Closed.Left)
                        g.DrawLine(thickPen, new Point(20, 0), new Point(20, 800));
                }
                using (Pen thickPen = new Pen(Color.Gray, 40)) // 회색, 40픽셀 두께
                {
                    if (closed == Closed.Top && isnotConnected[0])
                        g.DrawLine(thickPen, new Point(0, 60), new Point(800, 60));
                    else if (closed == Closed.Right && isnotConnected[1])
                        g.DrawLine(thickPen, new Point(740, 0), new Point(740, 800));
                    else if (closed == Closed.Bottom && isnotConnected[2])
                        g.DrawLine(thickPen, new Point(0, 740), new Point(800, 740));
                    else if (closed == Closed.Left && isnotConnected[3])
                        g.DrawLine(thickPen, new Point(60, 0), new Point(60, 800));
                }

            }
            pictureBox.Image = bitmap;
            return true;
        }

        public void playerOn(int R, int G, int B)
        {
            using (Graphics g = Graphics.FromImage(bitmap))
            {

                // 투명도 128(50%)의 빨간색 브러시
                using (Brush transparentRed = new SolidBrush(Color.FromArgb(25, R, G, B)))
                {
                    g.FillRectangle(transparentRed, 80, 80, 640, 640);
                }
            }
            pictureBox.Image = bitmap;
        }

        public bool removeclosed(Closed closed)
        {
            if (!closedSides.Contains(closed))
            {
                return false;
            }
            closedSides.Remove(closed);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.White); // 배경 흰색

                // 2. 두꺼운 선 그리기
                using (Pen thickPen = new Pen(Color.Gray, 40)) // 회색, 40픽셀 두께
                {
                    foreach (var side in closedSides)
                    {
                        if (side == Closed.Top)
                            g.DrawLine(thickPen, new Point(0, 20), new Point(800, 20));
                        else if (side == Closed.Right)
                            g.DrawLine(thickPen, new Point(780, 0), new Point(780, 800));
                        else if (side == Closed.Bottom)
                            g.DrawLine(thickPen, new Point(0, 780), new Point(800, 780));
                        else if (side == Closed.Left)
                            g.DrawLine(thickPen, new Point(20, 0), new Point(20, 800));
                    }
                }
                using (Pen thickPen = new Pen(Color.Gray, 40)) // 회색, 40픽셀 두께
                {
                    foreach (var side in closedSides)
                    {
                        if (side == Closed.Top && isnotConnected[0])
                            g.DrawLine(thickPen, new Point(0, 60), new Point(800, 60));
                        else if (side == Closed.Right && isnotConnected[1])
                            g.DrawLine(thickPen, new Point(740, 0), new Point(740, 800));
                        else if (side == Closed.Bottom && isnotConnected[2])
                            g.DrawLine(thickPen, new Point(0, 740), new Point(800, 740));
                        else if (side == Closed.Left && isnotConnected[3])
                            g.DrawLine(thickPen, new Point(60, 0), new Point(60, 800));
                    }
                }

            }
            pictureBox.Image = bitmap;
            return true;
        }

    }
}
