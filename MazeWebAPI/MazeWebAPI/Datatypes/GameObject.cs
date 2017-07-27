using System;
using MazeLib;

namespace MazeWebAPI
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
