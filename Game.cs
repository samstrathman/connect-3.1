using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe299
{
    class Game
    {
        public const int ROWS = 6;
        public const int COLS = 7;
        private int numFilledCells;
        private char [ , ] GameBoard;

        public char turn {get; set;}

        public Game(char t = 'X')
        {
            GameBoard = new char[ROWS, COLS];

            turn = t;
            numFilledCells = 0;

            //initialize each cell to empty
            for (int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j < COLS; j++)
                {
                    GameBoard[i, j] = ' ';
                }
            }
        }

        //Copy constructor to make a deep Game copy
        public Game(Game G)
        {
            GameBoard = new char[ROWS, COLS];

            turn = G.turn;
            numFilledCells = G.numFilledCells;

            for (int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j < COLS; j++)
                {
                    GameBoard[i, j] = G.GameBoard[i, j];
                }
            }
        }
        
        public char[,] getGameBoard()
        {
            return GameBoard;
        }

        public char getTurn()
        {
            return turn;
        }

        public bool CellIsEmpty(int row, int col){
            return (GameBoard[row, col] == ' ');
        }

        public bool CellIsPlayable(int row, int col)
        {
            if (row + 1 < ROWS && !CellIsEmpty(row + 1, col) || row + 1 >= ROWS)
            {
                return CellIsEmpty(row, col);
            }
            else return false;
        }
        public void MarkGameBoard(int row, int col){

            if (!CellIsEmpty(row, col))
            {
                return;
            }
            GameBoard[row, col] = turn;
            if (turn == 'X')
            {
                turn = 'O';
            }
            else
            {
                turn = 'X';
            }
            numFilledCells++;
        }

        public bool BoardIsFull()
        {
            return numFilledCells >= ROWS * COLS;
        }

        public Tuple<char, List<Tuple<int, int>>> checkWinner()
        {
            List<Tuple<int, int>> winningSpots = new List<Tuple<int, int>>();

            //check horizontal
            for (int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j <= 4; j++)
                {
                    if (GameBoard[i, j] != ' ' && GameBoard[i, j] == GameBoard[i, j + 1] && GameBoard[i, j + 1] == GameBoard[i, j + 2])
                    {
                        //check the four corners
                        if (i + 1 < ROWS && GameBoard[i, j] == GameBoard[i + 1, j])
                        {
                            winningSpots.Add(Tuple.Create(i + 1, j));
                            winningSpots.Add(Tuple.Create(i, j));
                            winningSpots.Add(Tuple.Create(i, j + 1));
                            winningSpots.Add(Tuple.Create(i, j + 2));
                            return Tuple.Create(GameBoard[i, j], winningSpots);
                        }
                        else if (i - 1 >= 0 && GameBoard[i, j] == GameBoard[i - 1, j] )
                        {
                            winningSpots.Add(Tuple.Create(i - 1, j));
                            winningSpots.Add(Tuple.Create(i, j));
                            winningSpots.Add(Tuple.Create(i, j + 1));
                            winningSpots.Add(Tuple.Create(i, j + 2));
                            return Tuple.Create(GameBoard[i, j], winningSpots);
                        }
                        else if (i + 1 < ROWS && GameBoard[i, j + 2] == GameBoard[i + 1, j + 2])
                        {
                            winningSpots.Add(Tuple.Create(i + 1, j + 2));
                            winningSpots.Add(Tuple.Create(i, j));
                            winningSpots.Add(Tuple.Create(i, j + 1));
                            winningSpots.Add(Tuple.Create(i, j + 2));
                            return Tuple.Create(GameBoard[i, j], winningSpots);
                        }
                        else if (i - 1 >= 0 && GameBoard[i, j + 2] == GameBoard[i - 1, j + 2])
                        {
                            winningSpots.Add(Tuple.Create(i - 1, j + 2));
                            winningSpots.Add(Tuple.Create(i, j));
                            winningSpots.Add(Tuple.Create(i, j + 1));
                            winningSpots.Add(Tuple.Create(i, j + 2));
                            return Tuple.Create(GameBoard[i, j], winningSpots);
                        }        
                    }
                }
            }

            //check vertical
            for (int i = 0; i <= 3; i++)
            {
                for (int j = 0; j < COLS; j++)
                {
                    if (GameBoard[i, j] != ' ' && GameBoard[i, j] == GameBoard[i+1, j] && GameBoard[i+1, j] == GameBoard[i+2, j])
                    {
                        //check the four corners
                        if (j + 1 < COLS && GameBoard[i, j] == GameBoard[i, j + 1])
                        {
                            winningSpots.Add(Tuple.Create(i, j + 1));
                            winningSpots.Add(Tuple.Create(i, j));
                            winningSpots.Add(Tuple.Create(i + 1, j));
                            winningSpots.Add(Tuple.Create(i + 2, j));
                            return Tuple.Create(GameBoard[i, j], winningSpots);
                        }
                        else if (j - 1 >= 0 && GameBoard[i, j] == GameBoard[i, j - 1])
                        {
                            winningSpots.Add(Tuple.Create(i, j - 1));
                            winningSpots.Add(Tuple.Create(i, j));
                            winningSpots.Add(Tuple.Create(i + 1, j));
                            winningSpots.Add(Tuple.Create(i + 2, j));
                            return Tuple.Create(GameBoard[i, j], winningSpots);
                        }
                        else if (j + 1 < COLS && GameBoard[i + 2, j] == GameBoard[i + 2, j + 1])
                        {
                            winningSpots.Add(Tuple.Create(i + 2, j + 1));
                            winningSpots.Add(Tuple.Create(i, j));
                            winningSpots.Add(Tuple.Create(i + 1, j));
                            winningSpots.Add(Tuple.Create(i + 2, j));
                            return Tuple.Create(GameBoard[i, j], winningSpots);
                        }
                        else if (j - 1 >= 0 && GameBoard[i + 2, j] == GameBoard[i + 2, j - 1])
                        {
                            winningSpots.Add(Tuple.Create(i + 2, j - 1));
                            winningSpots.Add(Tuple.Create(i, j));
                            winningSpots.Add(Tuple.Create(i + 1, j));
                            winningSpots.Add(Tuple.Create(i + 2, j));
                            return Tuple.Create(GameBoard[i, j], winningSpots);
                        }
                    }
                }
            }  
            return Tuple.Create(' ', winningSpots);
        }
    }
}
