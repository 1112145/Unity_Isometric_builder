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
	public Vector2 offset;
	public Quaternion rotation;

	// Convert from iso object to iso model.

	public IsoObjectModel FromIsoObject (IsoObject obj)
	{
		IsoObjectModel model = new IsoObjectModel ();

		model.position = obj.gameObject.transform.position;
		model.rotation = obj.gameObject.transform.rotation;
		SpriteRenderer objRenderer = obj.GetComponent<SpriteRenderer> ();
		model.SortingOrder = objRenderer.sortingOrder;
		model.ImgFilePath = obj.FilePath;
		model.offset = obj.offset;
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
	public List<IsoFactoryModel> FactoryModel = new List<IsoFactoryModel>();

	public IsoLayerModel FromLayer (Layer layer)
	{
		IsoLayerModel model = new IsoLayerModel ();
		model.layerId = layer.layerID;
		model.layerName = layer.gameObject.name;
		model.visible = (layer.gameObject.activeSelf) ? true : false; 

		for (int i = 0; i < layer.isoFactories.Count; i++) {
			model.FactoryModel.Add(new IsoFactoryModel(layer.isoFactories[i]));
		}

		for (int i = 0; i < layer.transform.childCount; i++) {
			IsoObject obj = layer.transform.GetChild (i).GetComponent<IsoObject> ();
			if(obj != null){
				IsoObjectModel objModel = new IsoObjectModel ();
				objModel = objModel.FromIsoObject (obj);
				model.objects.Add (objModel);
			}
		}

		return model;
	}



}

[Serializable]
public class IsoFactoryModel
{
	public string filePath;
	public Vector2 offset;

	public IsoFactoryModel(){}

	public IsoFactoryModel(IsoObjectFactory factory)
	{
		this.filePath = factory.FilePath;
		this.offset = factory.offset;
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