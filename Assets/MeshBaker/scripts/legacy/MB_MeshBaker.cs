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

public enum MB_OutputOptions{
	bakeIntoPrefab,
	bakeMeshsInPlace,
	bakeTextureAtlasesOnly,
	bakeIntoSceneObject
}

public class MB_MeshBaker : MB2_MeshBakerRoot {
	public class MB_DynamicGameObject:IComparable<MB_DynamicGameObject>{
		public GameObject go;
		public int vertIdx;
		public int numVerts;
				
		/*
		 combined mesh will have one submesh per result material
		 source meshes can have any number of submeshes. They are mapped to a result submesh based on their material
		 if two different submeshes have the same material they are merged in the same result submesh  
		*/
		
		//These are result mesh submeshCount comine these into a class
		public int[] submeshTriIdxs;
		public int[] submeshNumTris;
		public Rect[] obUVRects;
		
		//These are source go mesh submeshCount todo combined these into a class
		public int[] targetSubmeshIdxs;
		public Rect[] uvRects;
		public List<int>[] _submeshTris;
		
		public Mesh sharedMesh;
		public bool _beingDeleted=false;
		public int  _triangleIdxAdjustment=0;
		public Rect obUVRect = new Rect(0f,0f,1f,1f);
		
		public int CompareTo(MB_DynamicGameObject b){
			return this.vertIdx - b.vertIdx;
        }
	}
	
	static bool VERBOSE = false;
	
	[HideInInspector] public GameObject resultPrefab;
	[HideInInspector] public bool doMultiMaterial;
	[HideInInspector] public bool fixOutOfBoundsUVs = false;
	[HideInInspector] public Material resultMaterial;
	public MB_MultiMaterial[] resultMaterials = new MB_MultiMaterial[0];
	[HideInInspector] public int atlasPadding = 1;
	[HideInInspector] public bool resizePowerOfTwoTextures = true;
	[HideInInspector] public bool disableSourceRenderers = true;
	[HideInInspector] public MB_OutputOptions outputOption;
	public List<string> customShaderPropNames = new List<string>();
	public List<GameObject> objsToMesh;
	
	//use these to build the dictionary on start
	[SerializeField] Rect[] prefabUVRects = new Rect[0];
	[SerializeField] Material[] materials = new Material[0];
	Dictionary<Material,Rect> mat2rect_map = new Dictionary<Material, Rect>();
	
	List<MB_DynamicGameObject> objectsInCombinedMesh = new List<MB_DynamicGameObject>();
	Dictionary<GameObject,MB_DynamicGameObject> instance2combined_map = new Dictionary<GameObject, MB_DynamicGameObject>();
	
	Vector3[] verts = new Vector3[0];
	Vector3[] normals = new Vector3[0];
	Vector4[] tangents = new Vector4[0];
	Vector2[] uvs = new Vector2[0];
	Vector2[] uv1s = new Vector2[0];
	Vector2[] uv2s = new Vector2[0];
	Color[] colors = new Color[0];
//	int[] tris = new int[0];
	int[][] submeshTris = new int[0][];
	
	Mesh _mesh;
    
    GameObject[] empty = new GameObject[0];
	
	public override List<GameObject> GetObjectsToCombine(){
		return objsToMesh;	
	}
	
	void _initialize(){
		if (prefabUVRects.Length != materials.Length){
			Debug.LogError("Need to build texture atlases");
			return;
		}
		
		for (int i = 0; i < materials.Length; i++){
			mat2rect_map.Add(materials[i], prefabUVRects[i]);
		}
		if (VERBOSE) Debug.Log("Building mat to rect map mapping:" + materials.Length + " materials to rects.");
		BuildSceneMeshObject();	
	}
	
	public void EnableDisableSourceObjectRenderers(bool show){
		for (int i = 0; i < objsToMesh.Count; i++){
			GameObject go = objsToMesh[i];
			if (go != null){
				MeshRenderer mr = go.GetComponent<MeshRenderer>();
				if (mr != null){
					mr.enabled = show;
				}
			}
		}
	}
	
