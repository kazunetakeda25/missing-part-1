using UnityEngine;
using System.Collections;

public class SessionManager : MonoBehaviour {

	private static SessionManager instance;
	public static SessionManager Instance { get { return instance; } }

	public SessionDataManager sessionDataManager;
	public SubjectData currentSubject;
	public VignetteManager vignetteManager;
	
	public static SessionManager GetSessionManager() {
		return GetSessionManager("DEBUG");
	}
	
	public static SessionManager GetSessionManager(string subjectID) {
		GameObject sessionManagerGO = GameObject.FindGameObjectWithTag(Tags.SESSION_MANAGER_TAG);
		
		if(sessionManagerGO == null) {
			sessionManagerGO = CreateNewSession(subjectID);
		}
		
		return sessionManagerGO.GetComponent<SessionManager>();
	}

	private void Awake()
	{
		if(instance != null) {
			Debug.Log("Deleting Duplicate SessionManager!");
			GameObject.Destroy(this.gameObject);
		}

		instance = this;
	}
	
	public void Init(string subjectID) {
		vignetteManager = new VignetteManager();
		vignetteManager.Init();

		MissingComplete.SaveGameManager sgm = MissingComplete.SaveGameManager.Instance;

		if(sgm == null) {
			CreateNewSubjectData(subjectID);
		} else {
			currentSubject = sgm.GetSubjectData();
			if(currentSubject == null) {
				CreateNewSubjectData(subjectID);
			}
		}
		
		sessionDataManager = SessionDataManager.BeginSession(currentSubject);
	}

	private void CreateNewSubjectData(string subjectID)
	{
		currentSubject = new SubjectData();
		currentSubject.subjectID = subjectID;
		currentSubject.Init();
	}

	
	private void OnApplicationQuit() {
		if(sessionDataManager != null)
			sessionDataManager.CloseSession();
	}
	
	private static GameObject CreateNewSession(string subjectID) {
		GameObject sessionManagerGO;
		sessionManagerGO = new GameObject("Session Manager");
		sessionManagerGO.tag = Tags.SESSION_MANAGER_TAG;
		
		GameObject.DontDestroyOnLoad(sessionManagerGO);
		SessionManager sessionManager = sessionManagerGO.AddComponent<SessionManager>();
		sessionManager.Init(subjectID);
		
		return sessionManagerGO;
	}
	
}
