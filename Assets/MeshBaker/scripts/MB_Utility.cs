//----------------------------------------------
//            MeshBaker
// Copyright © 2011-2012 Ian Deane
//----------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

#if UNITY_EDITOR
	using UnityEditor;
#endif

public enum MB_ObjsToCombineTypes{
	prefabOnly,
	sceneObjOnly,
	dontCare
}

public class MB_Utility{
	public static Texture2D createTextureCopy(Texture2D source){
		Texture2D newTex = new Texture2D(source.width,source.height,TextureFormat.ARGB32,true);
		newTex.SetPixels(source.GetPixels());
		return newTex;
	}
	
	public static Material[] GetGOMaterials(GameObject go){
		if (go == null) return null;
		MeshRenderer mr = go.GetComponent<MeshRenderer>();
		if (mr != null){
			return mr.sharedMaterials;
		}
		
		SkinnedMeshRenderer smr = go.GetComponent<SkinnedMeshRenderer>();
		if (smr != null){
			return smr.sharedMaterials;
		}
		
		Debug.LogError("Object " + go.name + " does not have a MeshRenderer or a SkinnedMeshRenderer component");
		return null;
	}

	public static Mesh GetMesh(GameObject go, bool forceSharedMesh=false){
		if (go == null) return null;
		MeshFilter mf = go.GetComponent<MeshFilter>();
		if (mf != null){
			if (Application.isPlaying && !forceSharedMesh){
				return mf.mesh;	
			} else {
				return mf.sharedMesh;
			}
		}
		
		SkinnedMeshRenderer smr = go.GetComponent<SkinnedMeshRenderer>();
		if (smr != null){
			return smr.sharedMesh;
		}
		
		Debug.LogError("Object " + go.name + " does not have a MeshFilter or a SkinnedMeshRenderer component");
		return null;
	}
	
	public static Renderer GetRenderer(GameObject go){
		if (go == null) return null;
		MeshRenderer mr = go.GetComponent<MeshRenderer>();
		if (mr != null) return mr; 
		
		
		SkinnedMeshRenderer smr = go.GetComponent<SkinnedMeshRenderer>();
		if (smr != null) return smr;
		return null;		
	}
	
	public static void DisableRendererInSource(GameObject go){
		if (go == null) return;
		MeshRenderer mf = go.GetComponent<MeshRenderer>();
		if (mf != null){
			mf.enabled = false;
			return;
		}
		
		SkinnedMeshRenderer smr = go.GetComponent<SkinnedMeshRenderer>();
		if (smr != null){
			smr.enabled = false;
			return;
		}			
	}
	
	public static bool hasOutOfBoundsUVs(Mesh m, ref Rect uvBounds, int submeshIndex = -1){
		Vector2[] uvs = m.uv;
		if (uvs.Length == 0) return false;
		float minx,miny,maxx,maxy;
		if (submeshIndex > m.subMeshCount) return false;
		if (submeshIndex >= 0){
			int[] tris = m.GetTriangles(submeshIndex);
			minx = maxx = uvs[tris[0]].x;
			miny = maxy = uvs[tris[0]].y;
			for (int idx = 0; idx < tris.Length; idx++){
				int i = tris[idx];
				if (uvs[i].x < minx) minx = uvs[i].x;
				if (uvs[i].x > maxx) maxx = uvs[i].x;
				if (uvs[i].y < miny) miny = uvs[i].y;
				if (uvs[i].y > maxy) maxy = uvs[i].y;
			}			
		} else {
			minx = maxx = uvs[0].x;
			miny = maxy = uvs[0].y;
			for (int i = 0; i < uvs.Length; i++){
					if (uvs[i].x < minx) minx = uvs[i].x;
					if (uvs[i].x > maxx) maxx = uvs[i].x;
					if (uvs[i].y < miny) miny = uvs[i].y;
					if (uvs[i].y > maxy) maxy = uvs[i].y;
			}
		} 
		uvBounds.x = minx;
		uvBounds.y = miny;
		uvBounds.width = maxx - minx;
		uvBounds.height = maxy - miny;
		if (maxx > 1f || minx < 0f || maxy > 1f || miny < 0f){
			return true;
		}
		//all well behaved objs use the same rect so TexSets compare properly
		uvBounds.x = uvBounds.y = 0f;
		uvBounds.width = uvBounds.height = 1f;
		return false;
	}
	
