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
        public SinglePlayer()
        {
            this.DataContext = new SPViewModel();
            InitializeComponent();
        }

        private void restart_Click(object sender, RoutedEventArgs e)
        {
            (this.DataContext as SPViewModel).VMCols = 4;
            (this.DataContext as SPViewModel).VMRows = 4;
            (this.DataContext as SPViewModel).VMSerializedGame = "0001101010010101";
            (this.DataContext as SPViewModel).VMInitialPos = "0,1";
            (this.DataContext as SPViewModel).VMGoalPos = "3,2";

            mazeControl.Focus();
        }

        private void back_to_main_Click(object sender, RoutedEventArgs e)
        {

        }

        private void solve_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SinglePlayerWin_KeyDown(object sender, KeyEventArgs e)
        {
           
        }
    }
}
