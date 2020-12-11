using UnityEngine;
using System.Collections;

public class GraphRenderWorkingSet
{
	public readonly int m_width;
	public readonly int m_height;
	public readonly int m_totalPixels;
	private Color[] m_pixelWorkingSet;
	
	public enum BlendMode
	{
		Overwrite,			//Take the new one
		Blend,				//Blends alpha values, using paint program algo
		Additive,			//Adds color components (Red + Green = Yellow)
		OverwriteNonClear,	//Overwrites anything that isn't 0 alpha.
	}
	
	public Color[] GetPixels()
	{
		return m_pixelWorkingSet;
	}
	
	public GraphRenderWorkingSet( int width
					  			, int height
					  			, Color initialFill
					  			)
	{
		m_width = width;
		m_height = height;
		m_totalPixels = m_width * m_height;
		m_pixelWorkingSet = new Color[ m_totalPixels ];
		
		for ( int ctr = 0; ctr < m_totalPixels; ++ctr )
		{
			m_pixelWorkingSet[ ctr ] = initialFill;
		}
	}
	
	//Copy constructor.
	public GraphRenderWorkingSet( GraphRenderWorkingSet other )
	{
		m_width = other.m_width;
		m_height = other.m_height;
		m_totalPixels = other.m_totalPixels;
		m_pixelWorkingSet = new Color[ m_totalPixels ];
		
		for ( int ctr = 0; ctr < m_totalPixels; ++ctr )
		{
			m_pixelWorkingSet[ ctr ] = other.m_pixelWorkingSet[ ctr ];
		}
	}
	
	//Applies another working set atop this one.
	public bool ApplyWorkingSet( GraphRenderWorkingSet other
							   , BlendMode blendMode
							   )
	{
		//Ensure they are the same size.
		if ( ( other.m_width != m_width ) || ( other.m_height != m_height ) )
		{
			Debug.LogError( "Attempted to apply two working sets of different sizes!" );
			return false;
		}
		
		for ( int ctr = 0; ctr < m_totalPixels; ++ctr )
		{
			Color oldColor = m_pixelWorkingSet[ ctr ];
			Color newColor = other.m_pixelWorkingSet[ ctr ];
			m_pixelWorkingSet[ ctr ] = GetColorForBlendMode( ref oldColor
														   , ref newColor
														   , blendMode
														   );
		}
		
		return true;
	}
	
	public static Color GetColorForBlendMode( ref Color oldColor
											, ref Color newColor
											, BlendMode blendMode
											)
	{
		//Switch based on blending mode.
		switch( blendMode )
		{
		case BlendMode.Additive:
			return ( oldColor + newColor );
		case BlendMode.Blend:
		{
			if ( newColor.a > 0.0f )
			{
				//This is the what most paint programs use.
				float oneMinus = 1.0f - newColor.a;
				float newAlpha = Mathf.Clamp01( newColor.a + ( oldColor.a * oneMinus ) );
				float invNewAlpha = 1.0f / newAlpha;
				float newRed = invNewAlpha * ( ( newColor.r * newColor.a ) + ( ( oldColor.r * oldColor.a ) * oneMinus ) );
				float newGreen = invNewAlpha * ( ( newColor.g * newColor.a ) + ( ( oldColor.g * oldColor.a ) * oneMinus ) );
				float newBlue = invNewAlpha * ( ( newColor.b * newColor.a ) + ( ( oldColor.b * oldColor.a ) * oneMinus ) );
				return new Color( newRed, newGreen, newBlue, newAlpha );
			}
			return oldColor;
		}
		case BlendMode.Overwrite:
			return newColor;
		case BlendMode.OverwriteNonClear:
			return ( newColor.a > 0.0f ) ? newColor : oldColor;
		default:
			Debug.LogError( "Unknown/unsupported blend mode encountered!" );
			return oldColor;
		}
	}
	
	public int ConvertPctXToPixelX( float pctX )
	{
		int pos = (int)( pctX * (float)m_width );
		return pos;
	}
	
	public int ConvertPctYToPixelY( float pctY )
	{
		int pos = (int)( pctY * (float)m_height );
		return pos;
	}
	
	public void ConvertPctToPixel( float pctX
							  	 , float pctY
							  	 , out int xPos
								 , out int yPos
								 )
	{
		xPos = (int)( pctX * (float)m_width );
		yPos = (int)( pctY * (float)m_height );
	}
	
	public int GetIndexForPos( int xPos, int yPos )
	{
		int index = ( yPos * m_width ) + xPos;
		return index;
	}
	
