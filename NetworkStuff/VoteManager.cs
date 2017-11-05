using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace NetworkStuff
{
	public class VoteManager
	{
		const string OPTIONS_FILE_PATH = "options.xml";
		public VoteCollection vCollection;
		public VoteBox currentVoteBox;

        public VoteManager()
        {
            vCollection = new VoteCollection();

            LoadOptionCollection(OPTIONS_FILE_PATH);

            currentVoteBox = vCollection.VoteList[0];
            Console.WriteLine(currentVoteBox.Title);

        }
	
		// Применяем голос
		public void Vote(int ch)
		{
			currentVoteBox.Options[ch-1].Count++;
            SaveOptionCollection();
		}
		
		// Загружаем колекцию из XML файла
		private void LoadOptionCollection(string filename)
		{
			string data = null;
			
			if (File.Exists(filename))
    	    {
    	        data = File.ReadAllText(filename, Encoding.UTF8).Replace("\n", " ");
    	    }
			
			var xmlSerializer = new XmlSerializer(typeof (VoteCollection));
			var stringReader = new StringReader(data);
			
			vCollection = (VoteCollection) xmlSerializer.Deserialize(stringReader);
		}


        // Сохранение всей колекции в файл XML.
        // Можно было бы прикрутить БД, но кому это сейчас нужно?
        // Смысл от этого не меняеся
        private void SaveOptionCollection()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(VoteCollection));
            StringWriter stringWriter = new StringWriter();
            xmlSerializer.Serialize(stringWriter, vCollection);

            string xml = stringWriter.ToString();

            File.WriteAllText(OPTIONS_FILE_PATH, xml, Encoding.UTF8);
        }

		// Создаем шаблон XML файла если вдруг оригинала нет.
		public void GenerateFooFile()
		{
			VoteCollection vCollection = new VoteCollection();
			VoteBox vBox = new VoteBox();
			vBox.Title = "Любимый язык программирования?";
			vBox.Options.Add(new Option("C#",0));
			vBox.Options.Add(new Option("Java",0));
			vBox.Options.Add(new Option("Python",0));
			vBox.Options.Add(new Option("Basic",0));
			vBox.Options.Add(new Option("Pascal",0));
			vCollection.VoteList.Add(vBox);

            SaveOptionCollection();
		}
	}
}
