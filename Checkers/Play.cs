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

            if (move.X_End < 0 || move.X_End > 7 || move.Y_End < 0 || move.Y_End > 7)
            {
                return false;
            }
            
            Board destination = board[move.X_End][move.Y_End];
            if (destination.Check != null)
            {
                return false;
            }

            Board start = board[move.X_Start][move.Y_Start];
            if (start.Check == null)
            {
                return false;
            }
            bool checkKing = start.Check.isKing;
            bool isForward = start.Check.isAI && move.Y_End >= move.Y_Start;
            isForward = isForward || (!start.Check.isAI && move.Y_End <= move.Y_Start);
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
            else if ((Math.Abs(move.X_End - move.X_Start) == 2 && Math.Abs(move.Y_End - move.Y_Start) == 2))
            {
                if (checkersToRemove == null)
                {
                    checkersToRemove = new List<Point>();
                }
                var x_FieldBetween = move.X_Start + (move.X_End - move.X_Start) / 2;
                var y_FieldBetween = move.Y_Start + (move.Y_End - move.Y_Start) / 2;
                Board between = board[x_FieldBetween][y_FieldBetween];
                if (between.Check == null)
                {
                    return false;
                }
                if (!start.Check.isAI)
                {
                    if (between.Check.isAI)
                    {
                        checkersToRemove.Add(new Point(x_FieldBetween, y_FieldBetween));
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
                        checkersToRemove.Add(new Point(x_FieldBetween, y_FieldBetween));
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
                    if (Math.Abs(move.X_End - move.X_Start) == Math.Abs(move.Y_End - move.Y_Start))
                    {
                        var Xmodifier = Math.Sign(move.X_End - move.X_Start) * 1;
                        var YModifier = Math.Sign(move.Y_End - move.Y_Start) * 1;
                        int x0 = move.X_Start + Xmodifier;
                        int y0 = move.Y_Start + YModifier;
                        do
                        {
                            if (board[x0][y0].Check != null)
                            {
                                checkersToRemove.Add(new Point(x0, y0));
                            }
                            x0 += Xmodifier;
                            y0 += YModifier;

                        } while (x0 != move.X_End);
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
                    new Move(move.X_Start,move.Y_Start, move.X_Start + 2, move.Y_Start + 2),
                    new Move(move.X_Start,move.Y_Start, move.X_Start - 2, move.Y_Start + 2),
                    new Move(move.X_Start,move.Y_Start, move.X_Start + 2, move.Y_Start - 2),
                    new Move(move.X_Start,move.Y_Start, move.X_Start - 2, move.Y_Start - 2)
                };
            foreach (var possibility in possibilities)
            {
                Board[][] fakeBoard = board.DeepCopy();
                if (IsMovePossible(fakeBoard, possibility, checkersToRemove))
                {
                    atLeastOneGood = true;
                    fakeBoard[possibility.X_End][possibility.Y_End].Check = fakeBoard[possibility.X_Start][possibility.Y_Start].Check;
                    checkersToRemove.Add(new Point(possibility.X_Start, possibility.Y_Start));
                    Move fakeMove = new Move(possibility.X_End, possibility.Y_End, -1, -1);
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