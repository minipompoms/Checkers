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
        private int startX, startY, endX, endY;
        private Move move; // = new Move(); //construct our move class for actual functionality
        //private Board[,] board;
        private GameBoard board = new GameBoard();
        private MiniMax minimax = new MiniMax();


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
                    buttonarray[i, j].Tag = 8 * i + j; //not sure what this does
                    buttonarray[i, j].Click += new System.EventHandler(this.PieceSelection);
                    

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
                    //i just set the background colors to red and black for now, and Ks for kings
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
                bool canJump = false;
                var redMoves = minimax.GetPossibleMoves(board.Board, false);
                if (redMoves.Count == 0)
                {
                    gameOver(false);
                }
                else
                {
                    foreach (Move myMove in redMoves)
                    {
                        if (Math.Abs(myMove.XStart - myMove.XEnd) == 2)
                        {
                            canJump = true;
                            break;
                        }
                    }
                }

                var Square = (Button)sender;
                
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {

                        if (Square.Location == buttonarray[i, j].Location)
                        {
                            tryAgain:
                            if (!pieceIsUp) //user hasn't yet picked up a piece
                            {
                                if (Square.BackColor.Equals(Color.Red)) //check if there's a piece there (assuming player is red)
                                {
                                    startX = i;
                                    startY = j;
                                    Square.BackColor = Color.Gray; //'pick up' the piece by erasing it from the board
                                    Square.Text = "";

                                    pieceIsUp = true;
                                    break;
                                }

                            }
                            else //this is where they want to put down the piece...
                            {
                                if (Square.BackColor.Equals(Color.Gray)) //there's not already a piece in that cell
                                {
                                    endX = i;
                                    endY = j;

                                    move = new Move(startX, startY, endX, endY);

                                    if (canJump && Math.Abs(startX - endX) != 2)
                                    {
                                        buttonarray[startX, startY].BackColor = Color.Red;
                                        if (board.Board[startX][startY].isKing)
                                        {
                                            buttonarray[startX, startY].Text = "K"; //show user that it's a king
                                        }
                                        pieceIsUp = false;
                                        goto tryAgain;
                                    }
                                    if (doTheMove(board, move)) //if move works
                                    {
                                        /*if (endX == 7)
                                        {
                                            board.Board[endX][endY] = new Cell(PawnStatus.RedKing, true);
                                            //board.Board[endX][endY].isKing = true;
                                            //board.Board[endX][endY].StatusCheck = new Pawn(true, false);
                                        }*/
                                        Console.WriteLine("moved is true!");
                                        Square.BackColor = Color.Red; //put them there in UI
                                        if(board.Board[startX][startY].isKing || board.Board[endX][endY].isKing)
                                        {
                                            Square.Text = "K"; //show user that it's a king
                                        }

                                        if (Math.Abs(startX - endX) == 2) eraseJumpedPiece(startX, startY, endX, endY); //if a piece was jumped
                                        
                                        pieceIsUp = false;

                                        //this is throwing exception
                                        Cell[][] cells = board.Board;
                                        move = minimax.GetNextMove(cells);
                                        if (move == null) gameOver(true);
                                        if (doTheMove(board, move)) //if move (returned by MiniMax) works,
                                        {
                                            startX = move.XStart;
                                            startY = move.YStart;
                                            endX = move.XEnd;
                                            endY = move.YEnd;
                                            /*if (endX == 0)
                                            {
                                                board.Board[endX][endY] = new Cell(PawnStatus.BlackKing, true);
                                                //board.Board[endX][endY].isKing = true;
                                                //board.Board[endX][endY].StatusCheck = new Pawn(true, true);
                                            }*/
                                            //move them there in the UI
                                            buttonarray[startX, startY].BackColor = Color.Gray;
                                            buttonarray[startX, startY].Text = "";
                                            buttonarray[endX, endY].BackColor = Color.Black;
                                            if (board.Board[startX][startY].isKing || board.Board[endX][endY].isKing)
                                            {
                                                buttonarray[endX, endY].Text = "K"; //show user that it's a king
                                                buttonarray[endX, endY].ForeColor = Color.White;
                                            }
                                            if (Math.Abs(startX - endX) == 2) eraseJumpedPiece(startX, startY, endX, endY);
                                            usersTurn = true; //and now it's their turn again
                                        }
                                        //usersTurn = true;
                                    }
                                    else //if user's move doesn't work,
                                    {
                                        //send them back to where they came from
                                        buttonarray[startX, startY].BackColor = Color.Red;
                                        if (board.Board[startX][startY].isKing)
                                        {
                                            buttonarray[startX, startY].Text = "K"; //show user that it's a king
                                        }
                                        pieceIsUp = false;
                                        goto tryAgain;
                                    }

                                    break;

                                }
                                else //if user tries to put piece down on a black piece
                                {
                                    buttonarray[startX, startY].BackColor = Color.Red;
                                    if (board.Board[startX][startY].isKing)
                                    {
                                        buttonarray[startX, startY].Text = "K";
                                    }
                                    pieceIsUp = false;
                                    goto tryAgain;
                                }
                            }
                        }
                    }
                }
            }
        }

        //if jumped, erase the jumped piece
        private void eraseJumpedPiece(int startX, int startY, int endX, int endY)
        {
                int eraseX = (startX + endX) / 2;
                int eraseY = (startY + endY) / 2;
                buttonarray[eraseX, eraseY].BackColor = Color.Gray;
                buttonarray[eraseX, eraseY].Text = "";
        }

        private bool doTheMove(GameBoard board, Move move)
        {
            return board.MakeMove(move);
        }

        private void gameOver(bool playerWon)
        {
            String winner;
            if (playerWon) winner = "You";
            else winner = "I";
            String message = $"{winner} won!";
            MessageBoxButtons button = MessageBoxButtons.OK;
            String title = "Game Over";
            DialogResult result = MessageBox.Show(message, title, button);
            if (result == DialogResult.OK) this.Close();
        }
    }
}
