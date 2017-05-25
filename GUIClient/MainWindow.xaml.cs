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
using ap2ex1_server;
using System.Net.Sockets;

namespace GUIClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            BitmapImage logo = new BitmapImage(new Uri("pack://application:,,,/GUIClient;component/Resources/logo.png"));

            logoRec.Fill = new ImageBrush(logo);
        }


        /// <summary>
        /// Initiate single player dialog.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void singleButton_Click(object sender, RoutedEventArgs e)
        {
            SPDialog spDialog = new SPDialog();

            spDialog.ShowDialog();
        }

        /// <summary>
        /// Initiate multiplayer dialog.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void MultiplayButton_Click(object sender, RoutedEventArgs e)
        {
            MPDialog multiplay = new MPDialog();
            multiplay.ShowDialog();
        }

        /// <summary>
        /// Initiate settings.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void settingsButton_Click(object sender, RoutedEventArgs e)
        {
            Settings settings = new Settings();
            settings.Show();
        }
    }
}
