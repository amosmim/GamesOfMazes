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
using System.ComponentModel;

namespace GUIClient
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        private SetModel setModel;
        private SetViewModel setViewModel;

        public Settings()
        {
            this.setModel = new SetModel();
            this.setViewModel = new SetViewModel(this.setModel);

            DataContext = setViewModel;
            InitializeComponent();

            this.setViewModel.GetData();
        }

        private void Ok_Button_Click(object sender, RoutedEventArgs e)
        {
            SettingsData setData = new SettingsData();

            setData.IP = IPText.Text;
            setData.port = Int32.Parse(PortText.Text);
            setData.rows = Int32.Parse(MazeRows.Text);
            setData.cols = Int32.Parse(MazeCols.Text);

            if((AlgoChooser.SelectedItem as ComboBoxItem).Content.ToString() == "BFS")
            {
                MessageBox.Show("0");
                setData.algo = 0;
            }
            else
            {
                MessageBox.Show("1");
                setData.algo = 1;
            }
            

            this.setViewModel.SetData(setData);

            this.Close();
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AlgoChooser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}