	public static void setSolidColor(Texture2D t, Color c){
		Color[] cs = t.GetPixels();
		for (int i = 0; i < cs.Length; i++){
			cs[i] = c;	
		}
		t.SetPixels(cs);
		t.Apply();
	}
	
	public static Texture2D resampleTexture(Texture2D source, int newWidth, int newHeight){
		TextureFormat f = source.format;
		if (f == TextureFormat.ARGB32 ||
			f == TextureFormat.RGBA32 ||
			f == TextureFormat.BGRA32 ||
			f == TextureFormat.RGB24  ||
			f == TextureFormat.Alpha8 ||
			f == TextureFormat.DXT1)
		{
			Texture2D newTex = new Texture2D(newWidth,newHeight,TextureFormat.ARGB32,true);
			float w = newWidth;
			float h = newHeight;
			for (int i = 0; i < newWidth; i++){
				for (int j = 0; j < newHeight; j++){
					float u = i/w;
					float v = j/h;
					newTex.SetPixel(i,j,source.GetPixelBilinear(u,v));
				}
			}
			newTex.Apply(); 		
			return newTex;
		} else {
			Debug.LogError("Can only resize textures in formats ARGB32, RGBA32, BGRA32, RGB24, Alpha8 or DXT");	
			return null;
		}
	}

	class MB_Triangle{
		int submeshIdx;
		int[] vs = new int[3];
		
		public bool isSame(object obj){
			MB_Triangle tobj = (MB_Triangle) obj;
			if (vs[0] == tobj.vs[0] &&
				vs[1] == tobj.vs[1] &&
				vs[2] == tobj.vs[2] &&
				submeshIdx != tobj.submeshIdx){
				return true;	
			}
			return false;
		}
		
		public bool sharesVerts(MB_Triangle obj){
			if (vs[0] == obj.vs[0] ||
				vs[0] == obj.vs[1] ||
				vs[0] == obj.vs[2]){
				if (submeshIdx != obj.submeshIdx) return true;	
			}
			if (vs[1] == obj.vs[0] ||
				vs[1] == obj.vs[1] ||
				vs[1] == obj.vs[2]){
				if (submeshIdx != obj.submeshIdx) return true;
			}	
			if (vs[2] == obj.vs[0] ||
				vs[2] == obj.vs[1] ||
				vs[2] == obj.vs[2]){
				if (submeshIdx != obj.submeshIdx) return true;	
			}
			return false;			
		}
		
		public MB_Triangle(int[] ts, int idx, int sIdx){
			vs[0] = ts[idx];
			vs[1] = ts[idx + 1];
			vs[2] = ts[idx + 2];
			submeshIdx = sIdx;
			Array.Sort(vs);
		}
	}
	
	public static bool validateOBuvsMultiMaterial(Material[] sharedMaterials){
		for (int i = 0; i < sharedMaterials.Length; i++){
			for (int j = i + 1; j < sharedMaterials.Length; j++){
				if (sharedMaterials[i] == sharedMaterials[j]){
					return false;
				}
			}
		}
		return true;
	}
	
	public static int doSubmeshesShareVertsOrTris(Mesh m){
		List<MB_Triangle> tlist = new List<MB_Triangle>();
		bool sharesVerts = false;
		bool sharesTris = false;
		for (int i = 0; i < m.subMeshCount; i++){
			int[] sm = m.GetTriangles(i);
			for (int j = 0; j < sm.Length; j+=3){
				MB_Triangle consider = new MB_Triangle(sm,j,i);
				for (int k = 0; k < tlist.Count; k++){
					if (consider.isSame(tlist[k])) sharesTris = true;
					if (consider.sharesVerts(tlist[k])){
						sharesVerts = true;
					}
				}
				tlist.Add(consider);
			}
		}
		if (sharesTris) return 2;
		if (sharesVerts) return 1;
		return 0;
	}	

