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
		Debug.Log (" PivotEditForm Awake");
	}

	void Start ()
	{
		orgPos = pivotCursor.transform.localPosition;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}


	public void OnClickApply ()
	{
		Debug.Log ("Apply!");

		ImportItemManager.instance.AddItem ();
		dialog.ShowDiaLog (false);
	}

	public void OnClickCancel ()
	{
		Debug.Log ("Cancel!");
		dialog.ShowDiaLog (false);
	}

	public void SetTextPivotLocation ()
	{
		Vector2 pivot = pivotCursor.transform.localPosition;
		txtPivotLocation.text = "Offset: " + " ( " + pivot.x + "," + pivot.y + ")";
	}


	public void LoadImage (Sprite sprite)
	{
		imgSpriteContent.sprite = sprite;
		AspectRatioFitter ratio = imgSpriteContent.GetComponent<AspectRatioFitter> ();
		ratio.aspectRatio = sprite.rect.width / sprite.rect.height;
		txtSize.text = "Size:   " + sprite.rect.width + ", " + sprite.rect.height;
		spriteRec = new Vector2 (sprite.rect.width, sprite.rect.height);
		imageRec = new Vector2 (512 * ratio.aspectRatio, 512);
	}

	public void OnEndEdit ()
	{
		UpdateOffset ();
	}

	void UpdateOffset ()
	{
		//TODO: Nhan gia tri tu input field.
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
		Debug.Log (y + "," + spriteRec.y);
		Debug.Log ("Offset: " + realSpriteOffset);

	}

	// Reset Form
	public void ResetForm ()
	{
		
	}

}
