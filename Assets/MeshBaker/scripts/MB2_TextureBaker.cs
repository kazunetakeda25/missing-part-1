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

#if UNITY_EDITOR
	using UnityEditor;
#endif 


public class MB2_TextureBaker : MB2_MeshBakerRoot {	
	static bool VERBOSE = false;
		
	[HideInInspector] public int maxTilingBakeSize = 1024;
	[HideInInspector] public bool doMultiMaterial;
	[HideInInspector] public bool fixOutOfBoundsUVs = false;
	[HideInInspector] public Material resultMaterial;
	public MB_MultiMaterial[] resultMaterials = new MB_MultiMaterial[0];
	[HideInInspector] public int atlasPadding = 1;
	[HideInInspector] public bool resizePowerOfTwoTextures = true;
	public List<string> customShaderPropNames = new List<string>();
	public List<GameObject> objsToMesh;
	
	public override List<GameObject> GetObjectsToCombine(){
		return objsToMesh;
	}
	
	public MB_AtlasesAndRects[] CreateAtlases(ProgressUpdateDelegate progressInfo){
		if (doMultiMaterial){
			for (int i = 0; i < resultMaterials.Length; i++){
				MB_MultiMaterial mm = resultMaterials[i];
				if (mm.combinedMaterial == null){
					Debug.LogError("Combined Material is null please create and assign a result material.");
					return null;					
				}
				Shader targShader = mm.combinedMaterial.shader;
				for (int j = 0; j < mm.sourceMaterials.Count; j++){
					if (mm.sourceMaterials[j] == null){
						Debug.LogError("There are null entries in the list of Source Materials");
						return null;
					}
					if (targShader != mm.sourceMaterials[j].shader){
						Debug.LogWarning("Source material " + mm.sourceMaterials[j] + " does not use shader " + targShader + " it may not have the required textures. If not empty textures will be generated.");	
					}
				}
			}
		} else {
			if (resultMaterial == null){
				Debug.LogError("Combined Material is null please create and assign a result material.");
				return null;
			}
			Shader targShader = resultMaterial.shader;
			for (int i = 0; i < objsToMesh.Count; i++){
				Material[] ms = MB_Utility.GetGOMaterials(objsToMesh[i]);
				for (int j = 0; j < ms.Length; j++){
					Material m = ms[j];
					if (m == null){
						Debug.LogError("Game object " + objsToMesh[i] + " has a null material. Can't build atlases");
						return null;
					}
					if (m.shader != targShader){
						Debug.LogWarning("Game object " + objsToMesh[i] + " does not use shader " + targShader + " it may not have the required textures. If not empty textures will be generated.");
					}
				}
			}
		}

		int numResults = 1;
		if (doMultiMaterial) numResults = resultMaterials.Length;
		MB_AtlasesAndRects[] results = new MB_AtlasesAndRects[numResults];
		for (int i = 0; i < results.Length; i++){
			results[i] = new MB_AtlasesAndRects();
		}
		MB_TextureCombiner tc = new MB_TextureCombiner();
		
		Material[] resMatsToPass = new Material[1];
		List<Material> sourceMats = null;
		for (int i = 0; i < results.Length; i++){
			if (doMultiMaterial) {
				sourceMats = resultMaterials[i].sourceMaterials;
				resMatsToPass[0] = resultMaterials[i].combinedMaterial;
			} else {
				resMatsToPass[0] = resultMaterial;	
			}
			Debug.Log("Creating atlases for result material " + resMatsToPass[0]);
			if(!tc.combineTexturesIntoAtlases(progressInfo, results[i], resMatsToPass, objsToMesh,sourceMats, atlasPadding, customShaderPropNames, resizePowerOfTwoTextures, fixOutOfBoundsUVs, maxTilingBakeSize)){
				return null;
			}
		}
		if (VERBOSE) Debug.Log("Created Atlases");
		return results;
	}

	public MB_AtlasesAndRects[] CreateAtlases(){
		return CreateAtlases(null);
	}		


}

