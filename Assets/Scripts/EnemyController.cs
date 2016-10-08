using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour { //NOTE: many of these variables were set to public so that they can be monitored in Unity inspector
	private Animator animator;
	private GameObject player;
	private float range;
	private float meleeRange;
	public bool inRange;
	public bool inAttackRange;
	public bool isFacingRight = true;
	private Rigidbody2D rb2d;
	public float enemyMoveForce = 200f;
	public float enemyMaxSpeed = 4f;
	public float enemyJumpForce = 500f;
	public float attackCooldownTimer;
	public bool walking;
	public bool idle;
	public float attackCooldown;
	public float spawnX;
	public float patrolMin;
	public float patrolMax;
	public bool setRam;
	public Vector3 ramX;
	public float patrolCooldown;
	public float patrolCooldownTimer;
	public bool attackIdle;
	public bool patrolIdle;
	Vector3 patrolLeftEnd;
	Vector3 patrolRightEnd;
	public float ramLeft;
	public float ramRight;
	public float rightRange;
	public float leftRange;
	public bool equalX;
	public bool isAttacking;

	// Use this for initialization
	void Awake () {
		player = GameObject.FindGameObjectWithTag("Player");
		animator = GetComponent<Animator>();
		range = 15f;
		meleeRange = 10f;
		inRange = false; //checks if enemy is detected
		inAttackRange = false; //checks if enemy is in ramming range
		rb2d = GetComponent<Rigidbody2D>();
		walking = false; //used to check if enemy is walking
		attackCooldown = 0;
		attackCooldownTimer = 3f;
		patrolCooldown = 0;
		patrolCooldownTimer = 3f;
		spawnX = transform.position.x; //stores the x coordinate of spawn so that patrol boundaries may be calculated
		patrolMin = spawnX - 10;
		patrolMax = spawnX + 10;
		attackIdle = false;
		patrolIdle = false;
		patrolLeftEnd = new Vector3(patrolMin, transform.position.y, 0); //left bound of patrol area
		patrolRightEnd = new Vector3(patrolMax, transform.position.y, 0); //right bound of patrol area
		equalX = false; //checks to see if enemy and player are vertically aligned
		isAttacking = false;
	}

	// Update is called once per frame
	void Update () {
		if(player == null)
		{
			player = GameObject.FindGameObjectWithTag("Player");
		}
		if (Vector3.Distance(transform.position, player.transform.position) < range) //enemy detection
		{
			if (!inRange)
			{
				inRange = true;
			}
		}
		else
		{
			inRange = false;
		}
		if (Vector3.Distance(transform.position, player.transform.position) < attackRange()) //enemy is in ramming range
		{
			if (!inAttackRange)
			{
				inAttackRange = true;
				enemyMaxSpeed = 12f; //enemy moves faster to ram the player
			}
		}
		else
		{
			inAttackRange = false;
			enemyMaxSpeed = 4f; //enemy moves at normal speed if player is not in ramming range
		}
		getRange(); //primarily used to display range in unity inspector at any given time
		if(player.transform.position.x == transform.position.x) //checks if enemy and player are vertically aligned
		{
			equalX = true;
		}
		else
		{
			equalX = false;
		}
					
		if (inRange) //enemy is in range
		{
			patrolCooldown = 0;
			patrolIdle = false;
			if(setRam) //enemy has tried to ram the player
			{
				float step = enemyMaxSpeed * Time.deltaTime;
				transform.position = Vector3.MoveTowards(transform.position, ramX, step);
				if((isFacingRight && transform.position.x >= ramX.x) || (!isFacingRight && transform.position.x <= ramX.x))
				{
					setRam = false;
					setIdle();
					attackIdle = true;
					attackCooldown = attackCooldownTimer;
				}
			}
			else if (equalX && !attackIdle) //enemy will ram the player if the x matches up
			{
				setRam = true;
				setWalk();
				if (isFacingRight)
				{
					ramRight = transform.position.x + 10;
					ramX = new Vector3(ramRight, transform.position.y, 0);
				}
				else
				{
					ramLeft = transform.position.x - 10;
					ramX = new Vector3(ramLeft, transform.position.y, 0);
				}
			}
			else if(!attackIdle) //if the player is out of range then it will chase its target
			{
				setWalk();
				if (player.transform.position.x < transform.position.x && isFacingRight)
				{
					Flip();
				}
				else if (transform.position.x < player.transform.position.x && !isFacingRight)
				{
					Flip();
				}
				float step = enemyMaxSpeed * Time.deltaTime;
				Vector3 targetOnDifferentY = new Vector3(player.transform.position.x, transform.position.y, 0); //ignores y value of target
				transform.position = Vector3.MoveTowards(transform.position, targetOnDifferentY, step);
			}
		}
		else //enemy patrolling
		{
			float step = enemyMaxSpeed * Time.deltaTime;
			if (transform.position.x <= patrolMin || (isFacingRight && transform.position.x < patrolMax)) // enemy is past left bound or they are in bounds facing the right bound
			{
				if(!patrolIdle && transform.position.x <= patrolMin && !isFacingRight)
				{
					setIdle();
					patrolCooldown = patrolCooldownTimer;
					patrolIdle = true;
				}
				else if (patrolCooldown <= 0)
				{
					patrolIdle = false;
					if (!isFacingRight)
					{
						Flip();
					}
					setWalk();
					transform.position = Vector3.MoveTowards(transform.position, patrolRightEnd, step);
				}
			}
			else if (transform.position.x >= patrolMax || (!isFacingRight && transform.position.x > patrolMin)) //enemy is past right bound or they are in bounds facing left bound
			{
				if (!patrolIdle && transform.position.x >= patrolMax && isFacingRight)
				{
					setIdle();
					patrolCooldown = patrolCooldownTimer;
					patrolIdle = true;
				}
				else if (patrolCooldown <= 0)
				{
					patrolIdle = false;
					if (isFacingRight)
					{
						Flip();
					}
					setWalk();
					transform.position = Vector3.MoveTowards(transform.position, patrolLeftEnd, step);
				}
			}
		}

		if (!walking)
		{
			setIdle();
		}
		if(attackCooldown > 0)
		{
			attackCooldown -= Time.deltaTime;
		}
		else
		{
			attackIdle = false;
		}
		if(patrolCooldown > 0)
		{
			patrolCooldown -= Time.deltaTime;
		}
	}

	void getRange() //used to display enemy range in Unity inspector
	{
		rightRange = transform.position.x + range;
		leftRange = transform.position.x - range;
	}

	float attackRange() //returns attack range based on enemy
	{
		if(tag == "Enemy")
		{
			return meleeRange;
		}
		return 0;
	}

	public void setIdle() //triggers idle animation
	{
		walking = false;
		if (tag == "Enemy")
		{
			animator.SetTrigger("enemy1MeleeIdle");
		}
	}

	public void setHurt() //triggers hurt animation
	{
		if (tag == "Enemy")
			{
				animator.SetTrigger("enemy1MeleeHurt");
			}
	}

	void setWalk() //triggers walk animation
	{
		walking = true;
		if(tag == "Enemy")
		{
			animator.SetTrigger("enemy1MeleeWalk");
		}
	}

	void FixedUpdate()
	{

	}

	void Flip() //flips sprite
	{
		isFacingRight = !isFacingRight;
		Vector3 enemyScale = transform.localScale;
		enemyScale.x *= -1;
		transform.localScale = enemyScale;
	}
}
