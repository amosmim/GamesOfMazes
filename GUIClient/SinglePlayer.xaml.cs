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

        /// <summary>
        /// Constructor.
        /// </summary>
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
        
        /// <summary>
        /// Restart the game.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void restart_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.Restart();

            mazeControl.Restart();

            this.AddBoardKeyDownEvent();
        }

        /// <summary>
        /// Check if player move is valid.
        /// 
        /// if result = 2 than player won.
        /// </summary>
        /// <param name="direction">direction</param>
        /// <returns>int - 0 = no move 1 = valid move 2 = win</returns>
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

        /// <summary>
        /// Go back to main.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void back_to_main_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Initiate solving algorithm and animate the solution.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
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
        
        /// <summary>
        /// Helper function to add the ability to make key down actions on the user control.
        /// </summary>
        private void AddBoardKeyDownEvent()
        {
            this.KeyDown += mazeControl_KeyDown;
        }

        /// <summary>
        /// Helper function to remove the ability to make key down actions on the user control.
        /// </summary>
        private void RemoveBoardKeyDownEvent()
        {
            this.KeyDown -= mazeControl_KeyDown;
        }

        /// <summary>
        /// Get the focus back to the user control.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void mazeControl_LostFocus(object sender, RoutedEventArgs e)
        {
           mazeControl.Focus();
        }

        /// <summary>
        /// Using helper function inside the user control to assit with preserving focus
        /// on key down actions.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void mazeControl_KeyDown(object sender, KeyEventArgs e)
        {
            mazeControl.BoardKeyDown(sender, e);
        }

        /// <summary>
        /// Set the details of the maze.
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="rows">rows</param>
        /// <param name="cols">cols</param>
        public void SetDetails(string name, int rows, int cols)
        {
            this.name = name;
            this.rows = rows;
            this.cols = cols;
        }

        /// <summary>
        /// Initiate the game when single player window finished loading.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
