//----------------------------------------------
//            MeshBaker
// Copyright © 2011-2012 Ian Deane
//----------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Specialized;
using System;
using System.Collections.Generic;
using System.Text;


public class MB2_MeshBakerRoot : MonoBehaviour {
	[HideInInspector] public MB2_TextureBakeResults textureBakeResults; //todo validate that is same on texture baker and meshbaker
	
	public virtual List<GameObject> GetObjectsToCombine(){
		return null;	
	}
}

