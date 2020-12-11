using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerResponseMemory : MonoBehaviour {
	
	private const string ImmuneString1 = "Ask something else.";
	
	private static PlayerResponseMemory mInstance;
	
	private List<string> responses = new List<string>();
	
	public static PlayerResponseMemory Instance() {
		if(mInstance == null) {
			GameObject mInstanceGO = new GameObject("PlayerResponseMemory");
			mInstance = mInstanceGO.AddComponent<PlayerResponseMemory>();
			return mInstance;
		} else {
			return mInstance;
		}
	}
	
	public void RegisterResponse(string response) {
		responses.Add(response);
	}
	
	public bool CheckResponseExist(string response) {
		switch(response) {
		case ImmuneString1:
			return false;
		}
		
		if(responses.Exists(element => element == response)) {
			return true;
		}
		
		return false;
	}

    public void ClearResponses()
    {
        responses.Clear();
    }
}
