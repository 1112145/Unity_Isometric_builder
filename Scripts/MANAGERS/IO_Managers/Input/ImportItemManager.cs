using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ImportItemManager : MonoBehaviour {

	public static Sprite loadedImage; // Image load out of project.
	public static GameObject currentButtonContainer;
	public static ImportItemManager instance;

	#region private field
	private string path;
	private Texture2D texture;
	#endregion

	// Use this for initialization
	void Awake () {
		instance = this;
		Global.importItemManager = this;
	}
	
	// Update is called once per frame
	void Update () {
	}

	#region EVENT FUNCTION/ DELEGATE
	public void ImportNewItem()
	{
		OpenFileDialog opd = new OpenFileDialog();
		opd.Filter = "PNG Files|*.png";

		if(opd.ShowDialog() == DialogResult.OK)
		{
			CameraTool._isDragging = false;
			// Read PNG Files.
			path = opd.FileName;
			StartCoroutine(LoadImage("file:///" + path));

		}
		else {
			CameraTool._isDragging = false;
		}

	}
	#endregion

	#region PRIVATE FUNCTION

	IEnumerator LoadImage(string url)
	{
		texture = new Texture2D(4, 4, TextureFormat.DXT1, false);
		WWW www = new WWW(url);
		yield return www;
		www.LoadImageIntoTexture(texture);
		// Show PNG Images.
		PivotEditForm.instance.SetSprite(Ultils.ChangeOffset (texture));
		PivotEditForm.instance.dialog.ShowDiaLog(true);
	}

	#endregion

	#region STATIC FUNCTION
	public static void AddRightClickEvent (GameObject item)
	{
		EventTrigger evTrigger = item.AddComponent<EventTrigger> ();
		EventTrigger.Entry entry = new EventTrigger.Entry ();
		entry.eventID = EventTriggerType.PointerClick;

		entry.callback.AddListener (EventData =>  {
			if (((PointerEventData)EventData).button == PointerEventData.InputButton.Right) {
				MenuItemEditor.instance.currentItem = item;
				MenuItemEditor.instance.dialog.TurnOn(true);
			}
		});
		evTrigger.triggers.Add (entry);
	}
	#endregion

	public void AddItem ()
	{
		
		GameObject item = new GameObject ("item");
		Image imgItem = item.AddComponent<Image> ();

		AddRightClickEvent (item);

		UnityEngine.UI.Button btnItem = item.AddComponent<UnityEngine.UI.Button> ();

		Sprite sprite = Ultils.ChangeOffset (texture);
		imgItem.sprite = sprite;
		AspectRatioFitter ratioFitter = item.AddComponent<AspectRatioFitter> ();
		ratioFitter.aspectRatio = sprite.rect.width / sprite.rect.height;
		ratioFitter.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;

		item.transform.SetParent (currentButtonContainer.transform, false);
		item.transform.localScale = Vector3.one;
		item.transform.localPosition = Vector3.zero;
		IsoObjectFactory factory = item.AddComponent<IsoObjectFactory> ();
		factory.FilePath = path;
		factory.offset = PivotEditForm.realSpriteOffset;

		IsoLayerManager.currentLayer.isoFactories.Add (factory);
		btnItem.onClick.AddListener (() =>  {
			loadedImage = btnItem.image.sprite;
			IsoObjectFactory.instance = factory;
			IsoLayerManager.currentLayer.NewObject ();
		});
	}

	public void AddItem(Sprite sprite)
	{
		if(MenuItemEditor.instance.currentItem != null){
			//Copy file path
			path = MenuItemEditor.instance.currentItem.GetComponent<IsoObjectFactory>().FilePath;

			MenuItemEditor.instance.RemoveCurrentItem();
		}
		texture = sprite.texture;
		AddItem();
		Debug.Log(path);
	}
}
