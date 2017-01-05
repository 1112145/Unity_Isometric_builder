using UnityEngine;
using System.Collections;

//https://www.youtube.com/watch?v=jOnxYT8Iaoo

public class Memento {
	
	private GameObject gameState;

	public Memento(GameObject stateToSave) { gameState = stateToSave;}

	public GameObject getSavedState() { return gameState;}

}
