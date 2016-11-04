using UnityEngine;
using System.Collections;

public class floatingTextController : MonoBehaviour {
	private static floatingText popupText;
	private static GameObject canvas;

	public static void initialize(){
		canvas = GameObject.Find ("Canvas");
		if(!popupText)
			popupText = Resources.Load<floatingText> ("Prefabs/FX/PopupTextParent");
	}
	public static void createFloatingText(string text, Transform location){
		floatingText instance = Instantiate (popupText);
		Vector2 screenPosition = Camera.main.WorldToScreenPoint (new Vector2(location.position.x + Random.Range(-.5f, .5f),location.position.y + Random.Range(.5f, 1.5f)));

		instance.transform.SetParent (canvas.transform, false);
		instance.transform.position = screenPosition;
		instance.setText (text);
	}
}
