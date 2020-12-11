using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using HutongGames.PlayMaker.Actions;

public class SayLineScript : MonoBehaviour 
{
	private const string BASE_AUDIO_PATH = "dialogue/";
	private const string BASE_JSON_PATH = "json/";
	private const string EP1_PATH = "ep1/";
	private const string EP2_PATH = "ep2/";
	private const string EP3_PATH = "ep3/";
	
	//*** Local variables, passed down from the FSM Action.
	public SayLine.Bundle episode;
	public string line = "";
	public string json = "";
	public GameObject character = null;
	
	//*** Audio clip fetched from the asset bundle.
	public AudioClip audioLine;

	//*** String for conversation line.
	public string convoLine;	
	
	//*** Path to find JSON file and asset bundle.
	public string path;

	//*** Helper Clsas for JSON serialization.
	private DataConversation data = new DataConversation();
	
	//Dictionary to transfer JSON into. Assists in searching for a line by name (key).
	private Dictionary<string, Dictionary<string,string>> loadedData = new Dictionary<string, Dictionary<string, string>>();
	
	//*** Event handler delegate.
	public delegate void LineSayEventHandler(AudioClip clip, string convoLine);
	public event LineSayEventHandler LineEventFinished;
	
	public void Start()
	{		
		LoadData();
		
		//*** Load up audioclip using the reference from JSON to the Asset Bundle file.
		//audioLine = www.assetBundle.Load(loadedData[line]["audioClip"]) as AudioClip;
		audioLine = GetAudioClip(loadedData[line]["audioClip"]);
		convoLine = loadedData[line]["line"];
		
		//Fire off event, hand off back to FSM Action.
		LineEventFinished(audioLine, convoLine);
		
		//www.assetBundle.Unload(false);
	}
	
	private AudioClip GetAudioClip(string line) {
		string resourcesPath = BASE_AUDIO_PATH + GetEpisodePath();
		return (AudioClip) Resources.Load(resourcesPath + line);
	}
	
	private string GetEpisodePath() {
		switch(episode) {
		case SayLine.Bundle.ep1AudioAll:
			return EP1_PATH;
		case SayLine.Bundle.ep2AudioAll:
			return EP2_PATH;
		case SayLine.Bundle.ep3AudioAll:
			return EP3_PATH;
		}
		return "";
	}
	
	//*** Loads data into dictionary and parses JSON.
	void LoadData()
	{
		//*** Read the JSON file.
		TextAsset txtAsset = Resources.Load<TextAsset>(BASE_JSON_PATH + json) as TextAsset;
		string loadedText = txtAsset.text;
		
		//*** Deserialize data into DataConversation object.
		DataConversation data = JsonUtility.FromJson<DataConversation>(loadedText);
		
		//*** Feed the data back into a dicionary.
		for(int i = 0; i < data.convo.Length; i++)
		{			
			//Debug.Log("Entering Dict Entry: " + i);
			loadedData.Add(data.convo[i].convoBranchName, new Dictionary<string,string>());
			loadedData[data.convo[i].convoBranchName].Add("audioClip", data.convo[i].audioclipName);
			loadedData[data.convo[i].convoBranchName].Add("line", data.convo[i].convoLine);
		}
	}
	
	public void SelfDestruct() {
		Destroy(this.gameObject);
	}
}
