using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject currentCheckpoint;
	private playerController player;
	// Use this for initialization
	void Start () {
		player = FindObjectOfType<playerController> ();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void respawnPlayer(){
		Debug.Log ("Player Respawn");
		player.transform.position = currentCheckpoint.transform.position;
	}
}
