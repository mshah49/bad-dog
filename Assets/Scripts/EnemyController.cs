using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour { //NOTE: many of these variables were set to public so that they can be monitored in Unity inspector
	private Animator animator;
	private GameObject player;
	private float range;
    private float ramRange;
    private float meleeRange;
    private float hitRange;
	public bool inRange;
    public bool inRamRange;
	public bool inAttackRange;
    public bool inHitRange;
	public bool isFacingRight = true;
	private Rigidbody2D rb2d;
    private BoxCollider2D b2d;
	public float enemyMoveForce = 200f;
    public float enemyMaxSpeed;
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
    public float meleeDamage;
    public float ramDamage;
	public bool equalX;
    public bool equalY;
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
    public bool ramming;
    public bool attacking;
    public float attackMaxLength;
    public float attackDuration;
    public bool isAttacking;
    public bool attackLaunched;
    public float enemyProjectileSpeed;
    public float rangeDamage;
    public GameObject enemyProjectile;
    public Transform enemyGunTip;

	// Use this for initialization
	void Awake () {
		player = GameObject.FindGameObjectWithTag("Player");
		animator = GetComponent<Animator>();
        enemyHP = GetComponent<enemyHealth>(); //gets enemyHealth component from inspector
		inRange = false; //checks if enemy is detected
		inAttackRange = false; //checks if enemy is in ramming range
		rb2d = GetComponent<Rigidbody2D>();
        b2d = GetComponent<BoxCollider2D>();
		walking = false; //used to check if enemy is walking
		attackCooldown = 0;
		patrolCooldown = 0;
		patrolCooldownTimer = 3f;
		spawnX = transform.position.x; //stores the x coordinate of spawn so that patrol boundaries may be calculated
        spawnY = transform.position.y;
		attackIdle = false;
		patrolIdle = false;
		equalX = false; //checks to see if enemy and player are vertically aligned
        checker = false;
        enemyName = "";
        isHurt = false;
        if(name.Contains("enemy1Melee")) //checks the name for the substring
        {
            enemyName = "Enemy 1 Melee"; //name is set in case of extra spawns which are numbered (e.g. "enemy1Melee (1)", etc.)
        }
        if(name.Contains("enemy1Range1"))
        {
            enemyName = "Enemy 1 Range 1";
        }
        enemyHP.setHP(enemyName);
        canFlip = true;
        hurtCountdownTimer = 0.5f;
        hurtTest = false;
        hurtTestComplete = false;
        touchedGround = false;
        groundObtained = false;
        ramming = false;
        attacking = false;
        setStats();
        patrolLeftEnd = new Vector3(patrolMin, transform.position.y, 0); //left bound of patrol area
        patrolRightEnd = new Vector3(patrolMax, transform.position.y, 0); //right bound of patrol area
        isAttacking = false;
        attackLaunched = false;
    }

    // Update is called once per frame
    void Update () {
		if(player == null)
		{
			player = GameObject.FindGameObjectWithTag("Player");
        }

        rangeDetect();
        if (hurtTest) //if Hurt Test Complete is checked, uncheck it and check Hurt Test to test the hurt animation
        {
            if (!hurtTestComplete)
            {
                damageTest();
            }
        }
        getRange(); //primarily used to display range in unity inspector at any given time
        fixPatrolBound(); //corrects patrol coordinates of enemy while grounded
        checkVertical(); //checks vertical alignment with player
        checkHorizontal();

		if (inRange || setRam) //enemy is in range
		{
            enemyFight();
        }
		else //enemy patrolling
		{
            patrol();
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
		
        if(isHurt)
        {
            setHurt();
        }
        countdownUpdate();
	}

    void setStats() //sets stats of enemies, based on type
    {
        if (enemyName == "Enemy 1 Melee")
        {
            range = 15f;
            ramRange = 12f;
            meleeRange = 7f;
            hitRange = 2f;
            meleeDamage = 3f;
            ramDamage = 5f;
            enemyMaxSpeed = 4f;
            attackCooldownTimer = 3f;
            attackMaxLength = .5f;
            patrolMin = spawnX - 10;
            patrolMax = spawnX + 10;
            enemyProjectileSpeed = 20.0f;
        }
        else if(enemyName == "Enemy 1 Range 1")
        {
            range = 10f;
            enemyMaxSpeed = 3f;
            attackCooldownTimer = 2f;
            attackMaxLength = .3f;
            patrolMin = spawnX - 5;
            patrolMax = spawnX + 5;
            enemyProjectileSpeed = 20.0f;
            isFacingRight = false;
            Flip();
            rangeDamage = 4f;
        }
    }

    void countdownUpdate() //updates all time-based countdowns
    {
        if (attackDuration > 0)
        {
            attackDuration -= Time.deltaTime;
            setAttack();
        }
        else
        {
            if(attackLaunched)
            {
                attackLaunched = false;
                attackCooldown = attackCooldownTimer;
            }
            isAttacking = false;
        }
        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }
        else
        {
            attackIdle = false;
        }
        
        if (patrolCooldown > 0)
        {
            patrolCooldown -= Time.deltaTime;
        }
        if (hurtCountdown > 0)
        {
            hurtCountdown -= Time.deltaTime;
        }
        if (hurtCountdown <= 0)
        {
            isHurt = false;
            if (hurtTest && !hurtTestComplete)
            {
                hurtTestComplete = true;
                hurtTest = false;
            }
            if (inAttackRange)
            {
                enemyMaxSpeed = 12f;
            }
        }
    }

    void rangeDetect() //detects enemies in range, based on type
    {
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
        if (enemyName == "Enemy 1 Melee")
        {
            if (Vector3.Distance(transform.position, player.transform.position) < ramRange) //enemy detection
            {
                if (!inRamRange)
                {
                    inRamRange = true;
                }
            }
            else
            {
                inRamRange = false;
            }
            if (ramming)
            {
                enemyMaxSpeed = 12f; //enemy moves faster to ram the player
                attacking = false;
            }
            else
            {
                enemyMaxSpeed = 4f; //enemy moves at normal speed if player is not in ramming range
            }
            if (Vector3.Distance(transform.position, player.transform.position) < attackRange()) //enemy is in ramming range
            {
                if (!inAttackRange)
                {
                    inAttackRange = true;
                }
            }
            else
            {
                inAttackRange = false;
            }
            if (Vector3.Distance(transform.position, player.transform.position) < hitRange) //enemy detection
            {
                if (!inHitRange)
                {
                    inHitRange = true;
                }
            }
            else
            {
                inHitRange = false;
            }
        }
    }

    void checkVertical() //checks if enemy and player are vertically aligned
    {
        if (Mathf.Abs(player.transform.position.x - transform.position.x) <= 0.5f)
        {
            equalX = true;
        }
        else
        {
            equalX = false;
        }
    }

    void checkHorizontal()
    {
        if (Mathf.Abs(player.transform.position.y - transform.position.y) <= 0.5f)
        {
            equalY = true;
        }
        else
        {
            equalY = false;
        }
    }

    void enemyFight() //invokes battle AI, based on enemy type
    {
        patrolCooldown = 0;
        patrolIdle = false;
        if (enemyName == "Enemy 1 Melee")
        {
            if (!ramming && !attacking && !attackIdle)
            {
                if (inAttackRange)
                {
                    attacking = true;
                }
                else if (inRamRange)
                {
                    attacking = false;
                    ramming = true;
                }
            }
            if (ramming && !attacking)
            {
                meleeRamThePlayer();
            }
            else if (attacking && !ramming)
            {
                meleeAttackThePlayer();
            }
            else if (!attackIdle) //if the player is out of attack range but still in sight then it will chase its target
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
        else if(enemyName == "Enemy 1 Range 1")
        {
            if (player.transform.position.x < transform.position.x && isFacingRight)
            {
                Flip();
            }
            else if (transform.position.x < player.transform.position.x && !isFacingRight)
            {
                Flip();
            }
            if(attackCooldown <= 0 && !attackLaunched && equalY)
            {
                setAttack();
                enemyBulletFire();
                attackDuration = attackMaxLength;
                attackLaunched = true;
            }
            else
            {
                setIdle();
            }
        }
    }

    void meleeRamThePlayer() //ramming function for enemy 1 melee
    {
        if (setRam) //enemy has tried to ram the player
        {
            move(transform.position, ramX); //enemy is forced to move to a certain point past where they were vertically aligned with player
            if ((isFacingRight && transform.position.x >= ramX.x) || (!isFacingRight && transform.position.x <= ramX.x))
            {
                setRam = false;
                setIdle();
                ramming = false;
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
        else if (!attackIdle) //if the player is out of attack range but still in sight then it will chase its target
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
    
    void meleeAttackThePlayer() //attacking function for enemy 1 melee
    {
        if (inHitRange)
        {
            if (attackCooldown <= 0 && !attackLaunched)
            {
                if (player.transform.position.x < transform.position.x && isFacingRight)
                {
                    Flip();
                }
                else if (transform.position.x < player.transform.position.x && !isFacingRight)
                {
                    Flip();
                }
                setAttack();
                attackDuration = attackMaxLength;
                attackLaunched = true;
                attacking = false;
            }
            else
            {
                setIdle();
                attackIdle = true;
            }
        }
        else if(!inHitRange && !attackIdle)
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

    void enemyBulletFire()
    {
        if (isFacingRight)
        {
            Instantiate(enemyProjectile, enemyGunTip.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        }
        else if (!isFacingRight)
        {
            Instantiate(enemyProjectile, enemyGunTip.position, Quaternion.Euler(new Vector3(0, 0, 180f)));
        }
    }

    void patrol() //patrol function
    {
        attackCooldown = 0; //ensures these values aren't arbitrarily applied
        attackDuration = 0;
        attacking = false;
        ramLeft = -999999999;
        ramRight = 999999999;
        ramX = new Vector3();
        if (transform.position.x <= patrolMin || (isFacingRight && transform.position.x < patrolMax)) // enemy is past left bound or they are in bounds facing the right bound
        {
            if (!patrolIdle && transform.position.x <= patrolMin && !isFacingRight)
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

    void move(Vector3 start, Vector3 end) //move function
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

    public void setIdle() //triggers idle animation
    {
        if (!isHurt && !attackLaunched) //idle animation cannot be triggered if hurt
        {
            walking = false;
            if (enemyName == "Enemy 1 Melee")
            {
                animator.SetTrigger("enemy1MeleeIdle");
            }
            else if (enemyName == "Enemy 1 Range 1")
            {
                animator.SetTrigger("enemy1Range1Idle");
            }
        }
    }

    public void setHurt() //triggers hurt animation
	{
        attackLaunched = false;
        attackDuration = 0;
        if (enemyName == "Enemy 1 Melee")
        {
            animator.SetTrigger("enemy1MeleeHurt");
        }
        else if(enemyName == "Enemy 1 Range 1")
        {
            animator.SetTrigger("enemy1Range1Hurt");
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
            else if(enemyName == "Enemy 1 Range 1")
            {
                animator.SetTrigger("enemy1Range1Walk");
            }
        }
    }

    void setAttack() //triggers attack animation
    {
        if(!isHurt)
        {
            if(enemyName == "Enemy 1 Melee")
            {
                animator.SetTrigger("enemy1MeleeAttack");
            }
            else if(enemyName == "Enemy 1 Range 1")
            {
                animator.SetTrigger("enemy1Range1Attack");
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
            else if (enemyName == "Enemy 1 Range 1")
            {
                isFacingRight = !isFacingRight;
                Vector3 enemyScale = transform.localScale;
                enemyScale.x *= -1;
                transform.localScale = enemyScale;
            }
            else if (enemyName != "Enemy 1 Melee" && enemyName != "Enemy 1 Range 1")
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
        if (enemyName == "Enemy 1 Melee")
        {
            if (touchedGround && !groundObtained)
            {
                if (transform.position.y != patrolRightEnd.y)
                {
                    patrolRightEnd.y = transform.position.y;
                }
                if (transform.position.x != patrolLeftEnd.y)
                {
                    patrolLeftEnd.y = transform.position.y;
                }
                if (transform.position.y != spawnY)
                {
                    spawnY = transform.position.y;
                }
                groundObtained = true;
            }
        }
        else if(enemyName == "Enemy 1 Range 1")
        {
            if (touchedGround && !groundObtained)
            {
                if (transform.position.y != patrolRightEnd.y)
                {
                    patrolRightEnd.y = transform.position.y;
                }
                if (transform.position.x != patrolLeftEnd.y)
                {
                    patrolLeftEnd.y = transform.position.y;
                }
                if (transform.position.y != spawnY)
                {
                    spawnY = transform.position.y;
                }
                groundObtained = true;
            }
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

    void OnTriggerEnter2D(Collider2D other) //hitboxes of enemy inflict damage
    {
        if(other.tag == "Player")
        {
            playerController playerController = player.GetComponent<playerController>();
            if (enemyName == "Enemy 1 Melee")
            {
                if (enemyMaxSpeed == 12)
                {
                    Debug.Log("Enemy rammed!");
                    playerController.takeDamage(ramDamage);
                }
                else
                {
                    Debug.Log("Enemy hit with pipe!");
                    playerController.takeDamage(meleeDamage);
                }
            }
        }
    }
}
