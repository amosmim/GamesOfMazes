using GUIClient.ViewModels;
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
    /// Interaction logic for MPDialog.xaml
    /// </summary>
    public partial class MPDialog : Window
    {
        public MPDialog()
        {
            InitializeComponent();
            NewGame.OnButtonClick += NewMultiGameLaunch;
            JoinGame.OnButtonClick += JoinGameLaunch;
           

        }
      

        private void JoinGameLaunch()
        {
            MultiPlayer mp = new MultiPlayer();
            mp.SetViewModel(this.JoinGame.getViewModel());
            string name =  this.JoinGame.gameList.SelectedItem.ToString();
            if(name == null) { return; }
            if (!mp.JoinToGame(name))
            {
                MessageBox.Show("can't connect to the server.");
                this.Close();
            }
            mp.Show();
            this.Close();
        }
        private void NewMultiGameLaunch(string name, int rows, int cols)
        {
            MultiPlayer mp = new MultiPlayer();
           
            mp.SetViewModel(this.JoinGame.getViewModel());
            mp.SetDetails(name, rows, cols);
            if (!mp.StartMultiGame())
            {
                MessageBox.Show("can't connect to the server.");
                this.Close();
            }
           
            mp.Show();

            this.Close();
        }
    }
}
