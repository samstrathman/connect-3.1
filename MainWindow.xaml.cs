using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Media.Animation;

namespace TicTacToe299
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AI_Player AI = new AI_Player();
        Game TTT_Game = new Game();

        private int difficulty = 3;
        private int xWins = 0;
        private int oWins = 0;
        private int numTies = 0;
        string winningName = "";

        public MainWindow()
        {
            InitializeComponent();
            DrawGridlines();
        }
        void DrawGridlines()
        {
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    Border b = new Border();
                    b.BorderBrush = Brushes.Black;
                    b.BorderThickness = new Thickness(1);

                    PlayGrid.Children.Add(b);
                    Grid.SetRow(b, i);
                    Grid.SetColumn(b, j);
                }
            }
        }

        void DrawX(int row, int col)
        {
            const int CELL_PADDING = 5;
            Ellipse ellipse = new Ellipse();
            ellipse.Stroke = Brushes.Black;
            ellipse.Fill = Brushes.Black;
            ellipse.Height = PlayGrid.ActualHeight / 6 - CELL_PADDING;
            ellipse.Width = PlayGrid.ActualWidth / 7 - CELL_PADDING;
            ellipse.StrokeThickness = 3;
            PlayGrid.Children.Add(ellipse);
            Grid.SetRow(ellipse, row);
            Grid.SetColumn(ellipse, col);
        }
        void DrawO(int row, int col)
        {
            const int CELL_PADDING = 5;
            Ellipse ellipse = new Ellipse();
            ellipse.Stroke = Brushes.Red;
            ellipse.Fill = Brushes.Red;
            ellipse.Height = PlayGrid.ActualHeight / 6 - CELL_PADDING;
            ellipse.Width = PlayGrid.ActualWidth / 7 - CELL_PADDING;
            ellipse.StrokeThickness = 3;
            PlayGrid.Children.Add(ellipse);
            Grid.SetRow(ellipse, row);
            Grid.SetColumn(ellipse, col);
        }

        private void PlayGrid_MouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            var point = Mouse.GetPosition(PlayGrid);

            int row = 0;
            int col = 0;
            double accumulatedHeight = 0.0;
            double accumulatedWidth = 0.0;

            // calc row mouse was over
            foreach (var rowDefinition in PlayGrid.RowDefinitions)
            {
                accumulatedHeight += rowDefinition.ActualHeight;
                if (accumulatedHeight >= point.Y)
                   break;
            }

            // calc col mouse was over
            foreach (var columnDefinition in PlayGrid.ColumnDefinitions)
            {
                accumulatedWidth += columnDefinition.ActualWidth;
                if (accumulatedWidth >= point.X)
                    break;
                col++;
            }
            if (TTT_Game.CellIsEmpty(row, col))
            {
                MarkBoard(row, col);
                char winner = TTT_Game.checkWinner().Item1;
                if (winner == ' ')
                {
                    if (chkAI.IsChecked.HasValue && chkAI.IsChecked.Value == true && TTT_Game.getTurn() == 'O')
                   {
                        if (difficultySlider.Value <= 1.5)
                        {
                            difficulty = 1;
                        }
                        else if (difficultySlider.Value > 1.5 && difficultySlider.Value <= 2.5)
                        {
                            difficulty = 2;
                        }
                        else if (difficultySlider.Value > 2.5 && difficultySlider.Value <= 3.5)
                        {
                            difficulty = 3;
                        }
                        else
                        {
                            difficulty = 4;
                        }
                        Spot S = AI.GetBestMove(TTT_Game, difficulty);
                        //MessageBox.Show(string.Format("Score is {0}!", S.score));
                        MarkBoard(S.row, S.col);
                    }
                }
            }
        }
        private void MarkBoard(int row, int col)
        {
            if (TTT_Game.getTurn() == 'X' && TTT_Game.CellIsEmpty(row, col))
            {
                //find the lowest available space for the piece to fall into
                int rowIndex = 0;
                while (rowIndex <= 5 && TTT_Game.CellIsEmpty(rowIndex, col))
                {
                    rowIndex++;
                }
                if (rowIndex <= 0)
                {
                    DrawX(0, col);
                }
                else
                {
                    DrawX(rowIndex - 1, col);
                    TTT_Game.MarkGameBoard(rowIndex - 1, col);
                }
            }
            else if (TTT_Game.getTurn() == 'O' && TTT_Game.CellIsEmpty(row, col))
            {
                //find the lowest available space for the piece to fall into
                int rowIndex = 0;
                while (rowIndex <= 5 && TTT_Game.CellIsEmpty(rowIndex, col))
                {
                    rowIndex++;
                }
                if (rowIndex <= 0)
                {
                    DrawO(0, col);
                }
                else
                {
                    DrawO(rowIndex - 1, col);
                    TTT_Game.MarkGameBoard(rowIndex - 1, col);
                }
            }

            char winner = TTT_Game.checkWinner().Item1;
            if (winner != ' ')
            {
                List<Tuple<int, int>> winningSpots = TTT_Game.checkWinner().Item2;
                foreach (var child in PlayGrid.Children)
                {
                    var shape = child as Shape;

                    if (shape != null)
                    {
                        foreach (var spot in winningSpots)
                        {
                            if (Grid.GetRow(shape) == spot.Item1 && Grid.GetColumn(shape) == spot.Item2)
                            {
                                shape.Stroke = Brushes.Blue;
                                shape.Fill = Brushes.Blue;
                                shape.StrokeThickness = 6;
                            }
                        }
                    }
                }

                if (winner == 'X')
                {
                    winningName = "Black";
                    xWins++;
                }
                else if (winner == 'O')
                {
                    winningName = "Red";
                    oWins++;
                }
                MessageBox.Show(string.Format("Winner is {0}!", winningName));
                lblP1Wins.Content = xWins.ToString();
                lblP2Wins.Content = oWins.ToString();
                reset();
            }
            else if (TTT_Game.BoardIsFull())
            {
                MessageBox.Show(string.Format("CAT SCRATCH!"));
                reset();
                numTies++;
                lblTies.Content = numTies.ToString();
            }
        }

        void reset()
        {
            PlayGrid.Children.Clear();
            DrawGridlines();
            TTT_Game = new Game(TTT_Game.turn); 
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            //Reset the board as well as the scores
            reset();
            lblP1Wins.Content = "0";
            lblP2Wins.Content = "0";
            lblTies.Content = "0";
            lblP1Name.Content = "Player 1 Wins";
            lblP2Name.Content = "Player 2 Wins";
            txtP1Name.Text = "Enter name here";
            txtP2Name.Text = "Enter name here";
            chkAI.IsChecked = true;
        }

        private void txtP1Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            lblP1Name.Content = txtP1Name.Text;
        }

        private void txtP2Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            lblP2Name.Content = txtP2Name.Text;
        }
    }
}
