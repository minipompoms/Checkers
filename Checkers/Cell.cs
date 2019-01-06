namespace Checkers
{
    public class Cell
    {
        public enum Pawn { RED, BLACK, NONE };
        public Pawn color { get; set; }

        public bool king { get; set; }
        public int x { get; }
        public int y { get; }


        public Cell(Pawn con, int row, int col)
        {
            color = con;
            x = row;
            y = col;
        }

        public void clearCell()
        {
            color = Pawn.NONE;
        }

        public bool changeColor(bool who)
        {
            if (color != Pawn.NONE)
            {
                if (who.Equals(Player.BLACK))
                {
                    color = Pawn.BLACK;
                    return true;
                }
                if (who.Equals(Player.RED))
                {
                    color = Pawn.RED;
                    return true;
                }
            }
            return false;
        }
    }
}