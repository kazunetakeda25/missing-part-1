//-----------------------------------------------------------------
//  Copyright 2014 Brady Wright and Above and Beyond Software
//	All rights reserved
//-----------------------------------------------------------------

using UnityEngine;

public interface IUIText : IUseCamera
{	
	/// <summary>
	/// Sets/gets the full, decorated (color ) text string.
	/// </summary>
	string Text
	{
		get;
		set;
	}
	
	/// <summary>
	/// Sets/gets whether or not the object is persistent 
	/// (survives scene loads via DontDestroyOnLoad()).
	/// </summary>
	bool Persistent
	{
		get;
		set;
	}
	
	/// <summary>
	/// Sets/gets the control that is the parent to this
	/// text object, if any.
	/// </summary>
	IControl Parent
	{
		get;
		set;
	}
	
	/// <summary>
	/// Gets or sets the color.
	/// </summary>
	/// <value>
	/// The color.
	/// </value>
	Color Color
	{
		get;
		set;
	}
	
	/// <summary>
	/// Sets the color.
	/// </summary>
	/// <param name='c'>
	/// C.
	/// </param>
	void SetColor(Color c);
	
	/// <summary>
	/// Undoes any clipping on the text
	/// </summary>
	void Unclip();
	
	/// <summary>
	/// Gets/Sets the status of clipping on the object.
	/// </summary>
	bool Clipped
	{
		get;
		set;
	}
	
	/// <summary>
	/// Gets/Sets the clipping rect of the object.
	/// (When setting, always sets Clipped to true.)
	/// </summary>
	Rect3D ClippingRect
	{
		get;
		set;
	}
	
	/// <summary>
	/// Hides or displays the text by disabling/enabling
	/// the mesh renderer component.
	/// </summary>
	/// <param name="tf">When true, the text is hidden, when false, the text will be displayed.</param>
	void Hide(bool tf);

	/// <summary>
	/// Returns whether the text is currently set to be hidden
	/// (whether its mesh renderer component is enabled).
	/// </summary>
	/// <returns>True when hidden, false when set to be displayed.</returns>
	bool IsHidden();

	/// <summary>
	/// Gets the position of the top-left corner of
	/// the text's extents, after clipping, in local space.
	/// </summary>
	Vector3 TopLeft
	{
		get;
	}
	
	/// <summary>
	/// Gets the position of the bottom-right corner of
	/// the text's extents, after clipping, in local space.
	/// </summary>
	Vector3 BottomRight
	{
		get;
	}

	/// <summary>
	/// Gets the position of the top-left corner of
	/// the text's extents, before clipping, in local space.
	/// </summary>
	Vector3 UnclippedTopLeft
	{
		get;
	}
	
	/// <summary>
	/// Gets the position of the bottom-right corner of
	/// the text's extents, before clipping, in local space.
	/// </summary>
	Vector3 UnclippedBottomRight
	{
		get;
	}
	
	/// <summary>
	/// Gets a value indicating whether the text's width is limited.
	/// </summary>
	/// <value>
	/// <c>true</c> if width limited; otherwise, <c>false</c>.
	/// </value>
	bool IsWidthLimited
	{
		get;
	}
	
	/// <summary>
	/// Performs a deep copy of the text object.
	/// </summary>
	/// <param name='text'>
	/// The object to be copied.
	/// </param>
	void Copy(IUIText text);
	
	/// <summary>
	/// Sets/Gets the Z-offset of the text.
	/// </summary>
	float ZOffset
	{
		get;
		set;
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
	Vector3 GetInsertionPointPos(int charIndex);
		
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
	int GetNearestInsertionPoint(Vector3 point);
	
	/// <summary>
	/// Returns the distance from the baseline to the
	/// top of a line, in local space units.
	/// </summary>
	float BaseHeight
	{
		get;
	}
	
	/// <summary>
	/// The distance, in local space units, from the top of
	/// one line to the top of the next.
	/// </summary>
	float LineSpan
	{
		get;
	}

	/// <summary>
	/// Converts a character index in the plain text string
	/// to a corresponding index in the display string.
	/// </summary>
	/// <param name="plainCharIndex">The index in the plain string to be converted.</param>
	/// <returns>The index in the display string that corresponds to the provided index in the plain text string.</returns>
	int PlainIndexToDisplayIndex(int plainCharIndex);

	/// <summary>
	/// Converts a character index in the display text string
	/// to a corresponding index in the plain text string.
	/// </summary>
	/// <param name="plainCharIndex">The index in the display string to be converted.</param>
	/// <returns>The index in the plain text string that corresponds to the provided index in the display text string.</returns>
	int DisplayIndexToPlainIndex(int dispCharIndex);
	
	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="IUIText"/> is password.
	/// </summary>
	/// <value>
	/// <c>true</c> if password; otherwise, <c>false</c>.
	/// </value>
	bool Password
	{
		get;
		set;
	}
	
	/// <summary>
	/// Gets or sets the masking character.
	/// </summary>
	/// <value>
	/// The masking character.
	/// </value>
	string MaskingCharacter
	{
		get;
		set;
	}
	
	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="IUIText"/> is multiline.
	/// </summary>
	/// <value>
	/// <c>true</c> if multiline; otherwise, <c>false</c>.
	/// </value>
	bool Multiline
	{
		get;
		set;
	}
	
	/// <summary>
	/// Gets or sets the anchor.
	/// </summary>
	/// <value>
	/// The anchor.
	/// </value>
	SpriteText.Anchor_Pos Anchor
	{
		get;
		set;
	}
	
	/// <summary>
	/// Gets or sets the max width of the text area in local space units.
	/// </summary>
	/// <value>
	/// The max width of the text area in local space units.
	/// </value>
	float MaxWidth
	{
		get;
		set;
	}
	
	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="IUIText"/> should remove unsupported characters.
	/// </summary>
	/// <value>
	/// <c>true</c> if unsupported characters will be removed; otherwise, <c>false</c>.
	/// </value>
	bool RemoveUnsupportedCharacters
	{
		get;
		set;
	}
	
	/// <summary>
	/// Gets or sets a value indicating whether tags should be parsed out of the text entered.
	/// </summary>
	/// <value>
	/// <c>true</c> if tags should be parsed; otherwise, <c>false</c>.
	/// </value>
	bool ParseTags
	{
		get;
		set;
	}
	
	Transform transform
	{
		get;
	}
	
	GameObject gameObject
	{
		get;
	}
}