	public static void buildSceneMeshObject(GameObject root, MB2_MeshBaker mom, Mesh m){	
		GameObject mesh;
		MeshFilter mf = null;
		MeshRenderer mr = null;
		SkinnedMeshRenderer smr = null;
		Transform mt = null;
		foreach (Transform t in root.transform){
			if (t.name.EndsWith("-mesh")){
				mt = t;
			}
		}
		if (mt == null){
			mesh = new GameObject(mom.name + "-mesh");
		} else {
			mesh = mt.gameObject;	
		}
		if (mom.renderType == MB_RenderType.skinnedMeshRenderer){
			smr = mesh.GetComponent<SkinnedMeshRenderer>();
			if (smr == null) smr = mesh.AddComponent<SkinnedMeshRenderer>();
		} else {
			mf = mesh.GetComponent<MeshFilter>();
			if (mf == null) mf = mesh.AddComponent<MeshFilter>();
			mr = mesh.GetComponent<MeshRenderer>();
			if (mr == null) mr = mesh.AddComponent<MeshRenderer>();
		}
		if (mom.textureBakeResults.doMultiMaterial){
			Material[] sharedMats = new Material[mom.textureBakeResults.resultMaterials.Length];
			for (int i = 0; i < sharedMats.Length; i++){
				sharedMats[i] = mom.textureBakeResults.resultMaterials[i].combinedMaterial;
			}
			if (mom.renderType == MB_RenderType.skinnedMeshRenderer){
				smr.materials = sharedMats;
				smr.bones = mom.getBones();
				smr.updateWhenOffscreen = true; //todo see if can remove
				//todo update bounds
				
			} else {
				mr.sharedMaterials = sharedMats;
			}
		} else {
			if (mom.renderType == MB_RenderType.skinnedMeshRenderer){
				smr.material = mom.textureBakeResults.resultMaterial;
				smr.bones = mom.getBones();
			} else {
				mr.material = mom.textureBakeResults.resultMaterial;
			}
		}
		if (mom.renderType == MB_RenderType.skinnedMeshRenderer){
			smr.sharedMesh = m;
			smr.lightmapIndex = mom.getLightmapIndex();
		} else {
			mf.mesh = m;
			mr.lightmapIndex = mom.getLightmapIndex();
		}
		if (mom.lightmapOption == MB2_LightmapOptions.preserve_current_lightmapping || mom.lightmapOption == MB2_LightmapOptions.generate_new_UV2_layout){
			mesh.isStatic = true;
		}
		
		List<GameObject> gos = mom.GetObjectsToCombine();
		if (gos.Count > 0){
			bool tagsAreSame = true;
			bool layersAreSame = true;
			string tag = gos[0].tag;
			int layer = gos[0].layer;
			for (int i = 0; i < gos.Count; i++){
				if (!gos[i].tag.Equals(tag)) tagsAreSame = false;
				if (gos[i].layer != layer) layersAreSame = false;				
			}
			if (tagsAreSame){ 
				root.tag = tag;
				mesh.tag = tag;
			}
			if (layersAreSame){ 
				root.layer = layer;
				mesh.layer = layer;
			}
		}
		mesh.transform.parent = root.transform;
	}	
	
	public static void Destroy(UnityEngine.Object o){
#if UNITY_EDITOR
		string p = AssetDatabase.GetAssetPath(o);
		if (p != null && p.Equals("")) // don't try to destroy assets
			MonoBehaviour.DestroyImmediate(o);
#else
		MonoBehaviour.Destroy(o);				
#endif		
	}
	
