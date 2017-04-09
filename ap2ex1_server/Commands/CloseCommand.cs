using System;
using System.Net.Sockets;
using Newtonsoft.Json.Linq;
using System.Text;

namespace ap2ex1_server
{
	public class CloseCommand : ICommandable
	{
		private IModel model;

		public CloseCommand(IModel model)
		{
			this.model = model;
		}

		public string Execute(string[] args, Socket client)
		{
			JObject msg = new JObject();

			if (model.Close(client) == "1")
			{
				msg["isClosed"] = true;
			}
			else
			{
				msg["isClosed"] = false;
			}

			byte[] data = new byte[1024];

			data = Encoding.ASCII.GetBytes(msg.ToString());

			client.Send(data, data.Length, SocketFlags.None);

			client.Shutdown(SocketShutdown.Both);
			client.Dispose();

			return "-1"; // No more multiplayer
		}
	}
}
