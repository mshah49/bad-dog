using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class startButton : MonoBehaviour {
	public Button yourButton;

	void Start () {
		Button btn = yourButton.GetComponent<Button>();
		btn.onClick.AddListener(startGame);
	}

	public void startGame()
	{
		print("Starting game");
		DontDestroyOnLoad(gameState.Instance);
		gameState.Instance.startState();       
	}
}