using System;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

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
			Task newClient = Task.Factory.StartNew(() =>
			{
				ICommandable value;

				int recv;
				string strData = "";
				// get new command
				while (strData != "-1")
				{
					byte[] data = new byte[1024];
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

					if (recv > 0)
					{
						strData = Encoding.ASCII.GetString(data, 0, recv);

						Console.WriteLine("command :" + strData + " bytes: " + recv);

						// Get parsed command
						string[] actionArray = this.ParseCommand(strData, ' ');

						// Check if command exist, if so, run it.
						if (!actions.TryGetValue(actionArray[0], out value))
						{
							Console.WriteLine("404 option not found");
							strData = "-1";
							client.Shutdown(SocketShutdown.Both);
							client.Dispose();
						}
						else
						{
							strData = actions[actionArray[0]].Execute(actionArray, client);
						}
					}
				}
				Console.WriteLine("end of communication");
			});

		}
	}
}
