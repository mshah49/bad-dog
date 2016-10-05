using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour {
    
	// components
	private Animator animator;
	private Rigidbody2D rigidbody;
	private GameObject player;
	public float spawnX;

	//attributes
	public int damage;
	public int currentHP;
	public int maxHP;

	//animation
	public bool walking;
	public bool idle;
	public bool attacking;

	//movement
	private Transform enemyTransform;
	public bool isFacingRight = true;
	public float enemyMoveForce = 200f;
	public float enemyMaxSpeed = 4f;
	public float patrolMin;
	public float patrolMax;

	//detection 
	private float detectionRange;
	public bool playerInRange;
    //attack
	private float meleeRange;
	public float attackCooldown;
	public float attackCooldownTimer;

	//initialization
	void Awake () {

		//get components
		animator = GetComponent<Animator>();
		rigidbody = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
		spawnX = transform.position.x;

		//enemy attributes
		maxHP = 3;
		currentHP = maxHP;
		damage = 1;

		//attack attributes
        meleeRange = 2f;
		playerInRange = false;
		attackCooldown = 0;
		attackCooldownTimer = 3f;
		attacking = false;

		//movement
		enemyTransform = transform;
		walking = false;
        idle = true;
        patrolMin = spawnX - 10;
        patrolMax = spawnX + 10;

		//enemy detection
		detectionRange = 10f;
	}

	void FixedUpdate()
	{

	}

	// Update is called once per frame
	void Update () {
      
		//idle
		if (!walking)
		{
			setIdle();
		}

		//player detection
		playerDetection();

		//attack cooldown
		if(attackCooldown > 0)
		{
			attackCooldown -= Time.deltaTime;
		}

		// checks if in attack range and attacks, else checks if in detection range and moves, else patrols
		if(Vector3.Distance(enemyTransform.position, player.transform.position) < meleeRange && attackCooldown <= 0)
        {
			//attack
            setAttack();
            attackCooldown = attackCooldownTimer;

        }

		//move toward enemy if in detection range
		else if(playerInRange)
        {
            idle = false;

			if(Vector3.Distance(enemyTransform.position, player.transform.position) > meleeRange)
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

		//patrol
        else
        {
            float step = enemyMaxSpeed * Time.deltaTime;
            Vector3 patrolLeftEnd = new Vector3(patrolMin, transform.position.y, 0); //left bound of patrol area
            Vector3 patrolRightEnd = new Vector3(patrolMax, transform.position.y, 0); //right bound of patrol area
            setWalk();
            if (transform.position.x <= patrolMin || (isFacingRight && transform.position.x < patrolMax))
            {
                if (!isFacingRight)
                {
                    Flip();
                }
                transform.position = Vector3.MoveTowards(transform.position, patrolRightEnd, step);
            }
            else if (transform.position.x >= patrolMax || (!isFacingRight && transform.position.x > patrolMin))
            {
                if (isFacingRight)
                {
                    Flip();
                }
                transform.position = Vector3.MoveTowards(transform.position, patrolLeftEnd, step);
            }
        }

		//death	
        if(currentHP == 0)
        {
            Destroy(gameObject);
        }
    }

	//player Detection
	void playerDetection(){
		if (Vector3.Distance(enemyTransform.position, player.transform.position) < detectionRange)
		{   
			playerInRange = true;
		}
		else
		{
			playerInRange = false;
		}
	}

	// animation code
    void setIdle()
    {
        walking = false;
        animator.SetTrigger("enemy1MeleeIdle");
    }

    void setAttack()
    {
        walking = false;
        attacking = true;
        animator.SetTrigger("enemy1MeleeAttack");
    }

    void setWalk()
    {
        walking = true;
        animator.SetTrigger("enemy1MeleeWalk");
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 enemyScale = transform.localScale;
        enemyScale.x *= -1;
        transform.localScale = enemyScale;
    }
}
