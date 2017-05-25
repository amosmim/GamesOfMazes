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
        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <returns></returns>
        public MPViewModel getViewModel()
        {
            return this.viewModel;
        }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message
        {
            get { return this.message; }
            set { this.message = value; }
        }

        /// <summary>
        /// Handles the SelectionChanged event of the ComboBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        /// <summary>
        /// Handles the Click event of the JoinButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void JoinButton_Click(object sender, RoutedEventArgs e)
        {
            OnButtonClick?.Invoke();
        }
    }
}
