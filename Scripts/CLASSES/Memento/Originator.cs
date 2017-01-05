using UnityEngine;
using System.Collections;

public class Originator {

	private GameObject gameState;

	public GameObject GetGameState()
	{
		return gameState;
	}

	public void SetState(GameObject State)
	{
		this.gameState = State;
	}

	public Memento SaveToMemento()
	{
		return new Memento(gameState);
	}

	public void RestoreFromMemento(Memento m)
	{
		this.gameState = m.getSavedState();
	}

}
