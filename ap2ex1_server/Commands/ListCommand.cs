using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Newtonsoft.Json;
using System.Text;

namespace ap2ex1_server
{
	/// <summary>
	/// List command.
	/// </summary>
	public class ListCommand : ICommandable
	{
		private IModel model;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ap2ex1_server.ListCommand"/> class.
		/// </summary>
		/// <param name="model">Model.</param>
		public ListCommand(IModel model)
		{
			this.model = model;
		}

		/// <summary>
		/// Execute list command via model, and send list of games.
		/// </summary>
		/// <returns>-1 to single connection needs to be closed</returns>
		/// <param name="args">Arguments.</param>
		/// <param name="client">Client.</param>
		public string Execute(string[] args, Socket client)
		{
			byte[] data = new byte[1024];

			data = Encoding.ASCII.GetBytes(model.List());

			client.Send(data, data.Length, SocketFlags.None);

			// End of service for this client
			//client.Shutdown(SocketShutdown.Both);
			//client.Dispose();

			return "-1"; 
		}
	}
}
