using UnityEngine;
using System.Collections;

public class HorizontalDropdown : MyDropdown {

	public enum Direction
	{
		LEFT,
		RIGHT
	}

	public Direction direction = Direction.LEFT;

	// Use this for initialization
	void Start () {
		type = Type.Horizontal;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void Show ()
	{
		base.Show ();
		if(direction == Direction.LEFT)
		{
			UITransition.MoveLeft(ContentView.transform,getContentViewWidth());
			UITransition.MoveLeft(transform,getContentViewWidth());

		}
		else
		{
			
		}
	}

	public override void Hide ()
	{
		base.Hide ();
		if(direction == Direction.LEFT)
		{
			UITransition.MoveRight(ContentView.transform,getContentViewWidth());
			UITransition.MoveRight(transform,getContentViewWidth());
		}
		else
		{

		}

	}

	float getContentViewWidth()
	{
		if(ContentView == null)
			return 0;

		return ((RectTransform)ContentView.transform).rect.width;
	}
		
}
