using System;
using System.Collections.Generic;

namespace Checkers
{

    public class Move
    {
        private Board board;

        public Move()
        {
            board = new Board();
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

      
        private bool checkNorth(Cell cell)
        {
            bool canJump = false;
            int x = cell.x;
            int y = cell.y;
            
            if (x >= 2)
            {
                Cell beingChecked;
                if (y >= 2) //check left
                {
                    beingChecked = board.getBoard()[x - 1, y - 1];
                    if (isNoMatch(beingChecked, cell)) 
                    {
                        canJump = isEmpty(board.getBoard()[x-2, y-2]);
                    }
                }
                if (!canJump && y <= 5)
                {
                    beingChecked = board.getBoard()[x-1, y+1];
                    if (isNoMatch(beingChecked, cell)) 
                    {
                        canJump = isEmpty(board.getBoard()[x-2, y+2]);
                    }

                }
            }
            return canJump;
        }

        private bool checkSouth(Cell cell)
        {
            bool canJump = false;
            int x = cell.x;
            int y = cell.y;

            if (x <= 5)
            {
                Cell beingChecked;
                if (y >= 2) //check left
                {
                    beingChecked = board.getBoard()[x + 1, y - 1];
                    if (isNoMatch(beingChecked, cell))
                    {
                        canJump = isEmpty(board.getBoard()[x + 2, y - 2]);
                    }
                }
                if (!canJump && y <= 5)
                {
                    beingChecked = board.getBoard()[x + 1, y + 1];
                    if (isNoMatch(beingChecked, cell))
                    {
                        canJump = isEmpty(board.getBoard()[x + 2, y + 2]);
                    }

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
            return (checkNorth(cell) || (cell.king && checkSouth(cell)))
                   || (checkSouth(cell) || (cell.king && checkNorth(cell)));
        }

        private bool isNoMatch(Cell checking, Cell currentCell)
        {
            return (!checking.color.Equals(currentCell.color) && (!checking.color.Equals(Cell.Contents.NONE)));
        }
        private bool isEmpty(Cell cell)
        {
            return cell.color.Equals(Cell.Contents.NONE);
        }

    }
}