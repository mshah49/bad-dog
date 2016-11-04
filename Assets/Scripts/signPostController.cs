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
	public string Tip = "If you are at Brawler level 2, you can destroy boulders with your punches!";


	void OnTriggerEnter2D( Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			tipText.text = Tip;

		}
	}

	void Update() {
		
	}

	void OnTriggerExit2D( Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			tipText.text = "";
		}
	}

}
