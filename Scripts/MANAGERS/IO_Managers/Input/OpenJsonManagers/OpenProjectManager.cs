using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System;
using UnityEngine.UI;

public class OpenProjectManager : MonoBehaviour
{

	public static IsoMetricRootModel InputRootModel;

	public static int version = 0;

	public static List<string> loadedUrls = new List<string>();
	public static List<Sprite> loadedSprites = new List<Sprite>();
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void OpenFile ()
	{
		IsoLayerManager.instance.DeleteAll ();

		OpenFileDialog ofd = new OpenFileDialog ();
		ofd.Filter = "JSON Files |*.json";
		if (ofd.ShowDialog () == DialogResult.OK) {
			InputRootModel = JsonUtility.FromJson<IsoMetricRootModel> (File.ReadAllText (ofd.FileName));
			RenderRootModel ();
			CameraTool._isDragging = false;
		}
	}

	public void RenderRootModel ()
	{
		for (int i = 0; i < InputRootModel.layers.Count; i++) {
			// Create Layer
			RenderLayer (InputRootModel.layers [i]);
		}
		IsoLayerManager.instance.SelectLayer (0);
	}

	public void RenderLayer (IsoLayerModel model)
	{
		IsoLayerManager.layernames.Add (model.layerName);
		Layer layer = IsoLayerManager.CreateLayerContainer (model.layerName, model.layerId);
		layer.gameObject.SetActive (model.visible);

		GameObject buttonGameObject = IsoLayerManager.CreateNewButtonOnLayerMenu (model.layerName);
		Toggle toggle = buttonGameObject.transform.FindChild ("Toggle").GetComponent<Toggle> ();
		toggle.onValueChanged.AddListener ((on) => {
			layer.SetVisible (on);
		});

		toggle.isOn = model.visible;

		GameObject dialog = IsoLayerManager.CreateAItemDialog ();

		IsoLayerManager.SetOnClickNewButton (layer, buttonGameObject, dialog);

		for (int i = 0; i < model.objects.Count; i++) {
			RenderObject (model.objects [i], layer);
		}
	}

	public void RenderObject (IsoObjectModel objModel, Layer layer)
	{
		GameObject obj = new GameObject ("obj");
		obj.transform.position = objModel.position;
		obj.transform.SetParent (layer.gameObject.transform);

		SpriteRenderer renderer = obj.AddComponent<SpriteRenderer> ();
		renderer.sortingOrder = objModel.SortingOrder;
		IsoObject isoObj = obj.AddComponent<IsoObject> ();
		isoObj.FilePath = objModel.ImgFilePath;
		layer.AddPositionData (obj.transform.position, obj);


		// Read image;
		StartCoroutine (LoadItemImage (objModel.ImgFilePath, (result) => {
			renderer.sprite = result;
			obj.AddComponent<PolygonCollider2D> ();
		}));
	}

//	IEnumerator LoadItemImage (string url, Action<Sprite> callback)
//	{
//		Texture2D texture = null;
//		WWW www = new WWW("file:///" + url);
//		yield return www;
//		texture = www.texture;
//
//		Sprite sprite = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0.5f, 0f));
//		callback (sprite);
//	}

	IEnumerator LoadItemImage (string url, Action<Sprite> callback)
	{
		Texture2D texture = null;
		if(!loadedUrls.Contains(url)){
			WWW www = new WWW("file:///" + url);
			yield return www;
			texture = www.texture;
			Sprite sprite = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0.5f, 0f));

			loadedUrls.Add(url);
			loadedSprites.Add(sprite);
			callback (sprite);
		}
		else
		{
			yield return null;
			callback(loadedSprites[loadedUrls.IndexOf(url)]);
		}
	}
}


