using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using NetworkStuff;

namespace VoteServer
{
	public class Server
	{
		private static Object mutex = new Object();
		VoteManager vManager;

		TcpListener listener;
		List<TcpClient> clients;
		
		public Server()
		{
			vManager = new VoteManager();
			
			// Создание файла опросника на тот случай если его нет.
			// Шаблон.
			//vManager.GenerateFooFile();
			
			listener = new TcpListener(new IPAddress(new Byte[]{127,0,0,1}),8789);
			clients = new List<TcpClient>();
		}
		
		public void Start()
		{
			Console.WriteLine("Listening...");

			// Принимаем клиентов в отдельном потоке
			Thread th = new Thread(new ThreadStart(AcceptClients));
			th.Start();
			
			while(true)
			{
				ManageClients();
				Thread.Sleep(10);
			}
		}
		
		// Обработка сокетов 
		private void ManageClients()
		{
			lock(mutex)	{
				foreach (TcpClient client in clients) {
					NetworkStream stream = client.GetStream();
					if(stream.DataAvailable)	{
						byte[] buf = new byte[4096];
						stream.Read(buf,0,4096);
						Packet data = (Packet) MyFormatter.Deserialize(buf);
						
						switch(data.Command){
							case CommandType.Message:
								Console.WriteLine(data.GetMessage());
								
								break;
							case CommandType.Collection:
								
								break;
								
							case CommandType.Choice:
								int choice = data.getChoice();
								vManager.Vote(choice);
								break;
								
							case CommandType.Stats:
								Console.WriteLine("StatRequest");
								Packet pack = new Packet();
								pack.Command = CommandType.Stats;
								pack.Data = (Object)vManager.currentVoteBox;
								
								SendPacket(client, pack);
								break;
							
							default:
								
								break;
						}
					}
					
					// На всякий случай спим, чтобы не напрягать процессор
					Thread.Sleep(100);
				}
			}
			
		}
		

		private void AcceptClients()
		{
			listener.Start();
			
			while(true)
			{
				TcpClient client = listener.AcceptTcpClient();
				lock(mutex)
				{
					clients.Add(client);
				}
				
				// Как только приняли, сразу задаем вопрос
				Packet pack = new Packet();
				pack.Command = CommandType.Collection;
				pack.Data = (Object)vManager.currentVoteBox;

				SendPacket(client, pack);
				
			}
		}
		
		private void SendPacket(TcpClient client, Packet pack)
		{
			NetworkStream stream = client.GetStream();
			byte[] buf = MyFormatter.Serialize(pack);
			
			stream.Write(buf,0,buf.Length);
		}
		
		
		
		
	}
}
