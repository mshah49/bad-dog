using UnityEngine;
using System.Collections;

public class gameStart : MonoBehaviour
{

	void awake(){
		OnGUI ();
	}

	void update(){
	}
	// Our Startscreen GUI
	void OnGUI ()
	{
		if (GUI.Button (new Rect (30, 30, 150, 30), "Start Game"))
		{
			startGame();
		}
	}

	private void startGame()
	{
		print("Starting game");
		//DontDestroyOnLoad(gameState.Instance);
		//gameState.Instance.startState();       
	}
}