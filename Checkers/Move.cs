using System;
using System.Collections.Generic;

namespace Checkers
{

    public class Move
    {
        /*
         * Maybe we should make the Board class have a Move (Instead of Move having a Board)? Because we want our move class
         * to constantly be updated with everything that happens on the board without having to manually update it or call board.getBoard()        
         */
        private OldBoard _oldBoard;
        private Board[,] currentBoard;



        public Move()
        {
            _oldBoard = new OldBoard();
        }
        public readonly int X_Start, Y_Start, X_End, Y_End;

        public Move(int x_start, int y_start, int x_end, int y_end)
        {
            this.X_Start = x_start;
            this.X_End = x_end;
            this.Y_Start = y_start;
            this.Y_End = y_end;
        }

        public bool IsBasicMove
        {
            get
            {
                bool is_X_single = Math.Abs(X_End - X_Start) == 1;
                bool is_Y_single = Math.Abs(Y_End - Y_Start) == 1;
                return is_X_single && is_Y_single;
            }
        }
        //This method should be called every time a move is made
        //Maybe we should also make a private List of jumpables and make methods to automatically update the jumpables list every time a move is made
        public void updateBoard()
        {
            currentBoard = _oldBoard.getBoard();
        }

        public bool makeMove(bool who, Board oldBoard, Board newBoard)
        {
            if ((isRed(who, oldBoard) || isBlack(who, oldBoard))
            && Math.Abs(newBoard.x - oldBoard.x) == 1
                && Math.Abs(newBoard.y - oldBoard.y) == 1
            && isEmpty(newBoard))
            {
                oldBoard.clearCell();
                newBoard.changeColor(who);
                return true;
            }
            return false;
        }

