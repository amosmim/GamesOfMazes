using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using MazeWebAPI.Datatypes;
using MazeWebAPI.Models;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json.Linq;
using MazeLib;
using Newtonsoft.Json;
namespace MazeWebAPI
{
    public class MultiplayerHub : Hub
    {
        private MultiplayerModel model;
       
        /// <summary>
        /// Constructor.
        /// </summary>
        public MultiplayerHub()
        {
            this.model = new MultiplayerModel();
            
        }

        /// <summary>
        /// Generate list of games.
        /// </summary>
        public void List()
        {
            string clientID = Context.ConnectionId;
            string serializedList = this.model.List();

            Clients.Client(clientID).onListReceived(serializedList);
        }

        /// <summary>
        /// Join to a session.
        /// </summary>
        /// <param name="name">maze name</param>
        public void Join(string mazename, string username)
        {
            string clientID = Context.ConnectionId;
            InitGameObject data = this.model.JoinGame(mazename, clientID, username);
            if (data.maze == "-1")
            {
                Clients.Client(clientID).onJoinGame("{\"error\":\"Maze doesn't exists\"}");
                return;
            }
            string json = JsonConvert.SerializeObject(data);

            Clients.Client(clientID).onJoinGame(json);
        }

        /// <summary>
        /// Play a move in a multiplayer session.
        /// </summary>
        /// <param name="direction">direction</param>
        public void Play(string direction)
        {
            string clientID = Context.ConnectionId;
            JObject data = this.model.Play(direction, clientID);
            // in case of error
            if (data == null)
            {
                Clients.Client(clientID).onPlay("{\"error\":\"Maze doesn't exists\"}");
                return; 
            }

            // send the move to the other player
            Clients.Client((string)data["to"]).onPlay(data);
        }

        /// <summary>
        /// Close the game with the specified client.
        /// </summary>
        public void Close()
        {
            string clientID = Context.ConnectionId;
            JObject data = this.model.Close(clientID);

            if (data == null)
            {
                // if the player tries to close a game that doesn't exist
                Clients.Client(clientID).onClose("{\"error\":\"Maze doesn't exists\"}");
                return;
            }

            // send the closing message to the other player
            Clients.Client((string)data["to"]).onClose (data.ToString());
        }

        /// <summary>
        /// Generate new maze and start an online session.
        /// </summary>
        /// <param name="name">maze name</param>
        /// <param name="rows">rows</param>
        /// <param name="cols">cols</param>
        public void StartMultiplayer(string name, int rows, int cols, string username)
        {
            Maze maze;
            
            string clientID = Context.ConnectionId;
            Task t = new Task(() =>
            {
                maze = this.model.Start(name, rows, cols, clientID ,username);
                
                // wait untill pairing
                while (this.model.isPaired(name).rival == null)
                {
                    Thread.Sleep(1000);
                }
                InitGameObject temp = this.model.isPaired(name);
                string json = JsonConvert.SerializeObject(temp);
             
                // send to the host the maze after the pairing
                Clients.Client(clientID).onGameStart(json);
            });

            t.Start();
        }
    }
}