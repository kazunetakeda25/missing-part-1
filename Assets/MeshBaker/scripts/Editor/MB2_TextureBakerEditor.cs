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

[CustomEditor(typeof(MB2_TextureBaker))]
public class MB2_TextureBakerEditor : Editor {
	
	MB2_TextureBakerEditorInternal tbe = new MB2_TextureBakerEditorInternal();
	
	public override void OnInspectorGUI(){
		tbe.DrawGUI((MB2_TextureBaker) target);	
	}

}

public class MB2_TextureBakerEditorInternal{
	//add option to exclude skinned mesh renderer and mesh renderer in filter
	//example scenes for multi material
	
	private static GUIContent insertContent = new GUIContent("+", "add a material");
	private static GUIContent deleteContent = new GUIContent("-", "delete a material");
	private static GUILayoutOption buttonWidth = GUILayout.MaxWidth(20f);

	private SerializedObject meshBaker;
	private SerializedProperty textureBakeResults, maxTilingBakeSize, doMultiMaterial, fixOutOfBoundsUVs, resultMaterial, resultMaterials, atlasPadding, resizePowerOfTwoTextures, customShaderPropNames, objsToMesh;
	
	bool resultMaterialsFoldout = true;
	bool showInstructions = false;
	
	private static GUIContent
		createPrefabAndMaterialLabelContent = new GUIContent("Create Empty Assets For Combined Material", "Creates a material asset and a 'MB2_TextureBakeResult' asset. You should set the shader on the material. Mesh Baker uses the Texture properties on the material to decide what atlases need to be created. The MB2_TextureBakeResult asset should be used in the 'Material Bake Result' field."),
		openToolsWindowLabelContent = new GUIContent("Open Tools For Adding Objects", "Use these tools to find out what can be combined, discover possible problems with meshes, and quickly add objects."),
		fixOutOfBoundsGUIContent = new GUIContent("Fix Out-Of-Bounds UVs", "If mesh has uvs outside the range 0,1 uvs will be scaled so they are in 0,1 range. Textures will have tiling baked."),
		resizePowerOfTwoGUIContent = new GUIContent("Resize Power-Of-Two Textures", "Shrinks textures so they have a clear border of width 'Atlas Padding' around them. Improves texture packing efficiency."),
		customShaderPropertyNamesGUIContent = new GUIContent("Custom Shader Propert Names", "Mesh Baker has a list of common texture properties that it looks for in shaders to generate atlases. Custom shaders may have texture properties not on this list. Add them here and Meshbaker will generate atlases for them."),
		combinedMaterialsGUIContent = new GUIContent("Combined Materials", "Use the +/- buttons to add multiple combined materials. You will also need to specify which materials on the source objects map to each combined material."),
		maxTilingBakeSizeGUIContent = new GUIContent("Max Tiling Bake Size","This is the maximum size tiling textures will be baked to."),
		objectsToCombineGUIContent = new GUIContent("Objects To Be Combined","These can be prefabs or scene objects. They must be game objects with Renderer components, not the parent objects. Materials on these objects will baked into the combined material(s)"),
		textureBakeResultsGUIContent = new GUIContent("Material Bake Result","This asset contains a mapping of materials to UV rectangles in the atlases. It is needed to create combined meshes or adjust meshes so they can use the combined material(s). Create it using 'Create Empty Assets For Combined Material'. Drag it to the 'Material Bake Result' field to use it.");
	
	[MenuItem("GameObject/Create Other/Mesh Baker/Material Baker")]
	public static void CreateNewMeshBaker(){
		MB2_TextureBaker[] mbs = (MB2_TextureBaker[]) Editor.FindObjectsOfType(typeof(MB2_TextureBaker));
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
		GameObject nmb = new GameObject("MaterialBaker" + largest);
		nmb.transform.position = Vector3.zero;
		nmb.AddComponent<MB2_TextureBaker>();
	}

	void _init(MB2_TextureBaker target) {
		meshBaker = new SerializedObject(target);
		doMultiMaterial = meshBaker.FindProperty("doMultiMaterial");
		fixOutOfBoundsUVs = meshBaker.FindProperty("fixOutOfBoundsUVs");
		resultMaterial = meshBaker.FindProperty("resultMaterial");
		resultMaterials = meshBaker.FindProperty("resultMaterials");
		atlasPadding = meshBaker.FindProperty("atlasPadding");
		resizePowerOfTwoTextures = meshBaker.FindProperty("resizePowerOfTwoTextures");
		customShaderPropNames = meshBaker.FindProperty("customShaderPropNames");
		objsToMesh = meshBaker.FindProperty("objsToMesh");
		maxTilingBakeSize = meshBaker.FindProperty("maxTilingBakeSize");
		textureBakeResults = meshBaker.FindProperty("textureBakeResults");
	}	
	
