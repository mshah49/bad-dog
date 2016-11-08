using UnityEngine;
using System.Collections;

public class EnemyBulletController : MonoBehaviour {

    public float speed;
    public GameObject enemy;
    public GameObject player;
    public Rigidbody2D rb2d;
    public EnemyController enemyController;
    public playerController playerController;

	// Use this for initialization
	void Awake () {
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        player = GameObject.FindGameObjectWithTag("Player");
        enemyController = enemy.GetComponent<EnemyController>();
        playerController = player.GetComponent<playerController>();
        rb2d = GetComponent<Rigidbody2D>();
        speed = enemyController.enemyProjectileSpeed;
        if (transform.localRotation.z > 0)
            rb2d.AddForce(new Vector2(-1 * speed, 0), ForceMode2D.Impulse);
        else
            rb2d.AddForce(new Vector2(1 * speed, 0), ForceMode2D.Impulse);
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            playerController.takeDamage(enemyController.rangeDamage);
            Destroy(gameObject);
        }
    }

    public void removeForce()
    {
        rb2d.velocity = new Vector2(0, 0);
    }
}
