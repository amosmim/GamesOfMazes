using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MazeWebAPI.Datatypes;
using MazeLib;
using MazeGeneratorLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace MazeWebAPI.Models
{
    public class MultiplayerModel
    {
        private static Dictionary<string, MultiplayerObject> multiplayer = new Dictionary<string, MultiplayerObject>();

        /// <summary>
        /// Generate new maze and start an online session.
        /// </summary>
        /// <returns>maze details</returns>
        /// <param name="mazeName">Maze name.</param>
        /// <param name="rows">Rows.</param>
        /// <param name="cols">Cols.</param>
        /// <param name="host">Host.</param>
        public Maze Start(string mazeName, int rows, int cols, string hostID, string userName)
        {
            DFSMazeGenerator generator = new DFSMazeGenerator();

            Maze newMaze = generator.Generate(rows, cols);
            newMaze.Name = mazeName;

            // creating a MultiplayerSessionObject that represents a multiplayer maze map
            MultiplayerObject session = new MultiplayerObject();
            session.game = newMaze;
            session.hostID = hostID;
            session.hostName = userName;
            session.guestID = null;
            session.guestName = null;

            multiplayer.Add(mazeName, session);

            return newMaze;
        }

        /// <summary>
        /// Check to see if there pairing between two players for a specified game.
        /// </summary>
        /// <returns><c>full InitGameObject </c>, if paired, <c>nulled</c> otherwise.</returns>
        /// <param name="maze">Maze.</param>
        public InitGameObject isPaired(string maze)
        {
            InitGameObject returns = new InitGameObject();

            if (multiplayer[maze].guestID != null)
            {
                returns.rival = multiplayer[maze].guestName;
                returns.maze = multiplayer[maze].game.ToJSON();
            } else
            {
                returns.rival = null;
                returns.maze = null;
            }

            return returns;
        }


        /// <summary>
        /// Generate list of games.
        /// </summary>
        /// <returns>list of games</returns>
        public string List()
        {
            List<string> list = new List<string>();

            foreach (KeyValuePair<string, MultiplayerObject> entry in multiplayer)
            {
                list.Add(entry.Key);
            }

            return JsonConvert.SerializeObject(list);
        }


        /// <summary>
        /// Joins the game.
        /// </summary>
        /// <returns>InitGameObject with -1 if game doesn't exist else return game details as a InitGameObject</returns>
        /// <param name="maze">Maze.</param>
        /// <param name="joinClient">Join client.</param>
        public InitGameObject JoinGame(string maze, string guestID, string userName)
        {
            InitGameObject returns = new InitGameObject();
            if (!multiplayer.ContainsKey(maze))
            {
                
                returns.maze = "-1";
                returns.rival = "-1";
                return returns; // couldn't find the game
            }

            MultiplayerObject temp = multiplayer[maze];
            multiplayer.Remove(maze);

            temp.guestID = guestID;
            temp.guestName = userName;
            multiplayer.Add(maze, temp);
           
            returns.maze = temp.game.ToJSON();
            returns.rival = temp.hostName;

            return returns;
        }

        /// <summary>
        /// Play a move in a multiplayer session.
        /// </summary>
        /// <returns>details of the move or -1 if error or game doesn't exist anymore</returns>
        /// <param name="move">Move.</param>
        /// <param name="sender">Sender.</param>
        public JObject Play(string move, string sender)
        {
            JObject msg = new JObject();
            int mode = 0; /// determines who is the sender : host or guest
			string mazeName = "";
            foreach (KeyValuePair<string, MultiplayerObject> entry in multiplayer)
            {
                mazeName = entry.Key;
                if (entry.Value.guestID == sender)
                {
                    mode = 1; // guest
                    break;
                }
                else if (entry.Value.hostID == sender)
                {
                    mode = 2; // host
                    break;
                }
            }

            // we need to transfer the direction
            string otherClient = null;

            // set correct receiver
            if (mode == 1)
            {
                otherClient = multiplayer[mazeName].hostID;
            }
            else if (mode == 2)
            {
                otherClient = multiplayer[mazeName].guestID;
            }

            // all ok and we found our sender
            if (otherClient != null)
            {
                // prepare a json message
                msg["Name"] = mazeName;
                msg["Direction"] = move;
                msg["to"] = otherClient;

                // return ok
                return msg;
            }

            // couldn't find the sender
            return null;
        }

        /// <summary>
        /// Close the game with the specified client.
        /// </summary>
        /// <returns>true if closed properly
        /// 		 false if specified clint not found</returns>
        /// <param name="client">Client.</param>
        public JObject Close(string client)
        {
            JObject msg = new JObject();
            int mode = 0;
            string maze = null;
            foreach (KeyValuePair<string, MultiplayerObject> entry in multiplayer)
            {
                maze = entry.Key;
                if (entry.Value.guestID == client)
                {
                    mode = 1; // guest
                    break;
                }
                else if (entry.Value.hostID == client)
                {
                    mode = 2; // host
                    break;
                }
            }

            string otherPlayer = null;
            msg["isClosed"] = true;
            msg["Details"] = "The other player closed the game.";
            if (mode == 1)
            {
                otherPlayer = multiplayer[maze].hostID;
            }
            else if (mode == 2)
            {
                otherPlayer = multiplayer[maze].guestID;
            }

            // couldn't find appropriate client, client wasn't playing in multiplayer mode
            if (otherPlayer == null)
            {
                return null;
            }

            // remove the game from the multiplayer list
            multiplayer.Remove(maze);

            // add to the JObject the other player
            msg["to"] = otherPlayer;

            return msg;
        }
    }
}