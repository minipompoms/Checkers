using System.Collections.Generic;
using System.Drawing;

namespace Checkers
{
    public static class Utils
    {
        
        public static bool MakeMove(this Board[][] board, Move move)
        {
            return board.MakeMove(move.X_Start, move.Y_Start, move.X_End, move.Y_End);
        }
        public static bool MakeMove(this Board[][] board,int x_start, int y_start, int x_end, int y_end)
        {
            List<Point> checkersToRemove = new List<Point>();
            if (Play.IsMovePossible(board, x_start, y_start, x_end, y_end, checkersToRemove))
            {
                var movingChecker = board[x_start][y_start].Check;
                if (board[x_end][y_end].IsKing)
                {
                    if (!movingChecker.isKing)
                    {
                        movingChecker.isKing = true;
                    }
                }
                board[x_end][y_end].Check = movingChecker;
                board[x_start][y_start].Check = null;
                foreach (var field in checkersToRemove)
                {
                    board[(int)field.X][(int)field.Y].Check = null;
                }
                return true;
            }
            else
            {
            }
            return false;
        }
        public static Board[][] DeepCopy(this Board[][] sourceBoard)
        {
            Board[][] result = new Board[sourceBoard.Length][];

            for (int i = 0; i < sourceBoard.Length; i++)
            {
                result[i] = new Board[sourceBoard.Length];
                for (int j = 0; j < sourceBoard.Length; j++)
                {
                    Pawn check = null;
                    if (sourceBoard[i][j].Check != null)
                    {
                        check = new Pawn(sourceBoard[i][j].Check.isKing, sourceBoard[i][j].Check.isAI);
                    }
                    result[i][j] = new Board(check, sourceBoard[i][j].IsKing);
                }
            }

            return result;
        }
    }
}