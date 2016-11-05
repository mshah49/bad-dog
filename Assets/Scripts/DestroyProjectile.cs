
using UnityEngine;
using System.Collections;


public class DestroyProjectile : MonoBehaviour {
	public float timeAlive;
	public GameObject player;
	// Use this for initialization
	void Awake () {
		player = GameObject.FindGameObjectWithTag("Player");
		playerController playerController = player.GetComponent<playerController> ();
		if (playerController.currentStance == playerController.playerStance.brawler) {
			timeAlive = 0.25f;
		}
		else if (playerController.currentStance == playerController.playerStance.mobility) {
			timeAlive = 5;
		}	

		Destroy (gameObject, timeAlive);
		
	}
	// Update is called once per frame
	void Update () {
	}
}
