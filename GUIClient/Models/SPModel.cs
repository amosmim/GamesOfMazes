using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.ComponentModel;
using System.Net;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System.Threading;
using System.Windows;

using MazeGeneratorLib;
using MazeLib;

namespace GUIClient
{
    class SPModel : INotifyPropertyChanged
    {
        private Socket server;
        private int port;
        private string ip;
        private IPEndPoint ipep;

        private string playerPos;
        private string serializedGame;
        private string initialPos;
        private string goalPos;
        private string solution;
        private int rows;
        private int cols;

        private Maze maze;

        public string SerializedGame
        {
            get { return this.serializedGame; }
            set
            {
                this.serializedGame = value;
                this.OnPropertyChanged("SerializedGame");
            }
        }
        public string InitialPos
        {
            get { return this.initialPos; }
            set
            {
                this.initialPos = value;
                this.OnPropertyChanged("InitialPos");
            }
        }

        public string GoalPos
        {
            get { return this.goalPos; }
            set
            {
                this.goalPos = value;
                this.OnPropertyChanged("GoalPos");
            }
        }
        public int Rows
        {
            get { return this.rows; }
            set
            {
                this.rows = value;
                this.OnPropertyChanged("Rows");
            }
        }
        public int Cols
        {
            get { return this.cols; }
            set
            {
                this.cols = value;
                this.OnPropertyChanged("Cols");
            }
        }

        public string PlayerPosition
        {
            get { return this.playerPos; }
            set
            {
                this.playerPos = value;
                this.OnPropertyChanged("PlayerPosition");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public SPModel()
        {
            this.port = Properties.Settings.Default.ServerPort;
            this.ip = Properties.Settings.Default.ServerIP;

            this.ipep = new IPEndPoint(IPAddress.Parse(this.ip), this.port);
            this.server = null;
        }

        private bool Connect()
        {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(ipep);
            } catch (SocketException se)
            {
                return false;
            }

            return true;
        }

        private void CloseConnection()
        {
            server.Shutdown(SocketShutdown.Both);
            server.Dispose();
        }

        private string GetCorrectJSON(string data)
        {
            // single player command or end of communication
            if (data.EndsWith("-1"))
            {
                return data.Substring(0, data.Length - 2);
            }

            // multi player command
            if (data.EndsWith("1") && !data.EndsWith("-1"))
            {
                return data.Substring(0, data.Length - 1);
            }

            return data;
        }

        public int CheckMove(string direction)
        {
            string maze = this.maze.ToString();
            string[] temp = this.playerPos.Split(',');

            int x = Int32.Parse(temp[0]);
            int y = Int32.Parse(temp[1]);

            switch(direction)
            {
                case "up": int nX = x - 1;
                           int newPlayerPos = (nX * this.rows) + y;
                           if (this.serializedGame[newPlayerPos] == '0')
            }
        }

        public bool StartGame(string commandString)
        {
            // async job
            if (this.Connect())
            {
                new Thread(() =>
                {
                    byte[] data = new byte[8096];
                    int recv = 0;
                    try
                    {
                        server.Send(Encoding.ASCII.GetBytes(commandString));
                    } catch (SocketException se)
                    {
                        Console.WriteLine("error sending");
                        return;
                    }

                    try
                    {
                        recv = server.Receive(data);
                    }
                    catch (SocketException se)
                    {
                        Console.WriteLine("error reveining");
                        return;
                    }

                    // get data and parse it to json
                    string strData = Encoding.ASCII.GetString(data, 0, recv);
                    JObject tempJson = JObject.Parse(GetCorrectJSON(strData));
                    
                    this.maze = Maze.FromJSON(GetCorrectJSON(strData));

                    this.SerializedGame = (string)tempJson["Maze"];

                    this.Rows = this.maze.Rows;
                    this.Cols = this.maze.Cols;
                    this.InitialPos = this.maze.InitialPos.Row + "," + this.maze.InitialPos.Col;
                    this.GoalPos = this.maze.GoalPos.Row + "," + this.maze.GoalPos.Col;
                    this.PlayerPosition = this.InitialPos;
                }).Start();
            } else
            {
                return false;
            }

            return true;
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
