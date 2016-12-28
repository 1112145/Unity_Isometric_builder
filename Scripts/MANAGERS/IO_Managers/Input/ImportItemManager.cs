using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using UnityEngine.UI;

public class ImportItemManager : MonoBehaviour {

	public static List<string> prefabsDefaultPath = new List<string>();
	public static int currentIndex = 0;
	public static string currentPrefabs;

	public static Sprite loadedImage; // Image load out of project.
	public static GameObject currentButtonContainer;
	public static ImportItemManager instance;
	private string path;
	private Texture2D texture;

	// Use this for initialization
	void Awake () {
		prefabsDefaultPath.Add("Prefabs/floor");
		prefabsDefaultPath.Add("Prefabs/object1");
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		currentPrefabs = prefabsDefaultPath[currentIndex];
	}

	public void SetCurrentPrefabs(int index)
	{
		currentIndex = index;
		currentPrefabs = prefabsDefaultPath[currentIndex];
		IsoLayerManager.currentLayer.NewObject();
	}

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


	IEnumerator LoadImage(string url)
	{
		texture = new Texture2D(4, 4, TextureFormat.DXT1, false);
		WWW www = new WWW(url);
		yield return www;
		www.LoadImageIntoTexture(texture);
		// Show PNG Images.
		Debug.Log(PivotEditForm.instance);

		PivotEditForm.instance.LoadImage(Ultils.ChangeOffset (texture));
		PivotEditForm.instance.dialog.ShowDiaLog(true);
	}

	public void AddItem ()
	{
		GameObject item = new GameObject ("item");
		Image imgItem = item.AddComponent<Image> ();
		UnityEngine.UI.Button btnItem = item.AddComponent<UnityEngine.UI.Button> ();
		Sprite sprite = Ultils.ChangeOffset (texture);
		imgItem.sprite = sprite;
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
}
