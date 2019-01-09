using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Checkers
{
    public class Board : INotifyPropertyChanged
    {
        
        public enum PawnStatus
        {
            None,
            RedPawn,
            BlackPawn,
            RedKing,
            BlackKing
        }
        
        public bool IsKing
        {
            get { return isKing; }
        }
        public PawnStatus status;
        public PawnStatus color { get; set; }
        public bool isKing{ get; set; }

        public int x { get; }
        public int y { get; }


        public Board()
        {
            this.status = PawnStatus.None;
            this.isKing = false;
        }

        public Board(PawnStatus status, bool isKingChangingField)
        {
            this.status = status;
            this.isKing = isKingChangingField;
        }
        public Board(Checkers.Pawn check, bool isKingChangingField)
        {
            this.check = check;
            this.isKing = isKingChangingField;
        }
        public Board(PawnStatus con, int row, int col)
        {
            color = con;
            x = row;
            y = col;
        }

        public void clearCell()
        {
            color = PawnStatus.None;
        }

        private Checkers.Pawn check;

        public Checkers.Pawn Check
        {
            get { return check; }
            set
            {
                check = value;
                NotifyPropertyChanged("CheckerColor");
            }
        }
        public bool changeColor(bool who)
        {
            if (color != PawnStatus.None)
            {
                if (who.Equals(Player.BLACK))
                {
                    color = PawnStatus.BlackPawn;
                    return true;
                }
                if (who.Equals(Player.RED))
                {
                    color = PawnStatus.RedPawn;
                    return true;
                }
            }
            return false;
        }
        
        public PawnStatus Status
        {
            get { return status; }
            set
            {
                status = value;
                NotifyPropertyChanged("Status");
                NotifyPropertyChanged("CheckerColor");
            }
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