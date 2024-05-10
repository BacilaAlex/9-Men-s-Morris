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


            if (countBlack > 0 && countWhite > 0)
            {
                if (!Board[row, col].IsOccupied)
                {
                    Board[row, col].IsOccupied = true;
                    Board[row, col].OccupiedPiece = currentPlayer;
                    if (currentPlayer == PieceType.Black)
                    {
                        clickedIntersection.Image = blackPieceImage;
                    }
                    else
                    {
                        clickedIntersection.Image = whitePieceImage;
                    }
                }

                if (currentPlayer.Equals(PieceType.White))
                {
                    currentPlayer = PieceType.Black;
                    countWhite--;
                }
                else
                {
                    currentPlayer = PieceType.White;
                    countBlack--;
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
                    else if (!Board[row, col].OccupiedPiece.Equals(currentPlayer) && isMill)
                    {
                        Board[row, col].IsOccupied = false;
                        Board[row, col].OccupiedPiece = PieceType.None;
                        isMill = false;
                    }
                }
                else
                {
                    if (pieceSelected)
                    {
                        //Check for Valid Move
                        Board[row, col].OccupiedPiece = currentPlayer;
                        Board[row, col].IsOccupied = true;
                        //Check for 3 Men in Line

                    }
                }
            }

        }
    }
}

