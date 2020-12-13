using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MB2_TextureBakeResults : ScriptableObject {
	public MB_AtlasesAndRects[] combinedMaterialInfo;
	public Material[] materials;
	public Rect[] prefabUVRects;
	public Material resultMaterial;
	public MB_MultiMaterial[] resultMaterials;
	public bool doMultiMaterial;
	public bool fixOutOfBoundsUVs;
	
	public Dictionary<Material, Rect> GetMat2RectMap(){
		Dictionary<Material, Rect> mat2rect_map = new Dictionary<Material, Rect>();
		if (materials == null || prefabUVRects == null || materials.Length != prefabUVRects.Length){
			Debug.LogWarning("Bad TextureBakeResults could not build mat2UVRect map");
		} else {
			for (int i = 0; i < materials.Length; i++){
				mat2rect_map.Add(materials[i],prefabUVRects[i]);
			}
		}
		return mat2rect_map;		
	}
}