	public MB_AtlasesAndRects[] CreateAtlases(ProgressUpdateDelegate progressInfo){
		if (doMultiMaterial){
			for (int i = 0; i < resultMaterials.Length; i++){
				MB_MultiMaterial mm = resultMaterials[i];
				Shader targShader = mm.combinedMaterial.shader;
				for (int j = 0; j < mm.sourceMaterials.Count; j++){
					if (targShader != mm.sourceMaterials[j].shader){
						Debug.LogWarning("Source material " + mm.sourceMaterials[j] + " does not use shader " + targShader + " it may not have the required textures. If not empty textures will be generated.");	
					}
				}
			}
		} else {
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
			if(!tc.combineTexturesIntoAtlases(progressInfo, results[i], resMatsToPass, objsToMesh,sourceMats, atlasPadding, customShaderPropNames, resizePowerOfTwoTextures, fixOutOfBoundsUVs, 1024)){
				return null;
			}
		}
		updateMaterialToRectMapping(results);
		return results;
	}

	public MB_AtlasesAndRects[] CreateAtlases(){
		return CreateAtlases(null);
	}		

	public void BuildSceneMeshObject(){
		if (_mesh == null){
			_mesh = new Mesh();
		}
		MB_MeshBaker mom = this;
		GameObject mesh = mom.gameObject;
		MeshFilter mf = mesh.GetComponent<MeshFilter>();
		if (mf == null){
			mf = mesh.AddComponent<MeshFilter>();
		}
		MeshRenderer mr = mesh.GetComponent<MeshRenderer>();
		if (mr == null){
			mr = mesh.AddComponent<MeshRenderer>();
		}
		if (mom.doMultiMaterial){
			Material[] sharedMats = new Material[mom.resultMaterials.Length];
			for (int i = 0; i < sharedMats.Length; i++){
				sharedMats[i] = mom.resultMaterials[i].combinedMaterial;
			}
			mr.sharedMaterials	 = sharedMats;
		} else {
			mr.material = mom.resultMaterial;
		}
		mf.mesh = _mesh;
	}
	
	bool _collectMaterialTriangles(Mesh m, MB_DynamicGameObject dgo,Material[] sharedMaterials, OrderedDictionary sourceMats2submeshIdx_map){
		//everything here applies to the source object being added
		
		int numTriMeshes = m.subMeshCount;
		if (sharedMaterials.Length < numTriMeshes) numTriMeshes = sharedMaterials.Length;
		dgo._submeshTris = new List<int>[numTriMeshes];
		dgo.targetSubmeshIdxs = new int[numTriMeshes];
		for (int i = 0; i < dgo._submeshTris.Length; i++){
			dgo._submeshTris[i] = new List<int>();	
		}
		for(int i = 0; i < numTriMeshes; i++){
			if (doMultiMaterial){
				if (!sourceMats2submeshIdx_map.Contains(sharedMaterials[i])){
					Debug.LogError("Object " + dgo.go + " has a material that was not found in the result materials maping. " + sharedMaterials[i]);
					//todo might need to cleanup
					return false;
				}
				dgo.targetSubmeshIdxs[i] = (int) sourceMats2submeshIdx_map[sharedMaterials[i]];
			} else {
				dgo.targetSubmeshIdxs[i] = 0;
			}
			List<int> targTris = dgo._submeshTris[i];
			// add distinct triangles to master list
			int[] sourceTris = m.GetTriangles(i);
			for (int j = 0; j < sourceTris.Length; j++){
				targTris.Add(sourceTris[j]);	
			}
			if (VERBOSE) Debug.Log("Collecting triangles for: " + dgo.go.name + " submesh:" + i + " maps to submesh:" + dgo.targetSubmeshIdxs[i] + " added:" + sourceTris.Length);
		}
		return true;
	}
	
	bool _collectOutOfBoundsUVRects(Mesh m, MB_DynamicGameObject dgo,Material[] sharedMaterials, OrderedDictionary sourceMats2submeshIdx_map){
		int numResultMats = 1;
		if (doMultiMaterial) numResultMats = resultMaterials.Length;
		dgo.obUVRects = new Rect[numResultMats];
		for(int i = 0; i < dgo.obUVRects.Length; i++){
			dgo.obUVRects[i] = new Rect(0f,0f,1f,1f);
		}
		int numTriMeshes = m.subMeshCount;
		if (sharedMaterials.Length < numTriMeshes) numTriMeshes = sharedMaterials.Length;		
		for(int i = 0; i < numTriMeshes; i++){
			int combinedSubmeshIdx = 0;
			if (doMultiMaterial){
				if (!sourceMats2submeshIdx_map.Contains(sharedMaterials[i])){
					Debug.LogError("Object " + dgo.go + " has a material in sharedMaterials that was not found in the material to rect map. " + sharedMaterials[i] + " numKeys " + sourceMats2submeshIdx_map.Keys.Count);
					//todo might need to cleanup
					return false;
				}
				combinedSubmeshIdx = (int) sourceMats2submeshIdx_map[sharedMaterials[i]];
			}
			Rect r = new Rect();
			MB_Utility.hasOutOfBoundsUVs(m,ref r,combinedSubmeshIdx);
			dgo.obUVRects[dgo.targetSubmeshIdxs[i]] = r;
		}
		return true;		
	}
	
