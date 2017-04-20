using System;
using System.Net.Sockets;
using System.Text;

namespace ap2ex1_server
{
	/// <summary>
	/// Join command.
	/// </summary>
	public class JoinCommand : ICommandable
	{
		private IModel model;
		public const string KEEP_CONNECTION_ALIVE = "1";
		public const string ABORT_CONNECTION = "-1";

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ap2ex1_server.JoinCommand"/> class.
		/// </summary>
		/// <param name="model">Model.</param>
		public JoinCommand(IModel model)
		{
			this.model = model;
		}

		/// <summary>
		/// Execute join command via model, and send game details.
		/// </summary>
		/// <returns>-1 if error or game doesn't exist
		/// 		  1 to single that connection is alive</returns>
		/// <param name="args">Arguments.</param>
		/// <param name="client">Client.</param>
		public string Execute(string[] args, Socket client)
		{
			string name = args[1];
			string answer = model.JoinGame(name, client);
			// if -1 means that join is not successfull
			if (answer != "-1")
			{
				// send the maze information to the guest player
				byte[] data = new byte[8096];

				data = Encoding.ASCII.GetBytes(answer);

				client.Send(data, data.Length, SocketFlags.None);
				return KEEP_CONNECTION_ALIVE;
			}
			// join is not successful, kill socket
			return ABORT_CONNECTION;
		}
	}
}
