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
	SpriteRenderer renderer;
	private Material mat;
	public Color[] colors = {Color.red, Color.red};
	// Use this for initialization
	void Start () {
	
	}

	void Awake(){
		enemyHP = GetComponent<enemyHealth>();
		renderer = GetComponent<SpriteRenderer>();
		mat = GetComponent<SpriteRenderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
		if (onFire) {
			int index = 0;
			mat.color = colors[index % 4];
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
			renderer.color = new Color (234f,89f,10f,255F);
		}
	}
	public void doDamage(){
		enemyHP.addDamage (1);
	}
}
