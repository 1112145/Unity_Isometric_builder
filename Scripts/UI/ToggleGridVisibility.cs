using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ToggleGridVisibility : MonoBehaviour {

	public Grid gameGrid;
	public Toggle toggle;

	// Use this for initialization
	void Start () {
		toggle.onValueChanged.AddListener((on) => {
			gameGrid.SetVisible(on);
		});
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
