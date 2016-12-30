using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// TODO: Sprite Bounds Pivot Edit Form
using System;


public class PivotEditForm : MonoBehaviour
{
	public GameObject pivotCursor;
	public Image imgSprite;
	public Image imgSpriteContent;
	public Text txtPivotLocation;
	public Text txtSize;
	public InputField dropdownXOffset;
	public InputField dropdownYOffset;
	public Dialog dialog;

	private float OffsetX;
	private float OffsetY;
	public static Vector2 realSpriteOffset = new Vector2 (0.5f, 0);

	public static PivotEditForm instance;

	private Vector2 orgPos;
	private Vector2 spriteRec;
	private Vector2 imageRec;

	// Use this for initialization

	void Awake ()
	{
		instance = this;
		orgPos = pivotCursor.transform.localPosition;
	}

	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	}


	public void OnClickApply ()
	{
		ImportItemManager.instance.AddItem (imgSpriteContent.sprite);
		dialog.TurnOn (false);
	}

	public void OnClickCancel ()
	{
		dialog.TurnOn (false);
	}

	public void UpdatePivotLocation ()
	{
		// How many pixels the pivot position far it's original position
		Vector2 deltaPos = (Vector2)pivotCursor.transform.localPosition - orgPos; 
		float ratio = imageRec.x / spriteRec.x;
		float x = Mathf.Floor (deltaPos.x / ratio);
		float y = Mathf.Floor (deltaPos.y / ratio);
		SetTextLocation (x, y);
		UpdateOffset ();
	}

	public void SetTextLocation (float x, float y)
	{
		dropdownXOffset.text = x.ToString ();
		dropdownYOffset.text = y.ToString ();
	}

	public Sprite SetSprite (Sprite sprite)
	{
		imgSpriteContent.sprite = sprite;

		AspectRatioFitter ratio = imgSpriteContent.GetComponent<AspectRatioFitter> ();
		ratio.aspectRatio = sprite.rect.width / sprite.rect.height;

		txtSize.text = "Size:   " + sprite.rect.width + ", " + sprite.rect.height;
		spriteRec = new Vector2 (sprite.rect.width, sprite.rect.height);
		imageRec = new Vector2 (512 * ratio.aspectRatio, 512);
		return sprite;
	}



	public void OnEndEdit ()
	{
		UpdateOffset ();
	}

	public void OnValueChanged ()
	{
		UpdateOffset ();
	}

	void UpdateOffset ()
	{
		float xOffsetUI = 0;
		float yOffsetUI = 0;

		float.TryParse (dropdownXOffset.text,out xOffsetUI);
		float.TryParse (dropdownYOffset.text,out yOffsetUI);

		if(float.IsNaN(xOffsetUI)) xOffsetUI = 0;
		if(float.IsNaN(yOffsetUI)) yOffsetUI = 0;

		float realXOffsetWorld = xOffsetUI * imageRec.x / spriteRec.x;
		float realYOffsetWorld = yOffsetUI * imageRec.y / spriteRec.y;

		pivotCursor.transform.localPosition = orgPos + new Vector2 (realXOffsetWorld, realYOffsetWorld);
		ConvertToRealSpriteOffset (xOffsetUI, yOffsetUI);
	}

	public void ConvertToRealSpriteOffset (float x, float y)
	{
		// Caculate the real offset
		float xRealWorld = x / spriteRec.x;
		float yRealWorld = y / spriteRec.y;
		realSpriteOffset = new Vector2 (0.5f + xRealWorld, yRealWorld);
	}


	public Vector2 RealOffsetToUI (Vector2 realSpriteOffset)
	{
		float xReal = realSpriteOffset.x - 0.5f;
		float yReal = realSpriteOffset.y;
		return new Vector2 (spriteRec.x * xReal, spriteRec.y * yReal);
	}

	public void RenderUIFromOffset (Vector2 offset)
	{
		dropdownXOffset.text = Mathf.Round (offset.x).ToString ();
		dropdownYOffset.text = Mathf.Round (offset.y).ToString ();
	}

}
