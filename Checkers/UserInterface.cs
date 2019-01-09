using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Checkers.Annotations;

namespace Checkers
{
    public partial class UserInterface : Window, INotifyPropertyChanged
    {
        private GameBoard GameBoard;
            
        private Move CurrentMove;
        private IAI AI;

        public UserInterface()
        {
            InitializeComponent();
            GameBoard= new GameBoard();
            AI = new MiniMax();
            init();
        }

        //so
        private void init()
        {
            for (int i = 0; i < GameBoard.Board.Length; i++)
            {
                for (int j = 0; j < GameBoard.Board.Length; j++)
                {
                    StackPanel stackPanel = new StackPanel();
                    if ((i + j) % 2 == 0)
                    {
                        stackPanel.Background = (SolidColorBrush) (Application.Current.Resources["WhiteField"]);
                    }
                    else
                    {
                        stackPanel.Background = (SolidColorBrush) (Application.Current.Resources["BlackField"]);
                    }

                    Grid.SetColumn(stackPanel, i);
                    Grid.SetRow(stackPanel, j);
                    BoardHolder.Children.Add(stackPanel);


                    Button button = new Button
                    {
                        Style = (Style)Application.Current.Resources["RoundCorner"]
                    };
                    button.Click += clicker_Click;

                    Binding buttBind = new Binding
                    {
                        Source = this.boardGame,
                        Path = new PropertyPath($"Board[{i}][{j}].PawnColor"),
                        Mode = BindingMode.OneWay
                    };

                    Grid.SetColumn(button, i);
                    Grid.SetRow(button, j);
                    BindingOperations.SetBinding(button, Button.BackgroundProperty, buttBind);
                    BoardHolder.Children.Add(button);
                }

            }
        }


        private void clicker_Click(object sender, RoutedEventArgs e)
        {

            var button = (Button)sender;
            var col = Grid.GetColumn(button);
            var row = Grid.GetRow(button);
            if (currentMove == null)
            {
                currentMove = new Move(col, row, -1, -1);
            }
            else if (currentMove.XStart == col && currentMove.YStart == row)
            {
                //do nothing - start/end position are the same
            }
            else
            {
                currentMove = new Move(currentMove.XStart, currentMove.YStart, col, row);
               
                if (boardGame.MakeMove(currentMove))
                {
                    Move AIMove = AI.GetNextMove(boardGame.Board);
                    boardGame.MakeMove(AIMove);
                }
             
                currentMove = null;
             
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
