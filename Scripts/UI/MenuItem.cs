using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuItem : MonoBehaviour {


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void SetCurrentItemContainer ()
	{
		ImportItemManager.currentButtonContainer = this.gameObject;
	}

}
