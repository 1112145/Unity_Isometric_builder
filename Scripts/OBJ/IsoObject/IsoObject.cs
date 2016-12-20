using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Windows.Forms;

//http://answers.unity3d.com/questions/945172/sprite-with-higher-sorting-layer-not-firing-its-on.html
public class IsoObject : MonoBehaviour {

	public enum State
	{
		None,
		isHolding,
		isSelected
	}

	#region PUBLIC FIELD
	[HideInInspector]
	public State state = State.None;
	[HideInInspector]
	public string FilePath;
	#endregion

	#region PRIVATE FIELD
	private Vector3 _originScale;
	private Layer _layer; // layer that object belong to.
	#endregion


	// Use this for initialization
	void Start () {
		_originScale = this.transform.localScale;
		_layer = this.transform.parent.GetComponent<Layer>();
	}
	
	// Update is called once per frame
	void Update () {

		if(IsSelected ()) { OnSelected (); }

		if(IsDeselected ()) { OnDeselected();}

		// Short keys.
		if(state == State.isSelected){
			if(Input.GetKeyDown(KeyCode.D)) { Delete();}

			if(Input.GetKeyDown(KeyCode.R)) { Rotate();}

			if(Input.GetKeyDown(KeyCode.M))	{ Move();}
		}
			
	}

	bool IsSelected ()
	{
		if (Input.GetMouseButtonDown (0)) {
			RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll (Camera.main.ScreenPointToRay (Input.mousePosition));
			hits = SortDescSortingOder(hits);

			if(hits.Length == 0)
				return false;
			// Neu object co chi so sorting layer cao nhat trung voi obj thi day la object duoc chon.
			if (hits [0].collider.gameObject == this.gameObject && !_layer.startBuild) {
				if(IsoLayerManager.instance.CurrentLayerIndex == _layer.layerID){
					return true;
				}
			}
		}
		return false;
	}

	bool IsDeselected ()
	{
		if (Input.GetMouseButtonUp (0) && state == State.isSelected) {
			return true;
		}
		return false;
	}



	#region EVENT HANDLER FUNCTION
	public void OnSelected()
	{
		state = State.isSelected;
		if(EraserTool.IsInUse)
			return;
		// Zoom In.
		this.transform.localScale *= 1.3f;
		// Change Color.	
		this.GetComponent<SpriteRenderer>().color = Color.cyan;

	}

	public void OnDeselected() { Reset (); }
	#endregion


	public void Delete()
	{
		Destroy(this.gameObject);
		_layer.RemoveDataAtPosition(this.transform.position);
	}

	public void Rotate(){
		this.transform.Rotate(new Vector3(0,180,0));
	}

	public void Reset ()
	{
		state = State.None;
		// Reset all.
		this.GetComponent<SpriteRenderer> ().color = Color.white;
		this.transform.localScale = _originScale;
	}

	public void Move ()
	{
		MoveTool.MoveObject(this.gameObject);
		IsoLayerManager.currentLayer.RemoveDataAtPosition(this.transform.position);
	}
		
	public void ReduceOpacity()
	{
		SetOpacity(0.5f);
	}

	public void ResetOpacity()
	{
		SetOpacity(1f);
	}

	private void SetOpacity(float opacity)
	{
		SpriteRenderer renderer = this.GetComponent<SpriteRenderer>();

		if(renderer == null) return; 
		Color tmp = renderer.color;
		tmp.a = opacity;
		renderer.color = tmp;
	}


	#region ULTILITY Functions
	// SORT THE RAYCAST ARRAY TO FIND THE RAYCAST ON TOP.
	RaycastHit2D[] SortDescSortingOder(RaycastHit2D[] array)
	{
		// Remove raycast of difference layer.
		List<RaycastHit2D> tmp = new List<RaycastHit2D>(array);
		for (int i = 0; i < tmp.Count; i++) {
			if(tmp[i].collider.gameObject.GetComponent<IsoObject>()._layer.layerID != IsoLayerManager.instance.CurrentLayerIndex)
			{
				tmp.Remove(tmp[i]);	
				i--;
			}
		}
		array = tmp.ToArray();

		// Sorting base on z-index.
		for (int i = 0; i < array.Length - 1; i++) {
			for (int j = i + 1; j < array.Length; j++) {
				int x = array[i].collider.gameObject.GetComponent<SpriteRenderer>().sortingOrder;
				int y = array[j].collider.gameObject.GetComponent<SpriteRenderer>().sortingOrder;

				if(x < y)
				{
					RaycastHit2D temp = array[j];
					array[j] = array[i];
					array[i] = temp;
				}
			}
		}
		return array;
	}

	#endregion

}
