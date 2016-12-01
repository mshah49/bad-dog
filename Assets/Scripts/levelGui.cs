using UnityEngine;
using System.Collections;

public class levelGui : MonoBehaviour {

	// Initialize level
	void Start ()
	{
		print ("Loaded: " + gameState.Instance.getLevel());
	}



	// ---------------------------------------------------------------------------------------------------
	// OnGUI()
	// ---------------------------------------------------------------------------------------------------
	// Provides a GUI on level scenes
	// ---------------------------------------------------------------------------------------------------
	void OnGUI()
	{              
		// Create buttons to move between level 1 and level 2
		if (GUI.Button (new Rect (30, 30, 150, 30), "Load Level 1"))
		{
			gameState.Instance.setLevel("Level 1");
			Application.LoadLevel("level1");
		}

		if (GUI.Button (new Rect (300, 30, 150, 30), "Load Level 2"))
		{
			print ("Moving to level 2");
			gameState.Instance.setLevel("Level 2");
			Application.LoadLevel("duel2Level1");
		}


		// Output stats
		GUI.Label(new Rect(30, 100, 400, 30), "Name: " + gameState.Instance.getName());
		GUI.Label(new Rect(30, 130, 400, 30), "HP: " + gameState.Instance.getHP().ToString());
		GUI.Label(new Rect(30, 160, 400, 30), "MP: " + gameState.Instance.getMP().ToString());

	}
}