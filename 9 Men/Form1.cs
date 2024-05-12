namespace _9_Men
{
    public partial class Form1 : Form
    {
        public Intersection[,] Board = new Intersection[7, 7];
        public Form1()
        {
            InitializeComponent();
        }

        public void InitializeBoard()
        {
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    PictureBox pictureBox = new PictureBox();
                    pictureBox.Tag = new Point(i, j);
                    this.Controls.Add(pictureBox);
                    Board[i, j] = new Intersection(false, PieceType.None);
                }
            }
        }

        public PieceType currentPlayer = PieceType.White;

        static Dictionary<(int, int), List<(int, int)>> intersectionConnections = new Dictionary<(int, int), List<(int, int)>>()
        {
            { (0, 0), new List<(int, int)>{ (0, 3), (3, 0) } },
            { (0, 3), new List<(int, int)>{ (0, 0), (0, 6), (1, 3) } },
            { (0, 6), new List<(int, int)>{ (0, 3), (3, 6) } },
            { (1, 1), new List<(int, int)>{ (1, 3), (3, 1) } },
            { (1, 3), new List<(int, int)>{ (1, 1), (1, 5), (0, 3) } },
            { (2, 2), new List<(int, int)>{ (2, 3), (3, 2) } },
            { (2, 3), new List<(int, int)>{ (2, 2), (2, 4), (1, 3) } },
            { (2, 4), new List<(int, int)>{ (2, 3), (3, 4) } },
            { (3, 0), new List<(int, int)>{ (3, 1), (0, 0), (6, 0) } },
            { (3, 1), new List<(int, int)>{ (3, 0), (3, 2), (1, 1), (5, 1) } },
            { (3, 2), new List<(int, int)>{ (3, 1), (4, 2), (2, 2) } },
            { (3, 4), new List<(int, int)>{ (4, 4), (2, 4), (3, 5) } },
            { (3, 5), new List<(int, int)>{ (3, 4), (3, 6), (1, 5), (5, 5) } },
            { (3, 6), new List<(int, int)>{ (0, 6), (6, 6), (3, 5) } },
            { (4, 2), new List<(int, int)>{ (3, 2), (4, 3) } },
            { (4, 3), new List<(int, int)>{ (4, 2), (4, 4), (5, 3) } },
            { (4, 4), new List<(int, int)>{ (4, 3), (3, 4) } },
            { (5, 1), new List<(int, int)>{ (3, 1), (5, 3) } },
            { (5, 3), new List<(int, int)>{ (5, 1), (4, 3), (6, 3), (5, 5) } },
            { (5, 5), new List<(int, int)>{ (5, 3), (3, 5) } },
            { (6, 0), new List<(int, int)>{ (3, 0), (6, 3) } },
            { (6, 3), new List<(int, int)>{ (6, 0), (5, 3), (6, 6) } },
            { (6, 6), new List<(int, int)>{ (6, 3), (3, 6) } }
        };

        List<List<(int, int)>> possibleMills = new List<List<(int, int)>>()
        {
            new List<(int, int)>{ (0, 0), (0, 3), (0, 6) },
            new List<(int, int)>{ (0, 3), (3, 3), (6, 3) },
            new List<(int, int)>{ (6, 0), (6, 3), (6, 6) },
            new List<(int, int)>{ (0, 6), (3, 6), (6, 6) },
            new List<(int, int)>{ (1, 1), (1, 3), (1, 5) },
            new List<(int, int)>{ (1, 1), (3, 1), (5, 1) },
            new List<(int, int)>{ (5, 1), (5, 3), (5, 5) },
            new List<(int, int)>{ (5, 5), (3, 5), (1, 5) },
            new List<(int, int)>{ (2, 2), (2, 3), (2, 4) },
            new List<(int, int)>{ (2, 2), (3, 2), (4, 2) },
            new List<(int, int)>{ (4, 2), (4, 3), (4, 4) },
            new List<(int, int)>{ (4, 4), (3, 4), (2, 4) },
            new List<(int, int)>{ (0, 3), (1, 3), (2, 3) },
            new List<(int, int)>{ (3, 4), (3, 5), (3, 6) },
            new List<(int, int)>{ (4, 3), (5, 3), (6, 3) },
            new List<(int, int)>{ (3, 0), (3, 1), (3, 2) }
        };

        static string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        static string whitePieceImagePath = Path.Combine(projectDirectory, "Pieces", "WhitePiece.jpg");
        static string blackPieceImagePath = Path.Combine(projectDirectory, "Pieces", "BlackPiece.jpg");
        Image whitePieceImage = Image.FromFile(whitePieceImagePath);
        Image blackPieceImage = Image.FromFile(blackPieceImagePath);


        int countBlack = 9;
        int countWhite = 9;

        bool pieceSelected = false;
        Point selectedPieceCoords = new Point();
        bool isMill = false;

        public void IntersectinClick(object sender, EventArgs e)
        {
            var clickedIntersection = sender as PictureBox;

            var position = (Point)clickedIntersection.Tag;

            var row = position.X;
            var col = position.Y;


            if (isMill)
            {
                if (Board[row, col].IsOccupied && Board[row, col].OccupiedPiece != currentPlayer)
                {
                    Board[row, col].IsOccupied = false;
                    Board[row, col].OccupiedPiece = PieceType.None;

                    isMill = false;

                    ChangeTurn();
                }
            }

            else if (countBlack > 0 && countWhite > 0)
            {
                if (!Board[row, col].IsOccupied)
                {
                    Board[row, col].IsOccupied = true;
                    Board[row, col].OccupiedPiece = currentPlayer;
                    if (currentPlayer == PieceType.Black)
                    {
                        clickedIntersection.Image = blackPieceImage;
                        countBlack--;
                    }
                    else
                    {
                        clickedIntersection.Image = whitePieceImage;
                        countWhite--;
                    }

                    if (CheckMill((row, col), currentPlayer))
                    {
                        isMill = true;
                        //Highlight possible pieces to take
                    }
                    else
                    {
                        ChangeTurn();
                    }
                }
            }
            else
            {
                if (Board[row, col].IsOccupied)
                {
                    if (Board[row, col].OccupiedPiece.Equals(currentPlayer))
                    {
                        pieceSelected = true;
                        selectedPieceCoords = new Point(row, col);
                        // Highlight possible moves

                    }
                }
                else
                {
                    if (pieceSelected)
                    {
                        //Check for Valid Move
                        if (ValidateMove(row, col))
                        {
                            Board[row, col].OccupiedPiece = currentPlayer;
                            Board[row, col].IsOccupied = true;

                            Board[selectedPieceCoords.X, selectedPieceCoords.Y].IsOccupied = false;
                            Board[selectedPieceCoords.X, selectedPieceCoords.Y].OccupiedPiece = PieceType.None;

                            pieceSelected = false;

                            if (CheckMill((row, col), currentPlayer))
                            {
                                isMill = true;
                                //Highlight possible pieces to take
                            }
                            else
                            {
                                ChangeTurn();
                            }
                        }
                    }
                }
            }

        }

        private bool ValidateMove(int x, int y)
        {
            (int, int)pieceIntersection = (selectedPieceCoords.X,  selectedPieceCoords.Y);

            if (!Board[x, y].IsOccupied)
            {
                if (intersectionConnections.ContainsKey(pieceIntersection))
                {
                    List<(int, int)> connectedIntersections = intersectionConnections[pieceIntersection];
                    return connectedIntersections.Contains((x, y));
                }
            }
            return false;
        }

        private bool CheckMill((int, int) chosenIntersection, PieceType player)
        {
            foreach (var possibleMill in possibleMills)
            {
                if (possibleMill.Contains(chosenIntersection))
                {
                    bool isEveryPieceSameColor = true;
                    foreach (var intersection in possibleMill)
                    {
                        int intersectionX, intersectionY;
                        (intersectionX, intersectionY) = intersection;
                        if (!Board[intersectionX, intersectionY].IsOccupied || !(Board[intersectionX, intersectionY].OccupiedPiece != player))
                        {
                            isEveryPieceSameColor = false; 
                            break;
                        }
                    }
                    if (isEveryPieceSameColor)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void ChangeTurn()
        {
            if (currentPlayer == PieceType.White)
            {
                currentPlayer = PieceType.Black;
            }
            else
            {
                currentPlayer = PieceType.White;
            }
        }
    }
}

