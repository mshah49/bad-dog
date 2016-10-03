using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {

    public Text detectText;
    private Transform enemyTransform;
    private GameObject player;
    private float range;
    private bool inRange;
    public bool isFacingRight = true;
    private Rigidbody2D rb2d;
    public float enemyMoveForce = 200f;
    public float enemyMaxSpeed = 4f;
    public float enemyJumpForce = 500f;
    
	// Use this for initialization
	void Awake () {
        enemyTransform = transform;
        detectText.text = "No enemies in sight.";
        player = GameObject.FindGameObjectWithTag("Player");
        range = 10f;
        inRange = false;
        rb2d = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
	    if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if(Vector3.Distance(enemyTransform.position, player.transform.position) < range) //enemy detection
        {
            if(!inRange)
            {
                playerInRange();
            }
        }
        else
        {
            playerOutOfRange();
        }
	}

    void FixedUpdate()
    {
        if(inRange) //moving one object towards another
        {
            if(player.transform.position.x < transform.position.x && isFacingRight)
            {
                Flip();
            }
            else if(transform.position.x < player.transform.position.x && !isFacingRight)
            {
                Flip();
            }
            float step = enemyMaxSpeed * Time.deltaTime;
            Vector3 targetOnDifferentY = new Vector3(player.transform.position.x, transform.position.y, 0); //ignores y value of target
            transform.position = Vector3.MoveTowards(transform.position, targetOnDifferentY, step);
        }
    }
    void playerInRange()
    {
        detectText.text = "Enemy detected!";
        inRange = true;
    }

    void playerOutOfRange()
    {
        detectText.text = "No enemies in sight.";
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
