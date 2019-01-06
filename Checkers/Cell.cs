namespace Checkers
{
    public class Cell
    {
        public enum Contents { RED, BLACK, NONE };
        public Contents color { get; set; }

        public bool king { get; set; }
        public int x { get; }
        public int y { get; }


        public Cell(Contents con, int row, int col)
        {
            color = con;
            x = row;
            y = col;
        }

        public void clearCell()
        {
            color = Contents.NONE;
        }

        public bool changeColor(bool who)
        {
            if (color != Contents.NONE)
            {
                if (who.Equals(Player.BLACK))
                {
                    color = Contents.BLACK;
                    return true;
                }
                if (who.Equals(Player.RED))
                {
                    color = Contents.RED;
                    return true;
                }
            }
            return false;
        }
    }
}