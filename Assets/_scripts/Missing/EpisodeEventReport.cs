using UnityEngine;
using System.Collections;

public class EpisodeEventReport : MonoBehaviour {
	
	public Episode episode;
	
	// Use this for initialization
	void Start () {
		ReportEvent.EpisodeStarted(episode);
	}
	
}
