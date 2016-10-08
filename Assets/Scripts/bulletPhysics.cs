using UnityEngine;
using System.Collections;

public class bulletPhysics : MonoBehaviour {
	public float rocketSpeed;
	Rigidbody2D rigidBody;
	// Use this for initialization
	void Awake () {
		rigidBody = GetComponent<Rigidbody2D> ();
		if(transform.localRotation.z>0)
			rigidBody.AddForce (new Vector2 (-1, 0) * rocketSpeed, ForceMode2D.Impulse);
		else 
			rigidBody.AddForce (new Vector2 (1, 0) * rocketSpeed, ForceMode2D.Impulse);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void removeForce(){
		rigidBody.velocity = new Vector2 (0, 0);
	}
}
