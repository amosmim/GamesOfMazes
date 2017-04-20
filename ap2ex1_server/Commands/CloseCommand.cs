using System;
using System.Net.Sockets;
using Newtonsoft.Json.Linq;
using System.Text;

namespace ap2ex1_server
{
	/// <summary>
	/// Close command.
	/// </summary>
	public class CloseCommand : ICommandable
	{
		private IModel model;
		public const string KEEP_CONNECTION_ALIVE = "1";
		public const string ABORT_CONNECTION = "-1";

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ap2ex1_server.CloseCommand"/> class.
		/// </summary>
		/// <param name="model">Model.</param>
		public CloseCommand(IModel model)
		{
			this.model = model;
		}

		/// <summary>
		/// Execute close command via model, and send appropriate closing message
		/// to client.
		/// </summary>
		/// <returns>-1 to single that the connection need to be closed</returns>
		/// <param name="args">Arguments.</param>
		/// <param name="client">Client.</param>
		public string Execute(string[] args, Socket client)
		{
			JObject msg = new JObject();

			if (model.Close(client))
			{
				msg["isClosed"] = true;
			}
			else
			{
				msg["Error"] = "Error. You can't close a session you are not a part of.";
			}

			byte[] data = new byte[1024];

			data = Encoding.ASCII.GetBytes(msg.ToString());

			client.Send(data, data.Length, SocketFlags.None);

			//client.Shutdown(SocketShutdown.Both);
			//client.Dispose();

			return ABORT_CONNECTION; // No more multiplayer
		}
	}
}
