using UnityEngine;
using System.Collections;

// Add this script to the dialog group
public class DialogController : MonoBehaviour {

	public static DialogController instance;

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

	public void HideAllDialogExclude(GameObject obj)
	{
		for (int i = 0; i < this.transform.childCount; i++) {
			if(obj != this.transform.GetChild(i).gameObject)
			{
				this.transform.GetChild(i).gameObject.SetActive(false);
			}
		}
	}

}
