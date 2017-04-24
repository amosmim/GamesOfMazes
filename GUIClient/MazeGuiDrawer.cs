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
            int size = maze.Rows * maze.Cols;
            this.list = new Label[size];
            int rowDel = (int)canvas.Height /  maze.Rows;
            int colDel = (int)canvas.Width / maze.Cols;

            string toDrow = maze.ToString();

            for (int row = 0; row < size; row += maze.Cols)
            {
                for (int col = 0; col < size; col++)
                {
                    int i = row + col;
                    list[i] = new Label();
                    list[i].Width = rowDel;
                    list[i].Height = colDel;
                    Canvas.SetLeft(list[i], col*colDel);
                    if (toDrow[i] == '0')
                    {
                        list[i].Content = "f";
                    }
                    else
                    {
                        list[i].Content = "w";
                    }

                }
            }
        }

    }
}
