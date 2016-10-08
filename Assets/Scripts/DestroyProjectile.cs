using UnityEngine;
using System.Collections;

public class DestroyProjectile : MonoBehaviour {
	public float timeAlive;
	// Use this for initialization
	void Awake () {
		Destroy (gameObject, timeAlive);
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
