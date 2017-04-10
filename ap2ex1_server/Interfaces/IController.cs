using System;
using System.Net.Sockets;
namespace ap2ex1_server
{
	public interface IController
	{
		string ExecuteCommand(String command, Socket client);
	}
}