	public bool IsValidX( int xPos )
	{
		return ( ( xPos >= 0 ) && ( xPos < m_width ) );
	}
	
	public bool IsValidY( int yPos )
	{
		return ( ( yPos >= 0 ) && ( yPos < m_height ) );
	}
	
	public bool IsValidPoint( int xPos, int yPos )
	{
		return ( IsValidX( xPos ) && IsValidY( yPos ) );
	}
	
	public bool IsValidIndex( int index )
	{
		return ( ( index >= 0 ) && ( index < m_totalPixels ) );
	}
	
	public int ClampY( int yPos )
	{
		if ( yPos < 0 )
		{
			return 0;
		}
		else if ( yPos >= m_height )
		{
			return m_height - 1;
		}
		
		return yPos;	//Fine
	}
	
	public int ClampX( int xPos )
	{
		if ( xPos < 0 )
		{
			return 0;
		}
		else if ( xPos >= m_width )
		{
			return m_width - 1;
		}
		
		return xPos;	//Fine.
	}

	public void SetPixel( int xPos
						, int yPos
						, Color color
						, BlendMode blendMode
						)
	{
		if ( IsValidPoint( xPos, yPos ) )
		{
			int index = GetIndexForPos( xPos, yPos );
			Color oldColor = m_pixelWorkingSet[ index ];
			m_pixelWorkingSet[ index ] = GetColorForBlendMode( ref oldColor
															 , ref color
															 , blendMode
															 );
		}
	}
	
	public void DrawLineVertical( int xPos
								, int yPosStart
								, int yPosEnd
								, Color color
								, BlendMode blendMode
								)
	{
		//Ensure the X value actually makes sense.
		if ( false == IsValidX( xPos ) )
		{
			return;	//Get out.
		}
		
		int startY = yPosStart;
		int endY = yPosEnd;
		
		//Ensure they're sorted correctly.
		if ( yPosStart > yPosEnd )
		{
			//Swap.
			startY = yPosEnd;
			endY = yPosStart;
		}
		
		//Clip intelligently.
		if ( startY < 0 )
		{
			//We can get out if it ends out there, too.
			if ( endY < 0 )
			{
				return;
			}
			startY = 0;
		}
		else if ( startY >= m_height )
		{
			return;	//Get out.
		}

		if ( endY >= m_height )
		{
			endY = m_height - 1;
		}
		
		//Go to town.
		int startIndex = GetIndexForPos( xPos, startY );
		int endIndex = GetIndexForPos( xPos, endY );
		
		//Step by stride.
		for ( int ctr = startIndex; ctr <= endIndex; ctr += m_width )
		{
			if ( IsValidIndex( ctr ) )
			{
				m_pixelWorkingSet[ ctr ] = 
					GetColorForBlendMode( ref m_pixelWorkingSet[ ctr ]	//Old color
										, ref color						//New color
										, blendMode
										);
			}
		}
	}
	
	public void DrawLineHorizontal( int yPos
								  , int xPosStart
								  , int xPosEnd
								  , Color color
								  , BlendMode blendMode
								  )
	{
		//Ensure the Y value actually makes sense.
		if ( false == IsValidY( yPos ) )
		{
			return;	//Get out.
		}
		
		int startX = xPosStart;
		int endX = xPosEnd;
		
		//Ensure they're sorted correctly.
		if ( xPosStart > xPosEnd )
		{
			//Swap.
			startX = xPosEnd;
			endX = xPosStart;
		}
		
		//Clip intelligently.
		if ( startX < 0 )
		{
			//We can abort if the end is ALSO off here.
			if ( endX < 0 )
			{
				return;
			}
			startX = 0;	//Start on the left edge, please.
		}
		else if ( startX >= m_width )
		{
			return;	//Get out.
		}
		
		if ( endX >= m_width )
		{
			endX = m_width - 1;
		}
		
		//Go to town.
		int startIndex = GetIndexForPos( startX, yPos );
		int endIndex = GetIndexForPos( endX, yPos );
		
		//Smarter implementation would clip ahead of time.
		
		//Step by stride.
		for ( int ctr = startIndex; ctr <= endIndex; ++ctr )
		{
			if ( IsValidIndex( ctr ) )
			{
				m_pixelWorkingSet[ ctr ] = 
					GetColorForBlendMode( ref m_pixelWorkingSet[ ctr ]	//Old color
										, ref color						//New color
										, blendMode
										);
			}
		}
	}
	
};
