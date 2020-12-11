using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Designed by backCODE in California
//Creates a GameObject that displays a subtitle. Goes with a prefab.

public class SubtitleMachine : MonoBehaviour {
	
	public int subtitleLimit;
	public int wordWrapLimit;
	public SpriteText subtitleText;
	
	private string[] lines;
	
	private float subtitleDisplayTime;
	private float subtitleTimer;
	private int currentSubtitleIndex;
	private bool runningSubtitles;
	
	// Use this for initialization
	private void Awake () 
	{	
		subtitleText.maxWidth = wordWrapLimit;
		subtitleText.GetComponent<Renderer>().material.SetColor("_COLOR", new Color(1, 1, 1, 0));
	}
	
	private void Update() {
		
		if(runningSubtitles) {
		
			subtitleTimer += Time.deltaTime;
			
			if(subtitleTimer > subtitleDisplayTime) {
				currentSubtitleIndex++;
				//If this is the last subtitle, we're done.
				if((currentSubtitleIndex) == lines.Length) {
					runningSubtitles = false;
				} else {
					subtitleTimer = 0; //If there's more to display, reset the timer.
					DisplayCurrentSubtitle();
				}
			}
			
		}
		
	}
	
	private void DisplayCurrentSubtitle() {
		subtitleText.Text = lines[currentSubtitleIndex];
	}
	
	public void ShowSubtitle(string line, float lineDuration) {
		lines = BreakUpSubtitle(line);
		//Subtitle Display Time is how long we display each subtitle.
		//For now I guess we just divide this by the total length of the lines to display and the time it will take for the Vo to play.
		subtitleDisplayTime = lineDuration / lines.Length;
		subtitleTimer = 0;
		runningSubtitles = true;
		currentSubtitleIndex = 0;
		DisplayCurrentSubtitle();
	}
	
	public void SelfDestruct() {
		Destroy(this.gameObject);
	}
	
	//SpriteText already handles our normal wordwrap, so we just need to break this up if it's over the subtitle limit.
	private string[] BreakUpSubtitle(string line) {
		List<string> lines = new List<string>();
		lines.Add("");
		int linesIndex = 0;
		
		//Turn string into array of words.
		string[] words = StringTools.ExplodeString(line);
		
		for (int i = 0; i < words.Length; i++) {
			
			if((words[i].Length + lines[linesIndex].Length) > subtitleLimit) {
				lines.Add(words[i]);
				linesIndex++;
			} else {
				lines[linesIndex] += words[i];
			}
			
		}
		
		return lines.ToArray();
	}
	
}
