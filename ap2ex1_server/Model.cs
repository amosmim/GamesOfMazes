using System;
using System.Collections.Generic;
using System.Net.Sockets;
using MazeLib;
using MazeGeneratorLib;
using System.Text;
using Newtonsoft.Json;

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

		public string Close(string maze)
		{
			throw new NotImplementedException();
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
			if (multiPlayer.ContainsKey(maze))
			{
				
			}

			return "";
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

		public string Play(Moves move)
		{
			throw new NotImplementedException();
		}

		public string Solve(string maze, int algorithem)
		{
			throw new NotImplementedException();
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