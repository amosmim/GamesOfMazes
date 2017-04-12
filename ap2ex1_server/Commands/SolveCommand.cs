using System;
using System.Net.Sockets;
using System.Text;

namespace ap2ex1_server
{
	/// <summary>
	/// Solve command.
	/// </summary>
	public class SolveCommand : ICommandable
	{
		private IModel model;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ap2ex1_server.SolveCommand"/> class.
		/// </summary>
		/// <param name="model">Model.</param>
		public SolveCommand(IModel model)
		{
			this.model = model;
		}

		/// <summary>
		/// Execute solve command via model, and sent solution.
		/// </summary>
		/// <returns>-1 to single that the connection needs to be closed.</returns>
		/// <param name="args">Arguments.</param>
		/// <param name="client">Client.</param>
		public string Execute(string[] args, Socket client)
		{
			string mazeName = args[1];
			int algoritm = 0;

			// check to see what algorithm to use
			if (args[2] == "1")
			{
				algoritm = 1; // DFS
			}

			string answer = this.model.Solve(mazeName, algoritm);

			byte[] data = new byte[8096];
			data = Encoding.ASCII.GetBytes(answer);
			client.Send(data, data.Length, SocketFlags.None);

			return "-1"; // no need for the socket to be alive
		}
	}
}
