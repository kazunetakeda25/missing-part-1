using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StringTools {
	
	private const string lineBreak = "\n";
	private static char[] splitChars = new char[] { ' ', '-', '\t', '\n' };
	
	//Splits a string into an array of strings divided by word.
	//http://stackoverflow.com/questions/17586/best-word-wrap-algorithm
	public static string[] ExplodeString(string line) {
		
	    List<string> parts = new List<string>();
		
	    int startIndex = 0;
		
	    while (true) {
	        int index = line.IndexOfAny(splitChars, startIndex);
	
	        if (index == -1) {
	            parts.Add(line.Substring(startIndex));
	            return parts.ToArray();
	        }
			
	        string word = line.Substring(startIndex, index - startIndex);
	        char nextChar = line.Substring(index, 1)[0];
			
	        if (char.IsWhiteSpace(nextChar)) {
	            parts.Add(word);
	            parts.Add(nextChar.ToString());
	        }
	        else {
	            parts.Add(word + nextChar);
	        }
	
	        startIndex = index + 1;
	    }
	}
	
	public static string WordWrap(string textToWrap, int lineLimit) {
		string[] words = ExplodeString(textToWrap);
		
		//Line Index is where we are in the current line.
		int lineIndex = 0;
		//This is our full string with newly added breaks.
		string stringWBreaks = "";
		
		for (int i = 0; i < words.Length; i++) 
		{
			//Detect Line breaks
			if(words[i] == lineBreak) { 
				//Add break, start new line
				stringWBreaks += words[i];
				lineIndex = 0;
			}
			//If this word is less than line limit, add
			else if(words[i].Length + lineIndex <= lineLimit) 
			{
				stringWBreaks += words[i];
				lineIndex += words[i].Length;
			} 
			//Start new line
			else
			{
				stringWBreaks += lineBreak;
				
				//If word is not a space, we will start a new line.
				if(words[i] != " ")
				{
					stringWBreaks += words[i];
					lineIndex = words[i].Length;
				}
				//If the word is a space, then we don't want to add it to the string, just reset the line index
				//If we were to add the space, it would add unsightly spaces to the beginnings of lines.
				else
				{
					lineIndex = 0;
				}
			}
		}
		
		return stringWBreaks;
	}
}
