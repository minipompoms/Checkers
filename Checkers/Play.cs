using System;
using System.Collections.Generic;
using System.Drawing;

namespace Checkers
{
   
    public static class Play
    {
        
        public static bool IsMovePossible(Board[][] board, Move move, List<Point> checkersToRemove = default(List<Point>))
        {
            if (checkersToRemove == null)
            {
                checkersToRemove = new List<Point>();
            }

            if (move.XEnd < 0 || move.XEnd > 7 || move.YEnd < 0 || move.YEnd > 7)
            {
                return false;
            }
            
            Board destination = board[move.XEnd][move.YEnd];
            if (destination.Check != null)
            {
                return false;
            }

            Board start = board[move.XStart][move.YStart];
            if (start.Check == null)
            {
                return false;
            }
            bool checkKing = start.Check.IsKing;
            bool isForward = start.Check.isAI && move.YEnd >= move.YStart;
            isForward = isForward || (!start.Check.isAI && move.YEnd <= move.YStart);
            if (!isForward && !checkKing)
            {
                return false;
            }
            if (move.IsBasicMove && isForward)
            {
                return true;
            }
            if (move.IsBasicMove && checkKing)
            {
                return true;
            }
            else if ((Math.Abs(move.XEnd - move.XStart) == 2 && Math.Abs(move.YEnd - move.YStart) == 2))
            {
                if (checkersToRemove == null)
                {
                    checkersToRemove = new List<Point>();
                }
                var xFieldBetween = move.XStart + (move.XEnd - move.XStart) / 2;
                var yFieldBetween = move.YStart + (move.YEnd - move.YStart) / 2;
                Board between = board[xFieldBetween][yFieldBetween];
                if (between.Check == null)
                {
                    return false;
                }
                if (!start.Check.isAI)
                {
                    if (between.Check.isAI)
                    {
                        checkersToRemove.Add(new Point(xFieldBetween, yFieldBetween));
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
                else if (start.Check.isAI)
                {
                    if (!between.Check.isAI)
                    {
                        checkersToRemove.Add(new Point(xFieldBetween, yFieldBetween));
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
                //IsGoodRecurring(board, move, checkersToRemove, ref atLeastOneGood);
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
                            if (board[x0][y0].Check != null)
                            {
                                checkersToRemove.Add(new Point(x0, y0));
                            }
                            x0 += xmodifier;
                            y0 += yModifier;

                        } while (x0 != move.XEnd);
                        if (checkersToRemove.Count <= 1)
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

        private static bool IsGoodRecurring(Board[][] board, Move move, List<Point> checkersToRemove, ref bool atLeastOneGood)
        {
            if (checkersToRemove == null)
            {
                checkersToRemove = new List<Point>();
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
                Board[][] fakeBoard = board.DeepCopy();
                if (IsMovePossible(fakeBoard, possibility, checkersToRemove))
                {
                    atLeastOneGood = true;
                    fakeBoard[possibility.XEnd][possibility.YEnd].Check = fakeBoard[possibility.XStart][possibility.YStart].Check;
                    checkersToRemove.Add(new Point(possibility.XStart, possibility.YStart));
                    Move fakeMove = new Move(possibility.XEnd, possibility.YEnd, -1, -1);
                    if (IsGoodRecurring(fakeBoard, fakeMove, checkersToRemove,ref atLeastOneGood))
                    {
                        return true;
                    }
                   
                }
            }
            return atLeastOneGood;
        }

        public static bool IsMovePossible(Board[][] board, int x_start, int y_start, int x_end, int y_end, List<Point> checkersToRemove = default(List<Point>))
        {
            Move move = new Move(x_start, y_start, x_end, y_end);
            return IsMovePossible(board, move, checkersToRemove);
        }
    }
}