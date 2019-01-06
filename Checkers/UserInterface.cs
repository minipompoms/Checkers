using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Checkers
{
    public partial class UserInterface : Form
    {
        
        private Button[] buttonarray = new Button[64];

        public UserInterface()
        {
            InitializeComponent();
        }
        
         private void BuildGameBoard()
        {
            int Horizantal = 30;
            int Vertical = 100;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    buttonarray[8 * i + j] = new Button();
                    buttonarray[8 * i + j].Size = new Size(40, 40);
                    buttonarray[8 * i + j].Location = new Point(Horizantal, Vertical);
                    buttonarray[8 * i + j].FlatStyle = FlatStyle.Flat;
                    buttonarray[8 * i + j].FlatAppearance.BorderColor = Color.Black;
                    buttonarray[8 * i + j].FlatAppearance.BorderSize = 1;
                    buttonarray[8 * i + j].Tag = 8 * i + j;
                    buttonarray[8 * i + j].Click += new System.EventHandler(this.PieceSelection);
                    buttonarray[i].Enabled = false;

                    if ((j % 2 != 0 && i % 2 == 0) || (j % 2 == 0 && i % 2 != 0))
                    {
                        buttonarray[8 * i + j].BackColor = Color.White;
                    }
                    else
                    {
                        buttonarray[8 * i + j].BackColor = Color.Black;
                    }
                    this.Controls.Add(buttonarray[8 * i + j]);
                    Horizantal += 41;
                }
                Horizantal = 30;
                Vertical += 41;
            }
        }
        private void PieceSelection(object sender, EventArgs e)
        {
            var Square = (Button)sender;

        }
    }
}
