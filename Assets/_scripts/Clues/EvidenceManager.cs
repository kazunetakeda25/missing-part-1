using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;

public enum EvidenceGroup
{
	None,
	Ambiguious,
	Confirming,
	Disconfirming,
}
	
public class EvidenceReport
{
	// Dictionary of object names to object reports.
	public Dictionary<string, objectReport> m_facts = new Dictionary<string, objectReport>();
}

public class objectReport
{
	// How many times has this object been looked at?
	public int m_lookCount = 0;
	// How many times has this object been interacted with?
	public int m_interactCount = 0;
	// How many times was this object photographed?
	public int m_photographedCount = 0;
	// How many photographs is this object still present in?
	public int m_inPictureCount = 0;
	// How many times has this object been sent in a photo attachment?
	public int m_attachCount = 0;
	// Which evidence group is this object in?
	public EvidenceGroup m_group = EvidenceGroup.None;
	// What is the value of this evidence?
	public float m_evidenceStrength = 1.0f;
}

[AddComponentMenu("Framework/Evidence Manager")]
public class EvidenceManager : MonoBehaviour
{
	private LevelManager levelManager;
	
	private List<InteractableWorldObject> registeredEvidence = new List<InteractableWorldObject>();	
	
	public Material DefaultObjectMat;
	
	public void Start()
	{
		levelManager = GameObject.FindWithTag(Tags.LEVEL_MANAGER_TAG).GetComponent<LevelManager>();
	}
		
	public void registerEvidence(InteractableWorldObject target)
	{
		// Add the object to the list we care about if it is not in the None group.
		if(target.getEvidenceGroup() != EvidenceGroup.None)
			registeredEvidence.Add(target);
		
		return;
	}
	
	public void UnRegisterEvidence(InteractableWorldObject target)
	{
		if(registeredEvidence.Contains(target)) 
		{
			registeredEvidence.Remove(target);
		}
	}
	
	public EvidenceReport getEvidenceReport( Vignette.VignetteID vignetteID, bool bSpewStats = true )
	{
		EvidenceReport results = new EvidenceReport();
		LookManager peekaboo = levelManager.LookManager;
		
		// For each evidence object, iterate through the registered objects
		// and determine their facts.
		foreach(InteractableWorldObject evidence in registeredEvidence)
		{
			if(evidence != null)
			{
				if(evidence.getEvidenceGroup() == EvidenceGroup.None)
				{
					continue;	
				}
				
				//Does this object relate to the vignette ID specified?
				if (evidence.IsApplicableToVignette( vignetteID ) == false)
				{
					//Skip it!
					continue;
				}				
				
				if(!results.m_facts.ContainsKey(evidence.name))
				{
					results.m_facts.Add(evidence.name, new objectReport());
				}
				else
				{
					Debug.LogWarning(evidence.name + " already existed for some reason!");
					continue;
				}
			}
			
			objectReport newReport = new objectReport();
			PhotoManager pManager = levelManager.PhotoManager;
						
			// Find out how many times it was interacted with.
			newReport.m_interactCount = evidence.timesInteractedWith();

			// Ask the Photo Manager how many times this object was photographed.
			newReport.m_photographedCount = pManager.GetTimesPhotographed(evidence.name);
			
			// Ask the Look Manager if the object has been looked at.
			if(peekaboo.hasBeenLookedAt(evidence.name))
			{
				newReport.m_lookCount = 1;
			}
			else
			{
				newReport.m_lookCount = 0;	
			}
					
			// Ask the Photo Manager how many pictures it is currently in.	
			newReport.m_inPictureCount = pManager.GetTimesInPhotos(evidence.name);;
			
			// Ask the Photo Manager how many attachments it is currently in.
			newReport.m_attachCount = pManager.GetTimesInAttachments(evidence.name);
			
			newReport.m_group = evidence.getEvidenceGroup();
			newReport.m_evidenceStrength = evidence.getEvidenceValue();
			
			// Stash the facts.
			results.m_facts[evidence.name] = newReport;
			
			if ( bSpewStats )
			{
				Debug.Log(evidence.name + " was looked at " + newReport.m_lookCount + " times.");
				Debug.Log(evidence.name + " was interacted with " + newReport.m_interactCount + " times.");
				Debug.Log(evidence.name + " was photographed " + newReport.m_photographedCount + " times.");
				Debug.Log(evidence.name + " exists in " + newReport.m_inPictureCount + " photographs.");
				Debug.Log(evidence.name + " was sent as an attachment " + newReport.m_attachCount + " times.");
			}				
		}		
		
		return results;
	}
	
	public Material GetDefaultHighlightMaterial() 
	{
		return DefaultObjectMat;
	}
}