using UnityEngine;
using System.Collections;

#if USE_TEXTMESH_PRO_FOR_EZ_GUI

public class TMProAdapter : UITextBase 
{
	protected bool clipped = false;
	protected Rect3D clippingRect;
	protected bool hidden = false;
	protected float zOffset = 0;
	protected bool password = false;
	protected string maskingCharacter = "*";
	protected bool multiline = false;
	protected SpriteText.Anchor_Pos anchor;
	protected float maxWidth = 0;
	protected bool removeUnsupportedCharacters = true;
	protected bool parseTags = true;
	protected Camera renderCamera;
	
	// TODO:
	// Declare necessary variables to hold data and references
	// to all necessary TMPro objects.
	
	/// <summary>
	/// Creates and returns an instance of the text object.
	/// </summary>
	public static TMProAdapter CreateInstance(IControl control)
	{
		// TODO:
		// Create necessary objects and return a reference to
		// a new TMProAdapter object. (Change return statement
		// below!)
		
		Parent = control;
		
		return null;
	}
	
	/// <summary>
	/// Sets/gets the full, decorated (color ) text string.
	/// </summary>
	public override string Text
	{
		get
		{
			// TODO:
			// Return the text, including tags.
			return null;
		}
		set
		{
			// TODO:
			// Assign text. This is used to populate the text object
			// with text content (including tagged/marked up text).
		}
	}

	/// <summary>
	/// Sets/gets whether or not the object is persistent 
	/// (survives scene loads via DontDestroyOnLoad()).
	/// Once it is set to persist, it cannot be undone.
	/// </summary>
	public override bool Persistent
	{
		get { return persistent; }
		set
		{
			if(value == true)
			{
				DontDestroyOnLoad(this);
				persistent = value;
				
				// TODO:
				// Call DontDestroyOnLoad() on any other dependent objects
			}
		}
	}
	
	/// <summary>
	/// Gets or sets the color.
	/// </summary>
	/// <value>
	/// The color.
	/// </value>
	public override Color Color
	{
		get
		{
			// TODO:
			// Return the base color (including alpha) of the text.
			return Color.white;
		}
		set
		{
			SetColor(value);
		}
	}
	
	/// <summary>
	/// Sets the color.
	/// </summary>
	/// <param name='c'>
	/// C.
	/// </param>
	public override void SetColor(Color c)
	{
		// TODO:
		// Apply the specified color (including alpha) to the text.
	}

		
	/// <summary>
	/// Undoes any clipping on the text
	/// </summary>
	public override void Unclip()
	{
		// TODO:
		// Remove any clipping from the text.
	}
	
	/// <summary>
	/// Gets/Sets the status of clipping on the object.
	/// </summary>
	public override bool Clipped
	{
		get { return clipped; }
		set
		{
			clipped = value;
			
			// TODO:
			// Apply any already-defined clipping
			// (preferably by applying the current
			// clipping rect)
		}
	}
	
	/// <summary>
	/// Gets/Sets the clipping rect of the object.
	/// (When setting, always sets Clipped to true.)
	/// </summary>
	public override Rect3D ClippingRect
	{
		get { return clippingRect; }
		set
		{
			clippingRect = value;
			clipped = true;
			
			Rect localClipRect = Rect3D.MultFast(clippingRect, transform.worldToLocalMatrix).GetRect();

			// TODO:
			// Apply the local-space clipping rect to the text.
		}
	}
	
	/// <summary>
	/// Hides or displays the text by disabling/enabling
	/// the mesh renderer component.
	/// </summary>
	/// <param name="tf">When true, the text is hidden, when false, the text will be displayed.</param>
	public override void Hide(bool tf)
	{
		hidden = tf;
		
		// TODO:
		// Make text disappear if true, re-appear if false.
	}

	/// <summary>
	/// Returns whether the text is currently set to be hidden
	/// (whether its mesh renderer component is enabled).
	/// </summary>
	/// <returns>True when hidden, false when set to be displayed.</returns>
	public override bool IsHidden()
	{
		return hidden;
	}

	/// <summary>
	/// Gets the position of the top-left corner of
	/// the text's extents, after clipping, in local space.
	/// </summary>
	public override Vector3 TopLeft
	{
		get
		{
			// TODO:
			// Return the top-left coordinate, in local space,
			// of the text's extents after clipping has been applied.
			return Vector3.zero;
		}
	}
	
	/// <summary>
	/// Gets the position of the bottom-right corner of
	/// the text's extents, after clipping, in local space.
	/// </summary>
	public override Vector3 BottomRight
	{
		get
		{
			// TODO:
			// Return the bottom-right coordinate, in local space,
			// of the text's extents after clipping has been applied.
			return Vector3.zero;
		}
	}

	/// <summary>
	/// Gets the position of the top-left corner of
	/// the text's extents in local space.
	/// </summary>
	public override Vector3 UnclippedTopLeft
	{
		get
		{
			// TODO:
			// Return the top-left coordinate, in local space,
			// of the text's extents BEFORE clipping has been applied.
			return Vector3.zero;
		}
	}
	
