using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;

namespace Checkers
{
   
    public class GameBoard : INotifyPropertyChanged
    {
        private Cell[][] board;
        private int boardSize = 8;
        private string nameValue;

        private StringBuilder gameInfo;

        public GameBoard()
        {
            InitBoard();
            this.nameValue = "AI-Checkers!";
            gameInfo = new StringBuilder();
            AddGameLog("BoardGame initialized");
        }

        private void InitBoard()
        {
            board = new Cell[boardSize][];
            bool isAI = true;
            bool isKing = true;

            //for (int i = 0; i < boardSize; i++)
            //{
                
            //    if (i % 2 == 0)
            //    {
            //        board[0][i] = new Cell(new Pawn(!isKing, isAI), !isKing);
            //        board[2][i] = new Cell(new Pawn(!isKing, isAI), !isKing);
            //        board[6][i] = new Cell(new Pawn(!isKing, !isAI), !isKing);

            //        board[1][i] = new Cell(null, !isKing);
            //        board[3][i] = new Cell(null, !isKing);
            //        board[4][i] = new Cell(null, !isKing);
            //        board[5][i] = new Cell(null, !isKing);
            //        board[7][i] = new Cell(null, !isKing);
            //    }
            //    else
            //    {
            //        board[1][i] = new Cell(new Pawn(!isKing, isAI), !isKing);
            //        board[5][i] = new Cell(new Pawn(!isKing, !isAI), !isKing);
            //        board[7][i] = new Cell(new Pawn(!isKing, !isAI), !isKing);

            //        board[0][i] = new Cell(null, !isKing);
            //        board[2][i] = new Cell(null, !isKing);
            //        board[3][i] = new Cell(null, !isKing);
            //        board[4][i] = new Cell(null, !isKing);
            //        board[6][i] = new Cell(null, !isKing);
            //    }
            //}

            for (int row = 0; row < boardSize; ++row)
            {
                board[row] = new Cell[boardSize];
                for (int col = 0; col < boardSize; ++col)
                {
                    Cell cell;
                    if (((row == 0 || row == 2) && col % 2 == 0) || (row == 1 && col % 2 != 0))
                    {
                        cell = new Cell(new Pawn(!isKing, !isAI), !isKing);
                        board[row][col] = cell;
                    }
                    else if (((row == 5 || row == 7) && col % 2 != 0) || (row == 6 && col % 2 == 0))
                    {
                        cell = new Cell(new Pawn(!isKing, isAI), !isKing);
                        board[row][col] = cell;
                    }
                    else
                    {
                        cell = new Cell(null, !isKing);
                        board[row][col] = cell;
                    }
                }
            }



        }

        public string Name
        {
            get { return nameValue; }
            set
            {
                nameValue = value;
                NotifyPropertyChanged("Name");
            }
        }

        public string GameInfo
        {
            get
            {
                return gameInfo.ToString();
            }
        }

        private void AddGameLog(string message)
        {
            gameInfo.AppendLine(message);
            NotifyPropertyChanged("GameInfo");
        }

        public Cell[][] Board
        {
            get { return board; }
        }

        public bool MakeMove(Move move)
        {
            return MakeMove(move.XStart, move.YStart, move.XEnd, move.YEnd);
        }

        public bool MakeMove(int xStart, int yStart, int xEnd, int yEnd)
        {
            List<Point> checkersToRemove = new List<Point>();
            if (Play.IsMovePossible(board, xStart, yStart, xEnd, yEnd, checkersToRemove))
            {
                var movingChecker = board[xStart][yStart].StatusCheck;
                if (board[xEnd][yEnd].IsKing)
                {
                    if (!movingChecker.isKing)
                    {
                        movingChecker.isKing = true;
                    }
                }
                board[xEnd][yEnd].StatusCheck = movingChecker;
                board[xStart][yStart].StatusCheck = null;
                foreach (var field in checkersToRemove)
                {
                    board[(int)field.X][(int)field.Y].StatusCheck = null;
                }
                return true;
            }
            else
            {
                AddGameLog($"Move {xStart},{yStart} -> {xEnd},{yEnd} not possible");
            }
            return false;
        }

      

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}