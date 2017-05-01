using System;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
namespace ap2ex1_server
{
    /// <summary>
    /// Controller in MVC 
    /// </summary>
    /// <seealso cref="ap2ex1_server.IController" />
    public class Controller : IController
    {

        private Dictionary<string, ICommandable> actions;
        private IModel model;
        /// <summary>
        /// Initializes a new instance of the <see cref="Controller"/> class.
        /// </summary>
        public Controller()
        {
            this.model = new Model();
            this.actions = new Dictionary<string, ICommandable>();

            AddNewAction("generate", new GenerateCommand(model));
            AddNewAction("list", new ListCommand(model));
            AddNewAction("start", new StartCommand(model));
            AddNewAction("close", new CloseCommand(model));
            AddNewAction("join", new JoinCommand(model));
            AddNewAction("play", new PlayCommand(model));
            AddNewAction("solve", new SolveCommand(model));
        }

        /// <summary>
        /// Adds the new action.
        /// </summary>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="action">The action.</param>
        public void AddNewAction(string actionName, ICommandable action)
        {
            actions.Add(actionName, action);
        }

        /// <summary>
        /// Parses the command.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="delim">The delimiter.</param>
        /// <returns></returns>
        private string[] ParseCommand(string data, char delim)
        {
            string[] command;

            command = data.Split(delim);

            return command;
        }

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">Command.</param>
        /// <param name="client">Client.</param>
        /// <returns>
        /// the output of the command
        /// </returns>
        public string ExecuteCommand(string command, Socket client)
        {
            string strData;
            ICommandable value;
            // Get parsed command
            string[] actionArray = this.ParseCommand(command, ' ');

            // Check if command exist, if so, run it.
            if (!actions.TryGetValue(actionArray[0], out value))
            {
                Console.WriteLine("Server Error: 404 option not found");
                byte[] data = new byte[1024];
                data = Encoding.ASCII.GetBytes("Error: 404 option not found");
                client.Send(data, data.Length, SocketFlags.None);
                strData = "-1";
            }
            else
            {
                strData = actions[actionArray[0]].Execute(actionArray, client);
            }

            return strData;
        }
    }
}