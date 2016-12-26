using UnityEngine;
using System.Collections;

public class CameraTool : Tool
{
	public const string NAME_CAMERA = "CameraTool";
	public static bool _isDragging = false;
	public static CameraTool instance;

	#region private field
	private int[] _cameraSizes = new int[]{ 25 ,20, 16, 12, 8, 4 };
	private int _zoom = 2;
	private float _panSpeed = -16f;
	private Vector3 _mouseOrigin;
	private Vector3 _defaultPos;
	private int _defaultZoomLevel = 2;
	#endregion

	void Awake() { instance = this;}

	// Use this for initialization
	void Start ()
	{
		Camera.main.orthographicSize = _cameraSizes [_zoom];
		ToolName = NAME_CAMERA;
		_defaultPos = Camera.main.transform.position;

		ToolManager.tools.Add(this);
		ToolManager.SetDefaultTool(this); // Set camera as default tool
	}
	
	// Update is called once per frame
	void Update ()
	{
		OnMouseWheel ();
		OnReset();
		if(!active) 
		{
			_isDragging = false;
			return;
		}
		OnMouseDrag ();
	}


	#region CAMERA FUNCTIONS
	private void ZoomIn ()
	{
		_zoom++;
		_zoom = Mathf.Clamp (_zoom, 0, 4);
		Camera.main.orthographicSize = _cameraSizes [_zoom];
	}
	private void ZoomOut ()
	{
		_zoom--;
		_zoom = Mathf.Clamp (_zoom, 0, 4);
		Camera.main.orthographicSize = _cameraSizes [_zoom];
	}
	private void Drag ()
	{
		Vector3 pos = Camera.main.ScreenToViewportPoint (Input.mousePosition - _mouseOrigin);
		Vector3 move = new Vector3 (pos.x * _panSpeed, pos.y * _panSpeed, 0);
		Camera.main.transform.Translate (move, Space.Self);
		_mouseOrigin = Input.mousePosition;
	}
	private void Reset()
	{
		Camera.main.transform.position = _defaultPos;
		Camera.main.orthographicSize = _cameraSizes [_defaultZoomLevel];
	}
	public void ActiveCameraTool(bool activeFlag)
	{
		active = activeFlag;
	}
	#endregion

	#region EVENT HANDLER FUNCTIONS
	public void OnMouseWheel ()
	{
		if (Input.GetAxis ("Mouse ScrollWheel") > 0f) {// forward
			ZoomIn ();
		} else if (Input.GetAxis ("Mouse ScrollWheel") < 0f) {// backwards
			ZoomOut ();
		}
	}

	public void OnMouseDrag ()
	{
		if (Input.GetMouseButtonDown (0)) {
			_isDragging = true;
			_mouseOrigin = Input.mousePosition;
		}


		if (Input.GetMouseButtonUp (0) && _isDragging) {
			_isDragging = false;
		}

		if (_isDragging) {
			Drag ();
		}
	}

	public void OnZoomIn() { ZoomIn();}

	public void OnZoomOut() { ZoomOut();}

	public void OnReset()
	{
		if(Input.GetKeyDown(KeyCode.Space)) { Reset();}
	}
	#endregion
}
