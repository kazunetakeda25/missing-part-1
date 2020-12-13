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

[CustomEditor(typeof(MB_MeshBaker))]
public class MB_MeshBakerDynamicEditor : Editor {
	
	//todo make tile texture bake size a field
	//todo bakeInPlace validate should check that objs to add are prefabs
	//todo check is prefab for bake textures only and normal baking
	
	private static GUIContent insertContent = new GUIContent("+", "add a material");
	private static GUIContent deleteContent = new GUIContent("-", "delete a material");
	private static GUILayoutOption buttonWidth = GUILayout.MaxWidth(20f);
	
	private static GUIContent
		//bakeMeshesInPlaceLabelContent = new GUIContent("Bake Meshes In Place","Saves copies of meshes individually with adusted uvs. New meshes can all share 'combined mesh material' and be used with Unity's static/dynamic batching. Does not update 'combined mesh prefab'. You need to replace MeshFilter meshes with new meshes yourself."),
		//bakeTextureAtlasesOnlyLabelContent = new GUIContent("Bake Texture Atlases Only","Creates atlases, updates 'combined mesh material' to use atlases. Prepares this MeshBaker object for runtime use. Objects to Combine instances can by added at runtime after this has been run."),
		//bakeBakeMeshesIntoPrefabLabelContent = new GUIContent("Bake Meshes Into Prefab","Creates atlases, updates 'combined mesh material' to use atlases. Creates a combined mesh asset. Updates 'combined mesh prefab' to use 'combined mesh material' and created mesh."),
		createPrefabAndMaterialLabelContent = new GUIContent("Create Empty Prefab & Material For Combined", "Creates a material asset and a prefab asset. You should set the shader on the material. Mesh Baker uses the Texture properties on the material to decide what atlases need to be created."),
		openToolsWindowLabelContent = new GUIContent("Open Tools For Adding Objects", "Use these tools to find out what can be combined and quickly add objects."),
		outputOptionGUIContent = new GUIContent("Output", "BakeIntoPrefab: Texture atlases are generated and applied to result materials. Objects to Be Combined are combined into a single mesh which is added to Combined Prefab\n\n"+
														  "BakeInPlace: Texture atlases are generated and applied to result materials. Objects To Be Combined meshes are duplicated, have uvs adusted and are saved. You will need to add generated meshes and combined materials to objects manually\n\n" +
														  "BakeTextureAtlasesOnly: Texture atlases are generated. This is prepares materials to be used with the runtime API\n\n"),
		fixOutOfBoundsGUIContent = new GUIContent("Fix Out-Of-Bounds UVs", "If mesh has uvs outside the range 0,1 uvs will be scaled so they are in 0..1 range. Textures will have tiling baked."),
		resizePowerOfTwoGUIContent = new GUIContent("Resize Power-Of-Two Textures", "Shrinks textures so they have a clear border of width 'Atlas Padding' around them. Improves texture packing efficiency."),
		customShaderPropertyNamesGUIContent = new GUIContent("Custom Shader Propert Names", "Mesh Baker has a list of common texture properties that it looks for in shaders to generate atlases. Custom shaders may have texture properties not on this list. Add them here and Meshbaker will generate atlases for them."),
		combinedMaterialsGUIContent = new GUIContent("Combined Materials", "Use the +/- buttons to add multiple combined materials. You will also need to specify which materials on the source objects map to each combined material.");
	
	private SerializedObject meshBaker;
	private SerializedProperty outputOption, doMultiMaterial, fixOutOfBoundsUVs, resultMaterial, resultMaterials, atlasPadding, resizePowerOfTwoTextures, customShaderPropNames, objsToMesh;
	
	bool resultMaterialsFoldout = true;
	bool showInstructions = true;
	
	/*
	[MenuItem("GameObject/Create Other/Mesh Baker/MeshBaker")]
	public static void CreateNewMeshBaker(){
		MB_MeshBaker[] mbs = (MB_MeshBaker[]) FindObjectsOfType(typeof(MB_MeshBaker));
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
		nmb.AddComponent<MB_MeshBaker>();
	}
	*/

