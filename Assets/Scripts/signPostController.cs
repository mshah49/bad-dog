using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.IO;

public class signPostController : MonoBehaviour {
	private string textFieldString = "If you are at Brawler level 2, you can destroy boulders with your punches!";
	bool triggered =false;

	void OnTriggerEnter2D( Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			triggered = true;
			print("collision");
		}
	}

	void OnGUI() {
		if (triggered) {
			textFieldString = GUI.TextField (new Rect (25, 25, 500, 30), textFieldString);
		}
	}

	void OnTriggerExit2D( Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			triggered =false;
		}
	}
}
