using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]

public class playerController : MonoBehaviour {

	//Starting player attributes
	public float playerHealth = 5f;
	public float playerSpeed = 1.0f;
	public float playerMaxSpeed = 6f;
	public float playerJumpHeight = 4f;
	public bool playerDoubleJump = false;
	public float playerAttack = 3f; 
	public float playerFireRate = 1f;

	bool playerGrounded = false;
	bool facingRight;

	float groundCheckRadius = 0.2f;
	public LayerMask groundLayer;
	public Transform groundCheck;

	Rigidbody2D rigidBody;
	Animator animator;
	BoxCollider2D boxCollider;
	SpriteRenderer spriteRenderer;

	public void Awake(){
		rigidBody = GetComponent<Rigidbody2D> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
		boxCollider = GetComponent<BoxCollider2D> ();
		animator = GetComponent<Animator> ();
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
		float move = Input.GetAxis ("Horizontal");
		animator.SetFloat ("Speed", Mathf.Abs (move));
		rigidBody.velocity = new Vector2 (move * playerMaxSpeed, rigidBody.velocity.y);
		//Check direction to flip sprite
		if (move > 0 && !facingRight) 
			flip ();
		else if (move < 0 && facingRight)
			flip ();
		//playerJump
		if(playerGrounded && Input.GetAxis("Jump")>0){
			rigidBody.AddForce(new Vector2(0,playerJumpHeight));
		}
			
		//change stances
		if (Input.GetKeyDown("z")){
			updateStance (playerStance.mobility);
		}
		if (Input.GetKeyDown("x")){
			updateStance (playerStance.heavy);
		}
		if (Input.GetKeyDown("c")){
			updateStance (playerStance.brawler);
		}
	}

	///<summary>
	/// Animation Codes
	///</summary>

	// flips character sprite
	void flip(){
		facingRight = !facingRight;
		Vector3 myScale = transform.localScale;
		myScale.x *= -1;
		transform.localScale = myScale;
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
		}
	private void idleAnimation(){
		runAnimation (false);
	}

	///<summary>
	/// Stance Codes
	///</summary>

	public enum playerStance{
		brawler,heavy,mobility,
	}

	//Updates player sprite and attributes based on selected stance
	public void updateStance(playerStance currentStance){
		if (currentStance == playerStance.brawler){

			playerSpeed = 1.0f;
			playerJumpHeight = 6.0f;
			playerDoubleJump = false;
			playerAttack = 3f;
			playerFireRate = 1f;
			StartCoroutine(ChangeAnimatorController("AnimationControllers/playerBrawlerController"));
		}
		else if (currentStance == playerStance.heavy){
			playerSpeed = 0.5f;
			playerJumpHeight = 4f;
			playerDoubleJump = false;
			playerAttack = 2f; 
			playerFireRate = 0.5f;
		}
		else if (currentStance == playerStance.mobility){
			playerSpeed = 1.5f;
			playerJumpHeight = 12f;
			playerDoubleJump = true;
			playerAttack = 1f;
			playerFireRate = 2f;
			StartCoroutine(ChangeAnimatorController("AnimationControllers/playerMobilityController"));
		}
	}

}