	void OnEnable () {
		meshBaker = new SerializedObject(target);
		doMultiMaterial = meshBaker.FindProperty("doMultiMaterial");
		fixOutOfBoundsUVs = meshBaker.FindProperty("fixOutOfBoundsUVs");
		resultMaterial = meshBaker.FindProperty("resultMaterial");
		resultMaterials = meshBaker.FindProperty("resultMaterials");
		atlasPadding = meshBaker.FindProperty("atlasPadding");
		resizePowerOfTwoTextures = meshBaker.FindProperty("resizePowerOfTwoTextures");
		customShaderPropNames = meshBaker.FindProperty("customShaderPropNames");
		objsToMesh = meshBaker.FindProperty("objsToMesh");
		outputOption = meshBaker.FindProperty("outputOption");
	}	
	
	public override void OnInspectorGUI(){
		meshBaker.Update();

		showInstructions = EditorGUILayout.Foldout(showInstructions,"Instructions:");
		if (showInstructions){
			EditorGUILayout.HelpBox("1. Create empty assets for result materials and (if needed) result prefab.\n\n" +
									"2. Select shader on result materials.\n\n" +
									"3. Add scene objects or prefabs to combine. For best results these should use the same shader as result material.\n\n" +
									"4. Select an output option.\n\n" +
									"5. Bake objects.\n\n" +
									"6. Look at warnings/errors in console. Decide if action needs to be taken.\n\n" +
									"5. Use the combined/adjusted meshes.\n\n" +
									"7. (optional) Disable renderers in source objects.\n\n" +
									"8. (optional) Remove the MeshBaker object. You may want to keep it for easy re-baking if something changes in the source objects.", UnityEditor.MessageType.None);
			
			EditorGUILayout.Separator();
		}				
		
		MB_MeshBaker mom = (MB_MeshBaker) target;
		
		EditorGUILayout.LabelField("Options",EditorStyles.boldLabel);		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PropertyField(fixOutOfBoundsUVs,fixOutOfBoundsGUIContent);
		EditorGUILayout.PropertyField(doMultiMaterial,new GUIContent("Multiple Combined Materials"));
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.PropertyField(atlasPadding,new GUIContent("Atlas Padding"));
		EditorGUILayout.PropertyField(resizePowerOfTwoTextures, resizePowerOfTwoGUIContent);
		EditorGUILayout.PropertyField(customShaderPropNames,customShaderPropertyNamesGUIContent,true);
		
		if (mom.doMultiMaterial){
			MB_EditorUtil.DrawSeparator();
			EditorGUILayout.LabelField("Source Material To Combined Mapping",EditorStyles.boldLabel);
			EditorGUILayout.BeginHorizontal();
			resultMaterialsFoldout = EditorGUILayout.Foldout(resultMaterialsFoldout, combinedMaterialsGUIContent);
			if(GUILayout.Button(insertContent, EditorStyles.miniButtonLeft, buttonWidth)){
				if (resultMaterials.arraySize == 0){
					mom.resultMaterials = new MB_MultiMaterial[1];	
				} else {
					resultMaterials.InsertArrayElementAtIndex(resultMaterials.arraySize-1);
				}
			}
			if(GUILayout.Button(deleteContent, EditorStyles.miniButtonRight, buttonWidth)){
				resultMaterials.DeleteArrayElementAtIndex(resultMaterials.arraySize-1);
			}			
			EditorGUILayout.EndHorizontal();
			if (resultMaterialsFoldout){
				for(int i = 0; i < resultMaterials.arraySize; i++){
					EditorGUILayout.Separator();
					EditorGUILayout.LabelField("---------- submesh:" + i);
					EditorGUILayout.Separator();
					SerializedProperty resMat = resultMaterials.GetArrayElementAtIndex(i);
					EditorGUILayout.PropertyField(resMat.FindPropertyRelative("combinedMaterial"));
					SerializedProperty sourceMats = resMat.FindPropertyRelative("sourceMaterials");
					EditorGUILayout.PropertyField(sourceMats,true);
				}
			}
			
		} else {
			MB_EditorUtil.DrawSeparator();
			EditorGUILayout.LabelField("Combined Material",EditorStyles.boldLabel);			
			EditorGUILayout.PropertyField(resultMaterial,new GUIContent("Combined Mesh Material"));
		}
				
		MB_EditorUtil.DrawSeparator();
		EditorGUILayout.LabelField("Objects To Be Combined",EditorStyles.boldLabel);	
		if (GUILayout.Button(openToolsWindowLabelContent)){
			MB_MeshBakerEditorWindow mmWin = (MB_MeshBakerEditorWindow) EditorWindow.GetWindow(typeof(MB_MeshBakerEditorWindow));
			mmWin.target = (MB_MeshBaker) target;
		}	
		EditorGUILayout.PropertyField(objsToMesh,new GUIContent("Objects To Be Combined"), true);
		MB_EditorUtil.DrawSeparator();
		
		EditorGUILayout.Separator();
		mom.resultPrefab = (GameObject) EditorGUILayout.ObjectField("Combined Mesh Prefab", mom.resultPrefab, typeof(GameObject), false);								
		EditorGUILayout.PropertyField(outputOption, outputOptionGUIContent);
		if (GUILayout.Button("Bake")){
			if (mom.outputOption == MB_OutputOptions.bakeIntoPrefab){
				bakeMeshesIntoPrefab();
			} else if (mom.outputOption == MB_OutputOptions.bakeMeshsInPlace){
				bakeMeshesInPlace();
			} else if (mom.outputOption == MB_OutputOptions.bakeTextureAtlasesOnly){
				bakeTexturesOnly();
			}
		}
		EditorGUILayout.Separator();
			

		MB_EditorUtil.DrawSeparator();
		EditorGUILayout.LabelField("Utilities",EditorStyles.boldLabel);
		if (GUILayout.Button(createPrefabAndMaterialLabelContent)){
			string newPrefabPath = EditorUtility.SaveFilePanelInProject("Prefab name", "", "prefab", "Enter a name for the combined mesh prefab");
			if (newPrefabPath != null){
				createNewPrefab(newPrefabPath);
			}
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
	}
		
	public void updateProgressBar(string msg, float progress){
		EditorUtility.DisplayProgressBar("Combining Meshes", msg, progress);
	}
	
	void bakeTexturesOnly(){
		if (!doCombinedValidate(MB_ObjsToCombineTypes.prefabOnly)) return;
		
		MB_AtlasesAndRects[] mAndAs = null;
		try{
			MB_MeshBaker mbd = (MB_MeshBaker) target;
			mAndAs = mbd.CreateAtlases(updateProgressBar);
			if (mAndAs != null){
				for(int i = 0; i < mAndAs.Length; i++){
					MB_AtlasesAndRects mAndA = mAndAs[i];
					updateProgressBar("Created mesh saving assets",.6f);
					Material resMat = mbd.resultMaterial;
					if (mbd.doMultiMaterial) resMat = mbd.resultMaterials[i].combinedMaterial;
					saveAtlasesToAssetDatabase(mAndA, resMat);
					meshBaker.SetIsDifferentCacheDirty();
					updateProgressBar("Replacing prefab",.7f);
				}
			}
		} catch(Exception e){
			Debug.LogError(e);	
		} finally {
			EditorUtility.ClearProgressBar();
			if (mAndAs != null){
				for(int j = 0; j < mAndAs.Length; j++){
					MB_AtlasesAndRects mAndA = mAndAs[j];
					if (mAndA != null && mAndA.atlases != null){
						for (int i = 0; i < mAndA.atlases.Length; i++){
							if (mAndA.atlases[i] != null){
								MB_Utility.Destroy(mAndA.atlases[i]);
							}
						}
					}
				}
			}
		}		
	}
	
	enum MB_ObjsToCombineTypes{
		prefabOnly,
		sceneObjOnly,
		dontCare
	}
	
	bool doCombinedValidate(MB_ObjsToCombineTypes objToCombineType){
		MB_MeshBaker mom = (MB_MeshBaker) target;
		
		for (int i = 0; i < mom.objsToMesh.Count; i++){
			if (mom.objsToMesh[i] == null){
				Debug.LogError("The list of objects to combine contains nulls.");
				return false;					
			}
			for (int j = i + 1; j < mom.objsToMesh.Count; j++){
				if (mom.objsToMesh[i] == mom.objsToMesh[j]){
					Debug.LogError("The list of objects to combine contains duplicates.");
					return false;	
				}
			}
		}
		
		if (mom.doMultiMaterial){
			if (!validateMultipleMaterials()){
				return false;			
			}
			if (!validateSubmeshOverlap()){//only warns currently
				return false;
			}
		} else {
			if (mom.resultMaterial == null){
				Debug.LogError("Please assign a result material. The combined mesh will use this material.");
				return false;			
			}
		}
		if (mom.objsToMesh == null || mom.objsToMesh.Count == 0){
			Debug.LogError("No meshes to combine. Please assign some meshes to combine.");
			return false;
		}
		if (mom.atlasPadding < 0){
			Debug.LogError("Atlas padding must be zero or greater.");
			return false;
		}
		
		for (int i = 0; i < mom.objsToMesh.Count; i++){
			UnityEditor.PrefabType pt = UnityEditor.PrefabUtility.GetPrefabType(mom.objsToMesh[i]);
			if (pt == UnityEditor.PrefabType.None ||
				pt == UnityEditor.PrefabType.PrefabInstance || 
				pt == UnityEditor.PrefabType.ModelPrefabInstance || 
				pt == UnityEditor.PrefabType.DisconnectedPrefabInstance ||
				pt == UnityEditor.PrefabType.DisconnectedModelPrefabInstance){
				// these are scene objects
				if (objToCombineType == MB_ObjsToCombineTypes.prefabOnly){
					Debug.LogWarning("The list of objects to combine contains scene objects. You probably want prefabs." + mom.objsToMesh[i] + " is a scene object");	
				}
			} else if (objToCombineType == MB_ObjsToCombineTypes.sceneObjOnly){
				//these are prefabs
				Debug.LogError("The list of objects to combine contains prefab objects. You probably want scene objects." + mom.objsToMesh[i] + " is a prefab object");
			}
		}
		return true;
	}
	
	void bakeMeshesIntoPrefab(){
		MB_MeshBaker mom = (MB_MeshBaker) target;
		MB_AtlasesAndRects[] mAndAs = null;
		Mesh mesh;
		if (!doCombinedValidate(MB_ObjsToCombineTypes.sceneObjOnly)) return;	
		if (mom.resultPrefab == null){
			Debug.LogError("Need to set the result prefab field.");
			return;
		}
		try{
			mom.DestroyMesh();
			mAndAs = mom.CreateAtlases(updateProgressBar);
			if (mAndAs != null){
				for(int i = 0; i < mAndAs.Length; i++){
					MB_AtlasesAndRects mAndA = mAndAs[i];
					Material resMat = mom.resultMaterial;
					if (mom.doMultiMaterial) resMat = mom.resultMaterials[i].combinedMaterial;
					saveAtlasesToAssetDatabase(mAndA,resMat);
				}
				mesh = mom.AddDeleteGameObjects(mom.objsToMesh.ToArray(),null,false, true);
				if (mesh != null){
					mom.ApplyAll();
					updateProgressBar("Created mesh saving assets",.6f);
					saveMeshToAssetDatabase(mesh);
					updateProgressBar("Replacing prefab",.7f);
					rebuildPrefab(mAndAs,mesh);
				}
			}
		} catch(Exception e){
			Debug.LogError(e);	
		} finally {
			EditorUtility.ClearProgressBar();
			mom.DestroyMesh();
			if (mAndAs != null){
				for(int j = 0; j < mAndAs.Length; j++){
					MB_AtlasesAndRects mAndA = mAndAs[j];			
					if (mAndA != null && mAndA.atlases != null){
						for (int i = 0; i < mAndA.atlases.Length; i++){
							if (mAndA.atlases[i] != null){
								MB_Utility.Destroy(mAndA.atlases[i]);
							}
						}
					}
				}
			}
		}
	}

	void bakeMeshesInPlace(){
		MB_MeshBaker mom = (MB_MeshBaker) target;
		MB_AtlasesAndRects[] mAndAs = null;
		Mesh mesh;
		
		if (!doCombinedValidate(MB_ObjsToCombineTypes.prefabOnly)) return;
	
		try{
			mom.DestroyMesh();
			mAndAs = mom.CreateAtlases(updateProgressBar);
			if (mAndAs != null){
				for(int i = 0; i < mAndAs.Length; i++){
					MB_AtlasesAndRects mAndA = mAndAs[i];
					Material resMat = mom.resultMaterial;
					if (mom.doMultiMaterial) resMat = mom.resultMaterials[i].combinedMaterial;
					saveAtlasesToAssetDatabase(mAndA,resMat);
				}				
				GameObject[] objs = new GameObject[1];
				for (int i = 0; i < mom.objsToMesh.Count; i++){
					objs[0] = mom.objsToMesh[i];
					mesh = mom.AddDeleteGameObjects(objs,null,false);
					if (mesh != null){
						mom.ApplyAll();
						MeshFilter mf = objs[0].GetComponent<MeshFilter>();
						if (mf != null && mf.sharedMesh != null){
							string baseName, folderPath, newFilename;
							string pth = AssetDatabase.GetAssetPath(mf.sharedMesh);
							if (pth != null && pth.Length != 0){
								baseName = Path.GetFileNameWithoutExtension(pth);
								folderPath = Path.GetDirectoryName(pth); 		
								newFilename = Path.Combine(folderPath, baseName + "_MB.asset");
							} else { //try to get the name from prefab
								pth = AssetDatabase.GetAssetPath(objs[0]); //get prefab name
								if (pth != null && pth.Length != 0){
									baseName = Path.GetFileNameWithoutExtension(pth);
									folderPath = Path.GetDirectoryName(pth);		
									newFilename = Path.Combine(folderPath, baseName + "_" + objs[0].name + "_MB.asset"); 
								} else { //save in root
									newFilename = Path.Combine("Assets", objs[0].name + "mesh_MB.asset");
								}
							}
							updateProgressBar("Created mesh saving asset " + newFilename,.6f);
							if (newFilename != null && newFilename.Length != 0){
								Debug.Log("Created mesh with adjusted UVs at: " + newFilename);
								AssetDatabase.CreateAsset(mesh,  newFilename);
							} else {
								Debug.LogWarning("Could not save mesh for " + objs[0].name);	
							}
						}
					}
					mom.DestroyMesh();
				}

			}
		} catch(Exception e){
			Debug.LogError(e);	
		} finally {
			EditorUtility.ClearProgressBar();
			mom.DestroyMesh();
			if (mAndAs != null){
				for(int j = 0; j < mAndAs.Length; j++){
					MB_AtlasesAndRects mAndA = mAndAs[j];			
					if (mAndA != null && mAndA.atlases != null){
						for (int i = 0; i < mAndA.atlases.Length; i++){
							if (mAndA.atlases[i] != null){
								MB_Utility.Destroy(mAndA.atlases[i]);
							}
						}
					}
				}
			}	
		}
	}
	
	bool validateMultipleMaterials(){
		MB_MeshBaker mom = (MB_MeshBaker) target;
		MB_MultiMaterial[] rs = mom.resultMaterials;
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
	
	bool validateSubmeshOverlap(){
		MB_MeshBaker mom = (MB_MeshBaker) target;
		for (int i = 0; i < mom.objsToMesh.Count; i++){
			Mesh m = MB_Utility.GetMesh(mom.objsToMesh[i]);
			if (MB_Utility.doSubmeshesShareVertsOrTris(m) != 0){
				Debug.LogError("Object " + mom.objsToMesh[i] + " in the list of objects to combine has overlapping submeshes. This object can only be combined with objects that use the exact same set of textures. There may be other undesirable side affects as well.");	
				return true;
			}
		}
		return true;
	}
	
	void saveMeshToAssetDatabase(Mesh mesh){
		MB_MeshBaker mom = (MB_MeshBaker) target;
		string prefabPth = AssetDatabase.GetAssetPath(mom.resultPrefab);
		if (prefabPth == null || prefabPth.Length == 0){
			Debug.LogError("Could not save result to prefab. Result Prefab value is not an Asset.");
			return;
		}
		string baseName = Path.GetFileNameWithoutExtension(prefabPth);
		string folderPath = prefabPth.Substring(0,prefabPth.Length - baseName.Length - 7);		
		string newFilename = folderPath + baseName + "-mesh.asset";
		AssetDatabase.CreateAsset(mesh,  newFilename);
	}
	
	void saveAtlasesToAssetDatabase(MB_AtlasesAndRects newMesh, Material resMat){
		Texture2D[] atlases = newMesh.atlases;
		string[] texPropertyNames = newMesh.texPropertyNames;
		string prefabPth = AssetDatabase.GetAssetPath(resMat);
		if (prefabPth == null || prefabPth.Length == 0){
			Debug.LogError("Could not save result to prefab. Result Prefab value is not an Asset.");
			return;
		}
		string baseName = Path.GetFileNameWithoutExtension(prefabPth);
		string folderPath = prefabPth.Substring(0,prefabPth.Length - baseName.Length - 4);		
		string fullFolderPath = Application.dataPath + folderPath.Substring("Assets".Length,folderPath.Length - "Assets".Length);
		for(int i = 0; i < atlases.Length;i++){
			string pth = fullFolderPath + baseName + "-" + texPropertyNames[i] + "-atlas" + i + ".png";
			Debug.Log("Created atlas for: " + texPropertyNames[i] + " at " + pth);
			//need to create a copy because sometimes the packed atlases are not in ARGB32 format
			Texture2D newTex = MB_Utility.createTextureCopy(atlases[i]);
			byte[] bytes = newTex.EncodeToPNG();
			DestroyImmediate(newTex);
			updateProgressBar("Saving atlas " + pth,.8f);
			File.WriteAllBytes(pth, bytes);
			AssetDatabase.Refresh();
			
			string relativePath = folderPath + baseName +"-" + texPropertyNames[i] + "-atlas" + i + ".png";                      				

			_setMaterialTextureProperty(resMat, newMesh.texPropertyNames[i], relativePath);
		}
	}
	
	void _setMaterialTextureProperty(Material target, string texPropName, string texturePath){
		if (texPropName.Equals("_BumpMap")){
			setNormalMap( (Texture2D) (AssetDatabase.LoadAssetAtPath(texturePath, typeof(Texture2D))));
		}
		if (target.HasProperty(texPropName)){
			target.SetTexture(texPropName, (Texture2D) (AssetDatabase.LoadAssetAtPath(texturePath, typeof(Texture2D))));
		}
	}
	
	void createNewPrefab(string pth){
		string baseName = Path.GetFileNameWithoutExtension(pth);
		string folderPath = pth.Substring(0,pth.Length - baseName.Length - 7);		
		string matName = folderPath +  baseName + "-mat.mat";
		AssetDatabase.CreateAsset(new Material(Shader.Find("Diffuse")), matName);
		UnityEngine.Object prefabAsset = PrefabUtility.CreateEmptyPrefab(pth);
		
		MB_MeshBaker mom = (MB_MeshBaker) target;
		GameObject rootGO = new GameObject("combinedMesh-" + mom.name);
		PrefabUtility.ReplacePrefab(rootGO,prefabAsset,ReplacePrefabOptions.ConnectToPrefab);
		
		DestroyImmediate(rootGO);
		mom.resultMaterial = (Material) AssetDatabase.LoadAssetAtPath(matName,typeof(Material));
		mom.resultPrefab = (GameObject) AssetDatabase.LoadAssetAtPath(pth,typeof(GameObject));
		
		AssetDatabase.Refresh();
	}

	void rebuildPrefab(MB_AtlasesAndRects[] ms, Mesh mesh){
		MB_MeshBaker mom = (MB_MeshBaker) target;
		if (mom.resultPrefab == null){
			Debug.LogError("Prefab to store result did not exist.");	
		}
		GameObject rootGO = (GameObject) PrefabUtility.InstantiatePrefab(mom.resultPrefab);
		buildSceneMeshObject(rootGO, mesh);
		
		string prefabPth = AssetDatabase.GetAssetPath(mom.resultPrefab);
		PrefabUtility.ReplacePrefab(rootGO,AssetDatabase.LoadAssetAtPath(prefabPth,typeof(GameObject)),ReplacePrefabOptions.ConnectToPrefab);
		mom.resultPrefab = (GameObject) AssetDatabase.LoadAssetAtPath(prefabPth, typeof(GameObject));	
		DestroyImmediate(rootGO);
	}	
	
	//todo create prefab should handle multi material
	void buildSceneMeshObject(GameObject root, Mesh m){	
		MB_MeshBaker mom = (MB_MeshBaker) target;
		GameObject mesh;
		Transform mt = null;
		foreach (Transform t in root.transform){
			if (t.name.EndsWith("-mesh")){
				mt = t;
			}
		}
		if (mt == null){
			mesh = new GameObject(target.name + "-mesh");
		} else {
			mesh = mt.gameObject;	
		}
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
		mf.mesh = m;
		mesh.transform.parent = root.transform;
	}	
	
	void setNormalMap(Texture2D tx){
		AssetImporter ai = AssetImporter.GetAtPath( AssetDatabase.GetAssetOrScenePath(tx) );
		if (ai != null && ai is TextureImporter){
			TextureImporter textureImporter = (TextureImporter) ai;
			if (!textureImporter.normalmap){
				textureImporter.normalmap = true;
				AssetDatabase.ImportAsset(AssetDatabase.GetAssetOrScenePath(tx));
			}
		}		
	}	
}
