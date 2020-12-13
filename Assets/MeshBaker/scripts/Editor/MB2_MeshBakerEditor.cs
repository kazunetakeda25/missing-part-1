//----------------------------------------------
//            MeshBaker
// Copyright © 2011-2012 Ian Deane
//----------------------------------------------
using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEditor;

[CustomEditor(typeof(MB2_MeshBaker))]
public class MB2_MeshBakerEditor : Editor {
	MB2_MeshBakerEditorInternal mbe = new MB2_MeshBakerEditorInternal();
	[MenuItem("GameObject/Create Other/Mesh Baker/Mesh And Material Baker")]
	public static void CreateNewMeshBaker(){
		MB2_MeshBakerEditorInternal.CreateNewMeshBaker();
	}
	
//	void OnEnable () {
//		mbe.OnEnable((MB2_MeshBaker) target);
//	}
	
	public override void OnInspectorGUI(){
		mbe.OnInspectorGUI((MB2_MeshBaker) target);
	}
	
	
	
}

public class MB2_MeshBakerEditorInternal{
	//add option to exclude skinned mesh renderer and mesh renderer in filter
	//example scenes for multi material
	private static GUIContent
		outputOptoinsGUIContent = new GUIContent("Output", ""),
		openToolsWindowLabelContent = new GUIContent("Open Tools For Adding Objects", "Use these tools to find out what can be combined, discover problems with meshes, and quickly add objects."),
		renderTypeGUIContent = new GUIContent("Renderer","The type of renderer to add to the combined mesh."),
		objectsToCombineGUIContent = new GUIContent("Custom List Of Objects To Be Combined","You can add objects here that were not on the list in the MB2_TextureBaker as long as they use a material that is in the Material Bake Results"),
		textureBakeResultsGUIContent = new GUIContent("Material Bake Result","When materials are combined a MB2_TextureBakeResult Asset is generated. Drag that Asset to this field to use the combined material."),
		useTextureBakerObjsGUIContent = new GUIContent("Same As Texture Baker","Build a combined mesh using using the same list of objects that generated the Combined Material"),
		lightmappingOptionGUIContent = new GUIContent("Lightmapping UVs","preserve current lightmapping: Use this if all source objects are lightmapped and you want to preserve it. All source objects must use the same lightmap\n\n"+
																		 "generate new UV Layout: Use this if you want to bake a lightmap after the combined mesh has been generated\n\n" +
																		 "copy UV2 unchanged: Use this if UV2 is being used for something other than lightmaping.\n\n" + 
																		 "ignore UV2: A UV2 channel will not be generated for the combined mesh"),
		doNormGUIContent = new GUIContent("Include Normals"),
		doTanGUIContent = new GUIContent("Include Tangents"),
		doColGUIContent = new GUIContent("Include Colors"),
		doUVGUIContent = new GUIContent("Include UV"),
		doUV1GUIContent = new GUIContent("Include UV1");
	
	private SerializedObject meshBaker;
	private SerializedProperty  lightmappingOption, outputOptions, textureBakeResults, useObjsToMeshFromTexBaker, renderType, fixOutOfBoundsUVs, objsToMesh;
	private SerializedProperty doNorm, doTan, doUV, doUV1, doCol;
	bool showInstructions = false;
	
	public static void CreateNewMeshBaker(){
		MB2_MeshBaker[] mbs = (MB2_MeshBaker[]) Editor.FindObjectsOfType(typeof(MB2_MeshBaker));
    	Regex regex = new Regex(@"(\d+)$", RegexOptions.Compiled | RegexOptions.CultureInvariant);
		int largest = 0;
		try{
			for (int i = 0; i < mbs.Length; i++){
				Match match = regex.Match(mbs[i].name);
				if (match.Success){
					int val = Convert.ToInt32(match.Groups[1].Value);
					if (val >= largest)
						largest = val + 1;
				}
			}
		} catch(Exception e){
			if (e == null) e = null; //Do nothing supress compiler warning
		}
		GameObject nmb = new GameObject("MeshBaker" + largest);
		nmb.transform.position = Vector3.zero;
		nmb.AddComponent<MB2_TextureBaker>();
		nmb.AddComponent<MB2_MeshBaker>();
	}

	void _init (MB2_MeshBaker target) {
		meshBaker = new SerializedObject(target);
		outputOptions = meshBaker.FindProperty("outputOption");
		objsToMesh = meshBaker.FindProperty("objsToMesh");
		renderType = meshBaker.FindProperty("renderType");
		useObjsToMeshFromTexBaker = meshBaker.FindProperty("useObjsToMeshFromTexBaker");
		textureBakeResults = meshBaker.FindProperty("textureBakeResults");
		lightmappingOption = meshBaker.FindProperty("lightmapOption");
		doNorm = meshBaker.FindProperty("doNorm");
		doTan = meshBaker.FindProperty("doTan");
		doUV = meshBaker.FindProperty("doUV");
		doUV1 = meshBaker.FindProperty("doUV1");
		doCol = meshBaker.FindProperty("doCol");
	}	
	
