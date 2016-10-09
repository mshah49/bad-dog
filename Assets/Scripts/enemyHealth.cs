using UnityEngine;
using System.Collections;

public class enemyHealth : MonoBehaviour {

	public float currentHP = 3;
	public int maxHP = 3;

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
		if (currentHP <= 0) {
			killEnemy ();
		}

	}

	void killEnemy(){
			Destroy(gameObject);
	}
}
