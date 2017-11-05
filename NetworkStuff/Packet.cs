using System;
using System.Collections.Generic;

namespace NetworkStuff
{
	[Serializable]
	public class Packet
	{
		public CommandType Command { get; set; }
		public Object Data;
		
		public string GetMessage()
		{
			return (string)Data;
		}
		
		public VoteBox GetVoteBox()
		{
			return (VoteBox)Data;
		}
		
		public int getChoice()
		{
			return (int)Data;
		}
	}
	
	[Serializable]
	public enum CommandType
	{
		Message, Collection, Choice, Stats
	}
	
	
}