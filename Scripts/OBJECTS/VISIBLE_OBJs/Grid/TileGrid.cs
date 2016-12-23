using UnityEngine;
using System.Collections;

public class TileGrid : MonoBehaviour {

	public bool IsShow = false;

	private SpriteRenderer _renderer;

	// Use this for initialization
	void Awake () {
		this._renderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		VisbilityUpdate();
	}


	public void VisbilityUpdate()
	{
		if(this._renderer == null)
			return;

		_renderer.enabled = (IsShow)? true : false;

	}
}
