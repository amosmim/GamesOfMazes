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
    /// Interaction logic for SPDialog.xaml
    /// </summary>
    public partial class SPDialog : Window
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SPDialog()
        {
            InitializeComponent();

            mazeDetails.OnButtonClick += LaunchGame;
        }

        /// <summary>
        /// Launch single play game.
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="rows">rows</param>
        /// <param name="cols">cols</param>
        private void LaunchGame(string name, int rows, int cols)
        {
            SinglePlayer sp = new SinglePlayer();

            sp.SetDetails(name, rows, cols);

            sp.Show();

            this.Close();
        }
    }
}
