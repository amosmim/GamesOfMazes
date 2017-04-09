using System;
using System.Collections.Generic;
using System.Net.Sockets;
using MazeLib;
using MazeGeneratorLib;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SearchAlgorithmsLib;

namespace ap2ex1_server
{
	public class Model : IModel
	{
		private Dictionary<string, GameObject> singlePlayer;
		private Dictionary<string, MultiplayerSessionObject> multiPlayer;

		public Model()
		{
			this.singlePlayer = new Dictionary<string, GameObject>();
			this.multiPlayer = new Dictionary<string, MultiplayerSessionObject>();
		}

		public string Close(Socket client)
		{
			foreach (KeyValuePair<string, MultiplayerSessionObject> entry in this.multiPlayer)
			{
				if ((entry.Value.guest == client) || (entry.Value.host == client))
				{
					// becuase someone closed the game, the game is now removed from the dictionary
					this.multiPlayer.Remove(entry.Key);
					return "1";
				}
			}

			// couldn't find appropriate socket, socket wasn't playing in multiplayer mode
			return "-1";
		}

		public Maze GenerateMaze(string mazeName, int rows, int cols)
		{
			DFSMazeGenerator generator = new DFSMazeGenerator();

			Maze newMaze = generator.Generate(rows, cols);
			newMaze.Name = mazeName;

			// creating a GameObject that represents a single maze map
			GameObject gameObj = new GameObject();
			gameObj.maze = newMaze;
			gameObj.dfsSolution = null;
			gameObj.bfsSolution = null;

			singlePlayer.Add(mazeName, gameObj);

			return newMaze;
		}

		public string JoinGame(string maze, Socket joinClient)
		{
			if (!multiPlayer.ContainsKey(maze))
			{
				return "-1";
			}

			MultiplayerSessionObject temp = multiPlayer[maze];
			multiPlayer.Remove(maze);

			temp.guest = joinClient;
			multiPlayer.Add(maze, temp);

			return temp.maze.ToJSON();
		}

		public string List()
		{
			List<string> list = new List<string>();

			foreach (KeyValuePair<string, MultiplayerSessionObject> entry in this.multiPlayer)
			{
				list.Add(entry.Key);
			}

			return JsonConvert.SerializeObject(list);
		}

		public string Play(string move, Socket sender)
		{
			JObject msg = new JObject();
			int mode = 0; /// determines who is the sender : host or guest
			string mazeName = "";
			foreach (KeyValuePair<string, MultiplayerSessionObject> entry in this.multiPlayer)
			{
				mazeName = entry.Key;
				if (entry.Value.guest == sender)
				{
					mode = 1; // guest
					break;
				}
				else if (entry.Value.host == sender)
				{
					mode = 2; // host
					break;
				}
			}

			// we need to transfer the direction
			Socket otherClient = null;

			// set correct receiver
			if (mode == 1)
			{
				otherClient = this.multiPlayer[mazeName].host;
			}
			else if (mode == 2)
			{
				otherClient = this.multiPlayer[mazeName].guest;
			}

			// all ok and we found our sender
			if (otherClient != null)
			{
				// prepare a json message
				msg["Name"] = mazeName;
				msg["Direction"] = move;

				byte[] data = new byte[1024];

				data = Encoding.ASCII.GetBytes(msg.ToString());

				otherClient.Send(data, data.Length, SocketFlags.None);

				// return ok
				return "1";
			}

			// couldn't find the sender
			return "-1";
		}

		public string Solve(string maze, int algorithem)
		{
			SearchableMazeAdpter shMaze = new SearchableMazeAdpter(maze);
			Solution<Position> solution;
		}

		public Maze Start(string mazeName, int rows, int cols, Socket host)
		{
			DFSMazeGenerator generator = new DFSMazeGenerator();

			Maze newMaze = generator.Generate(rows, cols);
			newMaze.Name = mazeName;

			// creating a MultiplayerSessionObject that represents a multiplayer maze map
			MultiplayerSessionObject session = new MultiplayerSessionObject();
			session.maze = newMaze;
			session.host = host;
			session.guest = null;

			multiPlayer.Add(mazeName, session);

			return newMaze;
		}
	}
}

/* 1. start - > saving the client socket and putting a sign that  
 *				a player is waiting for another player to join.
 *	  join - >	when player sends "join" we check if the maze exists
 *				if so, check if there is a player who waits.
 *				if so, server acknowledge, and start transmiting data between
 *				the two players.
 *
 *				model can notify the controller(server) that someone joined
 *				said maze. model can save both client sockets, and transmit to server.
 *
 * 2.			another way, server is a member of model. server has startMultiplayer(client1,client2,maze)
 *				function which allow multiplayer session. the start method will not close the socket, but 
 *				the start method will put the socket on hold and end (the method). if a player
 *				joins to the same maze game of player1,the model will invoke said method.
 */