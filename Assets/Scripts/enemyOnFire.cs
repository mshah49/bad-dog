using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class enemyOnFire : MonoBehaviour {
	private enemyHealth enemyHP;

	private int chanceFire;
	private float burnCountdown;
	private float burnCountdownTimer = 0.5f;
	public float burnDamage;
	public bool onFire;
	// Use this for initialization
	void Start () {
	
	}

	void Awake(){
		enemyHP = GetComponent<enemyHealth>();
	}
	
	// Update is called once per frame
	void Update () {
		if (onFire) {
			if (burnCountdown > 0) {
				burnCountdown -= Time.deltaTime;
			}
			if (burnCountdown <= 0) {
				enemyHP.addDamage (1);
				burnCountdown = burnCountdownTimer;
			}
		}
	}

	public void catchFire(){
		chanceFire = Random.Range (0, 100);
		if (chanceFire>75){
			onFire = true;
		}
	}
	public void doDamage(){
		enemyHP.addDamage (1);
	}
}