        public List<Board> getJumpables(bool who)
        {
            List<Board> jumpables = new List<Board>();
            foreach (Board cell in _oldBoard.getBoard())
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

        //to be used if no jumps can be made, and no pieces can become kings
        public List<Board> getEndangeredPieces(bool who)
        {
            updateBoard();
            List<Board> endangeredPieces = new List<Board>();
            foreach (Board cell in currentBoard)
            {
                if (isRed(who, cell) && canBeJumped(cell))
                {
                    endangeredPieces.Add(cell);
                }
                else if (isBlack(who, cell) && canBeJumped(cell))
                {
                    endangeredPieces.Add(cell);
                }
            }
            return endangeredPieces;
        }

        public bool canBeJumped(Board board)
        {
            if (board.x == 0 || board.x == 7) // it is impossible to jump a piece in the top or bottom row
            {
                return false;
            }
            updateBoard(); //I updated the board here also just in case we call this method from another place (not from getEndangeredPieces())
            Board upLeft = getUpLeftCell(board);
            Board upRight = getUpRightCell(board);
            Board downLeft = getDownLeftCell(board);
            Board downRight = getDownRightCell(board);

            if (board.color.Equals(Board.PawnStatus.RedPawn))
            {
                return redPieceCanBeJumped(board, upLeft, upRight, downLeft, downRight);
            }
            if (board.color.Equals(Board.PawnStatus.BlackPawn))
            {
                return blackPieceCanBeJumped(board, upLeft, upRight, downLeft, downRight);
            }
            return false;
        }

        /*
         * This is a good defense tactic on the computer's move. A piece can never be jumped if it is on the edge (left or right side),
         * so if the computer has no better move to make, it may as well look for pieces that can move to the side
         * 
         * This can also be used in a foreach loop to check the list of jumpables - a jump that gets our piece to the
         * side might be better than a jump that gets our piece to the middle (unless a different jump gives us a double
         * jump)        
         */
        public List<Board> getPiecesThatCanMoveToEdge(bool who)
        {
            updateBoard();
            List<Board> pieces = new List<Board>();
            foreach (Board cell in currentBoard)
            {
                //only check pieces that can get to the edge this turn (either by a regular move or by a jump)
                //don't check a piece that is already at the edge
                if (cell.y <= 2 && cell.y != 0 && cell.y >= 5 && cell.y != 7)
                {
                    Board upLeft = getUpLeftCell(cell);
                    Board upRight = getUpRightCell(cell);
                    Board downLeft = getDownLeftCell(cell);
                    Board downRight = getDownRightCell(cell);
                    if (isRed(who, cell) && redPieceCanMoveToEdge(cell, upLeft, upRight, downLeft, downRight))
                    {
                        pieces.Add(cell);
                    }
                    else if (isBlack(who, cell) && blackPieceCanMoveToEdge(cell, upLeft, upRight, downLeft, downRight))
                    {
                        pieces.Add(cell);
                    }
                }
            }
            return pieces;
        }

        private bool redPieceCanMoveToEdge(Board board, Board upLeft, Board upRight, Board downLeft, Board downRight)
        {
            if (board.y == 1) //check if a red piece in the second column can move up-left to be at the edge, or down-left if it is a king
            {
                if ((upLeft != null && isEmpty(upLeft)) || (isKing(board) && downLeft != null && isEmpty(downLeft)))
                {
                    return true;
                }
            }
            else if (board.y == 6) //check if a red piece in the second to last column can move up-right to be at the edge, or down-right if it is a king
            {
                if ((upRight != null && isEmpty(upRight)) || (isKing(board) && downRight != null && isEmpty(downRight)))
                {
                    return true;
                }
            }

            //If the first two conditions weren't met, then this method was called by a piece that is 2 columns away from the edge. Maybe it can jump to get to the edge
            //Check if it can jump to the edge regularly, and check if it is a king and can jump backwards.
            else if (board.y == 2 && (canJumpUpLeft(board) || (isKing(board) && canJumpDownLeft(board))))
            {
                return true;
            }
            else if (board.y == 5 && (canJumpUpRight(board) || (isKing(board) && canJumpDownRight(board))))
            {
                return true;
            }
            return false;
        }

        private bool blackPieceCanMoveToEdge(Board board, Board upLeft, Board upRight, Board downLeft, Board downRight)
        {
            //check if there is a black piece in the second column that can get to the edge by moving down-left, or by moving up-left if it is a king
            if (board.y == 1)
            {
                if ((downLeft != null && isEmpty(downLeft)) || (isKing(board) && upLeft != null && isEmpty(upLeft)))
                {
                    return true;
                }
            }

            //check if there is a black piece in the second-to-last column that can get to the edge by moving down-right, or up-right if it is a king
            else if (board.y == 6)
            {
                if ((downRight != null && isEmpty(downRight)) || (isKing(board) && upRight != null && isEmpty(upRight)))
                {
                    return true;
                }
            }

            //If the first two conditions weren't met, then the piece is two columns away from the edge. Maybe it can get to the edge by jumping.
            //Check if it can jump to the edge regularly, and check if it is a king and can jump backwards.
            else if (board.y == 2 && (canJumpDownLeft(board) || (isKing(board) && canJumpUpLeft(board))))
            {
                return true;
            }
            else if (board.y == 5 && (canJumpDownRight(board) || (isKing(board) && canJumpUpRight(board))))
            {
                return true;
            }
            return false;
        }

        private Board getUpLeftCell(Board board)
        {
            updateBoard();
            if (board.x > 0 && board.y > 0)
            {
                return currentBoard[board.x - 1, board.y - 1];
            }
            return null;
        }

        private Board getUpRightCell(Board board)
        {
            updateBoard();
            if (board.x > 0 && board.y < currentBoard.Length)
            {
                return currentBoard[board.x - 1, board.y + 1];
            }
            return null;
        }

        private Board getDownLeftCell(Board board)
        {
            updateBoard();
            if (board.x < currentBoard.Length && board.y > 0)
            {
                return currentBoard[board.x + 1, board.y - 1];
            }
            return null;
        }

        private Board getDownRightCell(Board board)
        {
            updateBoard();
            if (board.x < currentBoard.Length && board.y < currentBoard.Length)
            {
                return currentBoard[board.x + 1, board.y + 1];
            }
            return null;
        }

        private bool redPieceCanBeJumped(Board board, Board upLeft, Board upRight, Board downLeft, Board downRight)
        {
            if (board.y >= 1 && board.y <= 6) //a piece cannot be jumped if it is on an edge (col 0 or 7)
            {

                if (isNoMatch(upLeft, board))
                {
                    //check if there is a piece up to the left that can jump this piece
                    if (isEmpty(downRight))
                    {
                        return true;
                    }
                }
                //Check if there is a king to the bottom right that can jump this piece
                else if (isEmpty(upLeft) && isNoMatch(downRight, board) && isKing(downRight))
                {
                    return true;
                }

                if (isNoMatch(upRight, board))
                {
                    //check if there is a piece up to the right that can jump ths piece
                    if (isEmpty(downLeft))
                    {
                        return true;
                    }
                }
                //check if there is a king to the bottom left that can jump this piece
                else if (isEmpty(upRight) && isNoMatch(downLeft, board) && isKing(downLeft))
                {
                    return true;
                }
            }
            return false;
        }

        private bool blackPieceCanBeJumped(Board board, Board upLeft, Board upRight, Board downLeft, Board downRight)
        {
            if (board.y >= 1 && board.y <= 6) // A piece cannot be jumped if it is on the edge (col 0 or 7)
            {
                if (isNoMatch(downRight, board))
                {
                    //Check if there is a piece down to the right that can jump this piece
                    if (isEmpty(upLeft))
                    {
                        return true;
                    }
                }
                //check if there is a king up to the left that can jump this piece
                else if (isEmpty(downRight) && isNoMatch(upLeft, board) && isKing(upLeft))
                {
                    return true;
                }
                if (isNoMatch(downLeft, board))
                {
                    //check if there is a piece down to the left that can jump this piece
                    if (isEmpty(upRight))
                    {
                        return true;
                    }
                }
                //check if there is a king up to the right that can jump this piece
                else if (isEmpty(downLeft) && isNoMatch(upRight, board) && isKing(upRight))
                {
                    return true;
                }
            }
            return false;
        }

        private bool checkNorth(Board board)
        {
            bool canJump = canJumpUpLeft(board);
            if (!canJump)
            {
                canJump = canJumpUpRight(board);
            }
            return canJump;
        }

        private bool canJumpUpLeft(Board board)
        {
            updateBoard();
            bool canJump = false;
            int x = board.x;
            int y = board.y;

            if (x >= 2 && y >= 2)
            {
                Board upLeft = getUpLeftCell(board);
                if (isNoMatch(upLeft, board))
                {
                    canJump = isEmpty(getUpLeftCell(upLeft));
                }
            }
            return canJump;
        }

        private bool canJumpUpRight(Board board)
        {
            updateBoard();
            bool canJump = false;
            int x = board.x;
            int y = board.y;

            if (x >= 2 && y <= 5)
            {
                Board upRight = getUpRightCell(board);
                if (isNoMatch(upRight, board))
                {
                    canJump = isEmpty(getUpRightCell(upRight));
                }
            }
            return canJump;
        }

        private bool checkSouth(Board board)
        {
            bool canJump = canJumpDownLeft(board);
            if (!canJump)
            {
                canJump = canJumpDownRight(board);
            }
            return canJump;
        }

        private bool canJumpDownLeft(Board board)
        {
            updateBoard();
            bool canJump = false;
            int x = board.x;
            int y = board.y;

            if (x <= 5 && y >= 2)
            {
                Board downLeft = getDownLeftCell(board);
                if (isNoMatch(downLeft, board))
                {
                    canJump = isEmpty(getDownLeftCell(downLeft));
                }
            }
            return canJump;
        }

        private bool canJumpDownRight(Board board)
        {
            updateBoard();
            bool canJump = false;
            int x = board.x;
            int y = board.y;

            if (x <= 5 && y <= 5)
            {
                Board downRight = getDownRightCell(board);
                if (isNoMatch(downRight, board))
                {
                    canJump = isEmpty(getDownRightCell(downRight));
                }
            }
            return canJump;
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
            return (checkNorth(board) || (board.isKing && checkSouth(board)) && board.color == Board.PawnStatus.BlackPawn)
                   || (checkSouth(board) || (board.isKing && checkNorth(board)) && board.color == Board.PawnStatus.RedPawn);
        }

        private bool isNoMatch(Board checking, Board currentBoard)
        {
            return (!checking.color.Equals(currentBoard.color) && (!checking.color.Equals(Board.PawnStatus.None)));
        }
        private bool isEmpty(Board board)
        {
            return board.color.Equals(Board.PawnStatus.None);
        }

        private bool isKing(Board board)
        {
            return board.isKing;
        }
    }
}