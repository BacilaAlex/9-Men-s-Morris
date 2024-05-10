namespace _9_Men
{
    public class Intersection
    {
        public bool IsOccupied { get; set; }
        public PieceType OccupiedPiece { get; set; }

        public Intersection(bool isOccupied, PieceType occupiedPiece)
        {
            OccupiedPiece = occupiedPiece;
            IsOccupied = isOccupied;
        }
    }
}
