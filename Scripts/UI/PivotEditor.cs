using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PivotEditor : MonoBehaviour, IDragHandler {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnBeginDrag(PointerEventData eventData){
		
	}

	public void OnDrag(PointerEventData data){
		Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		this.transform.position = mousePosition;
		CameraTool._isDragging = false;
		PivotEditForm.instance.UpdatePivotLocation();
	}

	public void OnEndDrag(PointerEventData eventData){
		
	}
}
