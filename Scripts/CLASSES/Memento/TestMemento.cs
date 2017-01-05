using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TestMemento : MonoBehaviour {
	public static TestMemento instance;
	private CareTaker careTaker = new CareTaker();
	private	Originator originator = new Originator(); 

	private int currentMemento = -1;
	private int totalMemento = 0;

	public GameObject gameState;

	Scene tmp;

 	void Awake()
	{
		instance = this;
//		SaveChange();
		tmp = SceneManager.CreateScene("temp");
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Z)){
			Undo();
		}

		if(Input.GetKeyDown(KeyCode.U)){
			Redo();
		}

	}

	public void SaveChange()
	{
		GameObject copy = Instantiate(gameState);
		SceneManager.MoveGameObjectToScene(copy,tmp);
		originator.SetState(gameState); 
		careTaker.AddMemento(originator.SaveToMemento());
		currentMemento++;
		totalMemento++;
		Debug.Log("Save Changed!");
		Debug.Log("totalMemento!" + totalMemento);
	}

	private void Undo()
	{
		if(currentMemento > 0)
		{
			currentMemento--;
			originator.RestoreFromMemento(careTaker.getMemento(currentMemento));
//			this.gameState = originator.GetGameState();
//			Destroy(this.gameState);
//			this.gameState = Instantiate(originator.GetGameState());

			Debug.Log("Undo! Current Memento: " + currentMemento );
		}
	}

	private void Redo()
	{
		if(currentMemento < totalMemento - 1)
		{
			currentMemento++;
			originator.RestoreFromMemento(careTaker.getMemento(currentMemento));
//			this.gameState = originator.GetGameState();
//			Destroy(this.gameState);
//			this.gameState = Instantiate(originator.GetGameState());

			Debug.Log("Redo! Current Memento: " + currentMemento);
		}
	}
}
