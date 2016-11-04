using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.IO;
using UnityEngine.UI;

public class signPostController : MonoBehaviour {
	private string textFieldString = "If you are at Brawler level 2, you can destroy boulders with your punches!";
	bool triggered =false;
	public Text tipText; 
	string Tip1;
	string Tip2;
	string Tip3;



	void OnTriggerEnter2D( Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			triggered = true;
			print("collision");
		}
	}

	void Update() {
		Tip1 = "If you are at Brawler level 2, you can destroy boulders with your punches!";
		if (triggered) {
			tipText.text = Tip1;
		} else {
			tipText.text = "";
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
