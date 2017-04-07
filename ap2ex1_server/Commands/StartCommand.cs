using System;
using System.Net.Sockets;

namespace ap2ex1_server
{
	public class StartCommand : ICommandable
	{
		private Model model;
		public StartCommand(Model model)
		{
			this.model = model;
		}

		public string Execute(string[] args, Socket client)
		{
			throw new NotImplementedException();
		}
	}
}

/*
 * to do : 
 * run while with model.IsPaired(mazename) if answer is no run while.
 * else, answer is yes, run model.ServeHost.
 * 
 * guest who joins the game using model.join will run model.ServeGuest.
 */