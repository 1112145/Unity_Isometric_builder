using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
	public static Vector2 realSpriteOffset = new Vector2(0.5f,0);

	public static PivotEditForm instance;

	private Vector2 orgPos;
	private Vector2 spriteRec;
	private Vector2 imageRec;

	// Use this for initialization

	void Awake ()
	{
		instance = this;
	}

	void Start ()
	{
		orgPos = pivotCursor.transform.localPosition;
		// Test. TODO: Delete after finalize.
		Sprite testSprite = Resources.Load<Sprite>("Images/floor2");
		LoadImage(testSprite);
	}
	
	// Update is called once per frame
	void Update ()
	{
	}


	public void OnClickApply ()
	{
		ImportItemManager.instance.AddItem ();
		dialog.ShowDiaLog (false);
	}

	public void OnClickCancel ()
	{
		dialog.ShowDiaLog (false);
	}

	public void UpdatePivotLocation ()
	{
		// How many pixels the pivot position far it's original position
		Vector2 deltaPos = (Vector2)pivotCursor.transform.localPosition - orgPos; 
		float ratio = imageRec.x / spriteRec.x;
		float x = Mathf.Floor(deltaPos.x / ratio);
		float y = Mathf.Floor(deltaPos.y / ratio);
		SetTextLocation(x,y);
		UpdateOffset ();
	}

	public void SetTextLocation(float x, float y)
	{
		dropdownXOffset.text = x.ToString();
		dropdownYOffset.text = y.ToString();
	}

	public Sprite LoadImage (Sprite sprite)
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

	void UpdateOffset ()
	{
		float xOffsetUI = (dropdownXOffset.text == "") ? 0 : float.Parse (dropdownXOffset.text);
		float yOffsetUI = (dropdownYOffset.text == "") ? 0 : float.Parse (dropdownYOffset.text);

		float realXOffset = xOffsetUI * imageRec.x / spriteRec.x;
		float realYOffset = yOffsetUI * imageRec.y / spriteRec.y;

		pivotCursor.transform.localPosition = orgPos + new Vector2 (realXOffset, realYOffset);
		ConvertToRealSpriteOffset (xOffsetUI, yOffsetUI);
	}

	public void ConvertToRealSpriteOffset (float x, float y)
	{
		// Caculate the real offset
		float xReal = x / spriteRec.x;
		float yReal = y / spriteRec.y;
		realSpriteOffset = new Vector2 (0.5f + xReal, yReal);
		Debug.Log ("Offset: " + realSpriteOffset);

	}

	// Reset Form
	public void ResetForm ()
	{
		
	}

}
