using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class playerEXPController : MonoBehaviour{

	public int playerLevel = 1;
	public float currentEXP = 0;
	public float maxEXP = 100;
	public Slider EXPSlider;  

	// Use this for initialization
	void awake(){
		EXPSlider.value = currentEXP;
	}
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if(currentEXP >= 100) //ensures enemy's current HP is not higher than max HP
		{
			playerLevel = playerLevel + 1;
			currentEXP = 0;
		}
		EXPSlider.value = currentEXP;
	}
}
