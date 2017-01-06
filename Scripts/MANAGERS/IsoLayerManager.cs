using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IsoLayerManager : MonoBehaviour
{
	#region CONSTANT
	public const string PATH_PREFAB_DIALOG = "Prefabs/Dialog_Object (1)";
	public const string PATH_PREFAB_BUTTON = "Prefabs/btn_demo_object_1";
	public const string STR_ADDTILE_BUTTON = "AddTile";
	public const string STR_SELECTING_LAYER = "Selecting Layer: ";
	public const string PREFIX_LAYER = "layer_";
	public const string STR_TOGGLE = "Toggle";
	#endregion

	#region PUBLIC Field
	public Text txtCurrenLayer;

	[HideInInspector]
	public int CurrentLayerIndex = 0;

	[HideInInspector]
	public string CurrentLayerName;

	public GameObject buttonLayerContainer;
	public GameObject groupDialog;
	public GameObject rootLayer;

	public static IsoLayerManager instance;
	public static Layer currentLayer;
	public static List<string> layernames = new List<string> ();
	public static List<GameObject> buttonLayers = new List<GameObject>();
	public static List<GameObject> dialogLayers = new List<GameObject>();
	public static List<GameObject> layerObjects = new List<GameObject>();

	[HideInInspector]
	public List<Layer> layers = new List<Layer> ();
	#endregion

	#region Private field
	private void Awake ()
	{
		IsoLayerManager.instance = this;
		Global.isoLayerManager = this;
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

	#endregion

	#region SELECT LAYER
	public void SelectLayer(Layer layer)
	{
		this.CurrentLayerIndex = layers.FindIndex(x => (x == layer));
		this.SetCurrentLayerName();
		IsoLayerManager.currentLayer = layer;
	}

	public void SelectLayer (int layerId)
	{
		this.CurrentLayerIndex = layers.FindIndex(x => (x.layerID == layerId));
		this.SetCurrentLayerName ();
		IsoLayerManager.currentLayer = this.layers [CurrentLayerIndex];
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
		IsoLayerManager.layernames.Add (PREFIX_LAYER + IsoLayerManager.layernames.Count);

		// Create layer to contain iso objects.
		int lastIndex = IsoLayerManager.layernames.Count -1;
		string layername = IsoLayerManager.layernames [lastIndex];
		int layerId = lastIndex;
		Layer layer = CreateLayer (layername,layerId);

		// Create a button to show layer menu.
		GameObject objButtonLayer = CreateNewButtonOnLayerMenu (layername);

		// set event listener for toggle to show/ hide this layer.
		Toggle tglLayerVisible = CreateToggle (objButtonLayer, layer);


		// Create a dialog (layer menu) to contain item type of this layer.
		GameObject menuItem = CreateLayerMenuItem ();


		SetOnClickNewButton (layer, objButtonLayer, menuItem);


	}

	public static Toggle CreateToggle (GameObject objButtonLayer, Layer layer)
	{
		Toggle tglLayerVisible = objButtonLayer.transform.FindChild (STR_TOGGLE).GetComponent<Toggle> ();
		tglLayerVisible.onValueChanged.AddListener (delegate(bool on) {
			layer.SetVisible (on);
		});
		return tglLayerVisible;
	}

	public static GameObject CreateLayerMenuItem ()
	{
		GameObject gameObject = (GameObject)Instantiate (Resources.Load<GameObject> (PATH_PREFAB_DIALOG), instance.groupDialog.transform);
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localPosition = Vector3.zero;
		Button component = gameObject.transform.GetChild (0).FindChild (STR_ADDTILE_BUTTON).GetComponent<Button> ();
		component.onClick.AddListener (delegate {
			ImportItemManager.instance.ImportNewItem ();
		});
		dialogLayers.Add(gameObject);
		return gameObject;
	}

	public static Layer CreateLayer (string layername, int layerId)
	{
		GameObject gameObject = new GameObject (layername);
		gameObject.transform.SetParent(IsoLayerManager.instance.rootLayer.transform);
		Layer layer = gameObject.AddComponent<Layer> ();
		layer.layerID = layerId;
		instance.layers.Add (layer);
		layerObjects.Add(layer.gameObject);
		return layer;
	}

	public static GameObject CreateNewButtonOnLayerMenu (string buttonName)
	{
		GameObject gameObject = (GameObject)Instantiate (Resources.Load<GameObject> (PATH_PREFAB_BUTTON), instance.buttonLayerContainer.transform);
		gameObject.transform.localScale = Vector3.one;
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

		EventTrigger evTrigger = newButton.AddComponent<EventTrigger> ();
		EventTrigger.Entry entry = new EventTrigger.Entry ();
		entry.eventID = EventTriggerType.PointerClick;

		entry.callback.AddListener (EventData =>  {
			if (((PointerEventData)EventData).button == PointerEventData.InputButton.Right) {
				LayerEditForm.instance.dialog.TurnOn(true);
				LayerEditForm.instance.currentLayerId = script.layerID;
			}
		});
		evTrigger.triggers.Add (entry);
	}

	#endregion

	#region DELETE LAYER
	public void DeleteLayer(int LayerID)
	{
		int index = layers.FindIndex(x => x.layerID == LayerID);
		Destroy(dialogLayers[index]);
		Destroy(buttonLayers[index]);
		Destroy(layerObjects[index]);
		Destroy(layers[index].gameObject);

		dialogLayers.RemoveAt(index);
		buttonLayers.RemoveAt(index);
		layerObjects.RemoveAt(index);
		layers.RemoveAt(index);
		layernames.RemoveAt(index);
	}

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
	#endregion

	#region RENAME LAYER
	public void RenameLayer(int LayerID, string NewName)
	{
		int index = layers.FindIndex(x => x.layerID == LayerID);
		layernames[index] = NewName;
		layerObjects[index].name = NewName;
		Text txtLayerName = buttonLayers[index].transform.GetChild(0).GetComponent<Text>();
		txtLayerName.text = NewName;
	}
	#endregion

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