	bool _addToCombined(GameObject[] goToAdd, GameObject[] goToDelete,bool disableRendererInSource){
		if (goToAdd == null) goToAdd = empty;
		if (goToDelete == null) goToDelete = empty;
		if (_mesh == null)
			_mesh = new Mesh();
		if (mat2rect_map.Keys.Count == 0){
			_initialize();
		}
		
		int numResultMats = 1;
		if (doMultiMaterial) numResultMats = resultMaterials.Length;
		
		if (VERBOSE) Debug.Log("_addToCombined objs adding:" + goToAdd.Length + " objs deleting:" + goToDelete.Length + " fixOutOfBounds:" + fixOutOfBoundsUVs + " doMultiMaterial:" + doMultiMaterial);
		
		OrderedDictionary  sourceMats2submeshIdx_map = null;
		if (doMultiMaterial){
			//build the sourceMats to submesh index map
			sourceMats2submeshIdx_map = new OrderedDictionary ();
			for(int i = 0; i < numResultMats; i++){
				MB_MultiMaterial mm = resultMaterials[i];				
				for (int j = 0; j < mm.sourceMaterials.Count; j++){
					if (mm.sourceMaterials[j] == null){
						Debug.LogError("Found null material in source materials for combined mesh materials " + i);
						return false;
					}
					if (!sourceMats2submeshIdx_map.Contains(mm.sourceMaterials[j])){
						sourceMats2submeshIdx_map.Add(mm.sourceMaterials[j],i);
					}
				}
			}
		}
		
		if (submeshTris.Length == 0){
			submeshTris = new int[numResultMats][];
			for (int i = 0; i < submeshTris.Length;i++) submeshTris[i] = new int[0];
		}
		
		//calculate num to delete
		int totalDeleteVerts = 0;
//		int totalDeleteTris = 0;
		int[] totalDeleteSubmeshTris = new int[numResultMats];
		for (int i = 0; i < goToDelete.Length; i++){
			MB_DynamicGameObject dgo;
			if(instance2combined_map.TryGetValue(goToDelete[i],out dgo)){
				totalDeleteVerts += dgo.numVerts;
//				totalDeleteTris += dgo.numTris;
				for (int j = 0; j < dgo.submeshNumTris.Length; j++){
					totalDeleteSubmeshTris[j] += dgo.submeshNumTris[j];	
				}
			}else{
				Debug.LogWarning("Trying to delete an object that is not in combined mesh");	
			}
		}
		
		//now add
		List<MB_DynamicGameObject> toAddDGOs = new List<MB_DynamicGameObject>();
		int totalAddVerts = 0;
//		int totalAddTris = 0;
		int[] totalAddSubmeshTris = new int[numResultMats];
		for (int i = 0; i < goToAdd.Length; i++){
			if(!instance2combined_map.ContainsKey(goToAdd[i])){
				MB_DynamicGameObject dgo = new MB_DynamicGameObject();

				GameObject go = goToAdd[i];
				
				Material[] sharedMaterials = MB_Utility.GetGOMaterials(go);

				if (sharedMaterials == null){
					Debug.LogError("Object " + go.name + " does not have a Renderer");
					goToAdd[i] = null;
					continue;
				}

				Mesh m = MB_Utility.GetMesh(go);				
				if (m == null){
					Debug.LogError("Object " + go.name + " MeshFilter or SkinedMeshRenderer had no mesh");
					goToAdd[i] = null;			
				}
				
				Rect[] uvRects=new Rect[sharedMaterials.Length];
				for (int j = 0; j < sharedMaterials.Length; j++){
					if (!mat2rect_map.TryGetValue(sharedMaterials[j],out uvRects[j])){
						Debug.LogError("Object " + go.name + " has an unknown material " + sharedMaterials[j] + ". Try baking textures");
						goToAdd[i] = null;			
					}
				}				
				if (goToAdd[i] != null){
					dgo.go = goToAdd[i];
					dgo.uvRects = uvRects;
					dgo.sharedMesh = m;
					dgo.numVerts = m.vertexCount;

					if (!_collectMaterialTriangles(m,dgo,sharedMaterials,sourceMats2submeshIdx_map)){
						return false;
					}
					dgo.submeshNumTris = new int[numResultMats];
					dgo.submeshTriIdxs = new int[numResultMats];

					if (fixOutOfBoundsUVs){
						if (!_collectOutOfBoundsUVRects(m,dgo,sharedMaterials,sourceMats2submeshIdx_map)){
							return false;
						}
					}
					toAddDGOs.Add(dgo);
					totalAddVerts += dgo.numVerts;
//					totalAddTris += dgo.numTris;
					
					for (int j = 0; j < dgo._submeshTris.Length; j++){
						totalAddSubmeshTris[dgo.targetSubmeshIdxs[j]] += dgo._submeshTris[j].Count;
					}			
				}
			}else{
				Debug.LogWarning("Object " + goToAdd[i].name + " has already been added to " + name);
				goToAdd[i] = null;
			}
		}
		
		for (int i = 0; i < goToAdd.Length; i++){
			if (goToAdd[i] != null && disableRendererInSource){ 
				MB_Utility.DisableRendererInSource(goToAdd[i]);
			}
		}
		
		int newVertSize = verts.Length + totalAddVerts - totalDeleteVerts;
//		int newTrisSize = tris.Length + totalAddTris - totalDeleteTris;
		int[] newSubmeshTrisSize = new int[numResultMats];
		if (VERBOSE) Debug.Log("Verts adding:" +totalAddVerts + " deleting:" + totalDeleteVerts);

		if (VERBOSE) Debug.Log("Submeshes:" + newSubmeshTrisSize.Length);
		for (int i = 0; i < newSubmeshTrisSize.Length; i++){
			newSubmeshTrisSize[i] = submeshTris[i].Length + totalAddSubmeshTris[i] - totalDeleteSubmeshTris[i];	
			if (VERBOSE) Debug.Log ("    submesh :" + i + " already contains:" + submeshTris[i].Length + " trisAdded:" + totalAddSubmeshTris[i] + " trisDeleted:" + totalDeleteSubmeshTris[i]);
		}		
		
		if (newVertSize > 65534){
			Debug.LogError("Cannot add objects. Resulting mesh will have more than 64k vertices .");
			return false;				
		}		
		
		Vector3[] nverts = new Vector3[newVertSize];
		Vector3[] nnormals = new Vector3[newVertSize];
		Vector4[] ntangents = new Vector4[newVertSize];
		Vector2[] nuvs = new Vector2[newVertSize];
		Vector2[] nuv1s = new Vector2[newVertSize];		
		Vector2[] nuv2s = new Vector2[newVertSize];
		Color[] ncolors = new Color[newVertSize];
//		int[] ntris = new int[newTrisSize];
		int[][] nsubmeshTris = null;
		
		nsubmeshTris = new int[numResultMats][];
		for (int i = 0; i < nsubmeshTris.Length; i++){
			nsubmeshTris[i] = new int[newSubmeshTrisSize[i]];
		}
		
		for (int i = 0; i < goToDelete.Length; i++){
			MB_DynamicGameObject dgo; 
			if (instance2combined_map.TryGetValue(goToDelete[i], out dgo)){
				dgo._beingDeleted = true;
			}
		}		
		
		objectsInCombinedMesh.Sort();
		
		//copy existing arrays to narrays gameobj by gameobj omitting deleted ones
		int targVidx = 0;
		int[] targSubmeshTidx = new int[numResultMats];
		int triangleIdxAdjustment = 0;
		for (int i = 0; i < objectsInCombinedMesh.Count; i++){
			MB_DynamicGameObject dgo = objectsInCombinedMesh[i];
			if (!dgo._beingDeleted){
				if (VERBOSE) Debug.Log("Copying obj in combined arrays idx:" + i);
				Array.Copy(verts,dgo.vertIdx,nverts,targVidx,dgo.numVerts);
				Array.Copy(normals,dgo.vertIdx,nnormals,targVidx,dgo.numVerts);
				Array.Copy(tangents,dgo.vertIdx,ntangents,targVidx,dgo.numVerts);
				Array.Copy(uvs,dgo.vertIdx,nuvs,targVidx,dgo.numVerts);
				Array.Copy(uv1s,dgo.vertIdx,nuv1s,targVidx,dgo.numVerts);
				Array.Copy(uv2s,dgo.vertIdx,nuv2s,targVidx,dgo.numVerts);
				Array.Copy(colors,dgo.vertIdx,ncolors,targVidx,dgo.numVerts);
				
				//adjust triangles, then copy them over

				for (int subIdx = 0; subIdx < numResultMats; subIdx++){
					int[] sTris = submeshTris[subIdx];
					int sTriIdx = dgo.submeshTriIdxs[subIdx];
					int sNumTris = dgo.submeshNumTris[subIdx];
					if (VERBOSE) Debug.Log("    Adjusting submesh triangles submesh:"+subIdx+" startIdx:"+sTriIdx+" num:"+sNumTris);
					for (int j = sTriIdx; j < sTriIdx + sNumTris; j++){
						sTris[j] = sTris[j] - triangleIdxAdjustment;
					}								
					Array.Copy(sTris,sTriIdx,nsubmeshTris[subIdx],targSubmeshTidx[subIdx],sNumTris);
				}

		
				
//				dgo.triIdx = targTidx;
				dgo.vertIdx = targVidx;
//				targTidx += dgo.numTris;
				
				for (int j = 0; j < targSubmeshTidx.Length; j++){
					dgo.submeshTriIdxs[j] = targSubmeshTidx[j];
					targSubmeshTidx[j] += dgo.submeshNumTris[j];
				}
				
				targVidx += dgo.numVerts;
			} else {
				if (VERBOSE) Debug.Log("Not copying obj: " + i);
				triangleIdxAdjustment += dgo.numVerts;
			}
		}
		
		for (int i = objectsInCombinedMesh.Count - 1; i >= 0;i--){
			if (objectsInCombinedMesh[i]._beingDeleted){
				instance2combined_map.Remove(objectsInCombinedMesh[i].go);
				objectsInCombinedMesh.RemoveAt(i);	
			}
		}

		verts = nverts;
		normals = nnormals;
		tangents = ntangents;
		uvs = nuvs;
		uv1s = nuv1s;
		uv2s = nuv2s;
		colors = ncolors;
//		tris = ntris;
		submeshTris = nsubmeshTris;
		
		//add new
		for (int i = 0; i < toAddDGOs.Count; i++){
			MB_DynamicGameObject dgo = toAddDGOs[i];
			GameObject go = dgo.go;
			int vertsIdx = targVidx;
//			int trisIdx = targTidx;
					
			Mesh mesh = dgo.sharedMesh;
			Matrix4x4 l2wMat = go.transform.localToWorldMatrix;
			Quaternion l2wQ = go.transform.rotation;
			nverts = mesh.vertices;
			Vector3[] nnorms = mesh.normals;
			Vector4[] ntangs = mesh.tangents;
			for (int j = 0; j < nverts.Length; j++){
				nverts[j] = l2wMat.MultiplyPoint(nverts[j]);
				nnorms[j] = l2wQ * nnorms[j];
				float w = ntangs[j].w; //need to preserve the w value
				ntangs[j] = l2wQ * ntangs[j];
				ntangs[j].w = w;
			}
	
			int numTriSets = mesh.subMeshCount;
			if (dgo.uvRects.Length < numTriSets){
				Debug.LogWarning("Mesh " + dgo.go.name + " has more submeshes than materials");
				numTriSets = dgo.uvRects.Length;
			} else if (dgo.uvRects.Length > numTriSets){
				Debug.LogWarning("Mesh " + dgo.go.name + " has fewer submeshes than materials");
			}
			nuvs = mesh.uv;
			int[] done = new int[nuvs.Length]; //use this to track uvs that have already been adjusted don't adjust twice
			for (int l = 0; l < done.Length; l++) done[l] = -1;
 					
			bool triangleArraysOverlap = false;
			Rect obUVRect = new Rect();
			for (int k = 0; k < dgo._submeshTris.Length; k++){
				List<int> subTris = dgo._submeshTris[k];
				Rect uvRect = dgo.uvRects[k];
				if (fixOutOfBoundsUVs) obUVRect = dgo.obUVRects[dgo.targetSubmeshIdxs[k]];
				for (int l = 0; l < subTris.Count; l++){
					int vidx = subTris[l];
					if (done[vidx] == -1){
						done[vidx] = k; //prevents a uv from being adjusted twice
						if (fixOutOfBoundsUVs){
							nuvs[vidx].x = nuvs[vidx].x / obUVRect.width - obUVRect.x/obUVRect.width;
							nuvs[vidx].y = nuvs[vidx].y / obUVRect.height - obUVRect.y/obUVRect.height;
						}
						nuvs[vidx].x = uvRect.x + nuvs[vidx].x*uvRect.width;
						nuvs[vidx].y = uvRect.y + nuvs[vidx].y*uvRect.height;
					}
					if (done[vidx] != k){
						triangleArraysOverlap = true;	
					}
				}
			}
			if (triangleArraysOverlap){
				Debug.LogWarning(dgo.go.name + "has submeshes which share verticies. Adjusted uvs may not map correctly in combined atlas.");	
			}		
			
			nverts.CopyTo(verts,vertsIdx);
			nnorms.CopyTo(normals,vertsIdx);
			ntangs.CopyTo(tangents,vertsIdx);
			nuvs.CopyTo(uvs,vertsIdx);
			mesh.uv2.CopyTo(uv1s,vertsIdx);
			mesh.uv2.CopyTo(uv2s,vertsIdx);
			mesh.colors.CopyTo(colors,vertsIdx);
//			ntris = mesh.triangles;
//			for (int j = 0; j < ntris.Length; j++){
//				ntris[j] = ntris[j] + vertsIdx;	
//			}
			
			for (int combinedMeshIdx = 0; combinedMeshIdx < targSubmeshTidx.Length; combinedMeshIdx++){
				dgo.submeshTriIdxs[combinedMeshIdx] = targSubmeshTidx[combinedMeshIdx];
			}
			for (int j = 0; j < dgo._submeshTris.Length; j++){
				List<int> sts = dgo._submeshTris[j];
				for (int k = 0; k < sts.Count; k++){
					sts[k] = sts[k] + vertsIdx;
				}
				int combinedMeshIdx = dgo.targetSubmeshIdxs[j];
				sts.CopyTo(submeshTris[combinedMeshIdx], targSubmeshTidx[combinedMeshIdx]);
				dgo.submeshNumTris[combinedMeshIdx] += sts.Count;
				targSubmeshTidx[combinedMeshIdx] += sts.Count;
			}
						
			dgo.vertIdx = targVidx;
			
			instance2combined_map.Add(go,dgo);
			objectsInCombinedMesh.Add(dgo);

			targVidx += nverts.Length;
			for (int j = 0; j < dgo._submeshTris.Length; j++) dgo._submeshTris[j].Clear();
			dgo._submeshTris = null;
			if (VERBOSE) Debug.Log("Added to combined:" + dgo.go.name + " verts:" + nverts.Length);
		}
		return true;
	}
	
