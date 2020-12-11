using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class WriteSessionXML {
	
	public static void WriteToXML(string name, Dictionary<string, string> values) 
	{
		return;

		XmlWriter writer = GetXmlWriter();
		if(writer == null) {
			Debug.LogError("NO XML WRITER!!!");
			return;
		}

		writer.WriteStartElement("event");
		
		writer.WriteStartAttribute("name");
		writer.WriteValue(name);
		writer.WriteEndAttribute();
		
		writer.WriteStartAttribute("sessionTimeElapsed");
		writer.WriteValue(SessionDataManager.GetSessionDataManager().GetSessionTime());
		writer.WriteEndAttribute();
		
		foreach(KeyValuePair<string, string> pair in values) 
		{
			writer.WriteStartElement("Param");
			writer.WriteStartAttribute("name");
			writer.WriteValue(pair.Key);
			writer.WriteEndAttribute();
			
			writer.WriteStartAttribute("value");
			writer.WriteValue(pair.Value);
			writer.WriteEndAttribute();
			writer.WriteEndElement();
		}
		
		writer.WriteEndElement();
	}
	
	private static XmlWriter GetXmlWriter() 
	{
		SessionManager sm = SessionManager.GetSessionManager();
		
		return sm.sessionDataManager.GetXMLWriter();
	}
	
}
