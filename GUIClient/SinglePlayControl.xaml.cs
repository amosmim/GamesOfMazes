﻿using ap2ex1_server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Net.Sockets;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MazeLib;
using System.Net;
using System.Configuration;
using Newtonsoft.Json;

namespace GUIClient
{
    
    /// <summary>
    /// Interaction logic for SinglePlayControl.xaml
    /// </summary>
    public partial class SinglePlayControl : UserControl
    {
        private Socket server;

        public SinglePlayControl()
        {
            this.DataContext = new SPViewModel();

            InitializeComponent();
        }

        /// <summary>
        /// need to be edit to pop up window!
        /// </summary>
        private void NoConection(SocketException se)
        {
            Console.WriteLine("no conection!!!\n" + se.ToString());
        }


        private void restart_Click(object sender, RoutedEventArgs e)
        {
            (this.DataContext as SPViewModel).VMCols = 4;
            (this.DataContext as SPViewModel).VMRows = 4;
            (this.DataContext as SPViewModel).VMSerializedGame = "0001101010010101";
            (this.DataContext as SPViewModel).VMInitialPos = "0,1";
            (this.DataContext as SPViewModel).VMGoalPos = "3,2";
        }

        private void back_to_main_Click(object sender, RoutedEventArgs e)
        {

        }

        private void solve_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
