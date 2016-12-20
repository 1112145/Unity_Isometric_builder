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
		Texture2D texture;
		texture = new Texture2D(4, 4, TextureFormat.DXT1, false);
		WWW www = new WWW(url);
		yield return www;
		www.LoadImageIntoTexture(texture);
		// Show PNG Images.
		GameObject item = new GameObject("item");
		Image imgItem = item.AddComponent<Image>();
		UnityEngine.UI.Button btnItem = item.AddComponent<UnityEngine.UI.Button>();

		Sprite sprite = Sprite.Create(texture, new Rect(0,0,texture.width,texture.height), new Vector2(0.5f,0f));
		imgItem.sprite = sprite;
		item.transform.parent = currentButtonContainer.transform;
		item.transform.localScale = new Vector3(1,1,1);
		item.transform.localPosition = new Vector3(0,0,0);

		IsoObjectFactory factory = item.AddComponent<IsoObjectFactory>();
		factory.FilePath = path;

		btnItem.onClick.AddListener(() =>{
			loadedImage = btnItem.image.sprite;
			IsoObject obj =  IsoLayerManager.currentLayer.NewObject();
			obj.FilePath = factory.FilePath;
		});
	}
}
