using UnityEngine;
using System.Collections;

public class projectileHit : MonoBehaviour {
	public float weaponDamage;

	bulletPhysics myBP;

	// Use this for initialization
	void Awake () {
		myBP = GetComponentInParent<bulletPhysics> ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.layer == LayerMask.NameToLayer ("Shootable")) {
			myBP.removeForce ();
			Destroy (gameObject);
		}
	}
	void OnTriggerStay2D(Collider2D other){
		if (other.gameObject.layer == LayerMask.NameToLayer ("Shootable")) {
			myBP.removeForce ();
			Destroy (gameObject);
		}
}
}
