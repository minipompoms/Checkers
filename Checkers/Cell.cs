using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Checkers

{
    public enum PawnStatus
    {
        None,
        RedPawn,
        BlackPawn,
        RedKing,
        BlackKing
    }
    
    public class Cell : INotifyPropertyChanged
    {
        public bool isKing;
        private Pawn statusCheck;
        public PawnStatus status;

       
        public Pawn StatusCheck
        {
            get { return statusCheck; }
            set
            {
                statusCheck = value;
                NotifyPropertyChanged("CheckerColor");
            }
        }
        
        public bool IsKing
        {
            get { return isKing; }
        }
        
  
        public Cell()
        {
            status = PawnStatus.None;
            isKing = false;
        }

        public Cell(PawnStatus status, bool isKingChangingField)
        {
            if (status.Equals(PawnStatus.None)) statusCheck = null;
            else if (status.Equals(PawnStatus.RedPawn) || status.Equals(PawnStatus.RedKing))
            {
                statusCheck = new Pawn(isKingChangingField, false);
            }
            else if (status.Equals(PawnStatus.BlackPawn) || status.Equals(PawnStatus.BlackKing))
            {
                statusCheck = new Pawn(isKingChangingField, true);
            }
            this.status = status;
            isKing = isKingChangingField;
        }
        public Cell(Pawn check, bool isKingChangingField)
        {
            statusCheck = check;
            isKing = isKingChangingField;
            if (check == null) status = status = PawnStatus.None;
            else if (check.isAI) status = PawnStatus.BlackPawn;
            else if (!check.isAI) status = PawnStatus.RedPawn;
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
        
        
        /* Not sure if this code works exactly but something like this.
         
         public Brush PawnColor
        {
            get
            {
                if (check != null)
                {
                    return check.Color;
                }
                else
                {
                    return Brushes.Transparent;
                }

            }
        }*/
    }
}