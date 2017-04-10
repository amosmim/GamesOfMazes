using System;
using System.Net.Sockets;
using MazeLib;
using System.Text;

namespace ap2ex1_server
{
	/// <summary>
	/// Generate command.
	/// </summary>
	public class GenerateCommand : ICommandable
	{
		private IModel model;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ap2ex1_server.GenerateCommand"/> class.
		/// </summary>
		/// <param name="model">Model.</param>
		public GenerateCommand(IModel model)
		{
			this.model = model;
		}

		/// <summary>
		/// Execute generate command via model, and send game details.
		/// </summary>
		/// <returns>-1 to single that the connection needs to be closed.</returns>
		/// <param name="args">Arguments.</param>
		/// <param name="client">Client.</param>
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
			//client.Shutdown(SocketShutdown.Both);
			//client.Dispose();
			                  
			return "-1";
		}
	}
}
