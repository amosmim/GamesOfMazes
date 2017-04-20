using System;
using System.Net.Sockets;
namespace ap2ex1_server
{
    /// <summary>
    /// Controller.
    /// </summary>
    public interface IController
    {
        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <returns>the output of the command</returns>
        /// <param name="command">Command.</param>
        /// <param name="client">Client.</param>
        string ExecuteCommand(String command, Socket client);
    }
}