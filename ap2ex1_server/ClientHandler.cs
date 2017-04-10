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
		private IController controller;

		public ClientHandler(IController controller)
		{
			this.controller = controller;
		}

		public void HandleClient(Socket client)
		{
			Task newClient = Task.Factory.StartNew(() =>
			{
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
						//client.Shutdown(SocketShutdown.Both);
						//client.Dispose();
						Console.WriteLine(se.ToString());
						strData = "-1";
						break;
					}

					if (recv > 0)
					{
						strData = Encoding.ASCII.GetString(data, 0, recv);

						Console.WriteLine("command :" + strData + " bytes: " + recv);

						// Execute command
						strData = controller.ExecuteCommand(strData, client);

						// send the the client whether the socket stays open or socket is closed
						Console.WriteLine("socket statue : " + strData);
						Thread.Sleep(100);
						data = new byte[1024];
						data = Encoding.ASCII.GetBytes(strData);
						client.Send(data, data.Length, SocketFlags.None);
					}
				}
				Console.WriteLine("end of communication");
			});

		}
	}
}
