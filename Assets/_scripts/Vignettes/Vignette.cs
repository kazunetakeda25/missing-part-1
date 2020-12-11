using UnityEngine;
using System.Collections;

public class Vignette
{
	public enum VignetteID
	{
		E1vTerrysApartmentSearch,
		E1vNervousElevator,
		E1vHomeOfficeSearch,
		E1vStephEvasive,
		E1vPlantHugger,
		E2vCopyProtection,
		E2vSorianoNice,
		E2vPressRelease,
		E2vGPCOfficeSearch,
		E2vDeadlyTreatment,
		E3vToYourHealth,
		E3vSuspiciousMen,
		E3vCoupleRomance,
		E3vChrisBriefcaseSearch
	};
	
	public enum VignetteType
	{
		None,
		FAE,
		Logic,
		Conf,
	};
	
	public VignetteID vignetteID;
	public VignetteType vignetteType;
	
	public Vignette (VignetteID id, VignetteType type) 
	{
		vignetteID = id;
		vignetteType = type;
	}
}

