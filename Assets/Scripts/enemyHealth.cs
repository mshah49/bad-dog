using UnityEngine;
using System.Collections;

public class enemyHealth : MonoBehaviour {

	public float currentHP = 3;
	public int maxHP = 3;
	private float deathHeight = -20.0f;

	// Use this for initialization
	void awake(){
		
	}


	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void addDamage (float damage){
		currentHP -= damage;
		if (currentHP <= 0 || transform.position.y < deathHeight) {
			killEnemy ();
		}

	}

	void killEnemy(){
			Destroy(gameObject);
	}
}
