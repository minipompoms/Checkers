using System.Collections.Generic;
using System.Drawing;

namespace Checkers
{
    public static class Utils
    {
        
        public static bool MakeMove(this Cell[][] cell, Move move)
        {
            return cell.MakeMove(move.XStart, move.YStart, move.XEnd, move.YEnd);
        }
        public static bool MakeMove(this Cell[][] cell,int xStart, int yStart, int xEnd, int yEnd)
        {
            List<Point> checkersToRemove = new List<Point>();
            if (Play.IsMovePossible(cell, xStart, yStart, xEnd, yEnd, checkersToRemove))
            {
                var movingChecker = cell[xStart][yStart].StatusCheck;
                if (cell[xEnd][yEnd].IsKing)
                {
                    if (!movingChecker.isKing)
                    {
                        movingChecker.isKing = true;
                    }
                }
                cell[xEnd][yEnd].StatusCheck = movingChecker;
                cell[xStart][yStart].StatusCheck = null;
                foreach (var field in checkersToRemove)
                {
                    cell[field.X][field.Y].StatusCheck = null;
                }
                return true;
            }
           
            return false;
        }
        public static Cell[][] DeepCopy(this Cell[][] sourceCell)
        {
            Cell[][] result = new Cell[sourceCell.Length][];

            for (int i = 0; i < sourceCell.Length; i++)
            {
                result[i] = new Cell[sourceCell.Length];
                for (int j = 0; j < sourceCell.Length; j++)
                {
                    Pawn check = null;
                    if (sourceCell[i][j].StatusCheck != null)
                    {
                        check = new Pawn(sourceCell[i][j].StatusCheck.isKing, sourceCell[i][j].StatusCheck.isAI);
                    }
                    result[i][j] = new Cell(check, sourceCell[i][j].IsKing);
                }
            }

            return result;
        }
    }
}