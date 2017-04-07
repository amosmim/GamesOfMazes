using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Newtonsoft.Json;
using System.Text;

namespace ap2ex1_server
{
	public class ListCommand : ICommandable
	{
		private Model model;

		public ListCommand(Model model)
		{
			this.model = model;
		}

		public string Execute(string[] args, Socket client)
		{
			byte[] data = new byte[1024];

			data = Encoding.ASCII.GetBytes(model.List());

			client.Send(data, data.Length, SocketFlags.None);

			// End of service for this client
			client.Shutdown(SocketShutdown.Both);
			client.Dispose();

			return model.List(); 
		}
	}
}