	public void OnInspectorGUI(MB2_MeshBaker target){
		DrawGUI(target);
	}
	
	public void DrawGUI(MB2_MeshBaker target){
		if (meshBaker == null){
			_init(target);
		}
		
		meshBaker.Update();

		showInstructions = EditorGUILayout.Foldout(showInstructions,"Instructions:");
		if (showInstructions){
			EditorGUILayout.HelpBox("1. Bake combined material(s).\n\n" +
									"2. If necessary set the 'Material Bake Results' field.\n\n" +
									"3. Add scene objects or prefabs to combine or check 'Same As Texture Baker'. For best results these should use the same shader as result material.\n\n" +
									"4. Select options and 'Bake'.\n\n" +
									"6. Look at warnings/errors in console. Decide if action needs to be taken.\n\n" +
									"7. (optional) Disable renderers in source objects.", UnityEditor.MessageType.None);
			
			EditorGUILayout.Separator();
		}				
		
		MB2_MeshBaker mom = (MB2_MeshBaker) target;
		
		EditorGUILayout.PropertyField(textureBakeResults, textureBakeResultsGUIContent);
		
		EditorGUILayout.LabelField("Objects To Be Combined",EditorStyles.boldLabel);	
		if (mom.GetComponent<MB2_TextureBaker>() != null){
			EditorGUILayout.PropertyField(useObjsToMeshFromTexBaker, useTextureBakerObjsGUIContent);
		} else {
			useObjsToMeshFromTexBaker.boolValue = false;
		}
		
		if (!mom.useObjsToMeshFromTexBaker){
			
			if (GUILayout.Button(openToolsWindowLabelContent)){
				MB_MeshBakerEditorWindow mmWin = (MB_MeshBakerEditorWindow) EditorWindow.GetWindow(typeof(MB_MeshBakerEditorWindow));
				mmWin.target = (MB2_MeshBakerRoot) target;
			}	
			EditorGUILayout.PropertyField(objsToMesh,objectsToCombineGUIContent, true);
		}
		
		EditorGUILayout.LabelField("Output",EditorStyles.boldLabel);
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PropertyField(doNorm,doNormGUIContent);
		EditorGUILayout.PropertyField(doTan,doTanGUIContent);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PropertyField(doUV,doUVGUIContent);
		EditorGUILayout.PropertyField(doUV1,doUV1GUIContent);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.PropertyField(doCol,doColGUIContent);	
		
		EditorGUILayout.PropertyField(lightmappingOption,lightmappingOptionGUIContent);
		
		EditorGUILayout.PropertyField(outputOptions,outputOptoinsGUIContent);
		EditorGUILayout.PropertyField(renderType, renderTypeGUIContent);
		if (mom.outputOption == MB2_OutputOptions.bakeIntoSceneObject){
			mom.resultSceneObject = (GameObject) EditorGUILayout.ObjectField("Combined Mesh Object", mom.resultSceneObject, typeof(GameObject), true);
		} else if (mom.outputOption == MB2_OutputOptions.bakeIntoPrefab){
			mom.resultPrefab = (GameObject) EditorGUILayout.ObjectField("Combined Mesh Prefab", mom.resultPrefab, typeof(GameObject), true);			
		}
		
		if (GUILayout.Button("Bake")){
			bake(mom);
		}

		string enableRenderersLabel;
		if (mom.disableSourceRenderers){
			enableRenderersLabel = "Disable Renderers On Combined Objects";
		} else {
			enableRenderersLabel = "Enable Renderers On Combined Objects";
		}
		if (GUILayout.Button(enableRenderersLabel)){
			mom.disableSourceRenderers = !mom.disableSourceRenderers;
			mom.EnableDisableSourceObjectRenderers(mom.disableSourceRenderers);
		}	
		
		meshBaker.ApplyModifiedProperties();		
		meshBaker.SetIsDifferentCacheDirty();
	}
		
	public void updateProgressBar(string msg, float progress){
		EditorUtility.DisplayProgressBar("Combining Meshes", msg, progress);
	}
		
