using UnityEngine;
using System.Collections;

public class IsoObjectFactory : MonoBehaviour {

	public string FilePath;
	public static IsoObjectFactory instance;

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
}
