using UnityEngine;
using System.Collections;

public class LayerEditForm : MonoBehaviour {

	public Dialog dialog;
	public int currentLayerId = -1;
	public static LayerEditForm instance;

	void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void OnClickDeleteLayer()
	{
		if(currentLayerId == -1) return;
		IsoLayerManager.instance.DeleteLayer(currentLayerId);
		currentLayerId = -1;
		dialog.TurnOn(false);
	}

	public void OnClickRename()
	{
		RenameLayerForm.instance.currentLayerId = currentLayerId;
		RenameLayerForm.instance.dialog.TurnOn(true); 
		dialog.TurnOn(false);
	}
}
