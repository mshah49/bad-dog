using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.IO;
using UnityEngine.UI;

public class stanceStatusGUI : MonoBehaviour {

	private GameObject player;
	private int brawlerLevel;
	private int mobilityLevel;
	private int heavyLevel;
	private string BrawlerString;
	private string BrawlerString2;
	private string MobilityString;
	private string HeavyString;
	public Text stanceText;  

	bool triggered =false;
	void Start () {
	}

	void Update() {
		getStanceLevel ();
		BrawlerString = "Brawler:Level " + brawlerLevel;
		MobilityString = "Mobility:Level " + mobilityLevel;
		HeavyString = "Heavy:Level " + heavyLevel;
		player = GameObject.FindGameObjectWithTag("Player");
		playerController playerController = player.GetComponent<playerController> ();

		if (playerController.currentStance ==playerController.playerStance.brawler) {
			stanceText.text = BrawlerString;
		}
		if (playerController.currentStance ==playerController.playerStance.mobility) {
			stanceText.text = MobilityString;
		}
		if (playerController.currentStance ==playerController.playerStance.heavy) {
			stanceText.text = HeavyString;
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