	/// <summary>
	/// Gets the position of the bottom-right corner of
	/// the text's extents in local space.
	/// </summary>
	public override Vector3 UnclippedBottomRight
	{
		get
		{
			// TODO:
			// Return the bottom-right coordinate, in local space,
			// of the text's extents BEFORE clipping has been applied.
			return Vector3.zero;
		}
	}
	
	/// <summary>
	/// Gets a value indicating whether the text's width is limited.
	/// </summary>
	/// <value>
	/// <c>true</c> if width limited; otherwise, <c>false</c>.
	/// </value>
	public override bool IsWidthLimited
	{
		get
		{
			// TODO:
			// Return whether or not the width of the text
			// is limited. Return false if the text can just
			// keep growing horizontally indefinitely without
			// limit.
			return false;
		}
	}

	/// <summary>
	/// Performs a deep copy of the text object.
	/// </summary>
	/// <param name='text'>
	/// The object to be copied.
	/// </param>
	public override void Copy(IUIText text)
	{
		if ( !(text is TMProAdapter) )
			return;
		
		TMProAdapter t = (TMProAdapter)text;
		
		zOffset = t.ZOffset;
		password = t.Password;
		maskingCharacter = t.MaskingCharacter;
		multiline = t.Multiline;
		Anchor = t.Anchor;
		maxWidth = t.MaxWidth;
		removeUnsupportedCharacters = t.RemoveUnsupportedCharacters;
		parseTags = t.ParseTags;
		if (t.Clipped)
			ClippingRect = t.clippingRect;
		Hide(t.IsHidden());
		
		// TODO:
		// Perform a deep copy of the objects and
		// settings of another text object of the
		// same type.
		// NOTE: You may want to use local variables
		// instead of the properties since
		// assigning to each property may cause
		// the text to be re-generated all over again
		// so it may be more performant to just set
		// the values of the relevant local variables
		// and then apply them all en masse at the end.
	}
	
	/// <summary>
	/// Sets/Gets the Z-offset of the text.
	/// </summary>
	public override float ZOffset
	{
		get
		{
			return zOffset;
		}
		set
		{
			zOffset = value;
			
			// TODO:
			// Apply the new Z-offset to the text, if relevant.
			// In SpriteText, this just offsets the plane in which
			// the text is drawn along the Z-axes by the specified
			// amount, interpreted as local space units.
		}
	}
	
	/// <summary>
	/// Returns the position, in world space, of the insertion
	/// point relative to a specified character (specified by index).
	/// The position returned is at the left edge of, and the baseline
	/// of, the specified character.
	/// </summary>
	/// <param name="charIndex">The 0-based index of the character to the right of 
	/// where you want an insertion point. 0 indicates the very beginning, 
	/// and an index equal to the string's length indicates an insertion point 
	/// at the very end of the string.</param>
	/// <returns>Returns a point at the left edge of, and the baseline of, the 
	/// specified character.</returns>
	public override Vector3 GetInsertionPointPos(int charIndex)
	{
		// TODO:
		// Calculate and return the world-space position of
		// the insertion point relative to the specified 
		// character.  The insertion point should be to the
		// left of the character at the specified index.
		return Vector3.zero;
	}
		
	/// <summary>
	/// Returns both the world-space position of the insertion point
	/// most nearly matching the specified point, as well as the
	/// index of the character to the left of which the insertion
	/// point corresponds.
	/// </summary>
	/// <param name="point">A point, in world space, from which you want to find
	/// the nearest insertion point.</param>
	/// <returns>The index of the character to
	/// the right of where the insertion point will be. If the insertion point is
	/// at the end of the string, this value will be one greater than the index
	/// of the last character.</returns>
	public override int GetNearestInsertionPoint(Vector3 point)
	{
		// TODO:
		// See comments above
		return 0;
	}
	
	/// <summary>
	/// Returns the distance from the baseline to the
	/// top of a line, in local space units.
	/// </summary>
	public override float BaseHeight
	{
		get
		{
			// TODO:
			// Calculate and return distance, in local space units,
			// from the baseline of a line to its top.
			return 0;
		}
	}
	
	/// <summary>
	/// The distance, in local space units, from the top of
	/// one line to the top of the next.
	/// </summary>
	public override float LineSpan
	{
		get
		{
			// TODO:
			// Calculate and return the distance, in local space units,
			// from the top of one line to the top of the next.
			return 0;
		}
	}

	/// <summary>
	/// Converts a character index in the plain text string
	/// to a corresponding index in the display string.
	/// </summary>
	/// <param name="plainCharIndex">The index in the plain string to be converted.</param>
	/// <returns>The index in the display string that corresponds to the provided index in the plain text string.</returns>
	public override int PlainIndexToDisplayIndex(int plainCharIndex)
	{
		// TODO:
		// Converts the index of a character in a string representing the
		// plain (non-markup) text to the index of the corresponding character
		// in a string that represents the actual displayed content (meaning
		// the string after any carriage returns, etc, characters have been
		// added for word-wrapping or formatting).
		// If TMPro doesn't add any of these to the post-markup text, then
		// just return plainCharIndex.
		return plainCharIndex;
	}

