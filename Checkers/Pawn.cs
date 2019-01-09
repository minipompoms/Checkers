namespace Checkers
{
    public class Pawn
    {
        public bool IsKing;
        public readonly bool isAI;

        public Pawn(bool isKing, bool isAI)
        {
            this.IsKing = isKing;
            this.isAI = isAI;
        }

    }
}