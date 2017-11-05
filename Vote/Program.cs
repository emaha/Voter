using System;

namespace VoteServer
{
	class Program
	{
		public static void Main(string[] args)
		{
			Server server = new Server();
			server.Start();


			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
	}
}