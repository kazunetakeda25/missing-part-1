using UnityEngine;
using System.Collections;

[System.Serializable]
public class DataConversation
{
	public DataConvoBranch[] convo;	
}

[System.Serializable]
public class DataConvoBranch
{
	public string convoBranchName;
	public string convoLine;
	public string audioclipName;
}
