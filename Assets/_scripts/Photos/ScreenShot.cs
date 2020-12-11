using UnityEngine;
using System.Collections;

public class ScreenShot : MonoBehaviour
{
    public LayerMask m_layerMask = -1;
    public Camera m_camera = null;
    public int m_imageWidth = 1024;
    public int m_imageHeight = 768;
    
    // Use this for initialization
    void Start()
    {
    
    }
    
    // Update is called once per frame
    void Update()
    {
		/*
        if ( Input.GetKeyDown( "1" ) )
        {
            TakeScreenShot();
        }
        */
    }
    
    private void TakeScreenShot()
    {
        Debug.Log( "TAKING SCREEN SHOT!" );
        
        //Preserve old state.
        RenderTexture oldCamRT = m_camera.targetTexture;
        RenderTexture oldActive = RenderTexture.active;
        int oldMask = m_camera.cullingMask;
        
        RenderTexture renderTexture = new RenderTexture( m_imageWidth, m_imageHeight, 24 );
        Texture2D screenShot = new Texture2D( m_imageWidth, m_imageHeight, TextureFormat.RGB24, false );
        //m_camera.cullingMask = m_layerMask.value;
        m_camera.targetTexture = renderTexture;
        m_camera.Render();
        RenderTexture.active = renderTexture;

        //Restore old settings.
        GetComponent<Camera>().targetTexture = oldCamRT;
        RenderTexture.active = oldActive;
        m_camera.cullingMask = oldMask;
        
        screenShot.ReadPixels( new Rect( 0.0f, 0.0f, m_imageWidth, m_imageHeight ), 0, 0 );

        Destroy( renderTexture );
        
        byte[] bytes = screenShot.EncodeToPNG();
        Destroy( screenShot );
        string fileName = "tempScreenShot.png";
        System.IO.File.WriteAllBytes( fileName, bytes );
    }
}

