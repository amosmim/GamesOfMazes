﻿using System;
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
        
        
        public MainWindow()
        {
           
            InitializeComponent();
        }

        private void SinglePlayControl_Loaded(object sender, RoutedEventArgs e)
        {
            //cont = new SinglePlayControl();
        }
        
    }
}
