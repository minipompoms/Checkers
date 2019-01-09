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
            this.status = status;
            isKing = isKingChangingField;
        }
        public Cell(Pawn check, bool isKingChangingField)
        {
            statusCheck = check;
            isKing = isKingChangingField;
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