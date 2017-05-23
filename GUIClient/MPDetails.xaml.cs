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
using GUIClient.Models;
using GUIClient.ViewModels;


namespace GUIClient
{
    /// <summary>
    /// Interaction logic for MPDetails.xaml
    /// </summary>
    public partial class MPDetails : UserControl
    {
        public delegate void PassInfo();
        public event PassInfo OnButtonClick;
        private List<string> gameslist;
        private MPViewModel viewModel;
        private string message;
        public MPDetails()
        {
            InitializeComponent();
            this.viewModel = new MPViewModel(new MPModel());
            List<string> gameslist =  viewModel.getGameList();
            foreach(string game in gameslist)
            {
                gameList.Items.Add(game);
            }
            this.Message = "";
            
        }
        public MPViewModel getViewModel()
        {
            return this.viewModel;
        } 

        public string Message
        {
            get { return this.message; }
            set { this.message = value; }
        }
        
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void JoinButton_Click(object sender, RoutedEventArgs e)
        {
            OnButtonClick?.Invoke();
        }
    }
}
