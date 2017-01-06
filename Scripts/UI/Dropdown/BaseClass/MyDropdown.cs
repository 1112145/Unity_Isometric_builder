using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MyDropdown : MonoBehaviour, IPointerClickHandler {

	public enum State
	{
		ON,
		OFF
	}
	public enum Type
	{
		Vertical,
		Horizontal
	}

	protected State state = State.ON;
	protected Type type;

	public Image image;
	public Sprite OnSprite;
	public Sprite OffSprite;
	public Transform ContentView;

	private Vector3 orgPosContentView;
	private Vector3 orgPos;

	void Awake()
	{
		Debug.Log("Debug.Log. Awake dropdown!");
		InitOrgPosition ();
	}

	void InitOrgPosition ()
	{
		orgPos = this.transform.position;
		orgPosContentView = ContentView.transform.position;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public virtual void Show()
	{
		state = State.ON;
		image.sprite = OnSprite;
	}

	public virtual void Hide()
	{
		state = State.OFF;
		image.sprite = OffSprite;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		OnClick();
	}

	public virtual void OnClick()
	{
		if(state == State.ON){
			Hide();
		} 
		else 
		{ 
			Show();
		}
	}

	public void Reset()
	{
		this.transform.position = orgPos;
		this.ContentView.transform.position = orgPosContentView;
		this.state = State.ON;
		image.sprite = OnSprite;
	}
}
