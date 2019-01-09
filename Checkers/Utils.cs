using System.Collections.Generic;
using System.Drawing;

namespace Checkers
{
    public static class Utils
    {
        
        public static bool MakeMove(this Board[][] board, Move move)
        {
            return board.MakeMove(move.XStart, move.YStart, move.XEnd, move.YEnd);
        }
        public static bool MakeMove(this Board[][] board,int xStart, int yStart, int xEnd, int yEnd)
        {
            List<Point> checkersToRemove = new List<Point>();
            if (Play.IsMovePossible(board, xStart, yStart, xEnd, yEnd, checkersToRemove))
            {
                var movingChecker = board[xStart][yStart].Check;
                if (board[xEnd][yEnd].IsKing)
                {
                    if (!movingChecker.IsKing)
                    {
                        movingChecker.IsKing = true;
                    }
                }
                board[xEnd][yEnd].Check = movingChecker;
                board[xStart][yStart].Check = null;
                foreach (var field in checkersToRemove)
                {
                    board[field.X][field.Y].Check = null;
                }
                return true;
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
                        check = new Pawn(sourceBoard[i][j].Check.IsKing, sourceBoard[i][j].Check.isAI);
                    }
                    result[i][j] = new Board(check, sourceBoard[i][j].IsKing);
                }
            }

            return result;
        }
    }
}