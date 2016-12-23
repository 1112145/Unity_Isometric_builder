using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

[Serializable]
public class IsoObjectModel
{
	public string ImgFileName;
	public string ImgFilePath;
	public string ImgFolderPath;

	public int SortingOrder;
	public Vector3 position;

	// Convert from iso object to iso model.

	public IsoObjectModel FromIsoObject (IsoObject obj)
	{
		IsoObjectModel model = new IsoObjectModel ();

		model.position = obj.gameObject.transform.position;
		model.SortingOrder = obj.GetComponent<SpriteRenderer> ().sortingOrder;
		model.ImgFilePath = obj.FilePath;
		model.ImgFileName = Path.GetFileName (obj.FilePath);
		model.ImgFolderPath = Path.GetDirectoryName (obj.FilePath);
		return model;
	}
}

[Serializable]
public class IsoLayerModel
{
	public int layerId;
	public string layerName;
	public bool visible;
	public List<IsoObjectModel> objects = new List<IsoObjectModel>();


	public IsoLayerModel FromLayer (Layer layer)
	{
		IsoLayerModel model = new IsoLayerModel ();
		model.layerId = layer.layerID;
		model.layerName = layer.gameObject.name;
		model.visible = (layer.gameObject.activeSelf) ? true : false; 

		for (int i = 0; i < layer.transform.childCount; i++) {
			IsoObject obj = layer.transform.GetChild (i).GetComponent<IsoObject> ();
			IsoObjectModel objModel = new IsoObjectModel ();
			objModel = objModel.FromIsoObject (obj);
			model.objects.Add (objModel);
		}

		return model;
	}

}

[Serializable]
public class IsoMetricRootModel
{
	public List<IsoLayerModel> layers = new List<IsoLayerModel> ();

	public void ConvertAllLayer ()
	{
		for (int i = 0; i < IsoLayerManager.layernames.Count; i++) {
			GameObject objLayer = IsoLayerManager.instance.layers [i].gameObject;	
			Layer isolayer = objLayer.GetComponent<Layer> ();
			IsoLayerModel model = new IsoLayerModel ();
			model = model.FromLayer (isolayer);
			layers.Add (model);
		}
	}
}