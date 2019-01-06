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
        private Board board;
        private Cell[,] currentBoard;



        public Move()
        {
            board = new Board();
        }

        //This method should be called every time a move is made
        //Maybe we should also make a private List of jumpables and make methods to automatically update the jumpables list every time a move is made
        public void updateBoard()
        {
            currentBoard = board.getBoard();
        }

        public bool makeMove(bool who, Cell oldCell, Cell newCell)
        {
            if ((isRed(who, oldCell) || isBlack(who, oldCell))
            && Math.Abs(newCell.x - oldCell.x) == 1
                && Math.Abs(newCell.y - oldCell.y) == 1
            && isEmpty(newCell))
            {
                oldCell.clearCell();
                newCell.changeColor(who);
                return true;
            }
            return false;
        }

        public List<Cell> getJumpables(bool who)
        {
            List<Cell> jumpables = new List<Cell>();
            foreach (Cell cell in board.getBoard())
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
        public List<Cell> getEndangeredPieces(bool who)
        {
            updateBoard();
            List<Cell> endangeredPieces = new List<Cell>();
            foreach (Cell cell in currentBoard)
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

        public bool canBeJumped(Cell cell)
        {
            if (cell.x == 0 || cell.x == 7) // it is impossible to jump a piece in the top or bottom row
            {
                return false;
            }
            updateBoard(); //I updated the board here also just in case we call this method from another place (not from getEndangeredPieces())
            Cell upLeft = getUpLeftCell(cell);
            Cell upRight = getUpRightCell(cell);
            Cell downLeft = getDownLeftCell(cell);
            Cell downRight = getDownRightCell(cell);

            if (cell.color.Equals(Cell.Contents.RED))
            {
                return redPieceCanBeJumped(cell, upLeft, upRight, downLeft, downRight);
            }
            if (cell.color.Equals(Cell.Contents.BLACK))
            {
                return blackPieceCanBeJumped(cell, upLeft, upRight, downLeft, downRight);
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
        public List<Cell> getPiecesThatCanMoveToEdge(bool who)
        {
            updateBoard();
            List<Cell> pieces = new List<Cell>();
            foreach (Cell cell in currentBoard)
            {
                //only check pieces that can get to the edge this turn (either by a regular move or by a jump)
                //don't check a piece that is already at the edge
                if (cell.y <= 2 && cell.y != 0 && cell.y >= 5 && cell.y != 7)
                {
                    Cell upLeft = getUpLeftCell(cell);
                    Cell upRight = getUpRightCell(cell);
                    Cell downLeft = getDownLeftCell(cell);
                    Cell downRight = getDownRightCell(cell);
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

        private bool redPieceCanMoveToEdge(Cell cell, Cell upLeft, Cell upRight, Cell downLeft, Cell downRight)
        {
            if (cell.y == 1) //check if a red piece in the second column can move up-left to be at the edge, or down-left if it is a king
            {
                if ((upLeft != null && isEmpty(upLeft)) || (isKing(cell) && downLeft != null && isEmpty(downLeft)))
                {
                    return true;
                }
            }
            else if (cell.y == 6) //check if a red piece in the second to last column can move up-right to be at the edge, or down-right if it is a king
            {
                if ((upRight != null && isEmpty(upRight)) || (isKing(cell) && downRight != null && isEmpty(downRight)))
                {
                    return true;
                }
            }

            //If the first two conditions weren't met, then this method was called by a piece that is 2 columns away from the edge. Maybe it can jump to get to the edge
            //Check if it can jump to the edge regularly, and check if it is a king and can jump backwards.
            else if (cell.y == 2 && (canJumpUpLeft(cell) || (isKing(cell) && canJumpDownLeft(cell))))
            {
                return true;
            }
            else if (cell.y == 5 && (canJumpUpRight(cell) || (isKing(cell) && canJumpDownRight(cell))))
            {
                return true;
            }
            return false;
        }

        private bool blackPieceCanMoveToEdge(Cell cell, Cell upLeft, Cell upRight, Cell downLeft, Cell downRight)
        {
            //check if there is a black piece in the second column that can get to the edge by moving down-left, or by moving up-left if it is a king
            if (cell.y == 1)
            {
                if ((downLeft != null && isEmpty(downLeft)) || (isKing(cell) && upLeft != null && isEmpty(upLeft)))
                {
                    return true;
                }
            }

            //check if there is a black piece in the second-to-last column that can get to the edge by moving down-right, or up-right if it is a king
            else if (cell.y == 6)
            {
                if ((downRight != null && isEmpty(downRight)) || (isKing(cell) && upRight != null && isEmpty(upRight)))
                {
                    return true;
                }
            }

            //If the first two conditions weren't met, then the piece is two columns away from the edge. Maybe it can get to the edge by jumping.
            //Check if it can jump to the edge regularly, and check if it is a king and can jump backwards.
            else if (cell.y == 2 && (canJumpDownLeft(cell) || (isKing(cell) && canJumpUpLeft(cell))))
            {
                return true;
            }
            else if (cell.y == 5 && (canJumpDownRight(cell) || (isKing(cell) && canJumpUpRight(cell))))
            {
                return true;
            }
            return false;
        }

        private Cell getUpLeftCell(Cell cell)
        {
            updateBoard();
            if (cell.x > 0 && cell.y > 0)
            {
                return currentBoard[cell.x - 1, cell.y - 1];
            }
            return null;
        }

        private Cell getUpRightCell(Cell cell)
        {
            updateBoard();
            if (cell.x > 0 && cell.y < currentBoard.Length)
            {
                return currentBoard[cell.x - 1, cell.y + 1];
            }
            return null;
        }

        private Cell getDownLeftCell(Cell cell)
        {
            updateBoard();
            if (cell.x < currentBoard.Length && cell.y > 0)
            {
                return currentBoard[cell.x + 1, cell.y - 1];
            }
            return null;
        }

        private Cell getDownRightCell(Cell cell)
        {
            updateBoard();
            if (cell.x < currentBoard.Length && cell.y < currentBoard.Length)
            {
                return currentBoard[cell.x + 1, cell.y + 1];
            }
            return null;
        }

        private bool redPieceCanBeJumped(Cell cell, Cell upLeft, Cell upRight, Cell downLeft, Cell downRight)
        {
            if (cell.y >= 1 && cell.y <= 6) //a piece cannot be jumped if it is on an edge (col 0 or 7)
            {

                if (isNoMatch(upLeft, cell))
                {
                    //check if there is a piece up to the left that can jump this piece
                    if (isEmpty(downRight))
                    {
                        return true;
                    }
                }
                //Check if there is a king to the bottom right that can jump this piece
                else if (isEmpty(upLeft) && isNoMatch(downRight, cell) && isKing(downRight))
                {
                    return true;
                }

                if (isNoMatch(upRight, cell))
                {
                    //check if there is a piece up to the right that can jump ths piece
                    if (isEmpty(downLeft))
                    {
                        return true;
                    }
                }
                //check if there is a king to the bottom left that can jump this piece
                else if (isEmpty(upRight) && isNoMatch(downLeft, cell) && isKing(downLeft))
                {
                    return true;
                }
            }
            return false;
        }

        private bool blackPieceCanBeJumped(Cell cell, Cell upLeft, Cell upRight, Cell downLeft, Cell downRight)
        {
            if (cell.y >= 1 && cell.y <= 6) // A piece cannot be jumped if it is on the edge (col 0 or 7)
            {
                if (isNoMatch(downRight, cell))
                {
                    //Check if there is a piece down to the right that can jump this piece
                    if (isEmpty(upLeft))
                    {
                        return true;
                    }
                }
                //check if there is a king up to the left that can jump this piece
                else if (isEmpty(downRight) && isNoMatch(upLeft, cell) && isKing(upLeft))
                {
                    return true;
                }
                if (isNoMatch(downLeft, cell))
                {
                    //check if there is a piece down to the left that can jump this piece
                    if (isEmpty(upRight))
                    {
                        return true;
                    }
                }
                //check if there is a king up to the right that can jump this piece
                else if (isEmpty(downLeft) && isNoMatch(upRight, cell) && isKing(upRight))
                {
                    return true;
                }
            }
            return false;
        }

        private bool checkNorth(Cell cell)
        {
            bool canJump = canJumpUpLeft(cell);
            if (!canJump)
            {
                canJump = canJumpUpRight(cell);
            }
            return canJump;
        }

        private bool canJumpUpLeft(Cell cell)
        {
            updateBoard();
            bool canJump = false;
            int x = cell.x;
            int y = cell.y;

            if (x >= 2 && y >= 2)
            {
                Cell upLeft = getUpLeftCell(cell);
                if (isNoMatch(upLeft, cell))
                {
                    canJump = isEmpty(getUpLeftCell(upLeft));
                }
            }
            return canJump;
        }

        private bool canJumpUpRight(Cell cell)
        {
            updateBoard();
            bool canJump = false;
            int x = cell.x;
            int y = cell.y;

            if (x >= 2 && y <= 5)
            {
                Cell upRight = getUpRightCell(cell);
                if (isNoMatch(upRight, cell))
                {
                    canJump = isEmpty(getUpRightCell(upRight));
                }
            }
            return canJump;
        }

        private bool checkSouth(Cell cell)
        {
            bool canJump = canJumpDownLeft(cell);
            if (!canJump)
            {
                canJump = canJumpDownRight(cell);
            }
            return canJump;
        }

        private bool canJumpDownLeft(Cell cell)
        {
            updateBoard();
            bool canJump = false;
            int x = cell.x;
            int y = cell.y;

            if (x <= 5 && y >= 2)
            {
                Cell downLeft = getDownLeftCell(cell);
                if (isNoMatch(downLeft, cell))
                {
                    canJump = isEmpty(getDownLeftCell(downLeft));
                }
            }
            return canJump;
        }

        private bool canJumpDownRight(Cell cell)
        {
            updateBoard();
            bool canJump = false;
            int x = cell.x;
            int y = cell.y;

            if (x <= 5 && y <= 5)
            {
                Cell downRight = getDownRightCell(cell);
                if (isNoMatch(downRight, cell))
                {
                    canJump = isEmpty(getDownRightCell(downRight));
                }
            }
            return canJump;
        }

        private bool isBlack(bool who, Cell cell)
        {
            return who.Equals(Player.BLACK) && cell.color.Equals(Cell.Contents.BLACK);
        }

        private bool isRed(bool who, Cell cell)
        {
            return who.Equals(Player.RED) && cell.color.Equals(Cell.Contents.RED);
        }


        private bool isChecked(Cell cell)
        {
            return (checkNorth(cell) || (cell.king && checkSouth(cell)) && cell.color == Cell.Contents.BLACK)
                   || (checkSouth(cell) || (cell.king && checkNorth(cell)) && cell.color == Cell.Contents.RED);
        }

        private bool isNoMatch(Cell checking, Cell currentCell)
        {
            return (!checking.color.Equals(currentCell.color) && (!checking.color.Equals(Cell.Contents.NONE)));
        }
        private bool isEmpty(Cell cell)
        {
            return cell.color.Equals(Cell.Contents.NONE);
        }

        private bool isKing(Cell cell)
        {
            return cell.king;
        }
    }
}