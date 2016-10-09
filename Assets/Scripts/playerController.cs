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
	public float playerHealth = 5f;
	public float playerSpeed = 1.0f;
	public float playerMaxSpeed = 6f;
	public float playerJumpHeight = 8f;
	public bool playerDoubleJump = false;
	public float playerAttack = 3f; 

	bool playerGrounded = false;
	bool facingRight;

	//for Respawn
	public GameManager gameManager;
	private float deathHeight = -20f;


	//for Projectiles
	public Transform gunTip;
	public GameObject bullet;
	public float playerAttackSpeed = 300f;
	public float playerFireRate = 2f;
	float nextFire = 0f;

	//ground check
	float groundCheckRadius = 0.2f;
	public LayerMask groundLayer;
	public Transform groundCheck;

	//stances
	public enum playerStance{
		brawler,heavy,mobility,
	}

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
	}
		
	// Use this for initialization

	public void Update(){

	}
	
	// Update is called once per frame, use for game physics such as movement or bullets or stance change
	public void FixedUpdate () {
		
		//check for ground
		playerGrounded = Physics2D.OverlapCircle(groundCheck.position,groundCheckRadius,groundLayer);

		//playerMovement
		float playerMove = Input.GetAxis ("Horizontal");
		animator.SetFloat ("Speed", Mathf.Abs (playerMove));
		rigidBody.velocity = new Vector2 (playerMove * playerMaxSpeed, rigidBody.velocity.y);

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

		if (playerGrounded) {
			groundedAnimation(true);
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
		if (Input.GetAxisRaw ("Fire1") > 0) {
			fireRocket ();
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
	private void jumpAnimation(){
		animator.SetTrigger ("jumpPressed");
		animator.SetBool ("isGrounded", false);
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
	public void updateStance(playerStance currentStance){
		if (currentStance == playerStance.brawler){

			playerSpeed = 1.0f;
			playerJumpHeight = 6.0f;
			playerDoubleJump = false;
			playerAttack = 3f;
			playerFireRate = 1f;
			playerAttackSpeed = 20f;
			StartCoroutine(ChangeAnimatorController("AnimationControllers/playerBrawlerController"));
		}
		else if (currentStance == playerStance.heavy){
			playerSpeed = 0.5f;
			playerJumpHeight = 4f;
			playerDoubleJump = false;
			playerAttack = 2f; 
			playerFireRate = 2f;
			playerAttackSpeed = 20f;
		}
		else if (currentStance == playerStance.mobility){
			playerSpeed = 1.5f;
			playerJumpHeight = 12f;
			playerDoubleJump = true;
			playerAttack = 1f;
			playerFireRate = 0.5f;
			playerAttackSpeed = 20f;
			StartCoroutine(ChangeAnimatorController("AnimationControllers/playerMobilityController"));
		}

	}

}