using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IsoLayerManager : MonoBehaviour
{
	public const string PATH_PREFAB_DIALOG = "Prefabs/Dialog_Object (1)";
	public const string PATH_PREFAB_BUTTON = "Prefabs/btn_demo_object_1";
	public const string STR_ADDTILE_BUTTON = "AddTile";
	public const string STR_SELECTING_LAYER = "Selecting Layer: ";
	public const string PREFIX_LAYER = "LAYER_";

	public Text txtCurrenLayer;

	[HideInInspector]
	public int CurrentLayerIndex = 0;

	[HideInInspector]
	public string CurrentLayerName;

	public GameObject buttonLayerContainer;
	public GameObject groupDialog;
	public static IsoLayerManager instance;
	public static Layer currentLayer;
	public static List<string> layernames = new List<string> ();
	public static List<GameObject> buttonLayers = new List<GameObject>();
	public static List<GameObject> dialogLayers = new List<GameObject>();
	public static List<GameObject> layerObjects = new List<GameObject>();

	[HideInInspector]
	public List<Layer> layers = new List<Layer> ();

	private void Awake ()
	{
		IsoLayerManager.instance = this;
	}

	private void Start ()
	{
		this.SetCurrentLayerName ();
	}

	private void Update ()
	{
		if (this.txtCurrenLayer != null) {
			this.txtCurrenLayer.text = STR_SELECTING_LAYER + this.CurrentLayerName;
		}
	}

	#region SELECT LAYER
	public void SelectLayer (int layerIndex)
	{
		this.CurrentLayerIndex = layerIndex;
		this.SetCurrentLayerName ();
		IsoLayerManager.currentLayer = this.layers [layerIndex];
		IsoLayerManager.instance = this;
	}

	private void SetCurrentLayerName ()
	{
		if (IsoLayerManager.layernames.Count == 0) {
			return;
		}
		this.CurrentLayerName = IsoLayerManager.layernames [this.CurrentLayerIndex];
	}
	#endregion

	#region CREATE NEW LAYER

	public void AddNewLayer ()
	{
		IsoLayerManager.layernames.Add ("layer_" + IsoLayerManager.layernames.Count);

		// Create layer to contain iso objects.
		string layername = IsoLayerManager.layernames [IsoLayerManager.layernames.Count -1];
		int layerId = IsoLayerManager.layernames.Count -1;
		Layer layer = CreateLayerContainer (layername,layerId);

		// Create a button to show layer menu.
		GameObject gameObject = CreateNewButtonOnLayerMenu (layername);

		// set event listener for toggle to show/ hide this layer.
		Toggle toggle = gameObject.transform.FindChild ("Toggle").GetComponent<Toggle> ();
		toggle.onValueChanged.AddListener (delegate(bool on) {
			layer.SetVisible (on);
		});

		// Create a dialog to contain item type of this layer.
		GameObject newDialog = CreateAItemDialog ();


		SetOnClickNewButton (layer, gameObject, newDialog);
	}


	public static GameObject CreateAItemDialog ()
	{
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate (Resources.Load<GameObject> ("Prefabs/Dialog_Object (1)"), instance.groupDialog.transform);
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localPosition = Vector3.zero;
		Button component = gameObject.transform.GetChild (0).FindChild ("AddTile").GetComponent<Button> ();
		component.onClick.AddListener (delegate {
			ImportItemManager.instance.ImportNewItem ();
		});
		dialogLayers.Add(gameObject);
		return gameObject;
	}

	public static Layer CreateLayerContainer (string layername, int layerId)
	{
		GameObject gameObject = new GameObject (layername);
		Layer layer = gameObject.AddComponent<Layer> ();
		layer.layerID = layerId;
		instance.layers.Add (layer);
		layerObjects.Add(layer.gameObject);
		return layer;
	}

	public static GameObject CreateNewButtonOnLayerMenu (string buttonName)
	{
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate (Resources.Load<GameObject> ("Prefabs/btn_demo_object_1"), instance.buttonLayerContainer.transform);
		gameObject.transform.localScale = new Vector3 (1f, 1f, 1f);
		gameObject.transform.localPosition = new Vector3 (gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, 0f);
		Text component = gameObject.transform.GetChild (0).GetComponent<Text> ();
		component.text = buttonName;
		buttonLayers.Add(gameObject);
		return gameObject;
	}

	public static void SetOnClickNewButton (Layer script, GameObject newButton, GameObject newDialog)
	{
		Button component = newButton.GetComponent<Button> ();
		component.onClick.AddListener (delegate {
			newDialog.GetComponent<Dialog> ().ShowDiaLog (true);
		});
		component.onClick.AddListener (delegate {
			IsoLayerManager.instance.SelectLayer (script.layerID);
		});
		component.onClick.AddListener (delegate {
			ImportItemManager.currentButtonContainer = newDialog.transform.GetChild (0).GetChild (0).gameObject;
		});
	}

	#endregion

	public void DeleteAll ()
	{
		foreach(var obj in buttonLayers) { Destroy(obj);}
		foreach(var obj in dialogLayers) { Destroy(obj);}
		foreach(var obj in layerObjects) { Destroy(obj);}

		buttonLayers.Clear();
		dialogLayers.Clear();
		layerObjects.Clear();
		layers.Clear();
		IsoLayerManager.layernames.Clear ();
	}

	#region FOCUS LAYER	
	public void FocusCurrentLayer ()
	{
		for (int i = 0; i < this.layers.Count; i++) {
			if (this.layers [i].layerID != this.CurrentLayerIndex) {
				for (int j = 0; j < this.layers [i].transform.childCount; j++) {
					IsoObject component = this.layers [i].transform.GetChild (j).GetComponent<IsoObject> ();
					if (component != null) {
						component.ReduceOpacity ();
					}
				}
			} else {
				for (int k = 0; k < this.layers [i].transform.childCount; k++) {
					IsoObject component2 = this.layers [i].transform.GetChild (k).GetComponent<IsoObject> ();
					if (component2 != null) {
						component2.ResetOpacity ();
					}
				}
			}
		}
	}

	public void ResetAllLayerFocus ()
	{
		for (int i = 0; i < this.layers.Count; i++) {
			for (int j = 0; j < this.layers [i].transform.childCount; j++) {
				IsoObject component = this.layers [i].transform.GetChild (j).GetComponent<IsoObject> ();
				if (component != null) {
					component.ResetOpacity ();
				}
			}
		}
	}

	#endregion
}
