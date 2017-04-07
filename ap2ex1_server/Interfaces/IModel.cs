using System;
using System.Net.Sockets;
using MazeLib;

namespace ap2ex1_server
{
	public interface IModel
	{
		string JoinGame(string maze, Socket joinClient);
		Maze GenerateMaze(string mazeName, int rows, int cols);
		string Solve(string maze, int algorithem);
		Maze Start(string mazeName, int rows, int cols, Socket host);
		string List();
		string Play(Moves move);
		string Close(string maze);
	}
}
