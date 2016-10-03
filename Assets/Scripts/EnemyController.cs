using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour {
    private Transform enemyTransform;
    private Animator animator;
    private GameObject player;
    private float range;
    private float meleeRange;
    public bool inRange;
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
    public int currentHP;
    public int maxHP;
    public bool attacking;
    
	// Use this for initialization
	void Awake () {
        enemyTransform = transform;
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        range = 10f;
        meleeRange = 2f;
        inRange = false;
        rb2d = GetComponent<Rigidbody2D>();
        walking = false;
        idle = true;
        attackCooldown = 0;
        attackCooldownTimer = 3f;
        spawnX = transform.position.x;
        patrolMin = spawnX - 10;
        patrolMax = spawnX + 10;
        maxHP = 3;
        currentHP = 3;
        attacking = false;
	}
	
	// Update is called once per frame
	void Update () {
	    if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if (Vector3.Distance(enemyTransform.position, player.transform.position) < range) //enemy detection
        {
            if (!inRange)
            {
                playerInRange();
            }
        }
        else
        {
            playerOutOfRange();
        }
        if(Vector3.Distance(enemyTransform.position, player.transform.position) < attackRange() && attackCooldown <= 0)
        {
            if (walking)
            {
                setIdle();
            }
            setAttack();
            attackCooldown = attackCooldownTimer;
        }
        else if(inRange) //moving one object towards another
        {
            idle = false;
            if (player.transform.position.x == transform.position.x)
            {
                setIdle();
            }
            else if(Vector3.Distance(enemyTransform.position, player.transform.position) > attackRange())
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
        else
        {
            idle = true;
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

        if (!walking)
        {
            setIdle();
        }
        if(attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }

        if(currentHP > maxHP)
        {
            currentHP = maxHP;
        }
        if(currentHP == 0)
        {
            Destroy(gameObject);
        }
    }

    float attackRange()
    {
        if(tag == "Enemy1Melee")
        {
            return meleeRange;
        }
        return 0;
    }

    void setIdle()
    {
        walking = false;
        if (tag == "Enemy1Melee")
        {
            animator.SetTrigger("enemy1MeleeIdle");
        }
    }

    void setAttack()
    {
        walking = false;
        attacking = true;
        if (tag == "Enemy1Melee")
        {
            animator.SetTrigger("enemy1MeleeAttack");
        }
        attacking = false;
    }

    void setWalk()
    {
        walking = true;
        if(tag == "Enemy1Melee")
        {
            animator.SetTrigger("enemy1MeleeWalk");
        }
    }

    void FixedUpdate()
    {

    }

    void playerInRange()
    {
        inRange = true;
    }

    void playerOutOfRange()
    {
        inRange = false;
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 enemyScale = transform.localScale;
        enemyScale.x *= -1;
        transform.localScale = enemyScale;
    }
}
