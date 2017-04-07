using System;
using System.Net.Sockets;
using MazeLib;

namespace ap2ex1_server
{
	public struct MultiplayerSessionObject
	{
		public Socket host;
		public Socket guest;
		public Maze maze;
	}
}
