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
using System.Windows.Shapes;

namespace GUIClient
{
    /// <summary>
    /// Interaction logic for SinglePlayer.xaml
    /// </summary>
    public partial class SinglePlayer : Window
    {
        private SPViewModel viewModel;
        private string name;
        private int rows;
        private int cols;

        public SinglePlayer()
        {
            this.viewModel = new SPViewModel(new SPModel());
            this.DataContext = this.viewModel;
            InitializeComponent();

            mazeControl.Focus();

            // to check if move is possible
            mazeControl.DoNotifyMove += this.viewModel.CheckMove;
        }

        private void restart_Click(object sender, RoutedEventArgs e)
        {
           

        
        }




        private void back_to_main_Click(object sender, RoutedEventArgs e)
        {

        }

        private void solve_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddBoardKeyDownEvent()
        {
            this.KeyDown += mazeControl_KeyDown;
        }

        private void RemoveBoardKeyDownEvent()
        {
            this.KeyDown -= mazeControl_KeyDown;
        }

        private void mazeControl_LostFocus(object sender, RoutedEventArgs e)
        {
           mazeControl.Focus();
        }

        private void mazeControl_KeyDown(object sender, KeyEventArgs e)
        {
            mazeControl.BoardKeyDown(sender, e);
        }

        public void SetDetails(string name, int rows, int cols)
        {
            this.name = name;
            this.rows = rows;
            this.cols = cols;
        }

        private void SinglePlayerWin_Loaded(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(this.name + this.rows.ToString() + this.cols.ToString());
            // add event handler for user control key down
            this.AddBoardKeyDownEvent();
            this.viewModel.StartGame(this.name, this.rows, this.cols);
        }
    }
}
