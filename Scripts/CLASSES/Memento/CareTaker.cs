using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CareTaker {
	private List<Memento> savedStates = new List<Memento>();

	public void AddMemento(Memento m) {
		savedStates.Add(m);
	}

	public Memento getMemento(int index){
		return savedStates[index];
	}

}
