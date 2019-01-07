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

        private Button[,] buttonarray = new Button[8, 8];
        private bool usersTurn = true; //when this is false, ignore their clicks!
        private bool pieceIsUp = false;
        private Cell pickedUp;
        private Cell putDown;
        private Move move = new Move(); //construct our move class for actual functionality

        public UserInterface()
        {
            InitializeComponent();
            BuildGameBoard();
            setUpPieces();
        }

        private void BuildGameBoard()
        {
            pnlBoard.Height = 500;
            pnlBoard.Width = 500;
            int Horizantal = 30;
            int Vertical = 100;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    buttonarray[i, j] = new Button();
                    buttonarray[i, j].Size = new Size(40, 40);
                    buttonarray[i, j].Location = new Point(Horizantal, Vertical);
                    buttonarray[i, j].FlatStyle = FlatStyle.Flat;
                    buttonarray[i, j].FlatAppearance.BorderColor = Color.Black;
                    buttonarray[i, j].FlatAppearance.BorderSize = 1;
                    buttonarray[i, j].Tag = 8 * i + j; //not sure what to do with this..
                    buttonarray[i, j].Click += new System.EventHandler(this.PieceSelection);
                    //buttonarray[i].Enabled = false; //this was disabling the top row of cells.
                    //not sure why this line of code was here

                    if ((j % 2 != 0 && i % 2 == 0) || (j % 2 == 0 && i % 2 != 0))
                    {
                        buttonarray[i, j].BackColor = Color.White;
                    }
                    else
                    {
                        buttonarray[i, j].BackColor = Color.Gray;
                    }
                    pnlBoard.Controls.Add(buttonarray[i, j]);
                    Horizantal += 41;
                }
                Horizantal = 30;
                Vertical += 41;
            }
        }

        private void setUpPieces()
        {
            for (int i = 0; i < 8; i += 1)
            {
                int offset = 0;
                if (i % 2 != 0)
                {
                    offset++;
                }
                for (int j = offset; j < 8; j += 2)
                {
                    //i just set the background colors to red and black for now,
                    //but we can put circles or pictures of pieces (and dif ones for kings)
                    if (i < 3) buttonarray[i, j].BackColor = Color.Red;
                    if (i > 4) buttonarray[i, j].BackColor = Color.Black;
                }
            }
        }
        private void PieceSelection(object sender, EventArgs e)
        {
            if (usersTurn) //only notice the clicking if it's actually their turn
            {
                var Square = (Button)sender;


                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {

                        if (Square.Location == buttonarray[i, j].Location)
                        {
                            /*
                             * now we're all ready to do stuff.
                             * we just have to somehow call currentBoard[i,j]
                             * from Move class and do stuff with it. 
                             * or maybe just instantiate move and it does all the work?
                             * 
                             * player has to be able to choose a) the piece they want to move,
                             *                          and    b) where they want to move it to
                             * that's 2 clicks. how will that be implemented? ANSWER using pieceIsUp field
                             * 
                             * also, we have to prevent the player from making illegal moves,
                             * so we have to check that AND if they can jump anywhere...
                             */

                            if (!pieceIsUp) //user hasn't yet picked up a piece
                            {
                                if (Square.BackColor.Equals(Color.Red)) //check if there's a piece there (assuming player is red)
                                {
                                    //we should probably also check here if there's another piece that can jump, cuz then this one can't move
                                    Square.BackColor = Color.Gray; //'pick up' the piece by erasing it from the board
                                    pickedUp = new Cell(Cell.Pawn.RED, i, j);
                                    pieceIsUp = true;
                                }

                            }
                            else //this is where they want to put down the piece...
                            {
                                if (Square.BackColor.Equals(Color.Gray)) //there's not already a piece in that cell
                                {
                                    putDown = new Cell(Cell.Pawn.NONE, i, j);
                                    //TODO now check if they're allowed to go there,
                                    //prob using canMove() or something
                                    //and if they can,
                                    Square.BackColor = Color.Red; //put them there

                                    //then do back-end functionality
                                    move.makeMove(true, pickedUp, putDown);

                                    pieceIsUp = false;
                                    //then make our own move, using our heuristics, etc.

                                    usersTurn = true; //and now it's their turn again
                                }
                            }


                        }
                    }
                }
            }
        }
    }
}
