//----------------------------------------------
//            MeshBaker
// Copyright © 2011-2012 Ian Deane
//----------------------------------------------
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class MB_MeshBakerEditorWindow : EditorWindow
{
	public MB2_MeshBakerRoot target = null;
	GameObject targetGO = null;
	GameObject oldTargetGO = null;
	MB2_TextureBaker textureBaker;
	MB2_MeshBaker meshBaker;
	bool onlyStaticObjects = false;
	Material shaderMat = null;
	Material mat = null;
	
	bool tbFoldout = false;
	bool mbFoldout = false;
	
	MB2_MeshBakerEditorInternal mbe = new MB2_MeshBakerEditorInternal();
	MB2_TextureBakerEditorInternal tbe = new MB2_TextureBakerEditorInternal();

	Vector2 scrollPos = Vector2.zero;
	
	void OnGUI()
	{
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(position.width), GUILayout.Height(position.height));

		EditorGUILayout.HelpBox("List shaders in scene prints a report to the console of shaders and which objects use them. This is useful for grouping objects to be combined. SkinnedMeshRenderers are ignored.", UnityEditor.MessageType.None);
		if (GUILayout.Button("List Shaders In Scene")){
			listMaterialsInScene();
		}
		
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		EditorGUILayout.HelpBox("Select one or more objects in the hierarchy view. Child Game Objects with MeshRender will be added. Use the fields below to filter what is added.", UnityEditor.MessageType.None);
		target = (MB2_MeshBakerRoot) EditorGUILayout.ObjectField("Target to add objects to",target,typeof(MB2_MeshBakerRoot),true);
		
		if (target != null){
			targetGO = target.gameObject;
		} else {
			targetGO = null;	
		}
			
		if (targetGO != oldTargetGO){
			textureBaker = targetGO.GetComponent<MB2_TextureBaker>();
			meshBaker = targetGO.GetComponent<MB2_MeshBaker>();
			tbe = new MB2_TextureBakerEditorInternal();
			mbe = new MB2_MeshBakerEditorInternal();
			oldTargetGO = targetGO;
		}		
		
		onlyStaticObjects = EditorGUILayout.Toggle("Only Static Objects", onlyStaticObjects);
		
		mat = (Material) EditorGUILayout.ObjectField("Using Material",mat,typeof(Material),true);
		shaderMat = (Material) EditorGUILayout.ObjectField("Using Shader",shaderMat,typeof(Material),true);
		
		if (GUILayout.Button("Add Selected Meshes")){
			addSelectedObjects();
		}
		
		if (textureBaker != null){
			MB_EditorUtil.DrawSeparator();
			tbFoldout = EditorGUILayout.Foldout(tbFoldout,"Texture Baker");
			if (tbFoldout){
				tbe.DrawGUI((MB2_TextureBaker) textureBaker);
			}
			
		}
		if (meshBaker != null){
			MB_EditorUtil.DrawSeparator();
			mbFoldout = EditorGUILayout.Foldout(mbFoldout,"Mesh Baker");
			if (mbFoldout){
				mbe.DrawGUI((MB2_MeshBaker) meshBaker);
			}
		}
		EditorGUILayout.EndScrollView();
	}
	

	void addSelectedObjects(){
		MB2_MeshBakerRoot mom = (MB2_MeshBakerRoot) target;
		if (mom == null){
			Debug.LogError("Must select a target MeshBaker to add objects to");
			return;
		}
		GameObject dontAddMe = null;
		Renderer r = MB_Utility.GetRenderer(mom.gameObject);
		if (r != null){ //make sure that this MeshBaker object is not in list
			dontAddMe = r.gameObject;	
		}
		
		int numAdded = 0;
		List<GameObject> momObjs = mom.GetObjectsToCombine();
		List<GameObject> newMomObjs = new List<GameObject>();
		GameObject[] gos = Selection.gameObjects;
		if (gos.Length == 0){
			Debug.LogWarning("No objects selected in hierarchy view. Nothing added.");	
		}
		
		for (int i = 0; i < gos.Length; i++){
			GameObject go = gos[i];
			Renderer[] mrs = go.GetComponentsInChildren<Renderer>();
			for (int j = 0; j < mrs.Length; j++){
				
				if (!momObjs.Contains(mrs[j].gameObject) && !newMomObjs.Contains(mrs[j].gameObject)){
					bool addMe = true;
					if (!mrs[j].gameObject.isStatic && onlyStaticObjects){
						addMe = false;
					}

					Mesh mm = MB_Utility.GetMesh(mrs[j].gameObject);
					if (mm != null){
						Rect dummy = new Rect();
						if (MB_Utility.hasOutOfBoundsUVs(mm, ref dummy)){
							Debug.LogWarning("Object " + mrs[j].gameObject.name + " uses uvs that are outside the range (0,1)" +
								"this object can only be combined with other objects that use the exact same set of source textures " +
								" this object has not been added. You will have to add it manually");
							if (shaderMat != null){
								addMe = false;
							}
						}
					}					
					
					if (shaderMat != null){
						Material[] nMats = mrs[j].sharedMaterials;
						bool usesShader = false;
						foreach(Material nMat in nMats){
							if (nMat != null && nMat.shader == shaderMat.shader){
								usesShader = true;	
							}
						}
						if (!usesShader){
							addMe = false;	
						}
					}
					
					if (mat != null){
						Material[] nMats = mrs[j].sharedMaterials;
						bool usesMat = false;
						foreach(Material nMat in nMats){
							if (nMat == mat){
								usesMat = true;
							}
						}
						if (!usesMat){
							addMe = false;
						}
					}		
								
					if (addMe && mrs[j].gameObject != dontAddMe){
						if (!newMomObjs.Contains(mrs[j].gameObject)){
							newMomObjs.Add(mrs[j].gameObject);
						}
						numAdded++;
					}	
				}
			}
		}
		
		Undo.RegisterUndo(mom, "Add Objects");
		for (int i = 0; i < newMomObjs.Count;i++){
			mom.GetObjectsToCombine().Add(newMomObjs[i]);
		}
		SerializedObject so = new SerializedObject(mom);
		so.SetIsDifferentCacheDirty();
		
		if (numAdded == 0){
			Debug.LogWarning("Added 0 objects. Make sure some or all objects are selected in the hierarchy view. Also check ths 'Only Static Objects' and 'Only Objects Using Shader' settings");
		} else {
			Debug.Log("Added " + numAdded + " objects to " + mom.name);
		}
		
	}
	
	public class _GameObjectAndWarning {
		public GameObject go;
		public string warning;
		public _GameObjectAndWarning(GameObject g, string w){
			go = g;
			warning = w;
		}
	}
	
	void listMaterialsInScene(){
			Dictionary<Shader,List<_GameObjectAndWarning>> shader2GameObjects = new Dictionary<Shader, List<_GameObjectAndWarning>>();
			Renderer[] rs = (Renderer[]) FindObjectsOfType(typeof(Renderer));
			for (int i = 0; i < rs.Length; i++){
				Renderer r = rs[i];
				Material[] mms = r.sharedMaterials;
				List<_GameObjectAndWarning> gos;
			
				foreach (Material mm in mms){
					string warn = "";
					if (shader2GameObjects.ContainsKey(mm.shader)){
						gos = shader2GameObjects[mm.shader];
					} else {
						gos = new List<_GameObjectAndWarning>();
						shader2GameObjects.Add(mm.shader,gos);
					}
					//todo add warning for texture scaling
					if (r.sharedMaterials.Length > 1){
						warn += " [Uses multiple materials] ";
					}
				
					if (gos.Find(x => x.go == r.gameObject) == null){
						gos.Add(new _GameObjectAndWarning(r.gameObject,warn));
					}
				}
			}
			string outStr = "(Click me, if I am too big copy and paste me into a text editor) Materials in scene " + shader2GameObjects.Keys.Count + " and the objects that use them:\n";
			foreach(Shader m in shader2GameObjects.Keys){
				int totalVerts = 0;
				string outStr2 = ""; 
				List<_GameObjectAndWarning> gos = shader2GameObjects[m];
				for (int i = 0; i < gos.Count; i++){
					Mesh mm = MB_Utility.GetMesh(gos[i].go);
					int nVerts = 0;
					if (mm != null){
						nVerts += mm.vertexCount;
						Rect dummy = new Rect();
						if (MB_Utility.hasOutOfBoundsUVs(mm,ref dummy)){
							int w = (int) dummy.width;
							int h = (int) dummy.height;
							gos[i].warning += " [WARNING: has uvs outside the range (0,1) tex is scaled " + w + "x" + h + " times]";
						}
						if (MB_Utility.doSubmeshesShareVertsOrTris(mm) != 0){
							gos[i].warning += " [WARNING: Submeshes share verts or triangles]";
						}					
					}
					totalVerts += nVerts;
					Renderer mr = gos[i].go.GetComponent<Renderer>();
					string matStr = "";
					if (!MB_Utility.validateOBuvsMultiMaterial(mr.sharedMaterials)){
						gos[i].warning += " [WARNING: Object uses same material on multiple submeshes. This may produce poor results when used with multiple materials or fix out of bounds uvs.]";
					}
					foreach(Material mmm in mr.sharedMaterials){
						if (mmm.shader == m){
							matStr += "[" + mmm + "] ";
						}
					}
					outStr2 += "\t\t" + gos[i].go.name + " (" + nVerts + " verts) " + matStr + gos[i].warning + "\n";	
				}
				outStr2 = "\t" + m.name + " (" + totalVerts + " verts): \n" + outStr2;
				outStr += outStr2;
			}
			Debug.Log(outStr);		
	}

}