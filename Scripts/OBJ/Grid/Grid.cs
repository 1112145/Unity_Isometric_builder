using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

	public const string PATH_PREFABS_ODD = "Prefabs/OddTile";
	public const string PATH_PREFABS_EVEN = "Prefabs/EvenTile";
	public const string PATH_IMAGE_TILE = "Images/tile";

	public bool isShowGrid = true;
	public int nRow = 50;
	public int nColumn = 25;
	public int ZoomLevel = 1;


	[HideInInspector]
	public List<GameObject> Tiles = new List<GameObject>();

	[HideInInspector]
	public static Grid instance;

	[HideInInspector]
	public float _unitWidth = 0;
	[HideInInspector]
	public float _unitHeight = 0;

	private float _gridWidth = 0;
	private float _gridHeight = 0;
	private Vector3 _centerAlign = new Vector3(0,0,0);


	void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start () {
		InitGrid();
		CreateGrid();
	}
	
	// Update is called once per frame
	void Update () {
		


	}

	public void InitGrid()
	{
		Rect rect = (Resources.Load<Sprite>(PATH_IMAGE_TILE)).rect;
		_unitHeight = (rect.width) * Mathf.Sqrt(0.5f) / 100;
		_unitWidth = 2 * _unitHeight;
		_gridWidth = nColumn * _unitWidth;
		_gridHeight = nRow * _unitHeight;
		_centerAlign = new Vector3((-1)*_gridWidth/2,(-1)*_gridHeight/4);
	}

	public void CreateGrid()
	{
		for (int i = 0; i < nRow; i++) {
			for (int j = 0; j < nColumn; j++) {

				GameObject tile;
				if(i % 2 == 0){
					// even row
					tile = (GameObject)Instantiate(Resources.Load<GameObject>(PATH_PREFABS_EVEN),this.transform);
					// Set the right position
					tile.transform.position = this.transform.position +  new Vector3(j * _unitWidth, i * _unitHeight /2) ;
				}
				else{
					// odd row
					tile = (GameObject)Instantiate(Resources.Load<GameObject>(PATH_PREFABS_ODD),this.transform);
					// Set the right position
					tile.transform.position = this.transform.position + new Vector3(j * _unitWidth + _unitWidth/2, i * _unitHeight /2);
				}
				tile.transform.position += _centerAlign; // Move the offset of the grid to center.
				Tiles.Add(tile);
			}
		}
	}

	public void SetVisible(bool show)
	{
		this.gameObject.SetActive(show);
	}
}