	public void DrawGUI(MB2_TextureBaker mom){
		if (meshBaker == null){
			_init(mom);
		}
		
		meshBaker.Update();

		showInstructions = EditorGUILayout.Foldout(showInstructions,"Instructions:");
		if (showInstructions){
			EditorGUILayout.HelpBox("1. Create Empty Assets For Combined Material(s)\n\n" +
									"2. Select shader on result material(s).\n\n" +
									"3. Add scene objects or prefabs to combine. For best results these should use the same shader as result material.\n\n" +
									"4. Bake materials into combined material(s).\n\n" +
									"5. Look at warnings/errors in console. Decide if action needs to be taken.\n\n" +
									"6. You are now ready to build combined meshs or adjust meshes to use the combined material(s).", UnityEditor.MessageType.None);
			
		}				
		
		//MB2_TextureBaker mom = (MB2_TextureBaker) target;
		
		EditorGUILayout.Separator();
		EditorGUILayout.LabelField("Output",EditorStyles.boldLabel);
		if (GUILayout.Button(createPrefabAndMaterialLabelContent)){
			string newPrefabPath = EditorUtility.SaveFilePanelInProject("Asset name", "", "asset", "Enter a name for the baked texture results");
			if (newPrefabPath != null){
				createCombinedMaterialAssets(mom, newPrefabPath);
			}
		}	
		EditorGUILayout.PropertyField(textureBakeResults, textureBakeResultsGUIContent);
		EditorGUILayout.PropertyField(doMultiMaterial,new GUIContent("Multiple Combined Materials"));		
		
		if (mom.doMultiMaterial){
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
					EditorGUILayout.LabelField("---------- submesh:" + i,EditorStyles.boldLabel);
					EditorGUILayout.Separator();
					SerializedProperty resMat = resultMaterials.GetArrayElementAtIndex(i);
					EditorGUILayout.PropertyField(resMat.FindPropertyRelative("combinedMaterial"));
					SerializedProperty sourceMats = resMat.FindPropertyRelative("sourceMaterials");
					EditorGUILayout.PropertyField(sourceMats,true);
				}
			}
			
		} else {			
			EditorGUILayout.PropertyField(resultMaterial,new GUIContent("Combined Mesh Material"));
		}		

		EditorGUILayout.Separator();		
		EditorGUILayout.LabelField("Objects To Be Combined",EditorStyles.boldLabel);	
		if (GUILayout.Button(openToolsWindowLabelContent)){
			MB_MeshBakerEditorWindow mmWin = (MB_MeshBakerEditorWindow) EditorWindow.GetWindow(typeof(MB_MeshBakerEditorWindow));
			mmWin.target = (MB2_MeshBakerRoot) mom;
		}	
		EditorGUILayout.PropertyField(objsToMesh,objectsToCombineGUIContent, true);		
		
		EditorGUILayout.Separator();
		EditorGUILayout.LabelField("Material Bake Options",EditorStyles.boldLabel);		
		EditorGUILayout.PropertyField(atlasPadding,new GUIContent("Atlas Padding"));
		EditorGUILayout.PropertyField(resizePowerOfTwoTextures, resizePowerOfTwoGUIContent);
		EditorGUILayout.PropertyField(customShaderPropNames,customShaderPropertyNamesGUIContent,true);
		EditorGUILayout.PropertyField(maxTilingBakeSize, maxTilingBakeSizeGUIContent);
		EditorGUILayout.PropertyField(fixOutOfBoundsUVs,fixOutOfBoundsGUIContent);		
		
		EditorGUILayout.Separator();				
		if (GUILayout.Button("Bake Materials Into Combined Material")){
			bake(mom);
		}
		meshBaker.ApplyModifiedProperties();		
		meshBaker.SetIsDifferentCacheDirty();
	}
		
	public void updateProgressBar(string msg, float progress){
		EditorUtility.DisplayProgressBar("Combining Meshes", msg, progress);
	}

	void bake(MB2_TextureBaker target){
		MB_AtlasesAndRects[] mAndAs = null;
		try{
			mAndAs = _bakeTexturesOnly(target);
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
	
	MB_AtlasesAndRects[] _bakeTexturesOnly(MB2_TextureBaker target){
		MB_AtlasesAndRects[] mAndAs = null;
		if (!MB_Utility.doCombinedValidate((MB2_MeshBakerRoot)target, MB_ObjsToCombineTypes.dontCare)) return null;
		MB2_TextureBaker mbd = (MB2_TextureBaker) target;
		mAndAs = mbd.CreateAtlases(updateProgressBar);
		if (mAndAs != null) _saveBakeTextureAssets(target, mAndAs);
		return mAndAs;		
	}

	void _saveBakeTextureAssets(MB2_TextureBaker target, MB_AtlasesAndRects[] mAndAs){
		MB2_TextureBaker mbd = (MB2_TextureBaker) target;
		for(int i = 0; i < mAndAs.Length; i++){
			MB_AtlasesAndRects mAndA = mAndAs[i];
			Material resMat = mbd.resultMaterial;
			if (mbd.doMultiMaterial) resMat = mbd.resultMaterials[i].combinedMaterial;
			_saveAtlasesToAssetDatabase(mAndA, resMat);
		}
		
		mbd.textureBakeResults.combinedMaterialInfo = mAndAs;
		mbd.textureBakeResults.doMultiMaterial = mbd.doMultiMaterial;
		mbd.textureBakeResults.resultMaterial = mbd.resultMaterial;
		mbd.textureBakeResults.resultMaterials = mbd.resultMaterials;
		mbd.textureBakeResults.fixOutOfBoundsUVs = mbd.fixOutOfBoundsUVs;
		unpackMat2RectMap(mbd.textureBakeResults);
		MB2_MeshBaker mb = mbd.GetComponent<MB2_MeshBaker>();
		if (mb != null){
			mb.textureBakeResults = mbd.textureBakeResults;	
		}
		EditorUtility.SetDirty(mbd.textureBakeResults); 
	}
	
	void _saveAtlasesToAssetDatabase(MB_AtlasesAndRects newMesh, Material resMat){
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
			Editor.DestroyImmediate(newTex);
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
	
	void createCombinedMaterialAssets(MB2_TextureBaker target, string pth){
		MB2_TextureBaker mom = (MB2_TextureBaker) target;
		string baseName = Path.GetFileNameWithoutExtension(pth);
		string folderPath = pth.Substring(0,pth.Length - baseName.Length - 6);
		
		List<string> matNames = new List<string>();
		if (mom.doMultiMaterial){
			for (int i = 0; i < mom.resultMaterials.Length; i++){
				matNames.Add( folderPath +  baseName + "-mat" + i + ".mat" );
				AssetDatabase.CreateAsset(new Material(Shader.Find("Diffuse")), matNames[i]);
				mom.resultMaterials[i].combinedMaterial = (Material) AssetDatabase.LoadAssetAtPath(matNames[i],typeof(Material));
			}
		}else{
			matNames.Add( folderPath +  baseName + "-mat.mat" );
			AssetDatabase.CreateAsset(new Material(Shader.Find("Diffuse")), matNames[0]);
			mom.resultMaterial = (Material) AssetDatabase.LoadAssetAtPath(matNames[0],typeof(Material));
		}
		//create the MB2_TextureBakeResults
		AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<MB2_TextureBakeResults>(),pth);
		mom.textureBakeResults = (MB2_TextureBakeResults) AssetDatabase.LoadAssetAtPath(pth, typeof(MB2_TextureBakeResults));
		AssetDatabase.Refresh();
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
	
	void unpackMat2RectMap(MB2_TextureBakeResults results){
		List<Material> ms = new List<Material>();
		List<Rect> rs = new List<Rect>();
		for (int i = 0; i < results.combinedMaterialInfo.Length; i++){
			MB_AtlasesAndRects newMesh = results.combinedMaterialInfo[i];
			Dictionary<Material,Rect> map = newMesh.mat2rect_map;
			foreach(Material m in map.Keys){
				ms.Add(m);
				rs.Add(map[m]);
			}
		}
		results.materials = ms.ToArray();
		results.prefabUVRects = rs.ToArray();
	}
}
