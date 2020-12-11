using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VignetteManager
{	
	public Vignette currentVignette;
	public List<Vignette> vignetteList;
	
	private int m_index;
	private bool vignetteActive;
	
	//backCODE: VignetteManager is designed to be a global Manager that tracks which episode is the current Vignette and setup Vignette types.
	//This replaces some of the functionality of Episode1Manager, which used to track Vignette Info.
	
	public void Init() 
	{
		GenerateVignetteList();
		currentVignette = vignetteList[0];
	}
	
	private void GenerateVignetteList() 
	{
		vignetteList = new List< Vignette >();
		
		vignetteList.Add(new Vignette(Vignette.VignetteID.E1vTerrysApartmentSearch, Vignette.VignetteType.Conf));
		vignetteList.Add(new Vignette(Vignette.VignetteID.E1vNervousElevator, Vignette.VignetteType.FAE));
		vignetteList.Add(new Vignette(Vignette.VignetteID.E1vHomeOfficeSearch, Vignette.VignetteType.Conf));
		vignetteList.Add(new Vignette(Vignette.VignetteID.E1vPlantHugger, Vignette.VignetteType.Logic));
		vignetteList.Add(new Vignette(Vignette.VignetteID.E1vStephEvasive, Vignette.VignetteType.FAE));
		vignetteList.Add(new Vignette(Vignette.VignetteID.E2vSorianoNice, Vignette.VignetteType.FAE));
		vignetteList.Add(new Vignette(Vignette.VignetteID.E2vCopyProtection, Vignette.VignetteType.Logic));
		vignetteList.Add(new Vignette(Vignette.VignetteID.E2vPressRelease, Vignette.VignetteType.FAE));
		vignetteList.Add(new Vignette(Vignette.VignetteID.E2vGPCOfficeSearch, Vignette.VignetteType.Conf));
		vignetteList.Add(new Vignette(Vignette.VignetteID.E2vDeadlyTreatment, Vignette.VignetteType.Logic));
		vignetteList.Add(new Vignette(Vignette.VignetteID.E3vToYourHealth, Vignette.VignetteType.Logic));
		vignetteList.Add(new Vignette(Vignette.VignetteID.E3vSuspiciousMen, Vignette.VignetteType.FAE));
		vignetteList.Add(new Vignette(Vignette.VignetteID.E3vCoupleRomance, Vignette.VignetteType.FAE));
		vignetteList.Add(new Vignette(Vignette.VignetteID.E3vChrisBriefcaseSearch, Vignette.VignetteType.Conf));
	}
	
	public Vignette GetVignetteByID(Vignette.VignetteID id) {
		
		for (int i = 0; i < vignetteList.Count; i++)
		{
			if(vignetteList[i].vignetteID == id)
			{
				return vignetteList[i];
			}
		}
		
		return vignetteList[0];
	}
	
	private void WrapVignetteScore(Vignette.VignetteID vignette) {
		LevelManager levelManager = LevelManager.FindLevelManager();
		ScoringManager scoreMgr = levelManager.ScoringManager;
		scoreMgr.GetScoresForMethod( vignette
								   , false		//Print individual scores?
								   , false		//Print group scores?
								   );
		
		levelManager.EvaluationManager.SetPlayerConfirmationBiasChoice(BiasChoice.None);
	}
	
	public void SetVignette(Vignette.VignetteID id) 
	{
		
		ReportEvent.StartVignette(id);
		
		for (int i = 0; i < vignetteList.Count; i++) 
		{
			if(vignetteList[i].vignetteID == id)
			{
				currentVignette = vignetteList[i];
				vignetteActive = true;
				break;
			}
		}
		
	}	
	
	public void VignetteComplete() 
	{
		Debug.Log("Completing Vignette: " + currentVignette.vignetteID);
		if(currentVignette.vignetteType == Vignette.VignetteType.Conf) 
		{
			Debug.Log("Saving Search Data...");
			SaveVignetteExplorationData(currentVignette.vignetteID);
			ReportEvent.VignetteComplete(currentVignette.vignetteID);
		}
		
		WrapVignetteScore(currentVignette.vignetteID);
		vignetteActive = false;
		
		LevelManager.FindLevelManager().PhotoManager.ClearPhotos();;
		
	}
	
	public bool isVignetteActive() 
	{
		return vignetteActive;
	}
	
	public Episode GetEpisode() 
	{
		switch(currentVignette.vignetteID) 
		{
		case Vignette.VignetteID.E1vTerrysApartmentSearch:
		case Vignette.VignetteID.E1vNervousElevator:
		case Vignette.VignetteID.E1vHomeOfficeSearch:
		case Vignette.VignetteID.E1vPlantHugger:
		case Vignette.VignetteID.E1vStephEvasive:
			return Episode.Episode1;
		case Vignette.VignetteID.E2vSorianoNice:
		case Vignette.VignetteID.E2vCopyProtection:
		case Vignette.VignetteID.E2vPressRelease:
		case Vignette.VignetteID.E2vDeadlyTreatment:
		case Vignette.VignetteID.E2vGPCOfficeSearch:
			return Episode.Episode2;
		case Vignette.VignetteID.E3vToYourHealth:
		case Vignette.VignetteID.E3vSuspiciousMen:
		case Vignette.VignetteID.E3vCoupleRomance:
		case Vignette.VignetteID.E3vChrisBriefcaseSearch:
			return Episode.Episode3;
		}
		
		return Episode.Episode1;
	}
	
	public bool isExplorationVignette() 
	{
		switch(currentVignette.vignetteID) {
			case Vignette.VignetteID.E1vHomeOfficeSearch:
			case Vignette.VignetteID.E1vTerrysApartmentSearch:
			case Vignette.VignetteID.E2vGPCOfficeSearch:
			case Vignette.VignetteID.E3vChrisBriefcaseSearch:
			return true;
		}
		
		return false;
	}
	
	private void SaveVignetteExplorationData(Vignette.VignetteID vignette) 
	{
		SubjectData currentSubject = SessionManager.GetSessionManager().currentSubject;
		switch(vignette) {
		case Vignette.VignetteID.E1vTerrysApartmentSearch:
			currentSubject.SaveE1V1Data();
			break;
		case Vignette.VignetteID.E1vHomeOfficeSearch:
			currentSubject.SaveE1V3Data();
			break;
		case Vignette.VignetteID.E2vGPCOfficeSearch:
			currentSubject.SaveE2V7Data();
			break;
		case Vignette.VignetteID.E3vChrisBriefcaseSearch:
			currentSubject.SaveE3V10Data();
			break;
		default:
			Debug.LogWarning("");
			break;
		}
	}
}