	public void ApplyAll(){
		Apply(true,true,true,true,true,true,true,true);
	}
	
	public void Apply(bool triangles,
					  bool vertices,
					  bool normals,
					  bool tangents,
					  bool uvs,
					  bool colors,
					  bool uv1,
					  bool uv2){
		if (_mesh != null){
			if (_mesh.vertexCount != verts.Length){
#if UNITY_3_5
				_mesh.Clear();
#else
				_mesh.Clear(false); //clear all the data and start with a blank mesh
#endif
			} else if (triangles /*|| _mesh.triangles.Length != submeshTris[0].Length*/){ 
				_mesh.Clear(); //only clear the triangles todo put above check back in	
			}
			if (vertices)  _mesh.vertices = verts;
			if (triangles){
				_mesh.triangles = submeshTris[0];
				if (doMultiMaterial){
					_mesh.subMeshCount = submeshTris.Length;
					for (int i = 0; i < submeshTris.Length; i++){
						_mesh.SetTriangles(submeshTris[i],i);
					}
				}
			}
			if (normals)   _mesh.normals = this.normals;
			if (tangents)  _mesh.tangents = this.tangents;
			if (uvs)	   _mesh.uv = this.uvs;
			if (colors)    _mesh.colors = this.colors;
			if (uv1)       _mesh.uv2 = this.uv1s;
			if (uv2)       _mesh.uv2 = this.uv2s;
			if (triangles || vertices) _mesh.RecalculateBounds();
		} else {
			Debug.LogError("Need to add objects to this meshbaker before calling Apply or ApplyAll");	
		}
	}
	
