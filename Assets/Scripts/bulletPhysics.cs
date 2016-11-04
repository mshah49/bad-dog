 using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(playerController))]

public class bulletPhysics : MonoBehaviour {

	Rigidbody2D rigidBody;
	Animator animator;
	BoxCollider2D boxCollider;

	//get player object
	public GameObject player;
	// use player object


	//player attack speed
	public float bulletAttackSpeed;


	// Use this for initialization
	void Awake () {
		//find player
		player = GameObject.FindGameObjectWithTag("Player");
		//get components
		rigidBody = GetComponent<Rigidbody2D> ();
		animator = GetComponent<Animator> ();
		playerController playerController = player.GetComponent<playerController> ();
	
		//set attackspeed to players current attack speed
		bulletAttackSpeed = playerController.playerAttackSpeed;
		//get components


		if (playerController.currentStance == playerController.playerStance.brawler) {
			animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController> ("AnimationControllers/playerBrawlerProjectile");
			print ("brawler");
		}
		else if (playerController.currentStance == playerController.playerStance.mobility) {
			animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController> ("AnimationControllers/playerMobileProjectile");
			print ("mobility");
		}	
	
		//depending on stance, change the animation of the projectile

		//launch projectiles
		if(transform.localRotation.z>0)
			rigidBody.AddForce (new Vector2 (-1*bulletAttackSpeed, 0), ForceMode2D.Impulse);
		else 
			rigidBody.AddForce (new Vector2 (1*bulletAttackSpeed, 0), ForceMode2D.Impulse);
	}
	
	// Update is called once per frame
	void Update () {
	}


	public void removeForce(){
		rigidBody.velocity = new Vector2 (0, 0);
	}
		
}
