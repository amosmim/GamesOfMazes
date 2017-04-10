using System;
using System.Net.Sockets;
using System.Text;

namespace ap2ex1_server
{
	public class SolveCommand : ICommandable
	{
		private IModel model;

		public SolveCommand(IModel model)
		{
			this.model = model;
		}

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

			byte[] data = new byte[1024];
			data = Encoding.ASCII.GetBytes(answer);
			client.Send(data, data.Length, SocketFlags.None);

			return "-1"; // no need for the socket to be alive
		}
	}
}
