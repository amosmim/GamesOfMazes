using System;
using System.Net.Sockets;
namespace ap2ex1_server
{
	/// <summary>
	/// ICommandable interface, for declaring user actions.
	/// </summary>
	public interface ICommandable
	{
		/// <summary>
		/// Execute the specified args and client.
		/// </summary>
		/// <returns>The execute.</returns>
		/// <param name="args">Arguments.</param>
		/// <param name="client">Client.</param>
		string Execute(string[] args, Socket client);
	}
}
