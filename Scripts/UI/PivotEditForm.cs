using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PivotEditForm : MonoBehaviour {

	public GameObject pivotCursor;
	public Image imgSprite;
	public Image imgSpriteContent;
	public Text txtPivotLocation;
	public InputField dropdownXOffset;
	public InputField dropdownYOffset;
	public Dialog dialog;

	private float OffsetX;
	private float OffsetY;
	private Vector2 realSpriteOffset;

	public static PivotEditForm instance;

	// Use this for initialization

	void Awake()
	{
		instance = this;
		Debug.Log(" PivotEditForm Awake");
	}

	void Start () {
		//Test function
		Sprite sprite = Resources.Load<Sprite>("Images/floor2");
		Debug.Log("Size: W: " + sprite.rect.width + " , H: " + sprite.rect.height);
		LoadImage(sprite);
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void OnClickApply()
	{
		Debug.Log("Apply!");
		ImportItemManager.instance.AddItem();
		dialog.ShowDiaLog(false);
	}

	public void OnClickCancel()
	{
		Debug.Log("Cancel!");
		dialog.ShowDiaLog(false);
	}

	public void SetTextPivotLocation()
	{
		Vector2 pivot = pivotCursor.transform.localPosition;
		txtPivotLocation.text = "Offset: " + " ( " + pivot.x + "," + pivot.y + ")";
	}


	public void LoadImage(Sprite sprite)
	{
		imgSpriteContent.sprite = sprite;
		AspectRatioFitter ratio = imgSpriteContent.GetComponent<AspectRatioFitter>();
		ratio.aspectRatio = sprite.rect.width / sprite.rect.height;
	}

	public void OnValueChange()
	{
		UpdateOffset();
	}

	void UpdateOffset()
	{
	}

	public void ConvertToRealSpriteOffset(float x, float y)
	{
		// Caculate the real offset

	}

	// Reset Form
	public void ResetForm()
	{
		
	}

}
