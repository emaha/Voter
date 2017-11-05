/*
 * Created by SharpDevelop.
 * User: barkin_am
 * Date: 31.10.2017
 * Time: 11:28
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace VoteClient
{
	class Program
	{
		public static void Main(string[] args)
		{
			Client client = new Client();
			client.Connect("localhost", 8789);
			
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
	}
}