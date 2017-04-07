using System;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;

namespace ap2ex1_server
{
	public class ClientHandler : IClientHandler
	{
		private Dictionary<string, ICommandable> actions;

		public ClientHandler(Dictionary<string, ICommandable> actions)
		{
			this.actions = actions;
		}

		private string[] ParseCommand(string data, char delim)
		{
			string[] command;

			command = data.Split(delim);

			return command;
		}

		public void HandleClient(Socket client)
		{ 
			ICommandable value;

			byte[] data = new byte[1024];
			int recv;
			// get new command
			try
			{
				recv = client.Receive(data);
			}
			catch (SocketException se)
			{
				// in case of connection failed or connection inturreption
				client.Shutdown(SocketShutdown.Both);
				client.Dispose();
				Console.WriteLine(se.ToString());
				return;
			}

			string strData = Encoding.ASCII.GetString(data, 0, recv);

			Console.WriteLine("command :" + strData);

			// Get parsed command
			string[] actionArray = this.ParseCommand(strData, ' ');

			// Check if command exist, if so, run it.
			if (!actions.TryGetValue(actionArray[0], out value))
			{
				Console.WriteLine("404 option not found");
			}
			else
			{
				strData = actions[actionArray[0]].Execute(actionArray, client);
			}

			Console.WriteLine("end of communication");
		}
	}
}
