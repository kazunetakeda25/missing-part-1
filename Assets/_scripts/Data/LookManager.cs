using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;

public class LookRecord
{
	public PhotoClueObject clue = null;
	public InteractableWorldObject interactable = null;
	public float currentTime = 0.0f;
}

[AddComponentMenu("Framework/Look Manager")]
public class LookManager : MonoBehaviour
{
	private List<LookRecord> m_registeredClues = new List<LookRecord>();
	private List<PhotoClueObject> m_lookedAtClues = new List<PhotoClueObject>();
	
	public void RegisterInteractable(PhotoClueObject target)
	{
		LookRecord newRecord = new LookRecord();
		newRecord.clue = target;
		newRecord.interactable = target.GetComponent<InteractableWorldObject>();
		newRecord.currentTime = 0.0f;
		
		// Add the object to the list we care about.
		if(newRecord.interactable != null)
		{
			m_registeredClues.Add(newRecord);	
		}
		else
		{
			Debug.LogWarning(target.name + " did not have an Interactable component!");	
		}
		
		return;
	}
	
	public void UnRegisterInteractable(PhotoClueObject target) 
	{
		//m_registeredClues.Remove(lookRecord);
		
		//m_lookedAtClues.Remove(clue);
	}
	
	public void Update()
	{
		/* backCODE - This was the old performance-intense Look Manager Update
		 * We're replacing this with a simple acknolwedgement of the object when it's moused over.
		//Find out if anything interactable is visible.
		CharPlayer player = (CharPlayer)PersonalityManager.GetPlayer();
		if ( null != player )
		{
			Camera cam = player.GetCamera();
			
			Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);
			
			LayerMask m_layerMaskForClueVisibility = App.Instance().GetPhotoManager().m_layerMaskForClueVisibility;
			
			Vignette.VignetteID currentVig = App.Instance().GetEpisode1Manager().getVignetteID();
			
			// Iterate through the registered list.
			foreach(LookRecord record in m_registeredClues)
			{	
				// Ignore if we're in the wrong vignette.
				if(!record.interactable.IsApplicableToVignette(currentVig))
				{
					continue;	
				}
				
				// Is the object being looked at now?
				float confidence = record.clue.GetVisiblilityConfidence( 		planes
																				, cam
																				, m_layerMaskForClueVisibility 
																				, false
																				, false
																				);
				
				// If that puts the object over the time to count as looked at, remove it from
				// the interactable list and add it to the looked at list.
				if(confidence > record.clue.m_confidenceForAcceptance)
				{
					record.currentTime = record.currentTime + Time.deltaTime;
					if(record.currentTime > m_timeToCountAsLookedAt)
					{
						Debug.LogWarning("You've looked at a " + record.clue.name + ".");
						m_registeredClues.Remove(record);
						m_lookedAtClues.Add(record.clue);
						break;
					}
				}
			}
		}		
		return;	
		*/
	}
	
	public void RegisterClueLookedAt(string targetName) 
	{
		foreach(LookRecord record in m_registeredClues)
		{
			if(record.clue != null) 
			{
				if(record.clue.name == targetName)
				{
					//Debug.Log("REGISTERED " + targetName);
					m_lookedAtClues.Add(record.clue);
				}
			}
		}
	}
	
	public bool hasBeenLookedAt(string targetName)		
	{
		foreach(PhotoClueObject clue in m_lookedAtClues)
		{
		//Debug.Log(targetName + " has been looked at.");
			if(clue != null) {
				if(clue.name == targetName)
				{					
					return true;	
				}
			} 
		}		
		return false;	
	}
}