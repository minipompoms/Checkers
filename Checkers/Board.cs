using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    class Board
    {
        public static readonly int NR_ROWS = 8;
        public static readonly int NR_COLS = 8;
        private Cell[,] board = new Cell[NR_ROWS, NR_COLS];

        public Board()
        {
            for (int row = 0; row < NR_ROWS; ++row)
            {
                for (int col = 0; col < NR_COLS; ++col)
                {
                    if (((row == 0 || row == 2) && col % 2 != 0) || (row == 1 && col % 2 == 0))
                    {
                        board[row, col] = new Cell(Cell.contents.BLACK, row, col);
                    }
                    else if (((row == 5 || row == 7) && col % 2 == 0) || (row == 6 && col % 2 != 0))
                    {
                        board[row, col] = new Cell(Cell.contents.RED, row, col);
                    }
                    else
                    {
                        board[row, col] = new Cell(Cell.contents.NONE, row, col);
                    }
                }
            }
        }

        public Board(Cell[,] other)
        {
            for (int row = NR_ROWS - 1; row >= 0; --row)
            {
                for (int col = 0; col < NR_COLS; ++col)
                {
                    board[row, col] = other[row, col];
                }
            }
        }

        //public Board makeMove(bool who, int column)
        //{
        //    Board newBoard = new Board(board);
        //    int row = 0;
        //    while (row < NR_ROWS && board[row, column] != new Cell(Cell.contents.NONE))
        //    {
        //        ++row;
        //    }
        //    if (row < NR_ROWS)
        //    {
        //        newBoard.board[row, column] = who == Player.MAX ? Cell.RED : Cell.BLACK;
        //    }
        //    else
        //    {
        //        // this cannot happen
        //        Console.WriteLine("Booga booga!");
        //    }
        //    return newBoard;
        //}

        public List<Cell> getJumpables(bool who)
        {
            List<Cell> jumpables = new List<Cell>();
            foreach (Cell cell in board)
            {
                if (who == Player.RED && cell.color == Cell.contents.RED)
                {
                    if (checkDown(cell) || (cell.king && checkUp(cell)))
                    {
                        jumpables.Add(cell);
                    }
                }
                else if (who == Player.BLACK && cell.color == Cell.contents.BLACK)
                {
                    if (checkUp(cell) || (cell.king && checkDown(cell)))
                    {
                        jumpables.Add(cell);
                    }
                }
            }
            return jumpables;
        }

        private bool checkUp(Cell cell)
        {
            bool canJump = false;
            if (cell.x >= 2)
            {
                Cell beingChecked = board[cell.x - 1, cell.y - 1];
                if (cell.y >= 2) //check left
                {
                    if (beingChecked.color != cell.color && beingChecked.color != Cell.contents.NONE)
                    {
                        canJump = cellIsEmpty(board[cell.x - 2, cell.y - 2]);
                    }
                }
                if (!canJump && cell.y <= 5)
                {
                    beingChecked = board[cell.x + 1, cell.y + 1];
                    if (beingChecked.color != cell.color && beingChecked.color != Cell.contents.NONE)
                    {
                        canJump = cellIsEmpty(board[cell.x + 2, cell.y + 2]);
                    }

                }
            }
            return canJump;
        }
        
        private bool cellIsEmpty(Cell cell)
        {
            bool isEmpty = false;
            if (cell.color == Cell.contents.NONE)
            {
                isEmpty = true;
            }
            return isEmpty;
        }

        private bool checkDown(Cell cell)
        {
            throw new NotImplementedException();
        }
    }
}
