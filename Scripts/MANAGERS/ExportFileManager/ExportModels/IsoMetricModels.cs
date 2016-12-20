using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class IsoObjectModel {
	public string ImgFileName;
	public string ImgFilePath;
	public int SortingOrder;
	public Vector3 position;

	public IsoObjectModel FromIsoObject(IsoObject obj)
	{
		IsoObjectModel model = new IsoObjectModel();
		// Convert from obj to model.

		model.position = obj.gameObject.transform.position;
		model.SortingOrder = obj.GetComponent<SpriteRenderer>().sortingOrder;
		model.ImgFilePath = obj.FilePath;
		// Then return model.
		return model;
	}
}


[Serializable]
public class IsoLayerModel 
{
	public int layerId;
	public List<IsoObjectModel> objects;
	public bool visible;


	public IsoLayerModel FromLayer(Layer layer)
	{
		IsoLayerModel layermodel = new IsoLayerModel();
		layermodel.layerId = layer.layerID;
		layermodel.objects = new List<IsoObjectModel>();
		layermodel.visible = (layer.gameObject.activeSelf)? true: false; 
		for (int i = 0; i < layer.transform.childCount; i++) {
			IsoObject obj = layer.transform.GetChild(i).GetComponent<IsoObject>();
			IsoObjectModel objModel = new IsoObjectModel();
			objModel = objModel.FromIsoObject(obj);
			layermodel.objects.Add(objModel);
		}
		return layermodel;
	}

}

[Serializable]
public class IsoMetricRootModel
{
	public List<IsoLayerModel> layers = new List<IsoLayerModel>();

	public void ConvertAllLayer()
	{
		for (int i = 0; i < IsoLayerManager.layers.Count; i++) {
			GameObject objLayer = GameObject.Find(IsoLayerManager.PREFIX_LAYER + IsoLayerManager.layers[i]);	
			Layer isolayer = objLayer.GetComponent<Layer>();
			IsoLayerModel model = new IsoLayerModel();
			model = model.FromLayer(isolayer);
			layers.Add(model);
		}
	}
}