using System;
using System.Collections.Generic;
using System.Net.Sockets;
using MazeLib;
using MazeGeneratorLib;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SearchAlgorithmsLib;
using SearchableMaze;

namespace ap2ex1_server
{
	public class Model : IModel
	{
		private Dictionary<string, GameObject> singlePlayer;
		private Dictionary<string, MultiplayerSessionObject> multiPlayer;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ap2ex1_server.Model"/> class.
		/// </summary>
		public Model()
		{
			this.singlePlayer = new Dictionary<string, GameObject>();
			this.multiPlayer = new Dictionary<string, MultiplayerSessionObject>();
		}

		/// <summary>
		/// Close the game with the specified client.
		/// </summary>
		/// <returns>1 if closed properly
		/// 		 0 if specified clint not found</returns>
		/// <param name="client">Client.</param>
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

		/// <summary>
		/// Generates the maze.
		/// </summary>
		/// <returns>The maze.</returns>
		/// <param name="mazeName">Maze name.</param>
		/// <param name="rows">Rows.</param>
		/// <param name="cols">Cols.</param>
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

		/// <summary>
		/// Joins the game.
		/// </summary>
		/// <returns>-1 if game doesn't exist else return game details</returns>
		/// <param name="maze">Maze.</param>
		/// <param name="joinClient">Join client.</param>
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

		/// <summary>
		/// Generate list of games.
		/// </summary>
		/// <returns>list of games</returns>
		public string List()
		{
			List<string> list = new List<string>();

			foreach (KeyValuePair<string, MultiplayerSessionObject> entry in this.multiPlayer)
			{
				list.Add(entry.Key);
			}

			return JsonConvert.SerializeObject(list);
		}

		/// <summary>
		/// Play a move in a multiplayer session.
		/// </summary>
		/// <returns>details of the move or -1 if error or game doesn't exist anymore</returns>
		/// <param name="move">Move.</param>
		/// <param name="sender">Sender.</param>
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

		/// <summary>
		/// Solve the specified maze with the specified algorithm.
		/// </summary>
		/// <returns>a solution to a maze or error if game doesn't exist</returns>
		/// <param name="maze">Maze.</param>
		/// <param name="algorithm">Algorithm. 0 - BFS 1 - DFS</param>
		public string Solve(string maze, int algorithm)
		{
			// check whether input is correct
			if (!singlePlayer.ContainsKey(maze))
			{
				return "Game doesn't exist !";
			}

			GameObject go = singlePlayer[maze];
			string solution;

			// check whether the solution exists in the server
			if (algorithm == 0)
			{
				// game exists but solution doesn't
				if (go.bfsSolution == null)
				{
				    solution = Solver(maze, algorithm);
					go.bfsSolution = solution;
				}
				else
				{
					return go.bfsSolution;
				}
			}
			else
			{
				// game exists but solution doesn't
				if (go.dfsSolution == null)
				{
					solution = Solver(maze, algorithm);
					go.dfsSolution = solution;
				}
				else
				{
					return go.dfsSolution;
				}
			}

			// because this dictionary is Read Only
			singlePlayer.Remove(maze);
			singlePlayer.Add(maze, go);

			return solution;
		}

		/// <summary>
		/// Helper method for Solve method.
		/// </summary>
		/// <returns>a solution to a maze</returns>
		/// <param name="maze">Maze.</param>
		/// <param name="algorithm">Algorithm.</param>
		private string Solver(string maze, int algorithm)
		{
			Maze mazeGame = this.singlePlayer[maze].maze;
			SearchableMazeAdpter shMaze = new SearchableMazeAdpter(mazeGame);
			Solution<Position> solution;
			MazeSolutionTranslator translator = new MazeSolutionTranslator();
			ISearcher<Position> searcher;

			if (algorithm == 0)
			{
				searcher = new BFS<Position>();
			}
			else
			{
				searcher = new DFS<Position>();
			}

			// try solving the maze
			try
			{
				solution = searcher.Search(shMaze);
			}
			catch (NotSolvableException)
			{
				// unable to solve
				JObject err = new JObject();
				err["Name"] = maze;
				err["Solution"] = "Maze is unsolvable";
				err["NodesEvaluated"] = searcher.GetNumberOfNodesEvaluated().ToString();
				return err.ToString();
			}

			JObject msg = new JObject();
			msg["Name"] = maze;
			msg["Solution"] = translator.SolutionToString(solution);
			msg["NodesEvaluated"] = searcher.GetNumberOfNodesEvaluated().ToString();

			return msg.ToString();
		}

		/// <summary>
		/// Generate new maze and start an online session.
		/// </summary>
		/// <returns>maze details</returns>
		/// <param name="mazeName">Maze name.</param>
		/// <param name="rows">Rows.</param>
		/// <param name="cols">Cols.</param>
		/// <param name="host">Host.</param>
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

		/// <summary>
		/// Check to see if there pairing between two players for a specified game.
		/// </summary>
		/// <returns><c>true</c>, if paired, <c>false</c> otherwise.</returns>
		/// <param name="maze">Maze.</param>
		public bool isPaired(string maze)
		{
			if (multiPlayer[maze].guest != null)
			{
				return true;
			}

			return false;
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