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
    private BoxCollider2D b2d;
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
	public Vector3 patrolLeftEnd;
	public Vector3 patrolRightEnd;
	public float ramLeft;
	public float ramRight;
	public float rightRange;
	public float leftRange;
	public bool equalX;
	public bool isAttacking;
    public bool checker;
    public string enemyName;
    public bool canFlip;
    public bool isHurt;
    public float hurtCountdownTimer;
    public float hurtCountdown;
    public bool hurtTest;
    public bool hurtTestComplete;
    private enemyHealth enemyHP;
    public Vector3 targetOnDifferentY;
    public float spawnY;
    public bool touchedGround;
    public bool groundObtained;

	// Use this for initialization
	void Awake () {
		player = GameObject.FindGameObjectWithTag("Player");
		animator = GetComponent<Animator>();
        enemyHP = GetComponent<enemyHealth>(); //gets enemyHealth component from inspector
		range = 15f;
		meleeRange = 10f;
		inRange = false; //checks if enemy is detected
		inAttackRange = false; //checks if enemy is in ramming range
		rb2d = GetComponent<Rigidbody2D>();
        b2d = GetComponent<BoxCollider2D>();
		walking = false; //used to check if enemy is walking
		attackCooldown = 0;
		attackCooldownTimer = 3f;
		patrolCooldown = 0;
		patrolCooldownTimer = 3f;
		spawnX = transform.position.x; //stores the x coordinate of spawn so that patrol boundaries may be calculated
        spawnY = transform.position.y;
		patrolMin = spawnX - 10;
		patrolMax = spawnX + 10;
		attackIdle = false;
		patrolIdle = false;
		patrolLeftEnd = new Vector3(patrolMin, transform.position.y, 0); //left bound of patrol area
		patrolRightEnd = new Vector3(patrolMax, transform.position.y, 0); //right bound of patrol area
		equalX = false; //checks to see if enemy and player are vertically aligned
		isAttacking = false;
        checker = false;
        enemyName = "";
        isHurt = false;
        if(name.Contains("enemy1Melee")) //checks the name for the substring
        {
            enemyName = "Enemy 1 Melee"; //name is set in case of extra spawns which are numbered (e.g. "enemy1Melee (1)", etc.)
        }
        enemyHP.setHP(enemyName);
        canFlip = true;
        hurtCountdownTimer = 1;
        hurtTest = false;
        hurtTestComplete = false;
        touchedGround = false;
        groundObtained = false;
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
        if(setRam)
        {
            enemyMaxSpeed = 12f;
        }
        if (hurtTest) //if Hurt Test Complete is checked, uncheck it and check Hurt Test to test the hurt animation
        {
            if (!hurtTestComplete)
            {
                damageTest();
            }
        }
        getRange(); //primarily used to display range in unity inspector at any given time
        fixPatrolBound(); //corrects patrol coordinates of enemy while grounded
		if(Mathf.Abs(player.transform.position.x - transform.position.x) <= 0.5f) //checks if enemy and player are vertically aligned
		{
			equalX = true;
		}
		else
		{
			equalX = false;
		}
					
		if (inRange || setRam) //enemy is in range
		{
			patrolCooldown = 0;
			patrolIdle = false;
			if(setRam) //enemy has tried to ram the player
			{
				move(transform.position, ramX); //enemy is forced to move to a certain point past where they were vertically aligned with player
                if ((isFacingRight && transform.position.x >= ramX.x) || (!isFacingRight && transform.position.x <= ramX.x))
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
				if (isFacingRight)
				{
					ramRight = transform.position.x + 10;
					ramX = new Vector3(ramRight, spawnY, 0);
				}
				else
				{
					ramLeft = transform.position.x - 10;
					ramX = new Vector3(ramLeft, spawnY, 0);
				}
			}
            else if(!attackIdle) //if the player is out of attack range but still in sight then it will chase its target
			{
				if (player.transform.position.x < transform.position.x && isFacingRight)
				{
					Flip();
                }
				else if (transform.position.x < player.transform.position.x && !isFacingRight)
				{
					Flip();
                }
				targetOnDifferentY = new Vector3(player.transform.position.x, spawnY, 0); //ignores y value of target
                move(transform.position, targetOnDifferentY);
            }
		}
		else //enemy patrolling
		{
            attackCooldown = 0; //ensures these values aren't arbitrarily applied
            ramLeft = -999999999;
            ramRight = 999999999;
            ramX = new Vector3();
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
                    move(transform.position, patrolRightEnd);
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
                    move(transform.position, patrolLeftEnd);
                }
			}
		}

		if (!walking && !isHurt) //ensures the idle animation is playing if it isn't supposed to be walking
		{
			setIdle();
		}
        if(inAttackRange) //enemy will not change directions while moving if the player is in attack range; can be modified later to account for height
        {
            canFlip = false;
        }
        else
        {
            canFlip = true;
        }

		if(attackCooldown > 0)
		{
			attackCooldown -= Time.deltaTime;
		}
		else
		{
			attackIdle = false;
		}
        if(isHurt)
        {
            setHurt();
        }
		if(patrolCooldown > 0)
		{
			patrolCooldown -= Time.deltaTime;
		}
        if(hurtCountdown > 0)
        {
            hurtCountdown -= Time.deltaTime;
        }
        if(hurtCountdown <= 0)
        {
            isHurt = false;
            if(hurtTest && !hurtTestComplete)
            {
                hurtTestComplete = true;
                hurtTest = false;
            }
            if(inAttackRange)
            {
                enemyMaxSpeed = 12f;
            }
        }
	}

    void move(Vector3 start, Vector3 end)
    {
        if(isHurt)
        {
            enemyMaxSpeed = 0;
        }
        if (enemyMaxSpeed > 0)
        {
            setWalk();
        }
        float step = enemyMaxSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(start, end, step);
    }

    void getRange() //used to display enemy range in Unity inspector
	{
		rightRange = transform.position.x + range;
		leftRange = transform.position.x - range;
	}

	float attackRange() //returns attack range based on enemy
	{
		if(enemyName == "Enemy 1 Melee")
		{
			return meleeRange;
		}
		return 0;
	}

	public void setIdle() //triggers idle animation
	{
        if(!isHurt) //idle animation cannot be triggered if hurt
        {
            walking = false;
            if (enemyName == "Enemy 1 Melee")
            {
                animator.SetTrigger("enemy1MeleeIdle");
            }
        }	
	}

    public void inflictDamage(float damage) //takes an int parameter to inflict desired amount of damage, will have to be called so that hurt animation is correctly played
    {
        if(!isHurt)
        {
            if (enemyName == "Enemy 1 Melee" && enemyHP.currentHP > 0)
            {
                setHurt();
                isHurt = true;
                enemyHP.addDamage(damage); //calls addDamage function of enemyHealth component
                hurtCountdown = hurtCountdownTimer;
            }
        }
    }

    public void damageTest() //tests hurt animation
    {
        inflictDamage(1);
    }

	public void setHurt() //triggers hurt animation
	{
        if (enemyName == "Enemy 1 Melee")
        {
            animator.SetTrigger("enemy1MeleeHurt");
        }
    }

	void setWalk() //triggers walk animation
	{
        if(!isHurt) //cannot move if hurt
        {
            walking = true;
            if (enemyName == "Enemy 1 Melee")
            {
                animator.SetTrigger("enemy1MeleeWalk");
            }
        }
    }

	void FixedUpdate()
	{
	}

	void Flip() //flips sprite
	{
        if(!isHurt) //cannot flip if hurt
        {
            if (enemyName == "Enemy 1 Melee") //enemy1Melee only flips if it is allowed to flip
            {
                isFacingRight = !isFacingRight;
                Vector3 enemyScale = transform.localScale;
                enemyScale.x *= -1;
                transform.localScale = enemyScale;
            }
            else if (enemyName != "Enemy 1 Melee")
            {
                isFacingRight = !isFacingRight;
                Vector3 enemyScale = transform.localScale;
                enemyScale.x *= -1;
                transform.localScale = enemyScale;
            }
        }
	}

    void fixPatrolBound()
    {
        if(touchedGround && !groundObtained)
        {
            if (transform.position.y != patrolRightEnd.y)
            {
                patrolRightEnd.y = transform.position.y;
            }
            if (transform.position.x != patrolLeftEnd.y)
            {
                patrolLeftEnd.y = transform.position.y;
            }
            if(transform.position.y != spawnY)
            {
                spawnY = transform.position.y;
            }
            groundObtained = true;
        }
    }

    void OnCollisionEnter2D(Collision2D other) //insert function for player getting hit and taking damage here
    {
        checker = true;
        if(other.gameObject.tag == "Ground")
        {
            if (transform.position.y != spawnY)
            {
                groundObtained = false;
            }
            touchedGround = true;
        }
        if(!isHurt) //cannot inflict damage if hurt
        {
            if (other.gameObject.tag == "Player")
            {
                Debug.Log("Collided with Player!");
                if (enemyName == "Enemy 1 Melee" && inAttackRange && !setRam && !attackIdle) //if the player is in enemy1Melee's attack range, then from an above function, enemy1Melee will be sped up, which is when the hitbox should be active
                {
                    setRam = true;
                    setWalk();
                    if (isFacingRight)
                    {
                        ramRight = transform.position.x + 10;
                        ramX = new Vector3(ramRight, spawnY, 0);
                    }
                    else
                    {
                        ramLeft = transform.position.x - 10;
                        ramX = new Vector3(ramLeft, spawnY, 0);
                    }
                }
                else if (enemyName != "Enemy 1 Melee")
                {

                }
            }
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if(other.gameObject.tag == "Ground")
        {
            touchedGround = false;
        }
    }
}
