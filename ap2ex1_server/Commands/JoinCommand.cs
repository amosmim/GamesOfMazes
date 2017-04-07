using System;
using System.Net.Sockets;

namespace ap2ex1_server
{
	public class JoinCommand : ICommandable
	{
		private Model model;

		public JoinCommand(Model model)
		{
			this.model = model;
		}

		public string Execute(string[] args, Socket client)
		{
			throw new NotImplementedException();
		}
	}
}
