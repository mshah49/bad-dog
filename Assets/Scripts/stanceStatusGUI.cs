using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.IO;

public class stanceStatusGUI : MonoBehaviour {

	private GameObject player;
	private int brawlerLevel;
	private int mobilityLevel;
	private int heavyLevel;
	private string BrawlerString;
	private string MobilityString;
	private string HeavyString;

	bool triggered =false;

	void OnGUI() {
		getStanceLevel ();
		BrawlerString = "Brawler:Level " + brawlerLevel;
		MobilityString = "Mobility:Level " + mobilityLevel;
		HeavyString = "Heavy:Level " + heavyLevel;
		player = GameObject.FindGameObjectWithTag("Player");
		playerController playerController = player.GetComponent<playerController> ();
		if (playerController.currentStance ==playerController.playerStance.brawler) {
			BrawlerString = GUI.TextField (new Rect (500, 25, 200, 30), BrawlerString);
		}
		if (playerController.currentStance ==playerController.playerStance.mobility) {
			MobilityString = GUI.TextField (new Rect (500, 25, 200, 30),  MobilityString);
		}
		if (playerController.currentStance ==playerController.playerStance.heavy) {
			HeavyString = GUI.TextField (new Rect (500, 25, 200, 30), HeavyString);
		}
	}

	void getStanceLevel(){
		player = GameObject.FindGameObjectWithTag("Player");
		playerController playerController = player.GetComponent<playerController> ();
		brawlerLevel = playerController.brawlerLevel;
		mobilityLevel = playerController.mobilityLevel;
		heavyLevel = playerController.heavyLevel;
	}
}
