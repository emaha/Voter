using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace NetworkStuff
{
	[Serializable]
	[XmlRoot("Collection")]
	public class VoteCollection
	{
		[XmlArray("VoteList"), XmlArrayItem("Item")]
   		public List<VoteBox> VoteList {get; set;}
   		
   		public VoteCollection()
   		{
   			VoteList = new List<VoteBox>();
   		}
	}
	
	[Serializable]
	public class VoteBox
	{
		[XmlElement("Title")]
		public string Title {get; set;}
		[XmlElement("Option")]
		public List<Option> Options {get;set;}
		
		public VoteBox()
		{
			Options = new List<Option>();
		}
	}
	
	[Serializable]
	public class Option
	{
		[XmlElement("Text")]
		public string Text;
		[XmlElement("Count")]
		public int Count;
		
		public Option()
		{
			
		}
		
		public Option(string txt, int cnt)
		{
			Text = txt;
			Count = cnt;
		}
	}
	
}
