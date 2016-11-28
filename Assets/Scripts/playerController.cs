using System.Linq;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]

public class playerController : MonoBehaviour {

	//Starting player attributes
	private bool playerFallDeath = false;
	public float playerSpeed;
	public float playerJumpHeight;
	public bool playerDoubleJump = false;
	public float playerAttack;
	public float playerAttackTimer = 0;
	public int brawlerLevel = 1;
	public int mobilityLevel = 1;
	public int heavyLevel = 1;
	public int fireDamage = 1;
	private playerHealthController playerHP;
	public Vector2 playerVelocity;

	public bool FacingRight {
		get { return spriteRenderer.flipX == false; }
		set { spriteRenderer.flipX = !value; }
	}


	bool playerGrounded;
	bool facingRight;
	bool isHurt;

	bool canDoubleJump;

	//for Respawn
	public GameManager gameManager;
	private float deathHeight = -7.5f;


	//for Projectiles
	public Transform gunTip;
	public GameObject bullet;
	public float playerProjectileSpeed;
	public float playerFireRate = 2.0f;
    private float nextFire;

	//ground check
	float groundCheckRadius = 0.2f;
	public LayerMask groundLayer;
	public Transform groundCheck;
	private bool hasLeftGround;
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

	public void Awake(){
		playerHP = GetComponent<playerHealthController>(); //gets enemyHealth component from inspector
		updateStance (playerStance.brawler);
		gameManager = FindObjectOfType<GameManager> ();
		rigidBody = GetComponent<Rigidbody2D> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
		boxCollider = GetComponent<BoxCollider2D> ();
		animator = GetComponent<Animator> ();
		facingRight = true;
		isAttacking (false);

	}

	// Use this for initialization

	// Update is called once per frame, use for game physics such as movement or bullets or stance change
	public void FixedUpdate () {
		GroundCheck ();
		if (playerGrounded) {
			groundedAnimation (playerGrounded);
		}
		playerVelocity = Vector2.zero;
	}
	public void Update () {
		if (playerVelocity == Vector2.zero) {
			idleAnimation ();
		}
		float playerMove = Input.GetAxis ("Horizontal");
		if(Mathf.Abs(playerMove) > 0.0f){
			Move (playerMove);
			runAnimation (true);
		}

		if (Input.GetKeyDown ("space") && currentStance != playerStance.heavy) {
			if (playerGrounded) {
				print (playerVelocity);
				playerVelocity = new Vector2 (rigidBody.velocity.x, Mathf.Sqrt (-2.0f * playerJumpHeight * Physics2D.gravity.y));
				print (playerVelocity);
				jumpAnimation ();
				if (currentStance == playerStance.mobility && mobilityLevel > 1) {
					canDoubleJump = true;
				}
			} else {
				if (canDoubleJump) {
					canDoubleJump = false;
					playerVelocity = Vector2.zero;
					playerVelocity = new Vector2 (rigidBody.velocity.x, Mathf.Sqrt (-2.0f * playerJumpHeight*1.5f*Physics2D.gravity.y));
					jumpAnimation ();
				}
			}
			}

		if (playerVelocity != Vector2.zero) {
			// Change the Velocity of our actor
			rigidBody.velocity = playerVelocity;
			// If we're supposed to be going up, and we're already grounded, make sure we leave the ground
		}

		//playerMovement

		//playerVelocity = Vector2.zero;

		//Check direction to flip sprite
		//Check direction to flip sprite

		//playerJump


		//setting player level for testing and demo
		//reset all to level 1
		if (Input.GetKeyDown("1")){
			brawlerLevel = 1;
			mobilityLevel = 1;
			heavyLevel = 1;
		}

		if (Input.GetKeyDown("2")){
			brawlerLevel = 2;
		}

		if (Input.GetKeyDown("3")){
			mobilityLevel = 2;
		}

		if (Input.GetKeyDown("4")){
			heavyLevel = 2;
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
			if (Time.time > nextFire) {
				attackAnimation ();
				nextFire = Time.time + playerFireRate;
				isAttacking (true);
				attackTimer = playerAttackTimer;
				fireRocket ();
			}
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
		if (FacingRight) {
			Instantiate (bullet, gunTip.position, Quaternion.Euler (new Vector3 (0, 0, 0)));
		} else if (!FacingRight) {
			Instantiate (bullet, gunTip.position, Quaternion.Euler (new Vector3 (0, 0, 180)));
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
			playerSpeed = 5.0f;
			playerJumpHeight = 2.5f;
			playerDoubleJump = false;
			playerAttack = 2f;
			playerFireRate = 1f;
			playerProjectileSpeed = 20.0f;
			playerAttackTimer = 0.5f;
			StartCoroutine(ChangeAnimatorController("AnimationControllers/playerBrawlerController"));
		}
		else if (newStance == playerStance.heavy){
			currentStance = playerStance.heavy;
			playerSpeed = 4.0f;
			playerJumpHeight = 0.0f;
			playerDoubleJump = false;
			playerAttack = 6f; 
			playerFireRate = 2f;
			playerProjectileSpeed = 10.0f;
			StartCoroutine(ChangeAnimatorController("AnimationControllers/playerHeavyController"));
		}
		else if (newStance == playerStance.mobility){
			currentStance = playerStance.mobility;
			playerSpeed = 7.0f;
			playerJumpHeight = 3.0f;
			playerDoubleJump = true;
			playerAttack = 1f;
			playerFireRate = 0.5f;
			playerProjectileSpeed = 15.0f;
			playerAttackTimer = 0.25f;
			StartCoroutine(ChangeAnimatorController("AnimationControllers/playerMobilityController"));
		}

	}

	public void takeDamage(float damage) //takes an int parameter to inflict desired amount of damage, will have to be called so that hurt animation is correctly played
	{
		if(!isHurt)
		{
			if (playerHP.currentHP > 0)
			{
				//setHurt();
				//isHurt = true;
				playerHP.addDamage(damage); //calls addDamage function of enemyHealth component
				//hurtCountdown = hurtCountdownTimer;
			}
		}
	}

	/// <summary>
	/// Move our actor in a desired direction
	/// </summary>
	/// <param name="direction">Positive is right, negative is left, 1.0f is the normal speed of the actor</param>
	public void Move(float direction) {
		// If our direction is positive, we're moving to the right
		if (direction > 0.0f) {
			FacingRight = true;
			// Otherwise, we're going left
			// Note: we don't care about 0.0f, because it'd be unusual for our character to constantly face right
		} else if (direction < 0.0f) {
			FacingRight = false;
		}

		// Set our Velocity to move on the next FixedUpdate tick
		playerVelocity = new Vector2(direction * playerSpeed, rigidBody.velocity.y);
	}

	private void GroundCheck() {
		// Cast a box below us just a hair to see if there's any objects below us
		var hits = Physics2D.BoxCastAll(boxCollider.bounds.center, new Vector2(boxCollider.bounds.size.x * 0.9f, boxCollider.bounds.size.y), 0.0f, Vector2.down, 0.5f);

		// Check to see if any of the things we hit is in the Terrain layer and that we've already left the ground
		if (hits.Any(hit => hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))) {
			playerGrounded = true;
		} else {
			playerGrounded = false;
			// If we can't hit the ground anymore, that means we've successfully left the ground below us
			hasLeftGround = true;
		}
	}
	public void UpdateCollider() {
		boxCollider.size = spriteRenderer.bounds.size;
	}

}