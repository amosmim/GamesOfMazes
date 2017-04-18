using System;
using System.Net.Sockets;
using MazeLib;

namespace ap2ex1_server
{
	/// <summary>
	/// Multiplayer session object.
	/// </summary>
	public struct MultiplayerSessionObject
	{
		public Socket host;
		public Socket guest;
		public Maze maze;
	}
}
