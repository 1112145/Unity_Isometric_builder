using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// Every Layer object on scene has owner Layer Builder on the inspector to build this layer.
public class Layer : MonoBehaviour
{
	public const string PATH_TILE_UNIT_SPRITE = "Images/tile";

	#region PUBLIC FIELD
	public enum MouseAction
	{
		none,
		cellhold
	}
	[HideInInspector]
	public GameObject currentObj;
	[HideInInspector]
	public bool canSnap = true;
	[HideInInspector]
	public MouseAction currentAction = MouseAction.none;
	public int layerID;
	[HideInInspector]
	public bool startBuild = false;
	[HideInInspector]
	public Hashtable positionData = new Hashtable ();
	#endregion 

	#region PRIVATE FIELD
	private int MAX_TILE_PER_LAYER = 512;
	private GameObject shadow;
	private float _unitHeight;
	private float _unitWidth;
	#endregion


	// Use this for initialization
	void Start ()
	{
		GetUnitSize ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (layerID != IsoLayerManager.instance.CurrentLayerIndex 
			&& IsoLayerManager.instance!= null)
			return;
		
		if (currentAction == MouseAction.cellhold) {
			// Change the mouse cursor into tile image.
			HoldObjectAndMove ();
			// Make a shadow of the floor tile on the grid.
			ShadowDebug (currentObj.transform.position);
			// Build
			BuildLayer ();

		}
	}

	#region SORTING ORDER FUNCTION
	public void SortOrderByYAxis(GameObject obj)
	{
		obj.GetComponent<SpriteRenderer> ().sortingOrder = Mathf.RoundToInt (obj.transform.position.y * 100f) * -1 + layerID * MAX_TILE_PER_LAYER;
	}
	#endregion

	#region BUILD LAYER FUNCTIONS
	void OnStartBuild ()
	{
		if (Input.GetMouseButtonDown (0) && !startBuild) {
			startBuild = true;
			CameraTool._isDragging = false;
		}
	}

	void OnBuild ()
	{
		if (startBuild) {
			// Focus on this layer.
			IsoLayerManager.instance.FocusCurrentLayer();
			if (canSnap) {
				//Snap it to grid!
				SnapObject ();
				// Clear cell. New Cell;
				currentObj = null;
				NewObject ();

			}
		}
	}

	void OnEndBuild ()
	{

		if (Input.GetMouseButtonUp (0) && startBuild ) {
			startBuild = false;
			IsoLayerManager.instance.ResetAllLayerFocus();
			NewObject ();
		}

		if (Input.GetMouseButtonDown (1)) {
			Destroy (currentObj);
			Destroy (shadow);
			ImportItemManager.loadedImage = null;
			currentAction = MouseAction.none;
			startBuild = false;
		}
	}

	void BuildLayer ()
	{
		OnStartBuild ();
		OnBuild ();
		OnEndBuild ();
	}
	#endregion

	#region PROCESS CURRENT OBJECT
	public void HoldObjectAndMove ()
	{
		currentObj.transform.position = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		currentObj.transform.position = new Vector3 (currentObj.transform.position.x, currentObj.transform.position.y, 0);
		SortOrderByYAxis(currentObj);
		currentObj.GetComponent<IsoObject> ().state = IsoObject.State.isHolding;
	}

	public IsoObject NewObject ()
	{
		IsoObject obj = null;
		currentAction = MouseAction.cellhold;
		if (currentObj == null) {
			currentObj = (GameObject)Instantiate (Resources.Load<GameObject> (ImportItemManager.currentPrefabs), this.transform);
			currentObj.transform.position = new Vector3 (-1000f, -1000f, -1000f);
			obj = currentObj.GetComponent<IsoObject> ();
			obj.state = IsoObject.State.isHolding;
			if(ImportItemManager.loadedImage != null)
			{
				// Change prefabs image. In case of image is loaded from outside project.
				currentObj.GetComponent<SpriteRenderer> ().sprite = ImportItemManager.loadedImage;
			}
		}
		return obj;
	}

	private void SnapObject ()
	{
		SnapObject(currentObj);
	}

	public void SnapObject(GameObject obj)
	{
		obj.transform.position = shadow.gameObject.transform.position;
		obj.GetComponent<IsoObject> ().Reset();
		SortOrderByYAxis(obj);
		AddPositionData(obj.transform.position);
	}

	public void DeleteObjectAtPosition(Vector3 pos)
	{
		string hashKey = pos.x + "," + pos.y;
		if(positionData[hashKey] != null)
		{
			Destroy((GameObject)positionData[hashKey]);
			RemoveDataAtPosition(pos);
		}
	}
	#endregion

	#region POSITION DATA ON THIS LAYER
	public void AddPositionData(Vector3 pos)
	{
		string hashKey = pos.x + "," + pos.y;
		if(positionData[hashKey] == null) positionData.Add(hashKey,currentObj);
	}

	public void RemoveDataAtPosition(Vector3 pos)
	{
		string hashKey = pos.x + "," + pos.y;
		positionData.Remove(hashKey);
	}

	public void ClearAllData()
	{
		positionData.Clear();
	}
	#endregion

	#region UTILITY FUNCTION
	public Vector3 ToIsoPosition(Vector3 pos)
	{
		Vector3 tileSizeInUnits = new Vector3 (_unitWidth, _unitHeight, 0f);

		float xx = Mathf.Round (pos.y / tileSizeInUnits.y - pos.x / tileSizeInUnits.x);
		float yy = Mathf.Round (pos.y / tileSizeInUnits.y + pos.x / tileSizeInUnits.x);

		//		// Calculate grid aligned position from current position
		float x = (yy - xx) * 0.5f * tileSizeInUnits.x;
		float y = (yy + xx) * 0.5f * tileSizeInUnits.y;

		return new Vector3(x,y,0);
	}

	public void ShadowDebug (Vector3 pos)
	{
		if (shadow == null) {
			shadow = new GameObject("gridtile");
			shadow.transform.SetParent(this.transform);
			SpriteRenderer renderer =  shadow.AddComponent<SpriteRenderer>();
			renderer.sprite = Resources.Load<Sprite>(Constants.PATH_IMAGE_TILE);
		}
		shadow.transform.position = ToIsoPosition(pos);

		string hashKey = shadow.transform.position.x + "," + shadow.transform.position.y;
		if (positionData [hashKey] == null) {
			shadow.GetComponent<SpriteRenderer> ().color = Color.green;
			canSnap = true;
		} else {
			shadow.GetComponent<SpriteRenderer> ().color = Color.red;
			canSnap = false;
		}

	}

	void GetUnitSize ()
	{
		_unitWidth = Constants.UNIT_TILE_SIZE_WIDTH / Constants.PIXEL_PER_UNIT;
		_unitHeight = Constants.UNIT_TILE_SIZE_HEIGHT / Constants.PIXEL_PER_UNIT;
	}

	public void SetVisible(bool show)
	{
		this.gameObject.SetActive(show);
	}
	#endregion


}
