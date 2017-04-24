using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeLib;
using System.Windows.Controls;
///using System.Drawing;

namespace GUIClient
{
    class MazeGuiDrawer
    {
        private Canvas canvas;
        private Label[] list;

        private Image wall;
        private Image floor;
        private Image playerPic;

        public MazeGuiDrawer(Canvas s )
        {
            this.canvas = s;
            //this.floor = floor;
            //this.wall = wall;
            //this.playerPic = player;
            
        }

        public void PlayerMoveTo(Position point)
        {

        }

        public void Draw( Maze maze, Position playerStart)
        {
            //Maze maze = new Maze(10, 10);
            int size = maze.Rows * maze.Cols;
            this.list = new Label[size];
            int rowDel = (int)canvas.Height /  maze.Rows;
            int colDel = (int)canvas.Width / maze.Cols;

            string toDrow = maze.ToString();
            toDrow= toDrow.Replace("\r\n", string.Empty);

            for (int row = 0; row < maze.Rows; row ++)
            {
                for (int col = 0; col < maze.Cols; col++)
                {
                    int i = (row * maze.Rows) + col;
                    list[i] = new Label()
                    {
                        Width = rowDel,
                        Height = colDel
                        
                    };
                    
                    canvas.Children.Add(list[i]);
                    //list[i].SetValue(Canvas.LeftProperty, col * colDel);
                    //list[i].SetValue(Canvas.LeftProperty, col * colDel);
                    Canvas.SetLeft(list[i], col*colDel);
                    Canvas.SetTop(list[i], row*rowDel);
                    if (toDrow[i] == '0')
                    {
                        list[i].Content = "f";
                       
                    }
                    else if (toDrow[i] == '1')
                    {
                        list[i].Content = "w";
                    }
                    else if (toDrow[i] == '#')
                    {
                        list[i].Content = "s";
                    }
                    else
                    {
                        list[i].Content = "e";
                    }

                }
            }
        }

    }
}
