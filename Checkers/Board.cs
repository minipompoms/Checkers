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

     


        public Board()
        {
            status = PawnStatus.None;
            isKing = false;
        }

        public Board(PawnStatus status, bool isKingChangingField)
        {
            this.status = status;
            isKing = isKingChangingField;
        }
        public Board(Pawn check, bool isKingChangingField)
        {
            this.check = check;
            isKing = isKingChangingField;
        }
       
        public void ClearCell()
        {
            color = PawnStatus.None;
        }

        private Pawn check;

        public Pawn Check
        {
            get { return check; }
            set
            {
                check = value;
                NotifyPropertyChanged("CheckerColor");
            }
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