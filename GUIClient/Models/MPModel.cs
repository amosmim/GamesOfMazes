using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using MazeLib;
using System.Net.Sockets;
using System.Threading;

namespace GUIClient.Models
{
    public class MPModel : INotifyPropertyChanged
    {
        private Socket server;
        private int port;
        private string ip;
        private IPEndPoint ipep;

        private string playerPos;
        private string serializedGame;
        private string mazeToString;
        private string initialPos;
        private string goalPos;
        private string name;
        private int rows;
        private int cols;

        private bool connectionOn;
        private bool gameIsInitilazed;

        private Maze maze;
        private string ravilplayerPos;

        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// Constractor.
        /// </summary>
        public MPModel()
        {

            this.port = Properties.Settings.Default.ServerPort;
            this.ip = Properties.Settings.Default.ServerIP;
            this.ipep = new IPEndPoint(IPAddress.Parse(this.ip), this.port);
            this.server = null;
            this.connectionOn = false;
            this.gameIsInitilazed = false;
        }

        /// <summary>
        /// Notify on property changed.
        /// </summary>
        /// <param name="propertyName">property name</param>
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// SerializedGame Property.
        /// </summary>
        public string SerializedGame
        {
            get { return this.serializedGame; }
            set
            {
                this.serializedGame = value;
                this.OnPropertyChanged("SerializedGame");
            }
        }

