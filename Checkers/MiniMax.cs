using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Checkers
{
    class MiniMax : IAI
    {
        const int AI_TREEDEPTH = 3;

        const int WEIGHT_SINGLECHECKER = 2;
        const int WEIGHT_KING = 4;

        Tree<Move> gameTree;

        public Move GetNextMove(Board[][] board)
        {
            Console.WriteLine();
            Console.WriteLine("Building Game Tree...");

            gameTree = new Tree<Move>(new Move(-1, -1, -1, -1));
            var possibleMoves = GetPossibleMoves(board.DeepCopy(), true);
            foreach (Move myPossibleMove in possibleMoves)
            {
                var isMaxing = true;
                CalculateChildTree(AI_TREEDEPTH, gameTree.AddChild(myPossibleMove), board.DeepCopy(), isMaxing);
            }

            Move nextMove = GetBestMove(gameTree);
            return nextMove;
        }

        private void CalculateChildTree(int depth, Tree<Move> tree, Board[][] board, bool isMaxing)
        {
            try
            {
                board.MakeMove(tree.Value.X_Start, tree.Value.Y_Start, tree.Value.X_End, tree.Value.Y_End);
                tree.Score = ScoreBoard(board, isMaxing);
                if (depth > 0)
                {
                    var possibleMoves = GetPossibleMoves(board.DeepCopy(), isMaxing);
                    foreach (Move nextMove in possibleMoves)
                    {
                        CalculateChildTree(depth - 1, tree.AddChild(nextMove), board.DeepCopy(), !isMaxing);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            //should i return sth or not
        }

        private List<Move> GetPossibleMoves(Board[][] board, bool getAiMoves)
        {
            var possibleMoves = new List<Move>();
            var openSquares = GetOpenSquares(board);
            for (int i = 0; i < board.Length; i++)
            {
                for (int j = 0; j < board.Length; j++)
                {
                    var check = board[i][j].Check;
                    if (check != null && check.isAI == getAiMoves)
                    {
                        foreach (var square in openSquares)
                        {
                            var tryMove = new Move(i, j, (int)square.X, (int)square.Y);
                            if (Play.IsMovePossible(board, tryMove))
                            {
                                possibleMoves.Add(tryMove);
                            }
                        }
                    }
                }
            }
            return possibleMoves;
        }
        private List<Point> GetOpenSquares(Board[][] board)
        {
            var openSquares = new List<Point>();
            for (int i = 0; i < board.Length; i++)
            {
                for (int j = 0; j < board.Length; j++)
                {
                    if (board[i][j].Check == null && (i + j) % 2 != 0)
                    {
                        openSquares.Add(new Point(i, j));
                    }
                }
            }
            return openSquares;
        }
              
        private float ScoreBoard(Board[][] board, bool isAiMax)
        {
            float score = 0;
            for (int i = 0; i < board.Length; i++)
            {
                for (int j = 0; j < board.Length; j++)
                {
                    var checker = board[i][j].Check;
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