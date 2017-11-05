
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using NetworkStuff;

namespace VoteClient
{
	public class Client
	{
		bool isExit = false;
		TcpClient client;
		NetworkStream stream;
		int myChoice;
		
		public Client()
		{
			
		}
		
		public void Connect(string address, int port)
		{
			client = new TcpClient();
			client.Connect(address, port);
			stream = client.GetStream();
			
			// Можно было бы сделать в потоке, 
			// но задание не предполагает постоянное общение
			// клиента и сервера
			Loop();
			
			stream.Close();
			client.Close();
				
		}
		
		private void Loop()
		{
			while(!isExit)
			{
				if(stream.DataAvailable)
				{
					byte[] buf = new byte[4096];
					
					try {
						stream.Read(buf,0,4096);
					} catch(Exception ex) {
						isExit = true;
						Console.WriteLine(ex.Message);
					}
					Packet pack = (Packet)MyFormatter.Deserialize(buf);
					
					switch(pack.Command){
						case CommandType.Message:
							Console.WriteLine(pack.GetMessage());
							break;
						case CommandType.Collection:
							VoteBox voteBox = pack.GetVoteBox();
							PrintVoteBox(voteBox);
							TakeChoice(voteBox);
							RequestStats();
							break;
							
						//вприципе тоже самое что и collection
						case CommandType.Stats:
							PrintStats(pack.Data);
							isExit= true;
							break;
						default:
							
							break;
					}
					
				}
				
				Thread.Sleep(10);	
			}
		}
		
		// Формируем запрос статистики
		private void RequestStats()
		{
			Thread.Sleep(100);
			Packet pack = new Packet();
			pack.Command = CommandType.Stats;
			pack.Data = null;
			
			SendPacket(client, pack);
		}
		
		// Отправка пакета по сокету 
		private void SendPacket(TcpClient client, Packet pack)
		{
			NetworkStream stream = client.GetStream();
			byte[] buf = MyFormatter.Serialize(pack);
			
			stream.Write(buf,0,buf.Length);
		}
		
		// Выводим статистику 
		private void PrintStats(Object stats)
		{
			VoteBox vBox = (VoteBox)stats;
			Console.WriteLine("Current Static:");
			int counter =1;
			foreach(Option opt in vBox.Options)
			{
				Console.WriteLine(counter++ + ": " + opt.Text + ": " + opt.Count + " глс.");
			}
		}
		
		// Выводим на экран опросник
		private void PrintVoteBox(VoteBox vBox)
		{
			Console.WriteLine(">>> Greetings!");
			Console.WriteLine(vBox.Title);
			int num=1;
			foreach(Option option in vBox.Options)
			{
				Console.WriteLine(num++ + ") " + option.Text);
			}
			Console.WriteLine("0: Статистика");
		}
		
		// Ждем действия пользователя(выбор/статистика)
		private void TakeChoice(VoteBox vBox)
		{
			
			bool isValid = false;
			while(!isValid){
				try
				{
					Console.Write("Choose: ");
					myChoice = Convert.ToInt32(Console.ReadLine());
					if(myChoice>=0 && myChoice<=vBox.Options.Count)
					{
						isValid = true;
					} 
					else
					{
						Console.WriteLine("Wrong option. Try Again");
					}
				}
				catch(Exception e)
				{
					Console.WriteLine("Wrong option. Try Again");
				}

				// Если просто посмотреть статистику
				// хотя это может повлиять на будущий выбор
				
				if(myChoice == 0) return;
			}
			
			
			
			Packet pack = new Packet();
			pack.Command = CommandType.Choice;	
			pack.Data = (Object)myChoice;
			
			SendPacket(client, pack);
			
			Console.WriteLine("Голос отправлен");
		}
		
	}
}