	public static bool doCombinedValidate(MB2_MeshBakerRoot mom, MB_ObjsToCombineTypes objToCombineType){
//		MB2_MeshBaker mom = (MB2_MeshBaker) target;
		
		if (mom.textureBakeResults == null){
			Debug.LogError("Need to set textureBakeResults");
			return false;
		}
		if (!(mom is MB2_TextureBaker)){
			MB2_TextureBaker tb = mom.GetComponent<MB2_TextureBaker>();
			if (tb != null && tb.textureBakeResults != mom.textureBakeResults){
				Debug.LogWarning("textureBakeResults on this component is not the same as the textureBakeResults on the MB2_TextureBaker.");
			}
		}
		
		List<GameObject> objsToMesh = mom.GetObjectsToCombine();
		for (int i = 0; i < objsToMesh.Count; i++){
			GameObject go = objsToMesh[i];
			if (go == null){
				Debug.LogError("The list of objects to combine contains nulls.");
				return false;					
			}
			for (int j = i + 1; j < objsToMesh.Count; j++){
				if (objsToMesh[i] == objsToMesh[j]){
					Debug.LogError("The list of objects to combine contains duplicates.");
					return false;	
				}
			}
			if (MB_Utility.GetGOMaterials(go) == null){
				Debug.LogError("Object " + go + " in the list of objects to be combined does not have a material");
				return false;
			}
			if (MB_Utility.GetMesh(go) == null){
				Debug.LogError("Object " + go + " in the list of objects to be combined does not have a mesh");
				return false;
			}			
		}
		
		if (mom.textureBakeResults.doMultiMaterial){
			if (!validateSubmeshOverlap(mom)){//only warns currently
				return false;
			}
		}
		List<GameObject> objs = objsToMesh;
		if (mom is MB2_MeshBaker){
			MB2_TextureBaker tb = mom.GetComponent<MB2_TextureBaker>();
			if (((MB2_MeshBaker)mom).useObjsToMeshFromTexBaker && tb != null) objs = tb.objsToMesh; 
			if (objs == null || objs.Count == 0){
				Debug.LogError("No meshes to combine. Please assign some meshes to combine.");
				return false;
			}
		}

#if UNITY_EDITOR		
		for (int i = 0; i < objsToMesh.Count; i++){
			UnityEditor.PrefabType pt = UnityEditor.PrefabUtility.GetPrefabType(objsToMesh[i]);
			if (pt == UnityEditor.PrefabType.None ||
				pt == UnityEditor.PrefabType.PrefabInstance || 
				pt == UnityEditor.PrefabType.ModelPrefabInstance || 
				pt == UnityEditor.PrefabType.DisconnectedPrefabInstance ||
				pt == UnityEditor.PrefabType.DisconnectedModelPrefabInstance){
				// these are scene objects
				if (objToCombineType == MB_ObjsToCombineTypes.prefabOnly){
					Debug.LogWarning("The list of objects to combine contains scene objects. You probably want prefabs." + objsToMesh[i] + " is a scene object");	
				}
			} else if (objToCombineType == MB_ObjsToCombineTypes.sceneObjOnly){
				//these are prefabs
				Debug.LogWarning("The list of objects to combine contains prefab objects. You probably want scene objects." + objsToMesh[i] + " is a prefab object");
			}
		}
#endif
		return true;
	}	
	
	static bool validateMultipleMaterials(MB2_MeshBakerRoot mom){
		MB_MultiMaterial[] rs = mom.textureBakeResults.resultMaterials;
		List<Material> allSource = new List<Material>();
		List<Material> allCombined = new List<Material>();
		for (int i = 0; i < rs.Length; i++){
			if (allCombined.Contains(rs[i].combinedMaterial)){
				Debug.LogError("There are duplicate combined materials in the combined materials list");
				return false;	
			}
			for (int j = 0; j < rs[i].sourceMaterials.Count; j++){
				if (rs[i].sourceMaterials[j] == null){
					Debug.LogError("There are nulls in the list of source materials");
					return false;					
				}
				if (allSource.Contains(rs[i].sourceMaterials[j])){
					Debug.LogError("There are duplicate source materials in the combined materials list");
					return false;	
				}
				allSource.Add(rs[i].sourceMaterials[j]);
			}
			allCombined.Add(rs[i].combinedMaterial);
		}
		return true;
	}
	
	static bool validateSubmeshOverlap(MB2_MeshBakerRoot mom){
		List<GameObject> objsToMesh = mom.GetObjectsToCombine();
		for (int i = 0; i < objsToMesh.Count; i++){
			Mesh m = MB_Utility.GetMesh(objsToMesh[i]);
			if (MB_Utility.doSubmeshesShareVertsOrTris(m) != 0){
				Debug.LogError("Object " + objsToMesh[i] + " in the list of objects to combine has overlapping submeshes. This object can only be combined with objects that use the exact same set of textures. There may be other undesirable side affects as well.");	
				return true;
			}
		}
		return true;
	}
		
}
