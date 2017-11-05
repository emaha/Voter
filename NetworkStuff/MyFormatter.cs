using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace NetworkStuff
{
	public class MyFormatter
	{
		public MyFormatter()
		{
		}
		
		public static byte[] Serialize(Object obj)
		{
			byte[] buf;

			BinaryFormatter formatter = new BinaryFormatter();
			using (MemoryStream ms = new MemoryStream())
   			{
		        formatter.Serialize(ms, obj);
		        buf = ms.ToArray();
		    }
			return buf;
		}
		
		public static Object Deserialize(byte[] buffer)
		{
			Object obj = null;
			
			BinaryFormatter formatter = new BinaryFormatter();
            using (var ms = new MemoryStream(buffer))
            {
	            obj = (Packet)formatter.Deserialize(ms);                        
            }
			
			return obj;
		}
		
	}
}