	/// <summary>
	/// Converts a character index in the display text string
	/// to a corresponding index in the plain text string.
	/// </summary>
	/// <param name="plainCharIndex">The index in the display string to be converted.</param>
	/// <returns>The index in the plain text string that corresponds to the provided index in the display text string.</returns>
	public override int DisplayIndexToPlainIndex(int dispCharIndex)
	{
		// TODO:
		// Do the reverse of PlainIndexToDisplayIndex().
		return dispCharIndex;
	}
	
	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="IUIText"/> is password.
	/// </summary>
	/// <value>
	/// <c>true</c> if password; otherwise, <c>false</c>.
	/// </value>
	public override bool Password
	{
		get { return password; }
		set
		{
			password = value;
			
			// TODO:
			// Apply the setting to the text by replacing
			// all the visible text in the object with
			// the maskingCharacter.
		}
	}
	
	/// <summary>
	/// Gets or sets the masking character.
	/// </summary>
	/// <value>
	/// The masking character.
	/// </value>
	public override string MaskingCharacter
	{
		get { return maskingCharacter; }
		set
		{
			maskingCharacter = value;
			
			// TODO:
			// If password is set to true, apply
			// the new masking character to the
			// text.
		}
	}
	
	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="IUIText"/> is multiline.
	/// </summary>
	/// <value>
	/// <c>true</c> if multiline; otherwise, <c>false</c>.
	/// </value>
	public override bool Multiline
	{
		get { return multiline; }
		set
		{
			multiline = value;
			
			// TODO:
			// Apply the setting, if applicable.
		}
	}
	
	/// <summary>
	/// Gets or sets the anchor.
	/// </summary>
	/// <value>
	/// The anchor.
	/// </value>
	public override SpriteText.Anchor_Pos Anchor
	{
		get { return anchor; }
		set
		{
			anchor = value;
			
			// TODO:
			// Apply this setting to the text, if applicable.
			// NOTE: This is used by UITextField to determine
			// how to size the text object to fit the text
			// field control's margins.
			// SpriteText uses this value to position the
			// text area relative to the object's transform
			// center. So for example an anchor of Upper_Left
			// results in the top-left corner of the text
			// being oriented at the transform center.
			// This is basically the same behavior as with
			// Unity's own TextMesh's anchor setting.
		}
	}
	
	/// <summary>
	/// Gets or sets the max width of the text area in local space units.
	/// </summary>
	/// <value>
	/// The max width of the text area in local space units.
	/// </value>
	public override float MaxWidth
	{
		get { return maxWidth; }
		set
		{
			maxWidth = value;
			
			// TODO:
			// Apply the specified maximum width (in local space units)
			// to the text, such that if multi-line is enabled, the
			// text will be wrapped when it exceeds the max width, or
			// if multiline is false, it will be truncated.
		}
	}
	
	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="IUIText"/> should remove unsupported characters.
	/// </summary>
	/// <value>
	/// <c>true</c> if unsupported characters will be removed; otherwise, <c>false</c>.
	/// </value>
	public override bool RemoveUnsupportedCharacters
	{
		get { return removeUnsupportedCharacters; }
		set
		{
			removeUnsupportedCharacters = value;
			
			// TODO:
			// When true, any characters in the text string which
			// are not supported by the current font will be
			// ommitted in the output/display text string.
		}
	}
	
	/// <summary>
	/// Gets or sets a value indicating whether tags should be parsed out of the text entered.
	/// </summary>
	/// <value>
	/// <c>true</c> if tags should be parsed; otherwise, <c>false</c>.
	/// </value>
	public override bool ParseTags
	{
		get { return parseTags; }
		set
		{
			parseTags = value;
			
			// TODO:
			// Control whether or not tags should be parsed
			// in the current text.  This is important when
			// using the text object in a user text input
			// field where you don't want the text they
			// type to be interpreted as tags/markup, but
			// rather should always be treated literally.
		}
	}
	
	public override void UpdateCamera()
	{
		RenderCamera = renderCamera;
	}
	
	public override void SetCamera()
	{
		SetCamera(renderCamera);
	}
	
	public override void SetCamera(Camera cam)
	{
		RenderCamera = cam;
	}
	
	public override Camera RenderCamera
	{
		get { return renderCamera; }
		set
		{
			renderCamera = value;
			
			// TODO:
			// If relevant, this is the camera intended
			// to render the text so you can perform any
			// needed pixel-perfect computations, etc.
			// So this is where those computations can
			// be re-applied.
			// If not relevant, just ignore.
		}
	}
	
	public override void Start()
	{
		// TODO:
		// Any initialization here.
	}
}

#endif