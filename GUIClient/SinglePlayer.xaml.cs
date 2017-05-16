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
        public SinglePlayer()
        {
            this.viewModel = new SPViewModel(new SPModel());
            this.DataContext = this.viewModel;
            InitializeComponent();

            mazeControl.Focus();

            mazeControl.DoNotifyMove += this.viewModel.CheckMove;
        }

        private void restart_Click(object sender, RoutedEventArgs e)
        {
            (this.DataContext as SPViewModel).VMCols = 4;
            (this.DataContext as SPViewModel).VMRows = 4;
            (this.DataContext as SPViewModel).VMSerializedGame = "0001101010010101";
            (this.DataContext as SPViewModel).VMInitialPos = "0,1";
            (this.DataContext as SPViewModel).VMGoalPos = "3,2";

            this.AddBoardKeyDownEvent();
        }




        private void back_to_main_Click(object sender, RoutedEventArgs e)
        {

        }

        private void solve_Click(object sender, RoutedEventArgs e)
        {

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
    }
}
