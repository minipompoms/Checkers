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
    }
}