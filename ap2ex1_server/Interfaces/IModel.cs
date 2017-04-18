using System;
using System.Net.Sockets;
using MazeLib;

namespace ap2ex1_server
{
	/// <summary>
	/// Model.
	/// </summary>
	public interface IModel
	{
		/// <summary>
		/// Joins the game.
		/// </summary>
		/// <returns>-1 if game doesn't exist else return game details</returns>
		/// <param name="maze">Maze.</param>
		/// <param name="joinClient">Join client.</param>
		string JoinGame(string maze, Socket joinClient);

		/// <summary>
		/// Generates the maze.
		/// </summary>
		/// <returns>The maze.</returns>
		/// <param name="mazeName">Maze name.</param>
		/// <param name="rows">Rows.</param>
		/// <param name="cols">Cols.</param>
		Maze GenerateMaze(string mazeName, int rows, int cols);

		/// <summary>
		/// Solve the specified maze with the specified algorithm.
		/// </summary>
		/// <returns>a solution to a maze or error if game doesn't exist</returns>
		/// <param name="maze">Maze.</param>
		/// <param name="algorithm">Algorithm. 0 - BFS 1 - DFS</param>
		string Solve(string maze, int algorithm);

		/// <summary>
		/// Generate new maze and start an online session.
		/// </summary>
		/// <returns>maze details</returns>
		/// <param name="mazeName">Maze name.</param>
		/// <param name="rows">Rows.</param>
		/// <param name="cols">Cols.</param>
		/// <param name="host">Host.</param>
		Maze Start(string mazeName, int rows, int cols, Socket host);

		/// <summary>
		/// Generate list of games.
		/// </summary>
		/// <returns>list of games</returns>
		string List();

		/// <summary>
		/// Play a move in a multiplayer session.
		/// </summary>
		/// <returns>details of the move or -1 if error or game doesn't exist anymore</returns>
		/// <param name="move">Move.</param>
		/// <param name="sender">Sender.</param>
		string Play(string move, Socket sender);

		/// <summary>
		/// Close the game with the specified client.
		/// </summary>
		/// <returns>1 if closed properly
		/// 		 0 if specified clint not found</returns>
		/// <param name="client">Client.</param>
		bool Close(Socket client);

		/// <summary>
		/// Check to see if there pairing between two players for a specified game.
		/// </summary>
		/// <returns><c>true</c>, if paired, <c>false</c> otherwise.</returns>
		/// <param name="maze">Maze.</param>
		bool isPaired(string maze);
	}
}
