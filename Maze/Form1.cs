namespace Maze
{
    public partial class Form1 : Form
    {
        MazeWall[,] mazeWall;
        bool isSecond = false;
        bool isWrite = true;
        HashSet<Point> prevBfsVisited = new HashSet<Point>();
        HashSet<Point> prevDfsVisited = new HashSet<Point>();

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

            if (isSecond)
            {
                prevBfsVisited = visited; // 1차 BFS에서 방문한 위치 저장
            }

            // bfsMoves 리스트를 따라 0.5초 간격으로 실제 이동
            return bfsMoves;
        }

        List<Point> Start2ndBFS(Player player, MazeWall[,] mazeWalls, int width, int height, HashSet<Point> visited1st)
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
                        if (!visited.Contains(next) && visited1st.Contains(next))
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

            if (isSecond)
            {
                prevDfsVisited = visited; // 1차 DFS에서 방문한 위치 저장
            }

            // DFS 경로 시뮬레이션
            return dfsMoves;
        }

        List<Point> Start2ndDFS(Player player, MazeWall[,] mazeWalls, int width, int height, HashSet<Point> visited1st)
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
                        if (!visited.Contains(next) && visited1st.Contains(next))
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

        List<Point> StartAStar(Player player, MazeWall[,] mazeWalls, int width, int height)
        {
            Point start = player.Location;
            Point goal = new Point(width - 1, height - 1);
            PriorityQueue<Point, int> open = new PriorityQueue<Point, int>();
            Dictionary<Point, Point?> cameFrom = new Dictionary<Point, Point?>();
            Dictionary<Point, int> gScore = new Dictionary<Point, int>();

            open.Enqueue(start, 0);
            gScore[start] = 0;

            while (open.Count > 0)
            {
                Point current = open.Dequeue();
                if (current == goal)
                    break;

                for (int i = 0; i < 4; i++)
                {
                    if (!mazeWalls[current.X, current.Y].isnotConnected[i] && !mazeWalls[current.X, current.Y].closedSides.Contains((MazeWall.Closed)i))
                    {
                        Point next = new Point(current.X + directions[i].X, current.Y + directions[i].Y);
                        int tentativeG = gScore[current] + 1;
                        if (!gScore.ContainsKey(next) || tentativeG < gScore[next])
                        {
                            cameFrom[next] = current;
                            gScore[next] = tentativeG;
                            int f = tentativeG + Math.Abs(goal.X - next.X) + Math.Abs(goal.Y - next.Y);
                            open.Enqueue(next, f);
                        }
                    }
                }
            }

            List<Point> path = new List<Point>();
            Point? trace = goal;
            while (trace != null && cameFrom.ContainsKey(trace.Value))
            {
                path.Add(trace.Value);
                trace = cameFrom[trace.Value];
            }
            path.Add(start);
            path.Reverse();
            return path;
        }

        List<Point> StartFloodFill(Player player, MazeWall[,] mazeWalls, int width, int height)
        {
            Point start = player.Location;
            Point goal = new Point(width - 1, height - 1);
            Queue<Point> queue = new Queue<Point>();
            Dictionary<Point, int> distance = new Dictionary<Point, int>();

            queue.Enqueue(goal);
            distance[goal] = 0;

            while (queue.Count > 0)
            {
                Point current = queue.Dequeue();
                for (int i = 0; i < 4; i++)
                {
                    if (!mazeWalls[current.X, current.Y].isnotConnected[i] && !mazeWalls[current.X, current.Y].closedSides.Contains((MazeWall.Closed)i))
                    {
                        Point next = new Point(current.X + directions[i].X, current.Y + directions[i].Y);
                        if (!distance.ContainsKey(next))
                        {
                            distance[next] = distance[current] + 1;
                            queue.Enqueue(next);
                        }
                    }
                }
            }

            List<Point> path = new List<Point>();
            if (!distance.ContainsKey(start))
            {
                path.Add(start);
                return path;
            }
            Point cur = start;
            path.Add(cur);
            while (cur != goal)
            {
                Point next = cur;
                int best = int.MaxValue;
                for (int i = 0; i < 4; i++)
                {
                    if (!mazeWalls[cur.X, cur.Y].isnotConnected[i] && !mazeWalls[cur.X, cur.Y].closedSides.Contains((MazeWall.Closed)i))
                    {
                        Point n = new Point(cur.X + directions[i].X, cur.Y + directions[i].Y);
                        if (distance.TryGetValue(n, out int d) && d < best)
                        {
                            best = d;
                            next = n;
                        }
                    }
                }
                if (next == cur) break;
                path.Add(next);
                cur = next;
            }
            return path;
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

        void SimulateMove(List<(Player player, Label label, string prefix)> players, int clock)
        {
            int[] time = new int[players.Count];
            while (true)
            {
                int a = 0;
                for (int i = 0; i < players.Count; i++)
                {
                    if (players[i].player.Path.Count > 0)
                    {
                        time[i]++;
                        a++;
                        Point nextLocation = players[i].player.Path[0];
                        players[i].player.Path.RemoveAt(0);
                        players[i].player.Location = nextLocation;
                        mazeWall[nextLocation.X, nextLocation.Y].playerOn(players[i].player.Color.R, players[i].player.Color.G, players[i].player.Color.B);
                    }
                }
                Delay(clock);
                if (a == 0)
                {
                    break;
                }
            }
            for (int i = 0; i < players.Count; i++)
            {
                players[i].label.Text = players[i].prefix + time[i] * clock / 1000.0 + " s";
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
                Heightstart = numericUpDown1.Location.Y + numericUpDown1.Size.Height + Size.Height / 10;
                Widthstart = Size.Width / 2 - Convert.ToInt32(size * (int)numericUpDown1.Value / 2.0);
            }
            else
            {
                size = 4 * (Size.Height - numericUpDown1.Location.Y - numericUpDown1.Size.Height) / ((int)numericUpDown1.Value * 5);
                Heightstart = numericUpDown1.Location.Y + numericUpDown1.Size.Height + Size.Height / 10;
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
            label6.Text = "n = " + numericUpDown1.Value.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var players = new List<(Player player, Label label, string prefix)>();

            if (checkBoxBFS.Checked)
            {
                var p = new Player(Color.Red);
                var path = StartBFS(p, mazeWall, (int)numericUpDown1.Value, (int)numericUpDown1.Value);
                p = SimulateMovement(p, path, mazeWall);
                p.Path.RemoveAt(0);
                players.Add((p, label1, "BFS : "));
            }
            else
            {
                label1.Text = "BFS : OFF";
            }

            if (checkBoxDFS.Checked)
            {
                var p = new Player(Color.Blue);
                var path = StartDFS(p, mazeWall, (int)numericUpDown1.Value, (int)numericUpDown1.Value);
                p = SimulateMovement(p, path, mazeWall);
                p.Path.RemoveAt(0);
                players.Add((p, label2, "DFS : "));
            }
            else
            {
                label2.Text = "DFS : OFF";
            }

            if (checkBoxAStar.Checked)
            {
                var p = new Player(Color.Green);
                var path = StartAStar(p, mazeWall, (int)numericUpDown1.Value, (int)numericUpDown1.Value);
                p = SimulateMovement(p, path, mazeWall);
                p.Path.RemoveAt(0);
                players.Add((p, label11, "A* : "));
            }
            else
            {
                label11.Text = "A* : OFF";
            }

            if (checkBoxFlood.Checked)
            {
                var p = new Player(Color.Gray);
                var path = StartFloodFill(p, mazeWall, (int)numericUpDown1.Value, (int)numericUpDown1.Value);
                p = SimulateMovement(p, path, mazeWall);
                p.Path.RemoveAt(0);
                players.Add((p, label12, "FLOOD : "));
            }
            else
            {
                label12.Text = "FLOOD : OFF";
            }

            button1.Enabled = false;
            button2.Enabled = false;

            if (isSecond)
            {
                if (checkBoxBFS.Checked)
                {
                    var p2 = new Player(Color.Red);
                    var path2 = Start2ndBFS(p2, mazeWall, (int)numericUpDown1.Value, (int)numericUpDown1.Value, prevDfsVisited);
                    p2 = SimulateMovement(p2, path2, mazeWall);
                    p2.Path.RemoveAt(0);
                    players.Add((p2, label10, "BFS : "));
                }
                else
                {
                    label10.Text = "BFS : OFF";
                }

                if (checkBoxDFS.Checked)
                {
                    var p2 = new Player(Color.Blue);
                    var path2 = Start2ndDFS(p2, mazeWall, (int)numericUpDown1.Value, (int)numericUpDown1.Value, prevBfsVisited);
                    p2 = SimulateMovement(p2, path2, mazeWall);
                    p2.Path.RemoveAt(0);
                    players.Add((p2, label9, "DFS : "));
                }
                else
                {
                    label9.Text = "DFS : OFF";
                }
            }
            else
            {
                label10.Text = "BFS : ";
                label9.Text = "DFS : ";
            }

            if (players.Count > 0)
            {
                SimulateMove(players, (int)numericUpDown2.Value);
            }

            if (isWrite)
            {
                if (!isSecond)
                {
                    Write_CSV(@"D:\이동하 Daniel\코딩&메이커\Team ToyoTech\maze\maze_data.csv");
                }
                else
                {
                    Write_CSV(@"D:\이동하 Daniel\코딩&메이커\Team ToyoTech\maze\2_maze_data.csv");
                }
            }
            button1.Enabled = true;
        }

        private void Write_CSV(string csvFilePath)
        {
            if (csvFilePath == null || csvFilePath == "")
            {
                return;
            }

            decimal[] rowData = { (int)numericUpDown1.Value, (int)numericUpDown2.Value,
                ExtractTime(label1), ExtractTime(label2), ExtractTime(label11), ExtractTime(label12) };
            if (isSecond)
            {
                rowData = new decimal[] { (int)numericUpDown1.Value, (int)numericUpDown2.Value,
                    ExtractTime(label1), ExtractTime(label2), ExtractTime(label11), ExtractTime(label12),
                    ExtractTime(label10), ExtractTime(label9) };
            }

            using (StreamWriter sw = new StreamWriter(csvFilePath, append: true))
            {
                sw.WriteLine(string.Join(",", rowData));
            }
        }

        private decimal ExtractTime(Label label)
        {
            var parts = label.Text.Split(' ');
            if (parts.Length >= 3 && decimal.TryParse(parts[2], out decimal value))
            {
                return value;
            }
            return 0;
        }

        private void numericUpDown1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick(); // Enter 키를 누르면 버튼 클릭 이벤트 발생
            }
            else if (e.KeyCode == Keys.Space)
            {
                button2.PerformClick(); // Space 키를 누르면 버튼 클릭 이벤트 발생
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Enabled = false;
            numericUpDown1.Enabled = false;
            numericUpDown2.Enabled = false;
            numericUpDown3.Enabled = false;
            for (int i = 0; i < numericUpDown3.Value; i++)
            {
                label7.Text = "횟수 = " + (i + 1).ToString();
                button1.PerformClick();
                button2.PerformClick();
            }
            button3.Enabled = true;
            numericUpDown1.Enabled = true;
            numericUpDown2.Enabled = true;
            numericUpDown3.Enabled = true;
            label7.Text = "횟수 = 0";
        }

        private void numericUpDown3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button3.PerformClick();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            isSecond = !isSecond;
            button4.Text = "2차 " + (isSecond ? "OFF" : "ON");
            label8.Text = isSecond.ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            isWrite = !isWrite;
            button5.Text = "기록 " + (isWrite ? "OFF" : "ON");
            label5.Text = isWrite.ToString();
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
