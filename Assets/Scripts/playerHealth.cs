using UnityEngine;
using System.Collections;

public class playerHealth : playerController{
	

	public float currentHP;
	public float maxHP;


	// Use this for initialization
	void awake(){
		maxHP = playerHealth;
	}


	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if(currentHP > maxHP) //ensures enemy's current HP is not higher than max HP
		{
			currentHP = maxHP;
		}
	}

	public void addDamage (float damage){
		float text = damage;
		floatingTextController.createFloatingText (text.ToString(), transform);
		currentHP -= damage;
		if (currentHP <= 0) {
			killPlayer ();
		}

	}

	void killPlayer(){
		Destroy(gameObject);
	}
}
