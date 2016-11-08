using UnityEngine;
using System.Collections;

public class enemyHealth : MonoBehaviour {

	public GameObject player;
	public float currentHP;
	public int maxHP;
	public int expGain = 0;

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
        currentHP = maxHP;
    }

	public void addDamage (float damage){
		float text = damage;
		floatingTextController.createFloatingText (text.ToString(), transform);
		currentHP -= damage;
		if (currentHP <= 0) {
			enemyEXP (expGain);
			killEnemy ();
		}

	}

	public void enemyEXP(int exp){
		player = GameObject.FindGameObjectWithTag("Player");
		playerEXPController playerEXPController = player.GetComponent<playerEXPController>();
		playerEXPController.currentEXP += exp;
	}

	void killEnemy(){
		Destroy(gameObject);
	}
}