        /// <summary>
        /// Name Property.
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set
            {
                this.name = value;
                this.OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// InitialPos Property.
        /// </summary>
        public string InitialPos
        {
            get { return this.initialPos; }
            set
            {
                this.initialPos = value;
                this.OnPropertyChanged("InitialPos");
            }
        }

        /// <summary>
        /// GoalPos Property.
        /// </summary>
        public string GoalPos
        {
            get { return this.goalPos; }
            set
            {
                this.goalPos = value;
                this.OnPropertyChanged("GoalPos");
            }
        }

        /// <summary>
        /// Rows Property.
        /// </summary>
        public int Rows
        {
            get { return this.rows; }
            set
            {
                this.rows = value;
                this.OnPropertyChanged("Rows");
            }
        }

        /// <summary>
        /// Cols Property.
        /// </summary>
        public int Cols
        {
            get { return this.cols; }
            set
            {
                this.cols = value;
                this.OnPropertyChanged("Cols");
            }
        }

        internal void CloseGame()
        {
            byte[] data = new byte[8096];
            if (this.connectionOn)
            {
                try
                {
                    server.Send(Encoding.ASCII.GetBytes("close " + this.Name));
                }
                catch (SocketException se)
                {
                    Console.WriteLine("error sending play command");

                }
                this.CloseConnection();
            }
            
            this.gameIsInitilazed = false;
        }

        /// <summary>
        /// PlayerPosition Property.
        /// </summary>
        public string PlayerPosition
        {
            get { return this.playerPos; }
            set
            {
                this.playerPos = value;

                this.OnPropertyChanged("PlayerPosition");

            }
        }
        public event Action RivalQuit;
        public bool StartUpdateThreads()
        {
            if (!this.connectionOn)
            {
                //debug print
                Console.WriteLine("conncetion doesn't started...");
                //end debug
                return false;
            }
            // wait for startgame() function end 
            while ((this.connectionOn) && (!this.gameIsInitilazed)) { }

            // open listen thread
            new Thread(() =>
            {
                while (this.connectionOn)
                {
                    byte[] data = new byte[8096];
                    int recv = 0;
                    try
                    {
                        recv = server.Receive(data);
                    }
                    catch (SocketException se)
                    {
                        Console.WriteLine("error reveining");
                        break;
                    }
                    string response = Encoding.ASCII.GetString(data, 0, recv);
                    if (response.Contains("The other player closed the game."))
                    {   // ravil end is connection
                        this.RivalQuit?.Invoke();
                        
                        break;
                    } else if (response.Contains("Direction"))
                    {   // get new ravil position
                        JObject tempJson = JObject.Parse(GetCorrectJSON(response));

                        RavilMove((string)tempJson["Direction"]);
                    }
                    else
                    { //get something else
                    }
                }

            }).Start();



            return true;
        }
        public bool IsStart
        {
            get { return this.gameIsInitilazed; }
           
        }

        public string RavilPlayerPosition {
            get { return this.ravilplayerPos; }
            set
            {
                this.ravilplayerPos = value;
                this.OnPropertyChanged("RavilPlayerPosition");
            }
        }

        public delegate void rivalWin();
        public event rivalWin RivalWin;

        /// <summary>
        /// Close Connection to the server.
        /// </summary>
        private void CloseConnection()
        {
            this.connectionOn = false;
            server.Shutdown(SocketShutdown.Both);
            server.Dispose();
            server = null;

        }


        /// <summary>
        /// Gets the games list from server.
        /// </summary>
        /// <returns></returns>
        public List<string> GetGameList()
        {
            List<string> gamelist;
            if (this.Connect())
            {
                    string strdata = SendAndRecived("list");
                    //JObject tempJson = JObject.Parse(GetCorrectJSON(strdata));
                    gamelist = JsonConvert.DeserializeObject<List<string>>(strdata);
                    this.CloseConnection();
                    return gamelist;
                

            } else { return new List<string>(); }
            
        }

   

        /// <summary>
        /// Check if player move is valid.
        /// </summary>
        /// <param name="direction">direction</param>
        /// <returns>
        /// 0 - not valid
        /// 1 - valid
        /// 2 - win
        /// </returns>
        public int CheckMove(string direction)
        {
            string maze = this.maze.ToString();
            string[] temp = this.playerPos.Split(',');
            int nX = 0;
            int nY = 0;
            int newPlayerPos;
            int returnVal = 0;

            int x = Int32.Parse(temp[0]);
            int y = Int32.Parse(temp[1]);

            switch (direction)
            {
                case "up":
                    nX = x - 1;
                    newPlayerPos = (nX * this.Cols) + y;
                    if ((returnVal = Move(newPlayerPos)) != 0)
                    {
                        this.PlayerPosition = nX + "," + y;
                    }
                    break;
                case "right":
                    nY = y + 1;
                    newPlayerPos = (x * this.Cols) + nY;
                    if ((returnVal = Move(newPlayerPos)) != 0)
                    {
                        this.PlayerPosition = x + "," + nY;
                    }
                    break;
                case "down":
                    nX = x + 1;
                    newPlayerPos = (nX * this.Cols) + y;
                    if ((returnVal = Move(newPlayerPos)) != 0)
                    {
                        this.PlayerPosition = nX + "," + y;
                    }
                    break;
                case "left":
                    nY = y - 1;
                    newPlayerPos = (x * this.Cols) + nY;
                    if ((returnVal = Move(newPlayerPos)) != 0)
                    {
                        this.PlayerPosition = x + "," + nY;
                    }
                    break;
                default: break;
            }

            // when the game is initilazed and the move is valid
            if (returnVal != 0 && this.gameIsInitilazed) 
            {
                // send update thread
                new Thread(() =>
                {
                    byte[] data = new byte[8096];
                    
                    try
                    {
                        server.Send(Encoding.ASCII.GetBytes("play " + direction));
                    }
                    catch (SocketException se)
                    {
                        Console.WriteLine("error sending play command");
                        
                    }
                }
                ).Start();
            }
            return returnVal;
        }


        private void RavilMove(string direction)
        {
            string[] temp = this.ravilplayerPos.Split(',');
            int x = Int32.Parse(temp[0]);
            int y = Int32.Parse(temp[1]);
            switch (direction)
            {
                case "up":
                    x = x - 1;
                    break;
                case "down":
                    x = x + 1;
                    break;
                case "right":
                    y= y + 1;
                    break;
                case "left":
                    y= y - 1;
                    break;
            }
            this.RavilPlayerPosition = x + "," + y;

            if (Move(x * this.Cols + y) == 2)
            {
                this.RivalWin?.Invoke();
            }

        }


        /// <summary>
        /// Helper function to check if player move is valid.
        /// </summary>
        /// <param name="newPlayerPos">the new position</param>
        /// <returns>0 - not valid
        ///          1 - valid
        ///          2 - win</returns>
        private int Move(int newPlayerPos)
        {
            if (newPlayerPos > this.mazeToString.Length || newPlayerPos < 0)
            {
                return 0;
            }

            if (this.mazeToString[newPlayerPos] == '0' || this.mazeToString[newPlayerPos] == '*')
            {
                return 1;
            }

            if (this.mazeToString[newPlayerPos] == '#')
            {
                return 2;
            }

            if (this.mazeToString[newPlayerPos] == '1')
            {
                return 0;
            }

            return 0;
        }

        /// <summary>
        /// Start and asynchronous job for generating a new game.
        /// </summary>
        /// <param name="commandString">command</param>
        /// <returns>true if success else false</returns>
        public bool StartGame(string commandString)
        {
            // async job
            if (this.Connect())
            {
                new Thread(() =>
                {

                    string data = SendAndRecived(commandString);
                    //debuging print
                    Console.WriteLine(data);
                    //end of debug
                    if (data == null || !data.Contains("Waiting for another player to join"))
                    {
                        return;
                    }
                    ReciveMaze();


                }).Start();

            }
            else
            {
                return false;
            }
            return true;
        }

        public bool JoinToGame(string name)
        {
            if (this.Connect())
            {
                new Thread(() =>
                {
                   
                    {
                        server.Send(Encoding.ASCII.GetBytes("join " + name));
                    }
                    ReciveMaze();
                }).Start();
            }
            else
            {
                return false;
            }
            return true;
        }

        private void ReciveMaze()
        {
            byte[] strDataBytes = new byte[8096];
            string strData = null;
            int recv = 0;
            try
            {
                recv = server.Receive(strDataBytes);
                strData = Encoding.ASCII.GetString(strDataBytes, 0, recv);
            }
            catch (SocketException se)
            {
                Console.WriteLine("error reveining");

            }
            if (strData == null)
            {
                return;
            }

            JObject tempJson = JObject.Parse(GetCorrectJSON(strData));

            this.maze = Maze.FromJSON(GetCorrectJSON(strData));

            this.SerializedGame = (string)tempJson["Maze"];
            this.Name = maze.Name;
            this.mazeToString = maze.ToString().Replace("\r\n", string.Empty);

            this.Rows = this.maze.Rows;
            this.Cols = this.maze.Cols;
            this.InitialPos = this.maze.InitialPos.Row + "," + this.maze.InitialPos.Col;
            this.GoalPos = this.maze.GoalPos.Row + "," + this.maze.GoalPos.Col;
            this.PlayerPosition = this.InitialPos;
            this.RavilPlayerPosition = this.InitialPos;
            this.gameIsInitilazed = true;
            
        }



        /// <summary>
        /// Connect to the game server.
        /// </summary>
        /// <returns>true if success else false</returns>
        private bool Connect()
        {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(ipep);
                this.connectionOn = true;

            }
            catch (SocketException se)
            {
                return false;
            }

            return true;
        }


