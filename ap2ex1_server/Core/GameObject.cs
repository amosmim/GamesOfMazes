using System;
using MazeLib;
namespace ap2ex1_server
{
	/// <summary>
	/// Game object for single player.
	/// </summary>
	public struct GameObject
	{
		public Maze maze;
		public string bfsSolution;
		public string dfsSolution;
	}
}
