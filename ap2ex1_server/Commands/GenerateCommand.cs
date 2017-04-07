using System;
using System.Net.Sockets;
using MazeLib;
using System.Text;

namespace ap2ex1_server
{
	public class GenerateCommand : ICommandable
	{
		private Model model;

		public GenerateCommand(Model model)
		{
			this.model = model;
		}

		public string Execute(string[] args, Socket client)
		{
			// generate new maze
			string name = args[1];
			int rows = int.Parse(args[2]);
			int cols = int.Parse(args[3]);

			Maze maze = model.GenerateMaze(name, rows, cols);

			byte[] data = new byte[1024];

			data = Encoding.ASCII.GetBytes(maze.ToJSON());

			client.Send(data, data.Length, SocketFlags.None);

			// End of service for this client
			client.Shutdown(SocketShutdown.Both);
			client.Dispose();
			                  
			return maze.ToJSON();
		}
	}
}
