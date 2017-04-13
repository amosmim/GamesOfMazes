using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Configuration;

namespace ap2ex1_server
{
	/// <summary>
	/// Server.
	/// </summary>
	public class Server
	{
		private int port;
		private bool isOnline;
		private Socket serverSocket;
		private IPEndPoint endPoint;
		private Thread acceptClients;
		private IClientHandler clientHandler;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ap2ex1_server.Server"/> class.
		/// </summary>
		/// <param name="port">Port.</param>
		/// <param name="clientHandler">Client handler.</param>
		public Server(int port, IClientHandler clientHandler)
		{
			this.port = port;
			this.endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), this.port);
			this.serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			this.clientHandler = clientHandler;
		}

		/// <summary>
		/// Initialize the server and start accepting clients.
		/// </summary>
		public void InitializeServer()
		{
			Console.WriteLine("start");
			// server bind and listen to the port
			try
			{
				this.serverSocket.Bind(this.endPoint);
				// no more than 10 connections
				this.serverSocket.Listen(10);
				Console.WriteLine("bind");
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

			// all gone without erros, we are online
			this.isOnline = true;

			this.acceptClients = new Thread(() =>
			{
				// while the server is online keep accepting new clients
				while (this.isOnline)
				{
					Socket newClient;
					// accept new client and get Socket object
					try
					{
						Console.WriteLine("get new client");
					    newClient = this.serverSocket.Accept();
						//clients.Add();
					}
					catch (SocketException se)
					{
						Console.WriteLine(se.ToString());
						//this.ShutDownServer();
						break;
					}
					Console.WriteLine("new client added :" + newClient.ToString());

					this.clientHandler.HandleClient(newClient);
				}
			});

			this.acceptClients.Start();

			this.acceptClients.Join();

		}

		/// <summary>
		/// Shutdown the server and clean the socket.
		/// </summary>
		public void ShutDownServer()
		{
			Console.WriteLine("server shutting down");
			this.isOnline = false;
			this.serverSocket.Shutdown(SocketShutdown.Both);
			this.serverSocket.Dispose();
		}

		/// <summary>
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
		/// <param name="args">The command-line arguments.</param>
		public static void Main(string[] args)
		{
			// port should be from app.config
			//int port = int.Parse(ConfigurationManager.AppSettings["port"]);
			IController controller = new Controller();
			IClientHandler clientHandler = new ClientHandler(controller);
			Server server = new Server(55555, clientHandler);

			// initialize and run
			server.InitializeServer();

			string c;

			while (true)
			{
				c = Console.ReadLine();
				if (c.Equals("end"))
					break;
			}

			server.ShutDownServer();
		}
	}
}