using System;
using System.Net.Sockets;
namespace ap2ex1_server
{
	/*
	 * ICommandable interface
	 * for declaring user actions.
	 */
	public interface ICommandable
	{
		string Execute(string[] args, Socket client);
	}
}
