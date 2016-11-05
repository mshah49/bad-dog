using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class floatingText : MonoBehaviour {
	public Animator animator;
	private Text damageText;
	// Use this for initialization
	void OnEnable () {
		AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo (0);
		Destroy (gameObject, clipInfo [0].clip.length);
		damageText = animator.GetComponent<Text> ();
	}
	
	// Update is called once per frame
	public void setText (string text) {
		Debug.Log ("setText");
		damageText.text = text;
	}
}