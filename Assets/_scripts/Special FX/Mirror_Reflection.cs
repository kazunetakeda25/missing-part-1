using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Mirror_Reflection : MonoBehaviour {
	
	/// <summary>
	/// Vairables
	/// </summary>
	
	public bool m_DisablePixelLights = true;
	public int m_TextureSize = 512;
	public float m_ClipPlanOffset = 0.07f;
	
	public LayerMask m_ReflectionLayers = -1;
	
	private Hashtable m_ReflectionsCameras = new Hashtable();
	
	private RenderTexture m_ReflectionTexture = null;
	private int m_OldReflectionTextureSize = 0;
	//public Camera cam;
	
	private static bool s_InsideRendering = false;
	
	public void OnWillRenderObject()
	{
		if( !enabled || !GetComponent<Renderer>() || !GetComponent<Renderer>().sharedMaterial || !GetComponent<Renderer>().enabled)
			return;
		//GameObject temp =  GameObject.FindGameObjectWithTag("MainCamera");
		//if(!temp)
			//return;
		Camera cam = Camera.main;
		if(!cam)
			return;
		
		//safeguard recursive reflections.
		if(s_InsideRendering)
			return;
		
		s_InsideRendering = true;
		
		Camera reflectionCamera;
		CreateMirrorObjects( cam, out reflectionCamera);
		
		//find out reflection plane: position and normal in world space
		
		//print("come back to this if reflection is not correct");
		Mesh mesh = gameObject.GetComponent<MeshFilter>().sharedMesh;// GetComponent(MeshFilter).mesh;
		//print(mesh.name);
		Vector3 pos = transform.position + mesh.vertices[0];// + mesh.vertices[0];
		Vector3 normal = transform.localToWorldMatrix.MultiplyVector(mesh.normals[0]).normalized;
		//print(mesh.normals[0] + " " + mesh.vertices[0] );
		Debug.DrawRay(transform.position + mesh.vertices[0], mesh.normals[0], Color.red);
		//Debug.DrawRay(transform.localPosition + mesh.vertices[0], mesh.normals[0]*2);
		
		// optionaly disable pixel lights for reflection
		int oldPixelLightCount = QualitySettings.pixelLightCount;
		if(m_DisablePixelLights)
			QualitySettings.pixelLightCount = 0;
		UpdateCameraModes(cam,reflectionCamera);
		
		//renderreflections
		//Reflect Camera around reflection plane
		
		float d = -Vector3.Dot(normal,pos) - m_ClipPlanOffset;
		Vector4 reflectionPlane = new Vector4(normal.x,normal.y,normal.z,d);
		
		Matrix4x4 reflection = Matrix4x4.zero;
		CalculateReflectionMatrix(ref reflection,reflectionPlane);
		Vector3 oldpos = cam.transform.position;
		Vector3 newpos = reflection.MultiplyPoint(oldpos);
		reflectionCamera.worldToCameraMatrix = cam.worldToCameraMatrix * reflection;
		
		//setup oblique projection matrix so that near plane is our reflection
		//plane. This way we clip everything below/aboce it for free
		Vector4 clipPlane = CameraSpacePlane(reflectionCamera,pos,normal,1.0f);
		Matrix4x4 projection = cam.projectionMatrix;
		CalculateObliqueMatrix(ref projection, clipPlane);
		reflectionCamera.projectionMatrix = projection;
		
		reflectionCamera.cullingMask = /*~(1<<4) &*/ m_ReflectionLayers.value;
		reflectionCamera.targetTexture = m_ReflectionTexture;
		GL.SetRevertBackfacing(true);
		reflectionCamera.transform.position = newpos;
		Vector3 euler = cam.transform.eulerAngles;
		reflectionCamera.Render();
		reflectionCamera.transform.position = oldpos;
		GL.SetRevertBackfacing(false);
		Material[] materials = GetComponent<Renderer>().sharedMaterials;
		foreach( Material m in materials)
		{
			if(m.HasProperty("_ReflectionTex"))
				m.SetTexture("_ReflectionTex", m_ReflectionTexture);
		}
		
		// Set matrix on the shader that transforms UCs from object space into screen
		//space. We want to just project reflection texture on screen.
		Matrix4x4 scaleOffset = Matrix4x4.TRS(new Vector3(0.5f,0.5f,0.5f),Quaternion.identity,new Vector3(0.5f,0.5f,0.5f));
		Vector3 scale = transform.lossyScale;
		Matrix4x4 mtx = transform.localToWorldMatrix * Matrix4x4.Scale( new Vector3(1.0f/scale.x, 1.0f/ scale.y, 1.0f/scale.z));
		mtx =  scaleOffset * cam.projectionMatrix * cam.worldToCameraMatrix * mtx;
		foreach(Material m in materials)
		{
			m.SetMatrix("_ProjMatrix",mtx);
		}
		
		//restore pixel light count
		if(m_DisablePixelLights)
			QualitySettings.pixelLightCount = oldPixelLightCount;
		
		s_InsideRendering = false;
		
	}
	
	void OnDisable()
	{
		if(m_ReflectionTexture)
		{
			DestroyImmediate(m_ReflectionTexture);
			m_ReflectionTexture = null;
		}
		foreach(DictionaryEntry kvp in m_ReflectionsCameras)
			DestroyImmediate( ((Camera)kvp.Value).gameObject);
		m_ReflectionsCameras.Clear();
	}
	private void UpdateCameraModes(Camera src, Camera dest)
	{
		if(dest == null)
			return;
		
		//set camera to clear the was as current camera
		
		dest.clearFlags = src.clearFlags;
		dest.backgroundColor = src.backgroundColor;
		if(src.clearFlags == CameraClearFlags.Skybox)
		{
			Skybox sky = src.GetComponent(typeof(Skybox))as Skybox;
			Skybox mysky = dest.GetComponent(typeof(Skybox))as Skybox;
			if(!sky || !sky.material)
				mysky.enabled = false;
			else{
				mysky.enabled = true;
				mysky.material = sky.material;
			}
			//update other values to match current camera
			//even if we are supplying custom camera & projection matrices
			//some of values are used elsewhere (E.G. skybox uses far plane
			dest.farClipPlane = src.farClipPlane;
	        dest.nearClipPlane = src.nearClipPlane;
	        dest.orthographic = src.orthographic;
	        dest.fieldOfView = src.fieldOfView;
	        dest.aspect = src.aspect;
	        dest.orthographicSize = src.orthographicSize;
		}
	}
	
	private void CreateMirrorObjects(Camera currentCamera, out Camera ReflectionCamera)
	{
		ReflectionCamera = null;
		
		if( !m_ReflectionTexture || m_OldReflectionTextureSize != m_TextureSize)
		{
			if(m_ReflectionTexture)
				DestroyImmediate(m_ReflectionTexture);
			m_ReflectionTexture = new RenderTexture(m_TextureSize,m_TextureSize,16);
			m_ReflectionTexture.name = "_MirrorReflection" + GetInstanceID();
			m_ReflectionTexture.isPowerOfTwo = true;
			m_ReflectionTexture.hideFlags = HideFlags.DontSave;
			m_OldReflectionTextureSize = m_TextureSize;
		}
		//Camera for reflections
		ReflectionCamera = m_ReflectionsCameras[currentCamera] as Camera;
		if(!ReflectionCamera)//catch both no-in-ditionary and in-dictionary-but-deleted-GO
		{
			GameObject go = new GameObject("Mirror Refl Camera id" + GetInstanceID() + " for " + currentCamera.GetInstanceID(), typeof(Camera), typeof(Skybox));
			ReflectionCamera = go.GetComponent<Camera>();
			ReflectionCamera.enabled = false;
			ReflectionCamera.transform.position = transform.position;
			ReflectionCamera.transform.rotation = transform.rotation;
			ReflectionCamera.gameObject.AddComponent<FlareLayer>();
			go.hideFlags = HideFlags.HideAndDontSave;
			m_ReflectionsCameras[currentCamera] = ReflectionCamera;
		}
		
	}
	
	private static float sgn(float a)
	{
		if (a > 0.0f) return 1.0f;
		if (a < 0.0f) return -1.0f;
		return 0.0f;
	}
	private Vector4 CameraSpacePlane( Camera cam , Vector3 pos, Vector3 normal, float sideSign)
	{
		Vector3 offsetPos = pos + normal * m_ClipPlanOffset;
		Matrix4x4 m = cam.worldToCameraMatrix;
		Vector3 cpos = m.MultiplyPoint(offsetPos);
		Vector3 cnormal = m.MultiplyVector(normal).normalized * sideSign;
		Debug.DrawRay(cpos, cnormal *5,Color.blue);
		
		return new Vector4(cnormal.x, cnormal.y, cnormal.z, -Vector3.Dot(cpos,cnormal));
	}
	private static void CalculateObliqueMatrix(ref Matrix4x4 projection, Vector4 clipPlane)
	{
		Vector4 q = projection.inverse * new Vector4(sgn(clipPlane.x),sgn(clipPlane.y),1.0f,1.0f);
		Vector4 c = clipPlane *(2.0F / (Vector4.Dot(clipPlane,q)));
		//third row = clipplane - fourth row
		projection[2] = c.x - projection[3];
		projection[6] = c.y - projection[7];
		projection[10] = c.z - projection[11];
		projection[14] = c.w - projection[15];
		
	}
	private static void CalculateReflectionMatrix(ref Matrix4x4 reflectionMat, Vector4 plane)
	{
		reflectionMat.m00 = (1F - 2F*plane[0]*plane[0]);
        reflectionMat.m01 = (   - 2F*plane[0]*plane[1]);
        reflectionMat.m02 = (   - 2F*plane[0]*plane[2]);
        reflectionMat.m03 = (   - 2F*plane[3]*plane[0]);
 
        reflectionMat.m10 = (   - 2F*plane[1]*plane[0]);
        reflectionMat.m11 = (1F - 2F*plane[1]*plane[1]);
        reflectionMat.m12 = (   - 2F*plane[1]*plane[2]);
        reflectionMat.m13 = (   - 2F*plane[3]*plane[1]);
 
        reflectionMat.m20 = (   - 2F*plane[2]*plane[0]);
        reflectionMat.m21 = (   - 2F*plane[2]*plane[1]);
        reflectionMat.m22 = (1F - 2F*plane[2]*plane[2]);
        reflectionMat.m23 = (   - 2F*plane[3]*plane[2]);
 
        reflectionMat.m30 = 0F;
        reflectionMat.m31 = 0F;
        reflectionMat.m32 = 0F;
        reflectionMat.m33 = 1F;
	}
}
