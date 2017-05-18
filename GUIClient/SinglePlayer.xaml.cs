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
using System.Windows.Shapes;

namespace GUIClient
{
    /// <summary>
    /// Interaction logic for SinglePlayer.xaml
    /// </summary>
    public partial class SinglePlayer : Window
    {
        private SPViewModel viewModel;
        private string name;
        private int rows;
        private int cols;
        private int searcher;

        public SinglePlayer()
        {
            this.viewModel = new SPViewModel(new SPModel());
            this.DataContext = this.viewModel;
            InitializeComponent();

            mazeControl.Focus();

            // to check if move is possible
            mazeControl.DoNotifyMove += this.CheckMove;

            this.searcher = Properties.Settings.Default.algo;
        }

        private void restart_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.Restart();

            mazeControl.Restart();

            this.AddBoardKeyDownEvent();
        }

        public int CheckMove(string direction)
        {
            int result = this.viewModel.CheckMove(direction);

            if (result == 2)
            {
                MessageBox.Show("Great! You Win !");
                this.RemoveBoardKeyDownEvent();
            }

            return result;
        }

        private void back_to_main_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void solve_Click(object sender, RoutedEventArgs e)
        {
            bool result;
            this.RemoveBoardKeyDownEvent();

            result = this.viewModel.InitiateSolution(this.name, this.searcher);
            if (!result)
            {
                MessageBox.Show("Error solving.");
            }
        }

        private void AddBoardKeyDownEvent()
        {
            this.KeyDown += mazeControl_KeyDown;
        }

        private void RemoveBoardKeyDownEvent()
        {
            this.KeyDown -= mazeControl_KeyDown;
        }

        private void mazeControl_LostFocus(object sender, RoutedEventArgs e)
        {
           mazeControl.Focus();
        }

        private void mazeControl_KeyDown(object sender, KeyEventArgs e)
        {
            mazeControl.BoardKeyDown(sender, e);
        }

        public void SetDetails(string name, int rows, int cols)
        {
            this.name = name;
            this.rows = rows;
            this.cols = cols;
        }

        private void SinglePlayerWin_Loaded(object sender, RoutedEventArgs e)
        {
            bool result;
            // add event handler for user control key down
            this.AddBoardKeyDownEvent();
            result = this.viewModel.StartGame(this.name, this.rows, this.cols);
            if (!result)
            {
                MessageBox.Show("Error generating game.");
            }
        }
    }
}
