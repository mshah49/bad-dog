using UnityEngine;
using System.Collections;



public class attackType : MonoBehaviour {
	private playerController player;
	private Animator animator;

	public enum playerStance{
		brawler,heavy,mobility,
	}
	void awake(){
		player = GetComponent<playerController> ();
	}
	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
	}

	public void updateAttackType(playerStance currentStance){
		if (currentStance == playerStance.brawler){
			player.playerAttack = 400f; 
			player.playerFireRate = 0.3f;
			//StartCoroutine(ChangeAnimatorController("AnimationControllers/playerBrawlerController"));
		}
		else if (currentStance == playerStance.heavy){
			player.playerAttack = 50f; 
			player.playerFireRate = 0.5f;
			//StartCoroutine(ChangeAnimatorController("AnimationControllers/playerBrawlerController"));
		}
		else if (currentStance == playerStance.mobility){
			player.playerAttack = 200f; 
			player.playerFireRate = 0.5f;
			//StartCoroutine(ChangeAnimatorController("AnimationControllers/playerBrawlerController"));
		}

	}
}
