using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Checkers
{
    class OldBoard
    {
        public static readonly int NR_ROWS = 8;
        public static readonly int NR_COLS = 8;
        private Board[,] board = new Board[NR_ROWS, NR_COLS];

        public OldBoard()
        {
            for (int row = 0; row < NR_ROWS; ++row)
            {
                for (int col = 0; col < NR_COLS; ++col)
                {
                    if (((row == 0 || row == 2) && col % 2 != 0) || (row == 1 && col % 2 == 0))
                    {
                        board[row, col] = new Board(Board.PawnStatus.BlackPawn, row, col);
                    }
                    else if (((row == 5 || row == 7) && col % 2 == 0) || (row == 6 && col % 2 != 0))
                    {
                        board[row, col] = new Board(Board.PawnStatus.RedPawn, row, col);
                    }
                    else
                    {
                        board[row, col] = new Board(Board.PawnStatus.None, row, col);
                    }
                }
            }
        }

        public OldBoard(Board[,] other)
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

        public List<Board> getJumpables(bool who)
        {
            List<Board> jumpables = new List<Board>();
            foreach (Board cell in board)
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

        private bool isBlack(bool who, Board board)
        {
            return who.Equals(Player.BLACK) && board.color.Equals(Board.PawnStatus.BlackPawn);
        }
        private bool isRed(bool who, Board board)
        {
            return who.Equals(Player.RED) && board.color.Equals(Board.PawnStatus.RedPawn);
        }

        private bool isChecked(Board board)
        {
            return (checkNorth(board) || (board.isKing && checkSouth(board)))
                    || (checkSouth(board) || (board.isKing && checkNorth(board)));
        }
        
        private bool checkNorth(Board board)
        {
            bool canJump = false;
            if (board.x >= 2)
            {
                Board beingChecked = this.board[board.x - 1, board.y - 1];
                if (board.y >= 2) //check left
                {
                    if (isNoMatch(beingChecked, board)) 
                    {
                        canJump = isEmpty(this.board[board.x - 2, board.y - 2]);
                    }
                }
                if (!canJump && board.y <= 5)
                {
                    beingChecked = this.board[board.x + 1, board.y + 1];
                    if (isNoMatch(beingChecked, board)) 
                    {
                        canJump = isEmpty(this.board[board.x + 2, board.y + 2]);
                    }

                }
            }
            return canJump;
        }

        private bool checkSouth(Board board)
        {
            throw new NotImplementedException();
        }
        
        private bool isNoMatch(Board checking, Board currentBoard)
        {
            return (!checking.color.Equals(currentBoard.color) && (!checking.color.Equals(Board.PawnStatus.None)));
        }
        private bool isEmpty(Board board)
        {
            return board.color.Equals(Board.PawnStatus.None);
        }

        public Board[,] getBoard()
        {
            return board;
        }
    }
}
