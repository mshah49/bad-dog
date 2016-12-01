using UnityEngine;
using System.Collections;

public class diamondController : MonoBehaviour {
	private GameObject player;
	public int coinWorth = 1;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D( Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{	
			coinEXP (coinWorth);
			Destroy(gameObject);

		}
	}

	public void coinEXP(int exp){
		player = GameObject.FindGameObjectWithTag("Player");
		playerEXPController playerEXPController = player.GetComponent<playerEXPController>();
		playerEXPController.currentEXP += exp;
	}
}
