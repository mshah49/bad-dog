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
	public float playerAttackSpeed;


	// Use this for initialization
	void Awake () {
	
		//get components
		rigidBody = GetComponent<Rigidbody2D> ();
		animator = GetComponent<Animator> ();
		playerController playerController = player.GetComponent<playerController> ();

		// use player object
		player = GameObject.FindGameObjectWithTag("Player");
		
		//set attackspeed to players current attack speed
		playerAttackSpeed = playerController.playerAttackSpeed;

		//depending on stance, change the animation of the projectile
		if (playerController.currentStance == playerController.playerStance.brawler) {
			ChangeAnimatorController("AnimationControllers/playerBrawlerProjectile");
			print ("brawlerShot");

		}


		//launch projectiles
		if(transform.localRotation.z>0)
			rigidBody.AddForce (new Vector2 (-1, 0) * playerAttackSpeed, ForceMode2D.Impulse);
		else 
			rigidBody.AddForce (new Vector2 (1, 0) * playerAttackSpeed, ForceMode2D.Impulse);
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void removeForce(){
		rigidBody.velocity = new Vector2 (0, 0);
	}

	private IEnumerator ChangeAnimatorController(string name) {

		// Assign our new animator controller
		animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(name);

		// Wait a frame so the sprite updates
		yield return new WaitForEndOfFrame();

	}	
		
}
