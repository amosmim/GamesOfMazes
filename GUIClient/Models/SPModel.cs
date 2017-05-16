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

        private JObject maze;

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
                this.OnPropertyChanged("PlayerPositon");
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

        public bool StartGame(string commandString)
        {
            // async job
            if (!this.Connect())
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
                    }

                    try
                    {
                        recv = server.Receive(data);
                    }
                    catch (SocketException se)
                    {
                        Console.WriteLine("error reveining");
                    }

                    // get data and parse it to json
                    string strData = Encoding.ASCII.GetString(data, 0, recv);
                    this.maze = JObject.Parse(strData);

                    // assign correct values
                    this.Rows = Convert.ToInt32((string)maze["Rows"]);
                    this.Cols = Convert.ToInt32((string)maze["Cols"]);
                    this.SerializedGame = (string)maze["Maze"];
                    string temp = (string)maze["Start"];
                    this.InitialPos = (string)JObject.Parse(temp)["Row"] + "," + (string)JObject.Parse(temp)["Col"];

                    temp = (string)maze["End"];
                    this.GoalPos = (string)JObject.Parse(temp)["Row"] + "," + (string)JObject.Parse(temp)["Col"];

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