	public void UpdateGameObjects(GameObject[] gos, bool recalcBounds = true){		
		_updateGameObjects(gos, recalcBounds);
	}
	
	void _updateGameObjects(GameObject[] gos, bool recalcBounds){
		for (int i = 0; i < gos.Length; i++){
			_updateGameObject(gos[i],false);
		}
		if (recalcBounds)
			_mesh.RecalculateBounds();
	}
	
	void _updateGameObject(GameObject go, bool recalcBounds){
			MB_DynamicGameObject dgo;
			if (!instance2combined_map.TryGetValue(go,out dgo)){
				Debug.LogError("Object " + go.name + " has not been added to " + name);
				return;
			}
			Mesh mesh = dgo.sharedMesh;
			if (dgo.numVerts != mesh.vertexCount){
				Debug.LogError("Object " + go.name + " source mesh has been modified since being added");
				return;			
			}
			Matrix4x4 l2wMat = go.transform.localToWorldMatrix;
			Quaternion l2wQ = go.transform.rotation;

			Vector3[] nverts = mesh.vertices;
			Vector3[] nnorms = mesh.normals;
			Vector4[] ntangs = mesh.tangents;
			for (int j = 0; j < nverts.Length; j++){
				int midx = dgo.vertIdx + j;
				verts[midx] = l2wMat.MultiplyPoint3x4(nverts[j]);
				normals[midx] = l2wQ * nnorms[j];
				float w = ntangs[j].w;
				tangents[midx] = l2wQ * ntangs[j];
				tangents[midx].w = w;
			}
	}
	
