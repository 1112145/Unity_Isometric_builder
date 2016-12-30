using UnityEngine;
using System.Collections;

public class Dialog : MonoBehaviour {

	public GameObject foreground;
	public GameObject contentView;
	public bool IsActive = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void TurnOn(bool On)
	{
		if(On)
		{
			foreground.SetActive(true);
			UITransition.PopIn(contentView.transform);
		}
		else
		{
			foreground.SetActive(false);
			contentView.SetActive(false);
		}
	}

	public void ShowDiaLog(bool show)
	{
		if(show)
		{
			TurnOn(true);
			DialogController.instance.HideAllDialogExclude(this.gameObject);

		}
		else
		{
			TurnOn(false);
		}
	}
}
