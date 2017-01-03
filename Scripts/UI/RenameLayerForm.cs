using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RenameLayerForm : MonoBehaviour {

	public InputField inputRename;
	public Dialog dialog;
	public static RenameLayerForm instance;
	public int currentLayerId = -1;

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

	public void OnClickRenameButton()
	{
		IsoLayerManager layerMgr = IsoLayerManager.instance;
		dialog.TurnOn(false);
		layerMgr.RenameLayer(currentLayerId,inputRename.text);
	}
}