	public Mesh AddDeleteGameObjects(GameObject[] gos, GameObject[] deleteGOs, bool disableRendererInSource=true, bool fixOutOfBoundUVs=false){
		//check for duplicates
		if (gos != null){
			for (int i = 0; i < gos.Length; i++){
				for (int j = i + 1; j < gos.Length; j++){
					if (gos[i] == gos[j]){
						Debug.LogError("GameObject " + gos[i] + "appears twice in list of game objects to add");
						return null;
					}
				}
			}
		}
		if (deleteGOs != null){
			for (int i = 0; i < deleteGOs.Length; i++){
				for (int j = i + 1; j < deleteGOs.Length; j++){
					if (deleteGOs[i] == deleteGOs[j]){
						Debug.LogError("GameObject " + deleteGOs[i] + "appears twice in list of game objects to delete");
						return null;
					}
				}
			}
		}
		if (!_addToCombined(gos,deleteGOs,disableRendererInSource)){
			Debug.LogError("Failed to add/delete objects to combined mesh");	
		}
		return _mesh;
	}
	
	void updateMaterialToRectMapping(MB_AtlasesAndRects[] newMeshs){
		mat2rect_map.Clear();
		List<Material> ms = new List<Material>();
		List<Rect> rs = new List<Rect>();
		for (int i = 0; i < newMeshs.Length; i++){
			MB_AtlasesAndRects newMesh = newMeshs[i];
			Dictionary<Material,Rect> map = newMesh.mat2rect_map;
			foreach(Material m in map.Keys){
				ms.Add(m);
				rs.Add(map[m]);
				mat2rect_map.Add(m,map[m]);
			}
		}
		materials = ms.ToArray();
		prefabUVRects = rs.ToArray();
	}
	
	public void DestroyMesh(){
		if (_mesh != null){
			MB_Utility.Destroy(_mesh);
			_mesh = null;
		}
		verts = new Vector3[0];
		normals = new Vector3[0];
		tangents = new Vector4[0];
		uvs = new Vector2[0];
		uv1s = new Vector2[0];
		uv2s = new Vector2[0];
		colors = new Color[0];
//		tris = new int[0];
		submeshTris = new int[0][];
		
		objectsInCombinedMesh.Clear();
		instance2combined_map.Clear();		
	}
}

