using UnityEngine;
using System.Collections;

public class DestroyEnemyProjectile : MonoBehaviour {

    public float timeAlive;
    public GameObject enemy;
    public GameObject player;
    // Use this for initialization
    void Start () {
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        EnemyController enemyController = enemy.GetComponent<EnemyController>();
        timeAlive = 1f;

        Destroy(gameObject, timeAlive);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Destroy(gameObject);
            if (other.tag == "Player")
            {
                playerController playerController = player.GetComponent<playerController>();
                EnemyController enemyController = enemy.GetComponent<EnemyController>();
                playerController.takeDamage(enemyController.rangeDamage);
            }
        }
    }
}
