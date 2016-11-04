using UnityEngine;
using System.Collections;

public class playerHealth : MonoBehaviour {

	public float currentHP;
	public int maxHP;

	// Use this for initialization
	void awake(){

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

	public void setHP(string enemyName)
	{
		if(enemyName == "Enemy 1 Melee") //sets maxHP based on enemy type
		{
			maxHP = 3;
		}
		currentHP = maxHP;
	}

	public void addDamage (float damage){
		float text = damage;
		floatingTextController.createFloatingText (text.ToString(), transform);
		currentHP -= damage;
		if (currentHP <= 0) {
			killEnemy ();
		}

	}

	void killEnemy(){
		Destroy(gameObject);
	}
}
