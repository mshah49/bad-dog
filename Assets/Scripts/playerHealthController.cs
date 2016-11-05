using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class playerHealthController : MonoBehaviour{


	private GameObject player;
	public float currentHP =100;
	public float maxHP = 100;
	public Slider healthSlider;  

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
		healthSlider.value = currentHP;
	}

	public void addDamage (float damage){
		float text = damage;
		floatingTextController.createFloatingText (text.ToString(), transform);
		currentHP -= damage;
		healthSlider.value = currentHP;
		if (currentHP <= 0) {
			killPlayer ();
		}

	}

	void killPlayer(){
		Destroy(gameObject);
	}
}
