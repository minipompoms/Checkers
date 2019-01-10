using System;
using System.Collections.Generic;
using System.Drawing;

namespace Checkers
{
   
    public static class Play
    {
        
        public static bool IsMovePossible(Cell[][] cell, Move move, List<Point> pawnsToRemove = default(List<Point>))
        {
            if (pawnsToRemove == null)
            {
                pawnsToRemove = new List<Point>();
            }

            if (move.XEnd < 0 || move.XEnd > 7 || move.YEnd < 0 || move.YEnd > 7)
            {
                return false;
            }
            
            Cell destination = cell[move.XEnd][move.YEnd];
            if (destination.StatusCheck != null)
            {
                return false;
            }

            Cell start = cell[move.XStart][move.YStart];
            if (start.StatusCheck == null)
            {
                return false;
            }
            bool checkKing = start.StatusCheck.isKing;
            //there was a bug in the following 2 lines- since AI starts from the bottom of the board,
            //it should be <= for when StatusCheck.isAI and >= when it's the user (!StatusCheck.isAI)
            //also, we changed all the Ys to Xs because the Xs are the rows and that's what we're checking
            bool isForward = start.StatusCheck.isAI && move.XEnd <= move.XStart;
            isForward = isForward || (!start.StatusCheck.isAI && move.XEnd >= move.XStart);
            if (!isForward && !checkKing)
            {
                return false;
            }
            if (move.IsBasicMove && (isForward || checkKing))
            {
                return true;
            }
           
            else if ((Math.Abs(move.XEnd - move.XStart) == 2 && Math.Abs(move.YEnd - move.YStart) == 2))
            {
                if (pawnsToRemove == null)
                {
                    pawnsToRemove = new List<Point>();
                }
                var xFieldBetween = move.XStart + (move.XEnd - move.XStart) / 2;
                var yFieldBetween = move.YStart + (move.YEnd - move.YStart) / 2;
                Cell between = cell[xFieldBetween][yFieldBetween];
                if (between.StatusCheck == null)
                {
                    return false;
                }
                if (!start.StatusCheck.isAI)
                {
                    if (between.StatusCheck.isAI)
                    {
                        pawnsToRemove.Add(new Point(xFieldBetween, yFieldBetween));
                        return true;
                    }
                    else
                    {
                        if (checkKing)
                        {
                            return true;
                        }
                        return false;
                    }
                }
                else if (start.StatusCheck.isAI)
                {
                    if (!between.StatusCheck.isAI)
                    {
                        pawnsToRemove.Add(new Point(xFieldBetween, yFieldBetween));
                        return true;
                    }
                    else
                    {
                        if (checkKing)
                        {
                            return true;
                        }
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                //its recurring capture
                bool atLeastOneGood = false;
                //IsGoodRecurring(cell, move, pawnsToRemove, ref atLeastOneGood);
                if (atLeastOneGood)
                {
                    return true;
                }

                if (checkKing)
                {
                    //long move with one capture maximum
                    if (Math.Abs(move.XEnd - move.XStart) == Math.Abs(move.YEnd - move.YStart))
                    {
                        var xmodifier = Math.Sign(move.XEnd - move.XStart) * 1;
                        var yModifier = Math.Sign(move.YEnd - move.YStart) * 1;
                        int x0 = move.XStart + xmodifier;
                        int y0 = move.YStart + yModifier;
                        do
                        {
                            if (cell[x0][y0].StatusCheck != null)
                            {
                                pawnsToRemove.Add(new Point(x0, y0));
                            }
                            x0 += xmodifier;
                            y0 += yModifier;

                        } while (x0 != move.XEnd);
                        if (pawnsToRemove.Count <= 1)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        private static bool IsGoodRecurring(Cell[][] cell, Move move, List<Point> pawnsToRemove, ref bool atLeastOneGood)
        {
            if (pawnsToRemove == null)
            {
                pawnsToRemove = new List<Point>();
            }
            List<Move> possibilities = new List<Move>()
                {
                    new Move(move.XStart,move.YStart, move.XStart + 2, move.YStart + 2),
                    new Move(move.XStart,move.YStart, move.XStart - 2, move.YStart + 2),
                    new Move(move.XStart,move.YStart, move.XStart + 2, move.YStart - 2),
                    new Move(move.XStart,move.YStart, move.XStart - 2, move.YStart - 2)
                };
            foreach (var possibility in possibilities)
            {
                Cell[][] fakeCell = cell.DeepCopy();
                if (IsMovePossible(fakeCell, possibility, pawnsToRemove))
                {
                    atLeastOneGood = true;
                    fakeCell[possibility.XEnd][possibility.YEnd].StatusCheck = fakeCell[possibility.XStart][possibility.YStart].StatusCheck;
                    pawnsToRemove.Add(new Point(possibility.XStart, possibility.YStart));
                    Move fakeMove = new Move(possibility.XEnd, possibility.YEnd, -1, -1);
                    if (IsGoodRecurring(fakeCell, fakeMove, pawnsToRemove,ref atLeastOneGood))
                    {
                        return true;
                    }
                   
                }
            }
            return atLeastOneGood;
        }

        public static bool IsMovePossible(Cell[][] cell, int x_start, int y_start, int x_end, int y_end, List<Point> checkersToRemove = default(List<Point>))
        {
            Move move = new Move(x_start, y_start, x_end, y_end);
            return IsMovePossible(cell, move, checkersToRemove);
        }
    }
}