	void bake(MB2_MeshBaker mom){
		try{
			//MB2_MeshBaker mom = (MB2_MeshBaker) target;
			if (mom.outputOption == MB2_OutputOptions.bakeIntoSceneObject){
				_bakeIntoCombined(mom, MB_OutputOptions.bakeIntoSceneObject);
			} else if (mom.outputOption == MB2_OutputOptions.bakeIntoPrefab){
				_bakeIntoCombined(mom, MB_OutputOptions.bakeIntoPrefab);
			} else {
				_bakeMeshesInPlace(mom);
			}
		} catch(Exception e){
			Debug.LogError(e);	
		} finally {
			EditorUtility.ClearProgressBar();
		}
	}
	
	void _bakeIntoCombined(MB2_MeshBaker mom, MB_OutputOptions prefabOrSceneObject){
		if (prefabOrSceneObject != MB_OutputOptions.bakeIntoPrefab && prefabOrSceneObject != MB_OutputOptions.bakeIntoSceneObject){
			Debug.LogError("Paramater prefabOrSceneObject must be bakeIntoPrefab or bakeIntoSceneObject");
			return;
		}
		//MB2_MeshBaker mom = (MB2_MeshBaker) target;
		MB2_TextureBaker tb = mom.GetComponent<MB2_TextureBaker>();
		if (mom.textureBakeResults == null && tb != null){
			Debug.Log("setting results");
			mom.textureBakeResults = tb.textureBakeResults;	
		}

		if (mom.useObjsToMeshFromTexBaker){
			if (tb != null){
				mom.objsToMesh.Clear();
				mom.objsToMesh.AddRange(tb.objsToMesh);
			}	
		}
		
		Mesh mesh;
		if (!MB_Utility.doCombinedValidate(mom, MB_ObjsToCombineTypes.sceneObjOnly)) return;	
		if (prefabOrSceneObject == MB_OutputOptions.bakeIntoPrefab && mom.resultPrefab == null){
			Debug.LogError("Need to set the Combined Mesh Prefab field.");
			return;
		}
		mom.ClearMesh();
		
		mesh = mom.AddDeleteGameObjects(mom.objsToMesh.ToArray(),null,false, true);
		if (mesh != null){
			mom.ApplyAll();
			updateProgressBar("Created mesh saving assets",.6f);
			if (prefabOrSceneObject == MB_OutputOptions.bakeIntoSceneObject){
				mom.BuildSceneMeshObject();
			} else if (prefabOrSceneObject == MB_OutputOptions.bakeIntoPrefab){
				saveMeshToAssetDatabase(mom, mesh);
				if (mom.renderType == MB_RenderType.skinnedMeshRenderer){
					Debug.LogWarning("Prefab will not be updated for skinned mesh. This is because all bones need to be included in the prefab for it to be usefull.");	
				} else {
					rebuildPrefab(mom, mesh);
				}
			} else {
				Debug.LogError("Unknown parameter");
			}
		}
		
	}	
	
	void _bakeMeshesInPlace(MB2_MeshBaker mom){
		Mesh mesh;
		//MB2_MeshBaker mom = (MB2_MeshBaker) target;
		if (!MB_Utility.doCombinedValidate(mom, MB_ObjsToCombineTypes.prefabOnly)) return;
		mom.DestroyMesh();

		List<GameObject> objsToMesh = mom.objsToMesh;
		if (mom.useObjsToMeshFromTexBaker && mom.GetComponent<MB2_TextureBaker>() != null){
			objsToMesh = mom.GetComponent<MB2_TextureBaker>().objsToMesh;
		}

		GameObject[] objs = new GameObject[1];
		List<string> usedNames = new List<string>();
		MB_RenderType originalRenderType = mom.renderType;
		for (int i = 0; i < objsToMesh.Count; i++){
			objs[0] = objsToMesh[i];
			Renderer r = MB_Utility.GetRenderer(objsToMesh[i]);
			if (r is SkinnedMeshRenderer){
				mom.renderType = MB_RenderType.skinnedMeshRenderer;	
			} else {
				mom.renderType = MB_RenderType.meshRenderer;
			}
			mesh = mom.AddDeleteGameObjects(objs,null,false);
			if (mesh != null){
				mom.ApplyAll();
				Mesh mf = MB_Utility.GetMesh(objs[0]);
				if (mf != null){
					string baseName, folderPath, newFilename;
					string pth = AssetDatabase.GetAssetPath(mf);
					if (pth != null && pth.Length != 0){
						baseName = Path.GetFileNameWithoutExtension(pth) + "_" + objs[0].name + "_MB";
						folderPath = Path.GetDirectoryName(pth); 		
					} else { //try to get the name from prefab
						pth = AssetDatabase.GetAssetPath(objs[0]); //get prefab name
						if (pth != null && pth.Length != 0){
							baseName = Path.GetFileNameWithoutExtension(pth) + "_" + objs[0].name + "_MB";
							folderPath = Path.GetDirectoryName(pth);		
						} else { //save in root
							baseName = objs[0].name + "mesh_MB";
							folderPath = "Assets";
						}
					}
					//make name unique
					newFilename = Path.Combine(folderPath, baseName + ".asset");
					int j = 0;
					while(usedNames.Contains(newFilename)){
						newFilename = Path.Combine(folderPath, baseName + j + ".asset");
						j++;
					}
					usedNames.Add(newFilename);
					updateProgressBar("Created mesh saving mesh on " + objs[0].name + " to asset " + newFilename,.6f);
					if (newFilename != null && newFilename.Length != 0){
						Debug.Log("Creating mesh for " + objs[0].name + " with adjusted UVs at: " + newFilename);
						AssetDatabase.CreateAsset(mesh,  newFilename);
					} else {
						Debug.LogWarning("Could not save mesh for " + objs[0].name);	
					}
				}
			}
			mom.DestroyMesh();
		}
		mom.renderType = originalRenderType;
		return;		
	}
		
