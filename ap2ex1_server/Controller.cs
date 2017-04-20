using System;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
namespace ap2ex1_server
{
    public class Controller : IController
    {
        private Dictionary<string, ICommandable> actions;
        private IModel model;
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

        public void AddNewAction(string actionName, ICommandable action)
        {
            actions.Add(actionName, action);
        }

        private string[] ParseCommand(string data, char delim)
        {
            string[] command;

            command = data.Split(delim);

            return command;
        }

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