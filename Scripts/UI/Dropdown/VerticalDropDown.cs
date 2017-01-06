using UnityEngine;
using System.Collections;

public class VerticalDropDown : MyDropdown {

	public enum Direction
	{
		TOP,
		BOTTOM
	}

	public Direction direction = Direction.BOTTOM;


	// Use this for initialization
	void Start () {
		type = Type.Vertical;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void Hide ()
	{
		base.Hide ();
		if(direction == Direction.BOTTOM)
		{
			UITransition.MoveDown(ContentView.transform,getContentViewHeight());
			UITransition.MoveDown(transform,getContentViewHeight());
			Debug.Log("Move Down");
		}
		else
		{
			// TODO: TOP CASE
		}
	}

	public override void Show ()
	{
		base.Show ();
		if(direction == Direction.BOTTOM)
		{
			UITransition.MoveUp(ContentView.transform,getContentViewHeight());
			UITransition.MoveUp(transform,getContentViewHeight());
			Debug.Log("Move Up");
		}
		else
		{
			// TODO: TOP CASE
		}
	}

	float getContentViewHeight()
	{
		if(ContentView == null)
			return 0;

		return ((RectTransform)ContentView.transform).rect.height;
	}
}
