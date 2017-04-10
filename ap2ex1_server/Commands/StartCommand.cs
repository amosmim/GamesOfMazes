using System;
using System.Net.Sockets;
using MazeLib;
using System.Text;

namespace ap2ex1_server
{
	/// <summary>
	/// Start command.
	/// </summary>
	public class StartCommand : ICommandable
	{
		private IModel model;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ap2ex1_server.StartCommand"/> class.
		/// </summary>
		/// <param name="model">Model.</param>
		public StartCommand(IModel model)
		{
			this.model = model;
		}

		/// <summary>
		/// Execute the start command via model, and sent game details.
		/// </summary>
		/// <returns>1 to single that the connection needs to be alive.</returns>
		/// <param name="args">Arguments.</param>
		/// <param name="client">Client.</param>
		public string Execute(string[] args, Socket client)
		{
			// generate new maze
			string name = args[1];
			int rows = int.Parse(args[2]);
			int cols = int.Parse(args[3]);

			Maze maze = model.Start(name, rows, cols, client);

			byte[] data = new byte[1024];

			data = Encoding.ASCII.GetBytes(maze.ToJSON());

			client.Send(data, data.Length, SocketFlags.None);
			return "1";
		}
	}
}

/*
 * to do : 
 * run while with model.IsPaired(mazename) if answer is no run while.
 * else, answer is yes, run model.ServeHost.
 * 
 * guest who joins the game using model.join will run model.ServeGuest.
 */