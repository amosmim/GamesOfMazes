using ap2ex1_server;
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
        private Maze maze;
        private MazeGuiDrawer drawer;
        public SinglePlayControl()
        {
            GetNewMaze();
            

            InitializeComponent();
            drawer = new MazeGuiDrawer(space);
            drawer.Draw(maze, maze.InitialPos);
            
            //drawer.Draw(null, new Position(0, 0));

        }

        private void GetNewMaze()
        {
            this.server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            server.Connect(new IPEndPoint(IPAddress.Parse(ConfigurationManager.AppSettings["ip"]), int.Parse(ConfigurationManager.AppSettings["port"])));
            string s = "generate Single " + ConfigurationManager.AppSettings["rows"] + " " + ConfigurationManager.AppSettings["cols"];
            server.Send(Encoding.ASCII.GetBytes(s));
            byte[] data = new byte[8096];
            int recv = 0;
            try
            {
                recv = server.Receive(data); 
                string json = System.Text.Encoding.ASCII.GetString(data);
                json =  json.Substring(0, recv) ;

                // set the new maze that the server give
                //this.maze = JsonConvert.DeserializeObject<Maze>(json);
                this.maze = Maze.FromJSON(json);
                return;
            }
            catch (SocketException se)
            {
                NoConection(se);
                server.Shutdown(SocketShutdown.Both);
                server.Dispose();
              
            }

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

        }

        private void back_to_main_Click(object sender, RoutedEventArgs e)
        {

        }

        private void solve_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