        /// <summary>
        /// Sends the and recived for server.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>recive data in string</returns>
        private string SendAndRecived(string command)
        {
            byte[] data = new byte[8096];
            int recv = 0;
            try
            {
                server.Send(Encoding.ASCII.GetBytes(command));
            }
            catch (SocketException se)
            {
                Console.WriteLine("error sending");
                return null;
            }

            try
            {
                recv = server.Receive(data);
            }
            catch (SocketException se)
            {
                Console.WriteLine("error reveining");
                return null;
            }

            // get data and parse it to json
            return Encoding.ASCII.GetString(data, 0, recv);

        }


        /// <summary>
        /// Fix json string and clean it.
        /// </summary>
        /// <param name="data">json string</param>
        /// <returns>fixed json string</returns>
        protected string GetCorrectJSON(string data)
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

        /// <summary>
        /// Converting int that represnts direction to a string.
        /// </summary>
        /// <param name="move">int</param>
        /// <returns>direction string</returns>
        private string IntToDirection(int move)
        {
            string direction = null;

            switch (move)
            {
                case '0':
                    direction = "left";
                    break;
                case '1':
                    direction = "right";
                    break;
                case '2':
                    direction = "up";
                    break;
                case '3':
                    direction = "down";
                    break;

            }

            return direction;
        }

    }


}
