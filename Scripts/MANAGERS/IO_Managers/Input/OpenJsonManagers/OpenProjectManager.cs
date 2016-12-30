using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System;
using UnityEngine.UI;


public class OpenProjectManager : MonoBehaviour
{
	public GameObject loading;
	public static IsoMetricRootModel InputRootModel;
	public static List<DownloadedResource> resources = new List<DownloadedResource> ();
	private List<string> urls = new List<string> ();
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	#region Load Resources

	void AddResourceItems ()
	{
		for (int i = 0; i < InputRootModel.layers.Count; i++) {
			for (int j = 0; j < InputRootModel.layers [i].FactoryModel.Count; j++) {
				IsoFactoryModel model = InputRootModel.layers [i].FactoryModel [j];
				if (!urls.Contains (model.filePath)) {
					urls.Add (model.filePath);
				}
			}
		}
	}

	IEnumerator LoadAllResources (Action<bool> callback)
	{
		yield return null;
		int nDone = 0;
		for (int i = 0; i < urls.Count; i++) {
			string url = urls [i];
			StartCoroutine (LoadItemMenuImg (url, (result) => {
				resources.Add (result);
				nDone++;
				if (nDone == urls.Count) {
					callback (true);
				}
			}));
		}
	}

	public IEnumerator LoadItemMenuImg (string url, Action<DownloadedResource> callback)
	{
		Texture2D texture = null;
		WWW www = new WWW ("file:///" + url);
		yield return www;
		texture = www.texture;
		Sprite sprite = Ultils.ChangeOffset (texture);
		DownloadedResource rs = new DownloadedResource (url, sprite);
		callback (rs);

	}

	#endregion

	public void OpenFile ()
	{
		IsoLayerManager.instance.DeleteAll ();

		OpenFileDialog ofd = new OpenFileDialog ();
		ofd.Filter = "JSON Files |*.json";
		if (ofd.ShowDialog () == DialogResult.OK) {
			InputRootModel = JsonUtility.FromJson<IsoMetricRootModel> (File.ReadAllText (ofd.FileName));
			AddResourceItems ();
			ExportFileManager.currentPath = ofd.FileName;
			loading.SetActive (true);
			StartCoroutine (LoadAllResources ((done) => {
				if (done) {
					RenderRootModel ();
					loading.SetActive (false);
				}
			}));

			CameraTool._isDragging = false;
		}

	}

	void RenderRootModel ()
	{
		for (int i = 0; i < InputRootModel.layers.Count; i++) {
			RenderLayer (InputRootModel.layers [i]);
		}
		IsoLayerManager.instance.SelectLayer (0);
	}

	#region Render Layer

	Layer CreateEmptyLayer (IsoLayerModel model)
	{
		IsoLayerManager.layernames.Add (model.layerName);
		Layer layer = IsoLayerManager.CreateLayer (model.layerName, model.layerId);
		layer.gameObject.SetActive (model.visible);
		return layer;
	}

	void CreateLayerToggle (IsoLayerModel model, Layer layer, GameObject button)
	{
		Toggle toggle = IsoLayerManager.CreateToggle (button, layer);
		toggle.isOn = model.visible;
	}

	void RenderLayer (IsoLayerModel model)
	{
		Layer layer = CreateEmptyLayer (model);

		GameObject buttonGameObject = IsoLayerManager.CreateNewButtonOnLayerMenu (model.layerName);

		CreateLayerToggle (model, layer, buttonGameObject);

		GameObject dialog = IsoLayerManager.CreateLayerMenuItem ();

		IsoLayerManager.SetOnClickNewButton (layer, buttonGameObject, dialog);

		RenderMenuItem (model, dialog, layer);

		for (int i = 0; i < model.objects.Count; i++) {
			RenderObject (model.objects [i], layer);
		}


	}

	#endregion

	#region Render MenuItem

	void AddRatioFitter (GameObject item, Image imgItem)
	{
		AspectRatioFitter ratioFitter = item.AddComponent<AspectRatioFitter> ();
		ratioFitter.aspectRatio = imgItem.sprite.rect.width / imgItem.sprite.rect.height;
		ratioFitter.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
	}

	void SetItemParent (GameObject parent, GameObject item)
	{
		item.transform.SetParent (parent.transform.GetChild (0).GetChild (0), false);
		item.transform.localScale = Vector3.one;
		item.transform.localPosition = Vector3.zero;
	}

	IsoObjectFactory AddObjectFactory (string url, Vector2 offset, GameObject item)
	{
		IsoObjectFactory factory = item.AddComponent<IsoObjectFactory> ();
		factory.FilePath = url;
		factory.offset = offset;
		return factory;
	}

	void RenderMenuItem (IsoLayerModel data, GameObject parent, Layer belongLayer)
	{
		for (int i = 0; i < data.FactoryModel.Count; i++) {
			

			GameObject item = new GameObject ("item");
			ImportItemManager.AddRightClickEvent (item);

			// Add image component.
			Image imgItem = item.AddComponent<Image> ();
			string url = data.FactoryModel [i].filePath;
			Vector2 offset = data.FactoryModel [i].offset;
			Sprite result = resources.Find (x => x.path == url).Sprite;
			result = Ultils.ChangeOffset (result, offset);
			imgItem.sprite = result;

			AddRatioFitter (item, imgItem);
			SetItemParent (parent, item);
			IsoObjectFactory factory = item.AddComponent<IsoObjectFactory> ();
			factory.FilePath = url;
			factory.offset = offset;
			belongLayer.isoFactories.Add (factory);

			UnityEngine.UI.Button btnItem = item.AddComponent<UnityEngine.UI.Button> ();
			btnItem.onClick.AddListener (() => {
				ImportItemManager.loadedImage = btnItem.image.sprite;
				IsoObjectFactory.instance = factory;
				belongLayer.NewObject ();
			});
		}
	}

	#endregion

	#region Render Object

	void RenderObject (IsoObjectModel objModel, Layer layer)
	{
		GameObject obj = new GameObject ("obj");
		obj.transform.position = objModel.position;
		obj.transform.rotation = objModel.rotation;
		obj.transform.SetParent (layer.gameObject.transform);


		IsoObject isoObj = obj.AddComponent<IsoObject> ();
		isoObj.FilePath = objModel.ImgFilePath;
		isoObj.offset = objModel.offset;
		layer.AddPositionData (obj.transform.position, obj);
		// Read image;
		SpriteRenderer renderer = obj.AddComponent<SpriteRenderer> ();
		renderer.sortingOrder = objModel.SortingOrder;
		Sprite result = resources.Find (x => x.path == isoObj.FilePath).Sprite;
		result = Ultils.ChangeOffset (result, isoObj.offset);
		renderer.sprite = result;

		obj.AddComponent<PolygonCollider2D> ();

	}

	#endregion
}

public class DownloadedResource
{
	public string path;
	public Sprite Sprite;

	public DownloadedResource ()
	{
	}

	public DownloadedResource (string path, Sprite sprite)
	{
		this.path = path;
		this.Sprite = sprite;
	}
}
