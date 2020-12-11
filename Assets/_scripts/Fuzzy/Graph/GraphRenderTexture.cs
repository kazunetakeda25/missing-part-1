using UnityEngine;
using System.Collections;

public class GraphRenderTexture
{
	private Texture2D m_texture;
	public Texture2D GetTexture()
	{
		return m_texture;
	}
	
	public GraphRenderTexture( GraphRenderWorkingSet initialImage )
	{
		m_texture = new Texture2D( initialImage.m_width
								 , initialImage.m_height
								 , TextureFormat.ARGB32
								 , false					//Mips?
								 );
		Color[] pixels = initialImage.GetPixels();
		m_texture.SetPixels( pixels );
		m_texture.Apply( false );	//Apply for mips?
	}
}

