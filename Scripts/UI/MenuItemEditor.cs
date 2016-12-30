using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class MenuItemEditor : MonoBehaviour {
	public static MenuItemEditor instance;
	public Dialog dialog;
	[HideInInspector]
	public GameObject currentItem;

	void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnClickRemove()
	{
		RemoveCurrentItem();
		dialog.ShowDiaLog(false);
	}

	public void RemoveCurrentItem()
	{
		int index = IsoLayerManager.currentLayer.isoFactories.FindIndex(x => x.gameObject == currentItem);
		if(index != -1)
		{
			IsoLayerManager.currentLayer.isoFactories.RemoveAt(index);
		}
		Destroy(currentItem);
		currentItem = null;

	}

	public void ChangePivot()
	{
		PivotEditForm.instance.SetSprite(currentItem.GetComponent<Image>().sprite);
		PivotEditForm.instance.dialog.ShowDiaLog(true);
		dialog.ShowDiaLog(false);
	}
}
