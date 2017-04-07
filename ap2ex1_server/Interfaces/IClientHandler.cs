using System;
using System.Net.Sockets;
namespace ap2ex1_server
{
	public interface IClientHandler
	{
		void HandleClient(Socket client);
	}
}
