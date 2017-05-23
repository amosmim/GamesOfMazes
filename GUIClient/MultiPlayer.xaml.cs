using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GUIClient.ViewModels;
using GUIClient.Models;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;

namespace GUIClient
{
    /// <summary>
    /// Interaction logic for MultiPlayer.xaml
    /// </summary>
    public partial class MultiPlayer : Window
    {
        private MPViewModel viewModel;
        private string name;
        private int rows;
        private int cols;
        private bool isEventHandled;

        public MultiPlayer()
        {
     
            InitializeComponent();
            mazeControl.Focus();

            // to check if move is possible
            mazeControl.DoNotifyMove += this.CheckMove;
           
            this.isEventHandled = false;
        }
        public void SetViewModel(MPViewModel viewModel)
        {
            this.viewModel = viewModel;
            this.DataContext = this.viewModel;
            this.viewModel.VMRavilWin += delegate ()
            {
                MessageBox.Show("Sorry... You Lose!");
                this.RemoveBoardKeyDownEvent();
            };
            this.viewModel.VMRavilQuit += delegate ()
            {
                MessageBox.Show("your ravil disconnected...");
                this.RemoveBoardKeyDownEvent();
            };
           
        }
        
        public bool StartMultiGame()
        {
            return this.viewModel.StartGame(this.name, this.rows, this.cols);
        }

        public bool JoinToGame(string name)
        {
            return this.viewModel.JoinToGame(name);
        }

        
        public void SetDetails(string name, int rows, int cols)
        {
            this.name = name;
            this.rows = rows;
            this.cols = cols;
           
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
        /// Helper function to remove the ability to make key down actions on the user control.
        /// </summary>
        private void RemoveBoardKeyDownEvent()
        {
            this.KeyDown -= mazeControl_KeyDown;
            this.isEventHandled = false;
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
        /// Using helper function inside the user control to assit with preserving focus
        /// on key down actions.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void mazeControl_KeyDown(object sender, KeyEventArgs e)
        {
            mazeControl.BoardKeyDown(sender, e);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            MessageBox.Show("The game will close now.");
            this.viewModel.CloseGame();
        }


        /// <summary>
        /// Initiate the game when multi player window finished loading.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MultiPlayerWin_Loaded(object sender, RoutedEventArgs e)
        {
            bool result;
            // add event handler for user control key down
            this.AddBoardKeyDownEvent();
            result = this.viewModel.UpdateGame();
            if (!result)
            {
                MessageBox.Show("Error generating game.");
            }
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
        /// Helper function to add the ability to make key down actions on the user control.
        /// </summary>
        private void AddBoardKeyDownEvent()
        {
            this.KeyDown += mazeControl_KeyDown;
            this.isEventHandled = true;
        }
    }
}
