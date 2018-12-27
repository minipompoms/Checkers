namespace Checkers
{
    class Cell
    {
        public enum contents { RED, BLACK, NONE };
        public contents color { get; set; }
        public bool king { get; set; }
        public int x { get; }
        public int y { get; }

        public Cell(contents content, int row, int col)
        {
            color = content;
            x = row;
            y = col;
        }
    }
}