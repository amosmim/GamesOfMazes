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


namespace GUIClient
{
    /// <summary>
    /// Interaction logic for MazeDetails.xaml
    /// </summary>
    public partial class MazeDetails : UserControl
    {
        public delegate void PassInfo(string mazeName, int rows, int cols);
        public event PassInfo OnButtonClick;

        /// <summary>
        /// Constructor.
        /// </summary>
        public MazeDetails()
        {
            InitializeComponent();
          

            rows.Text = Properties.Settings.Default.rows.ToString();
            cols.Text = Properties.Settings.Default.cols.ToString();
        }

        /// <summary>
        /// Invoke related event for button clicking.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OnButtonClick?.Invoke(name.Text, Convert.ToInt32(rows.Text), Convert.ToInt32(cols.Text));
        }
    }
}
