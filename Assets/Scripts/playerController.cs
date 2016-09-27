using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour {
	//Horizontal movement
	public float maxSpeed;

	//Vertical movement
	bool grounded = false;
	float groundCheckRadius = 0.2f;
	public LayerMask groundLayer;
	public Transform groundCheck;
	public float jumpHeight;

	Rigidbody2D myRB;
	Animator myAnim;
	bool facingRight;
	// Use this for initialization

	void Start () {
		myRB = GetComponent<Rigidbody2D> ();
		myAnim = GetComponent<Animator> ();
		facingRight = true;
	}

	void Update(){
		if(grounded && Input.GetAxis("Jump")>0){
			myRB.AddForce(new Vector2(0,jumpHeight));
			}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//check for ground
		grounded = Physics2D.OverlapCircle(groundCheck.position,groundCheckRadius,groundLayer);

		float move = Input.GetAxis ("Horizontal");
		myAnim.SetFloat ("Speed", Mathf.Abs (move));
		myRB.velocity = new Vector2 (move * maxSpeed, myRB.velocity.y);
		if (move > 0 && !facingRight) 
			flip ();
		  else if (move < 0 && facingRight)
			flip ();
			}


	void flip(){
		facingRight = !facingRight;
		Vector3 myScale = transform.localScale;
		myScale.x *= -1;
		transform.localScale = myScale;
	}



}