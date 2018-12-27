using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
                        board[row, col] = new Cell(Cell.Contents.BLACK, row, col);
                    }
                    else if (((row == 5 || row == 7) && col % 2 == 0) || (row == 6 && col % 2 != 0))
                    {
                        board[row, col] = new Cell(Cell.Contents.RED, row, col);
                    }
                    else
                    {
                        board[row, col] = new Cell(Cell.Contents.NONE, row, col);
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
                
                if (isRed(who, cell))
                {
                    if (isChecked(cell))
                    {
                        jumpables.Add(cell);
                    }
                }
                else if (isBlack(who, cell))
                {
                    if (isChecked(cell))
                    {
                        jumpables.Add(cell);
                    }
                }
            }
            return jumpables;
        }

        private bool isBlack(bool who, Cell cell)
        {
            return who.Equals(Player.BLACK) && cell.color.Equals(Cell.Contents.BLACK);
        }
        private bool isRed(bool who, Cell cell)
        {
            return who.Equals(Player.RED) && cell.color.Equals(Cell.Contents.RED);
        }

        private bool isChecked(Cell cell)
        {
            return (checkNorth(cell) || (cell.king && checkSouth(cell)))
                    || (checkSouth(cell) || (cell.king && checkNorth(cell)));
        }
        
        private bool checkNorth(Cell cell)
        {
            bool canJump = false;
            if (cell.x >= 2)
            {
                Cell beingChecked = board[cell.x - 1, cell.y - 1];
                if (cell.y >= 2) //check left
                {
                    if (isNoMatch(beingChecked, cell)) 
                    {
                        canJump = isEmpty(board[cell.x - 2, cell.y - 2]);
                    }
                }
                if (!canJump && cell.y <= 5)
                {
                    beingChecked = board[cell.x + 1, cell.y + 1];
                    if (isNoMatch(beingChecked, cell)) 
                    {
                        canJump = isEmpty(board[cell.x + 2, cell.y + 2]);
                    }

                }
            }
            return canJump;
        }

        private bool checkSouth(Cell cell)
        {
            throw new NotImplementedException();
        }
        
        private bool isNoMatch(Cell checking, Cell currentCell)
        {
            return (!checking.color.Equals(currentCell.color) && (!checking.color.Equals(Cell.Contents.NONE)));
        }
        private bool isEmpty(Cell cell)
        {
            return cell.color.Equals(Cell.Contents.NONE);
        }

        public Cell[,] getBoard()
        {
            return board;
        }
    }
}
