using UnityEngine;
using System.Collections;

public class CameraTool : Tool
{
	public const string NAME_CAMERA = "CameraTool";
	public static bool _isDragging = false;
	public static CameraTool instance;

	public const float MIN_SIZE = 0.25f;
	public const float MAX_SIZE = 2;
	public const float PANSPEED = -20f;

	#region private field
	private float _zoom = MIN_SIZE;
	private float _panSpeed = PANSPEED;
	private Vector3 _mouseOrigin;
	private Vector3 _defaultPos;

	#endregion

	void Awake ()
	{
		instance = this;
		Global.cameraTool = this;
	}

	void SetOrthographicSize ()
	{
		Camera.main.orthographicSize = (Screen.height / (_zoom * Constants.PIXEL_PER_UNIT)) * 0.5f;
	}

	// Use this for initialization
	void Start ()
	{
		SetOrthographicSize ();
		ToolName = NAME_CAMERA;
		_defaultPos = Camera.main.transform.position;

		ToolManager.tools.Add (this);
		ToolManager.SetDefaultTool (this); // Set camera as default tool
	}
	
	// Update is called once per frame
	void Update ()
	{
		OnMouseWheel ();
		OnReset ();
		if (!active) {
			_isDragging = false;
			return;
		}
		OnMouseDrag ();
	}


	#region CAMERA FUNCTIONS

	private void ZoomIn ()
	{
		_zoom++;
		_zoom = Mathf.Clamp (_zoom, MIN_SIZE, MAX_SIZE);
		SetOrthographicSize ();
	}

	private void ZoomOut ()
	{
		_zoom--;
		_zoom = Mathf.Clamp (_zoom, MIN_SIZE, MAX_SIZE);
		SetOrthographicSize ();
	}

	private void Drag ()
	{
		Vector3 pos = Camera.main.ScreenToViewportPoint (Input.mousePosition - _mouseOrigin);
		Vector3 move = new Vector3 ((pos.x * _panSpeed), (pos.y * _panSpeed), 0);
		Camera.main.transform.Translate (move, Space.Self);
		_mouseOrigin = Input.mousePosition;
	}

	private void Reset ()
	{
		Camera.main.transform.position = _defaultPos;
		_zoom = MIN_SIZE;
		SetOrthographicSize ();
	}

	public void ActiveCameraTool (bool activeFlag)
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

	public void OnZoomIn ()
	{
		ZoomIn ();
	}

	public void OnZoomOut ()
	{
		ZoomOut ();
	}

	public void OnReset ()
	{
		if (Input.GetKeyDown (KeyCode.Space)) {
			Reset ();
		}
	}

	#endregion
}
