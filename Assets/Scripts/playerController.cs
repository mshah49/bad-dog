using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(attackType))]


[RequireComponent(typeof(attackType))]

public class playerController : MonoBehaviour {

	//Starting player attributes
	private bool playerFallDeath = false;
	public float playerHealth;
	public float playerSpeed;
	public float playerJumpHeight;
	public bool playerDoubleJump = false;
	public float playerAttack;
	public float playerAttackTimer = 0;

	bool playerGrounded = false;
	bool facingRight;

	//for Respawn
	public GameManager gameManager;
	private float deathHeight = -20f;


	//for Projectiles
	public Transform gunTip;
	public GameObject bullet;
	public float playerAttackSpeed;
	public float playerFireRate = 2.0f;
	float nextFire = 0.0f;

	//ground check
	float groundCheckRadius = 0.2f;
	public LayerMask groundLayer;
	public Transform groundCheck;

	//stances
	public enum playerStance{
		brawler,heavy,mobility,
	}

	//attackAnimationTimer
	public float attackTimer = 0f;

	public playerStance currentStance = playerStance.brawler;

	Rigidbody2D rigidBody;
	Animator animator;
	BoxCollider2D boxCollider;
	SpriteRenderer spriteRenderer;
	attackType attackType;


	public void Awake(){
		
		gameManager = FindObjectOfType<GameManager> ();
		rigidBody = GetComponent<Rigidbody2D> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
		boxCollider = GetComponent<BoxCollider2D> ();
		animator = GetComponent<Animator> ();

		attackType = GetComponent<attackType> ();

		facingRight = true;
		isAttacking (false);
	}
		
	// Use this for initialization

	// Update is called once per frame, use for game physics such as movement or bullets or stance change
	public void Update () {
		
		//check for ground
		playerGrounded = Physics2D.OverlapCircle(groundCheck.position,groundCheckRadius,groundLayer);
		if (playerGrounded) {
			groundedAnimation (true);
		}
		//playerMovement
		float playerMove = Input.GetAxis ("Horizontal");
		if(playerMove != 0.0f){
			rigidBody.velocity = new Vector2 (playerMove * playerSpeed, rigidBody.velocity.y);
			runAnimation(true);
		}
		else if (playerMove == 0.0f){
			idleAnimation();
		}

		//Check direction to flip sprite
		if (playerMove > 0 && !facingRight) 
			spriteflip ();
		else if (playerMove < 0 && facingRight)
			spriteflip ();
		
		//playerJump
		if(playerGrounded && Input.GetAxis("Jump")>0){
			rigidBody.AddForce(new Vector2(0,playerJumpHeight));
			jumpAnimation ();
		}
			
			
		//change stances
		if (Input.GetKeyDown("z")){
			updateStance (playerStance.mobility);
			//attackType.updateAttackType (attackType.playerStance.mobility);
		}
		if (Input.GetKeyDown("x")){
			updateStance (playerStance.heavy);
			//attackType.updateAttackType(attackType.playerStance.heavy);
		}
		if (Input.GetKeyDown("c")){
			updateStance (playerStance.brawler);
			//attackType.updateAttackType (attackType.playerStance.brawler);
		}

		//player falls
		if (transform.position.y < deathHeight) {
			playerFallDeath = true;
			fallDeath ();
		}

		//player shooting
		if (Input.GetKeyDown(KeyCode.LeftControl)){
			attackAnimation ();
			fireRocket ();
			isAttacking (true);
			attackTimer = playerAttackTimer;
		}
		if (attackTimer < 0) {
			isAttacking (false);
		}

		if (attackTimer > 0) {
			attackTimer -= Time.deltaTime;
		}

	}

	///<summary>
	/// Animation Codes
	///</summary>

	// flips character sprite
	void spriteflip(){
		facingRight = !facingRight;
		Vector3 myScale = transform.localScale;
		myScale.x *= -1;
		transform.localScale = myScale;
	}
	// respawns player at beginning of level if fallen below map
	void fallDeath(){
		if(playerFallDeath == true)
			gameManager.respawnPlayer ();
	}



	//attack 
	void fireRocket(){
		if (Time.time > nextFire) {
			nextFire = Time.time + playerFireRate;
			if (facingRight) {
				Instantiate (bullet, gunTip.position, Quaternion.Euler (new Vector3 (0, 0, 0)));
			} else if (!facingRight) {
				Instantiate (bullet, gunTip.position, Quaternion.Euler (new Vector3 (0, 0, 180f)));
			}
		}
	}

	//changes animations
	private IEnumerator ChangeAnimatorController(string name) {

		// Assign our new animator controller
		animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(name);

		// Wait a frame so the sprite updates
		yield return new WaitForEndOfFrame();

	}	

	//Animation Scripts for State Machine


	private void runAnimation(bool value){
		animator.SetBool ("isRunning", value);
	}

	private void attackAnimation(){
		animator.SetTrigger ("attackPressed");
	}

	private void isAttacking(bool value){
		animator.SetBool ("isAttacking", value);
	}

	private void jumpAnimation(){
		animator.SetTrigger ("jumpPressed");
		animator.SetBool ("isGrounded", false);
		print ("jumped");
	}

	private void groundedAnimation(bool value){
		animator.SetBool ("isGrounded", value);
	}

	private void idleAnimation(){
		runAnimation (false);
	}

	///<summary>
	/// Stance Codes
	///</summary>


	//Updates player sprite and attributes based on selected stance
	public void updateStance(playerStance newStance){
		if (newStance == playerStance.brawler){
			currentStance = playerStance.brawler;
			playerSpeed = 10.0f;
			playerJumpHeight = 40.0f;
			playerDoubleJump = false;
			playerAttack = 6f;
			playerFireRate = 1f;
			playerAttackSpeed = 10.0f;
			playerAttackTimer = 0.5f;
			StartCoroutine(ChangeAnimatorController("AnimationControllers/playerBrawlerController"));
		}
		else if (newStance == playerStance.heavy){
			currentStance = playerStance.heavy;
			playerSpeed = 5.0f;
			playerJumpHeight = 30.0f;
			playerDoubleJump = false;
			playerAttack = 2f; 
			playerFireRate = 2f;
			playerAttackSpeed = 5.0f;
		}
		else if (newStance == playerStance.mobility){
			currentStance = playerStance.mobility;
			playerSpeed = 15.0f;
			playerJumpHeight = 50.0f;
			playerDoubleJump = true;
			playerAttack = 2f;
			playerFireRate = 0.5f;
			playerAttackSpeed = 10.0f;
			playerAttackTimer = 0.25f;
			StartCoroutine(ChangeAnimatorController("AnimationControllers/playerMobilityController"));
		}

	}

}