using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Program7_8
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        const string USER_SYMBOL = "X";
        const string COMPUTER_SYMBOL = "O";
        const string EMPTY = "";
        const int SIZE = 3;

        // constants for the 2 diagonals
        const int TOP_LEFT_TO_BOTTOM_RIGHT = 1;
        const int TOP_RIGHT_TO_BOTTOM_LEFT = 2;

        // constants for IsWinner
        const int NONE = -1;
        const int ROW = 1;
        const int COLUMN = 2;
        const int DIAGONAL = 3;

        // This method takes a row and column as parameters and 
        // returns a reference to a label on the form in that position
        private Label GetSquare(int row, int column)
        {
            int labelNumber = row * SIZE + column + 1;
            return (Label)(this.Controls["label" + labelNumber.ToString()]);
        }

        // This method does the "reverse" process of GetSquare
        // It takes a label on the form as it's parameter and
        // returns the row and column of that square as output parameters
        private void GetRowAndColumn(Label l, out int row, out int column)
        {
            int position = int.Parse(l.Name.Substring(5));
            row = (position - 1) / SIZE;
            column = (position - 1) % SIZE;
        }

        // This method takes a row (in the range of 0 - 2) and returns true if 
        // the row on the form contains 3 Xs or 3 Os.
        // Use it as a model for writing IsColumnWinner
        /*private bool IsRowWinner(int row)
        {
            if (GetSquare(row, 0).Text == GetSquare(row, 1).Text
                    && GetSquare(row, 1).Text == GetSquare(row, 2).Text
                    && GetSquare(row, 0).Text != EMPTY)
            {
                return true;
            }
            else
                return false;
        }*/

        // This is a better version of IsRowWinner.  Can you tell me what it does?
        private bool IsRowWinner(int row)
        {

            // Stores a copy of the return value of the GetSquare method
            Label square = GetSquare(row, 0);

            // Stores the symbol of the square
            string symbol = square.Text;

            // Iterates through each column in the row
            for (int col = 1; col < SIZE; col++)
            {

                // Changes the square object to the square we are checking
                square = GetSquare(row, col);

                // If the square has the wrong symbol or is empty...
                if (symbol == EMPTY || square.Text != symbol)

                    // ...Return false
                    return false;
            }

            // Otherwise, if no problem was encountered, every square must be the same and thus is a row winner
            return true;
        }

        // TODO:  Write these 3

        /*
            get the square with the given column

            check the symbol
                for every square in that column
                    check if the symbol is correct

                    if not, there's no winner
                    if so, move down one square
        */
        private bool IsColumnWinner(int col)
        {
            Label square = GetSquare(0, col);
            string symbol = square.Text;

            for(int row = 1; row < SIZE; row++)
            {
                square = GetSquare(row, col);
                if (symbol == EMPTY || square.Text != symbol)
                    return false;
            }
            return true;
        }

        /*
            get the starting square

            for every square starting with the top left
                check if the square's symbol is correct
                    if it isn't, there's not a diagonal winner

                    if it is, move down one square and right one square
            
            if the symbols were all the same, there is a diagonal winner
        */
        private bool IsDiagonal1Winner()
        {
            Label square = GetSquare(0, 0);
            string symbol = square.Text;

            for(int row = 0, col = 0; row < SIZE; row++, col++)
            {
                square = GetSquare(row, col);
                if (symbol == EMPTY || square.Text != symbol)
                    return false;
            }
            
            return true;
        }

        /*
            get the starting square

            for every square starting with the top right
                check if the square's symbol is correct
                    if it isn't, there's not a diagonal winner

                    if it is, move down one square and left one square
            
            if the symbols were all the same, there is a diagonal winner
        */
        private bool IsDiagonal2Winner()
        {
            Label square = GetSquare(0, 2);
            string symbol = square.Text;

            for (int row = 0, col = 2; row < SIZE; row++, col--)
            {
                square = GetSquare(row, col);
                if (symbol == EMPTY || square.Text != symbol)
                    return false;
            }

            return true;
        }

        // This method determines if any row, column or diagonal on the board is a winner.
        // It returns true or false and the output parameters will contain appropriate values
        // when the method returns true.  See constant definitions at top of form.
        private bool IsWinner(out int whichDimension, out int whichOne)
        {
            // rows
            for (int row = 0; row < SIZE; row++)
            {
                if (IsRowWinner(row))
                {
                    whichDimension = ROW;
                    whichOne = row;
                    return true;
                }
            }
            // columns
            for (int column = 0; column < SIZE; column++)
            {
                if (IsColumnWinner(column))
                {
                    whichDimension = COLUMN;
                    whichOne = column;
                    return true;
                }
            }
            // diagonals
            if (IsDiagonal1Winner())
            {
                whichDimension = DIAGONAL;
                whichOne = TOP_LEFT_TO_BOTTOM_RIGHT;
                return true;
            }
            if (IsDiagonal2Winner())
            {
                whichDimension = DIAGONAL;
                whichOne = TOP_RIGHT_TO_BOTTOM_LEFT;
                return true;
            }
            whichDimension = NONE;
            whichOne = NONE;
            return false;
        }

        // I wrote this method to show you how to call IsWinner
        private bool IsTie()
        {
            int winningDimension, winningValue;
            return (IsFull() && !IsWinner(out winningDimension, out winningValue));
        }

        /*
            for every square in row starting with leftmost
                check if the square is empty
                    return false if it is
                
                otherwise, check the next row
        */
        private bool IsFull()
        {
            Label square = GetSquare(0, 0);

            for (int row = 0; row < SIZE; row++)
            {
                for(int col = 0; col < SIZE; col++)
                {
                    square = GetSquare(row, col);
                    string symbol = square.Text;

                    if (symbol == EMPTY)
                        return false;
                }
            }

            return true;
        }

        // Inside the click event handler you have a reference to the label that was clicked
        // Use this method (and pass that label as a parameter) to disable just that one square
        private void DisableSquare(Label square)
        {
            square.Click -= new System.EventHandler(this.label_Click);
        }

        // Setting the enabled property changes the look and feel of the cell.
        // Instead, this code removes the event handler from each square.
        // Use it when someone wins or the board is full to prevent clicking a square.
        private void DisableAllSquares()
        {
            for (int row = 0; row < SIZE; row++)
            {
                for (int col = 0; col < SIZE; col++)
                {
                    Label square = GetSquare(row, col);
                    DisableSquare(square);
                }
            }
        }

        // You'll need this method to allow the user to start a new game
        private void EnableAllSquares()
        {
            for (int row = 0; row < SIZE; row++)
            {
                for (int col = 0; col < SIZE; col++)
                {
                    Label square = GetSquare(row, col);
                    square.Click += new System.EventHandler(this.label_Click);
                }
            }
        }

        // This method "highlights" a column by making the font color for the cells in the column red
        // It takes one parameter, the column and does not return a value
        private void HighlightColumn(int col)
        {
            for (int row = 0; row < SIZE; row++)
            {
                Label square = GetSquare(row, col);
                square.ForeColor = Color.Red;
            }
        }

        /*
            for each square, starting with the leftmost of the row
                change the font color to red
                move to the square to the right of the current square
            
            repeat until there's no more squares
        */
        private void HighlightRow(int row)
        {
            for(int col = 0; col < SIZE; col++)
            {
                Label square = GetSquare(row, col);
                square.ForeColor = Color.Red;
            }
        }

        /*
            for each square, starting with the top left
                change the square's font color to red
                move to the square that is one down and one right
            
            repeat until there's no more squares
        */
        private void HighlightDiagonal1()
        {
            for(int row = 0, col = 0; row < SIZE; row++, col++)
            {
                Label square = GetSquare(row, col);
                square.ForeColor = Color.Red;
            }
        }

        /*
            for each square, starting with the top right
                change the square's font color to red
                move to the square that is one down and one left

            repeat until there's no more squares
        */
        private void HighlightDiagonal2()
        {
            for (int row = 0, col = 2; row < SIZE; row++, col--)
            {
                Label square = GetSquare(row, col);
                square.ForeColor = Color.Red;
            }
        }
        
        // I needed this method to highlighting the diagonals work like rows and columns in the next method
        private void HighlightDiagonal(int whichDiagonal)
        {
            if (whichDiagonal == TOP_LEFT_TO_BOTTOM_RIGHT)
                HighlightDiagonal1();
            else
                HighlightDiagonal2();

        }

        /*
            if the winner was a row winner
                highlight the row
                output the player that won

            if the winner was a column winner
                highlight the column
                output the player that won
            
            if the winner was a diagonal winner
                highlight the diagonal
                output the player that won
        */
        private void HighlightWinner(string player, int winningDimension, int winningValue)
        {
            switch (winningDimension)
            {
                case ROW:
                    HighlightRow(winningValue);
                    resultLabel.Text = (player + " wins!");
                    break;
                case COLUMN:
                    HighlightColumn(winningValue);
                    resultLabel.Text = (player + " wins!");
                    break;
                case DIAGONAL:
                    HighlightDiagonal(winningValue);
                    resultLabel.Text = (player + " wins!");
                    break;
            }
        }

        /*
            enable all the squares

            for every square
                change the text to empty
                change the font color to black
        */
        private void ResetSquares()
        {
            EnableAllSquares();

            Label square = GetSquare(0, 0);

            for(int row = 0; row < SIZE; row++)
            {
                for(int col = 0; col < SIZE; col++)
                {
                    square = GetSquare(row, col);

                    square.Text = EMPTY;
                    square.ForeColor = Color.Black;
                }
            }
        }

        /*
            generate a random number between 0 and 2
            
            check the square at the given coordinates

            if the square isn't taken
                change the symbol of the square
                disable that square
            
            continue until a non-taken square is found
        */
        private void MakeComputerMove()
        {
            Label square;
            Random rnd = new Random();

            while(true)
            {
                square = GetSquare(rnd.Next(0, SIZE), rnd.Next(0, SIZE));

                string symbol = square.Text;
                
                if(symbol != USER_SYMBOL && symbol != COMPUTER_SYMBOL)
                {
                    square.Text = COMPUTER_SYMBOL;
                    DisableSquare(square);
                    break;
                }
            }
        }

        /*
            put an x in the label
            disable the aforementioned label

            if someone wins
                disable all squares
                highlight the winning row or col or diagonal and display a message

            else if the board is not full
                computer moves
                if someone wins
                    disable all squares
                    highlight the winner
            
            else
                declare a tie
        */
        private void label_Click(object sender, EventArgs e)
        {
            int winningDimension = NONE;
            int winningValue = NONE;

            Label clickedLabel = (Label)sender;
            clickedLabel.Text = USER_SYMBOL;
            DisableSquare(clickedLabel);

            if(IsWinner(out winningDimension, out winningValue))
            {
                DisableAllSquares();
                HighlightWinner("The user", winningDimension, winningValue);
            }

            else if (!IsFull())
            {
                MakeComputerMove();

                if(IsWinner(out winningDimension, out winningValue))
                {
                    DisableAllSquares();
                    HighlightWinner("The computer", winningDimension, winningValue);
                }
            }

            else
            {
                resultLabel.Text = "It's a tie!";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Closes the form
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Resets the squares
            ResetSquares();

            // Resets the output label
            resultLabel.Text = EMPTY;
        }
    }
}
