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
            this.board = new Cell[boardSize][];
            bool isAI = true;
            bool isKing = true;

            for (int i = 0; i < boardSize; i++)
            {
                board[i] = new Cell[boardSize];
                if (i % 2 != 0)
                {
                    board[i][0] = new Cell(new Pawn(!isKing, isAI), isKing);
                    board[i][2] = new Cell(new Pawn(!isKing, isAI), !isKing);
                    board[i][6] = new Cell(new Pawn(!isKing, !isAI), !isKing);

                    board[i][1] = new Cell(null, !isKing);
                    board[i][3] = new Cell(null, !isKing);
                    board[i][4] = new Cell(null, !isKing);
                    board[i][5] = new Cell(null, !isKing);
                    board[i][7] = new Cell(null, !isKing);
                }
                else
                {
                    board[i][1] = new Cell(new Pawn(!isKing, isAI), !isKing);
                    board[i][5] = new Cell(new Pawn(!isKing, !isAI), !isKing);
                    board[i][7] = new Cell(new Pawn(!isKing, !isAI), isKing);

                    board[i][0] = new Cell(null, !isKing);
                    board[i][2] = new Cell(null, !isKing);
                    board[i][3] = new Cell(null, !isKing);
                    board[i][4] = new Cell(null, !isKing);
                    board[i][6] = new Cell(null, !isKing);
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