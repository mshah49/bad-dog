using UnityEngine;
using System.Collections;

public class projectileHit : MonoBehaviour {
	public float weaponDamage = 1;
	bulletPhysics bullet;

	// Use this for initialization
	void Awake () {
		bullet = GetComponentInParent<bulletPhysics> ();
	}
	
	// Update is called once per frame
	void Update () {

	}
	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.layer == LayerMask.NameToLayer ("Shootable")) {
			bullet.removeForce ();
			Destroy (gameObject);
			if (other.tag == "Enemy") {
				enemyHealth hurtEnemy = other.gameObject.GetComponent<enemyHealth> ();
				hurtEnemy.addDamage (weaponDamage);
				EnemyController enemyHurtAnimation = other.gameObject.GetComponent<EnemyController> ();
				enemyHurtAnimation.setHurt();
			}
		}
	}
	void OnTriggerStay2D(Collider2D other){
		if (other.gameObject.layer == LayerMask.NameToLayer ("Shootable")) {
			bullet.removeForce ();
			Destroy (gameObject);
			if (other.tag == "Enemy") {
				enemyHealth hurtEnemy = other.gameObject.GetComponent<enemyHealth> ();
				hurtEnemy.addDamage (weaponDamage);

				EnemyController enemyController = other.gameObject.GetComponent<EnemyController> ();
				enemyController.setHurt();
			}
		}
}
}
