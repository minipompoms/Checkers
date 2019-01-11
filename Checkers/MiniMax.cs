using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Checkers
{
    internal class MiniMax : IAI
    {
        private int AI_TREEDEPTH = 3;

        private int WEIGHT_SINGLECHECKER = 2;
        private int WEIGHT_KING = 4;

        private Tree<Move> gameTree;

        public Move GetNextMove(Cell[][] cell)
        {
            Console.WriteLine();
            Console.WriteLine("Building BoardGame Tree...");

            gameTree = new Tree<Move>(new Move(-1, -1, -1, -1));
            var possibleMoves = GetPossibleMoves(cell.DeepCopy(), true);
            foreach (Move myPossibleMove in possibleMoves)
            {
                var isMaxing = true;
                CalculateChildTree(AI_TREEDEPTH, gameTree.AddChild(myPossibleMove), cell.DeepCopy(), isMaxing);
            }

            Move nextMove = GetBestMove(gameTree);
            return nextMove;
        }

        private void CalculateChildTree(int depth, Tree<Move> tree, Cell[][] cell, bool isMaxing)
        {
            try
            {
                cell.MakeMove(tree.Value.XStart, tree.Value.YStart, tree.Value.XEnd, tree.Value.YEnd);
                tree.Score = ScoreBoard(cell, isMaxing);
                if (depth > 0)
                {
                    var possibleMoves = GetPossibleMoves(cell.DeepCopy(), isMaxing);
                    foreach (Move nextMove in possibleMoves)
                    {
                        CalculateChildTree(depth - 1, tree.AddChild(nextMove), cell.DeepCopy(), !isMaxing);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

            }
            //should i return sth or not
        }

        private List<Move> GetPossibleMoves(Cell[][] cell, bool getAiMoves)
        {
            var possibleMoves = new List<Move>();
            var openSquares = GetOpenSquares(cell);
            for (int i = 0; i < cell.Length; i++)
            {
                for (int j = 0; j < cell.Length; j++)
                {
                    var check = cell[i][j].StatusCheck;
                    if (check != null && check.isAI == getAiMoves)
                    {
                        foreach (var square in openSquares)
                        {
                            var tryMove = new Move(i, j, (int)square.X, (int)square.Y);
                            if (Play.IsMovePossible(cell, tryMove))
                            {
                                possibleMoves.Add(tryMove);
                            }
                        }
                    }
                }
            }
            return possibleMoves;
        }
        private List<Point> GetOpenSquares(Cell[][] cell)
        {
            var openSquares = new List<Point>();
            for (int i = 0; i < cell.Length; i++)
            {
                for (int j = 0; j < cell.Length; j++)
                {
                    if (cell[i][j].StatusCheck == null && (i + j) % 2 == 0)
                    {
                        openSquares.Add(new Point(i, j));
                    }
                }
            }
            return openSquares;
        }
              
        private float ScoreBoard(Cell[][] cell, bool isAiMax)
        {
            float score = 0;
            for (int i = 0; i < cell.Length; i++)
            {
                for (int j = 0; j < cell.Length; j++)
                {
                    var checker = cell[i][j].StatusCheck;
                    if (checker != null)
                    {
                        if (checker.isAI)
                        {
                            score += WEIGHT_SINGLECHECKER;
                        }
                        if (!checker.isAI)
                        {
                            score -= WEIGHT_SINGLECHECKER;
                        }
                        if (checker.isAI && checker.isKing)
                        {
                            score += WEIGHT_KING;
                        }
                        if (!checker.isAI && checker.isKing)
                        {
                            score -= WEIGHT_KING;
                        }
                    }
                }
            }
            if (!isAiMax)
            {
                score = -1 * score;
            }
            return score;
        }

        private Move GetBestMove(Tree<Move> gameTree)
        {
            Move finaleMove = new Move(-2, -2, -2, -2);
            var bestscore = Minimax(AI_TREEDEPTH, gameTree, ref finaleMove, true);
            var lol = gameTree.Children.Select(x => x.Score);
            
            finaleMove = gameTree.Children.FirstOrDefault(x => x.Score == bestscore).Value;

            return finaleMove;
        }

        private float Minimax(int depth, Tree<Move> gameTree, ref Move finale, bool maximizingPlayer)
        {
            float bestScore = 0;
            if (depth == 0)
            {
                return gameTree.Score;
            }
            if (maximizingPlayer)
            {
                bestScore = -100000;
                foreach (var node in gameTree.Children)
                {
                    var value = Minimax(depth - 1, node, ref finale, false);
                    if (value > bestScore)
                    {
                        //finale = node.Value;
                        bestScore = value;
                    }
                }
            }
            else
            {
                bestScore = 100000;
                foreach (var node in gameTree.Children)
                {
                    var value = Minimax(depth - 1, node, ref finale, false);
                    if (value < bestScore)
                    {
                        bestScore = value;
                    }

                }
            }
            gameTree.Score = bestScore;
            return bestScore;
        }


    }
}