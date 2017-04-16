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

			return "-1"; // No more multiplayer
		}
	}
}