	void saveMeshToAssetDatabase(MB2_MeshBaker mom, Mesh mesh){
		//MB2_MeshBaker mom = (MB2_MeshBaker) target;
		string prefabPth = AssetDatabase.GetAssetPath(mom.resultPrefab);
		if (prefabPth == null || prefabPth.Length == 0){
			Debug.LogError("Could not save result to prefab. Result Prefab value is not an Asset.");
			return;
		}
		string baseName = Path.GetFileNameWithoutExtension(prefabPth);
		string folderPath = prefabPth.Substring(0,prefabPth.Length - baseName.Length - 7);		
		string newFilename = folderPath + baseName + "-mesh.asset";
		//check if mesh is already an asset
		string ap = AssetDatabase.GetAssetPath(mesh);
		if (ap == null || ap.Equals("")){
			Debug.Log("Saving mesh asset to " + newFilename);
			AssetDatabase.CreateAsset(mesh, newFilename);
		} else {
			Debug.Log("Mesh is an asset at " + ap);	
		}
	}
	
	void createNewPrefab(MB2_MeshBaker mom, string pth){
		//MB2_MeshBaker mom = (MB2_MeshBaker) target;
		string baseName = Path.GetFileNameWithoutExtension(pth);
		string folderPath = pth.Substring(0,pth.Length - baseName.Length - 7);
		
		List<string> matNames = new List<string>();
		if (mom.textureBakeResults.doMultiMaterial){
			for (int i = 0; i < mom.textureBakeResults.resultMaterials.Length; i++){
				matNames.Add( folderPath +  baseName + "-mat" + i + ".mat" );
				AssetDatabase.CreateAsset(new Material(Shader.Find("Diffuse")), matNames[i]);
				mom.textureBakeResults.resultMaterials[i].combinedMaterial = (Material) AssetDatabase.LoadAssetAtPath(matNames[i],typeof(Material));
			}
		}else{
			matNames.Add( folderPath +  baseName + "-mat.mat" );
			AssetDatabase.CreateAsset(new Material(Shader.Find("Diffuse")), matNames[0]);
			mom.textureBakeResults.resultMaterial = (Material) AssetDatabase.LoadAssetAtPath(matNames[0],typeof(Material));
		}
		//create the prefab
		UnityEngine.Object prefabAsset = PrefabUtility.CreateEmptyPrefab(pth);
		GameObject rootGO = new GameObject("combinedMesh-" + mom.name);
		PrefabUtility.ReplacePrefab(rootGO,prefabAsset,ReplacePrefabOptions.ConnectToPrefab);
		Editor.DestroyImmediate(rootGO);
		
		mom.resultPrefab = (GameObject) AssetDatabase.LoadAssetAtPath(pth,typeof(GameObject));
		
		AssetDatabase.Refresh();
	}

	void rebuildPrefab(MB2_MeshBaker mom, Mesh mesh){
		//MB2_MeshBaker mom = (MB2_MeshBaker) target;
		if (mom.resultPrefab == null){
			Debug.LogError("Prefab to store result did not exist.");	
		}
		GameObject rootGO = (GameObject) PrefabUtility.InstantiatePrefab(mom.resultPrefab);
		MB_Utility.buildSceneMeshObject(rootGO, mom, mesh);
		
		string prefabPth = AssetDatabase.GetAssetPath(mom.resultPrefab);
		PrefabUtility.ReplacePrefab(rootGO,AssetDatabase.LoadAssetAtPath(prefabPth,typeof(GameObject)),ReplacePrefabOptions.ConnectToPrefab);
		mom.resultPrefab = (GameObject) AssetDatabase.LoadAssetAtPath(prefabPth, typeof(GameObject));	
		Editor.DestroyImmediate(rootGO);
	}	
	
}
