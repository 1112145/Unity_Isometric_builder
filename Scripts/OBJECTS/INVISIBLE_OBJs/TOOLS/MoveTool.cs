using UnityEngine;
using System.Collections;

public class MoveTool : Tool {

	public const string NAME_MOVE_TOOL = "MoveTool";

	#region PUBLIC FIELD
	public static bool IsInUse = false;
	public static GameObject moveObject;
	public static MoveTool instance;
	#endregion

	void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start () {
		ToolName = NAME_MOVE_TOOL;
		ToolManager.tools.Add(this);
//		Debug.Log("Add " + ToolName + " Success!");
	}
	
	// Update is called once per frame
	void Update () {

		if(!active) return;

		if(IsInUse){
			
			moveObject.transform.position = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			Vector2 tmp = moveObject.transform.position;
			moveObject.transform.position = new Vector3 (tmp.x, tmp.y, 0);
			IsoLayerManager.currentLayer.SortOrderByYAxis(moveObject);
			IsoLayerManager.currentLayer.ShadowDebug(moveObject.transform.position);

			if(Input.GetMouseButtonUp(0))
			{
				OnDeSelect();
			}
		}

		
	}

	public static void MoveObject(GameObject Obj)
	{
		moveObject = Obj;
		IsInUse = true;
		instance.OnSelect();
	}

	#region EVENT HANDLER FUNCTIONS
	public void OnDeSelect()
	{
		if(!IsoLayerManager.currentLayer.canSnap) return;
		IsInUse = false;
		IsoLayerManager.currentLayer.SnapObject(moveObject);
		IsoLayerManager.currentLayer.RemoveDataAtPosition(moveObject.transform.position);
		ToolManager.DeselectTool();
	}

	public void OnSelect()
	{
		ToolManager.SelectTool(this);
	}
	#endregion
}
