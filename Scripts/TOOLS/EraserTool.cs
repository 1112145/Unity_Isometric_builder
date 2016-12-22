using UnityEngine;
using System.Collections;

public class EraserTool : Tool
{

	public const string PATH_MOUSE_ICO = "Images/erasercursor";
	public const string ERASER_NAME = "EraserTool";

	#region public field

	public static bool IsInUse = false;
	public static bool IsBusy = false;
	public static EraserTool instance;

	#endregion

	#region private field

	private Sprite MouseCursor;
	private GameObject cursor;

	#endregion

	void Awake ()
	{
		instance = this;
	}

	// Use this for initialization
	void Start ()
	{
		ToolName = ERASER_NAME;
		ToolManager.tools.Add (this);
		Debug.Log ("Add " + ToolName + " Success!");
	}
	
	// Update is called once per frame
	void Update ()
	{
		
		DetectSelect ();

		if (!active)
			return;

		if (Input.GetMouseButtonUp (1)) {
			OnDeselect ();
		} //Deselect eraser tool.

		if (Input.GetMouseButtonDown (0)) {
			IsBusy = true;
		} // Start an erase round

		// Erasing
		if (IsBusy) {
			Erase ();
		}

		if (Input.GetMouseButtonUp (0)) {
			IsBusy = false;
		} //Finish an erase round. The tool is free and can be erased again
	}

	#region EVENT HANDLER FUNCTIONS

	public void OnSelect ()
	{
		IsInUse = true;
		ChangeMouseCursor ();
		ToolManager.SelectTool (this);
	}

	public void OnDeselect ()
	{
		IsInUse = false;
		ResetMouseCursor ();
		ToolManager.DeselectTool ();
	}

	#endregion

	void ChangeMouseCursor ()
	{
		MouseCursor = Resources.Load<Sprite> (PATH_MOUSE_ICO);
		Cursor.SetCursor (MouseCursor.texture, Vector2.zero, CursorMode.ForceSoftware);
	}

	void ResetMouseCursor ()
	{
		Cursor.SetCursor (null, Vector2.zero, CursorMode.ForceSoftware);
	}

	void Erase ()
	{
		Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);

		if (IsoLayerManager.currentLayer == null)
			return;
		// Convert to isometric position.
		pos = IsoLayerManager.currentLayer.ToIsoPosition (pos);
		//Delete the obj at position.
		IsoLayerManager.currentLayer.DeleteObjectAtPosition (pos);
	}

	void DetectSelect ()
	{
		if (!IsInUse) {
			if (Input.GetKeyDown (KeyCode.E)) {
				OnSelect ();
			} else
				return;
			// Detect Select this tool event.
		}
	}
}
