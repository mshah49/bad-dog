using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]

public class bulletPhysics : MonoBehaviour {

	Rigidbody2D rigidBody;
	Animator animator;
	BoxCollider2D boxCollider;

	//get player object
	public GameObject player;

	//player attack speed
	public float bulletAttackSpeed;


	// Use this for initialization
	void Awake () {
		playerController playerController = player.GetComponent<playerController> ();
		//set attackspeed to players current attack speed
		bulletAttackSpeed = playerController.playerAttackSpeed;
		//get components
		rigidBody = GetComponent<Rigidbody2D> ();
		animator = GetComponent<Animator> ();


		// use player object
		player = GameObject.FindGameObjectWithTag("Player");
		

<<<<<<< HEAD
		if (playerController.currentStance == playerController.playerStance.brawler) {
			animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController> ("AnimationControllers/playerBrawlerProjectile");
			print ("brawler");
		}
		else if (playerController.currentStance == playerController.playerStance.mobility) {
			animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController> ("AnimationControllers/playerMobilityProjectile");
			print ("mobility");
		}	
=======

>>>>>>> master
	
		//depending on stance, change the animation of the projectile

		//launch projectiles
		if(transform.localRotation.z>0)
			rigidBody.AddForce (new Vector2 (-1*bulletAttackSpeed, 0), ForceMode2D.Impulse);
		else 
			rigidBody.AddForce (new Vector2 (1*bulletAttackSpeed, 0), ForceMode2D.Impulse);
	}
	
	// Update is called once per frame
	void Update () {
		playerController playerController = player.GetComponent<playerController> ();

		// use player object
		player = GameObject.FindGameObjectWithTag("Player");
		if (playerController.currentStance == playerController.playerStance.brawler) {
			animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController> ("AnimationControllers/playerBrawlerProjectile");
			print ("brawler");
		}
		else if (playerController.currentStance == playerController.playerStance.mobility) {
			animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController> ("AnimationControllers/playerMobilityProjectile");
			print ("mobility");
		}	

	}


	public void removeForce(){
		rigidBody.velocity = new Vector2 (0, 0);
	}
		
}
