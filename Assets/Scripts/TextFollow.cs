using UnityEngine;
using System.Collections;

public class TextFollow : MonoBehaviour {

    public Vector3 position;
    public Camera camera;
    public GameObject target;
    private Vector3 targetPosition;
    private RectTransform rt;
    private RectTransform canvasRT;
    private Vector3 targetScreenPosition;
    private Vector3 offset = new Vector3(0,2,0);

    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Enemy1Melee");
        targetPosition = target.transform.position;
        rt = GetComponent<RectTransform>();
        canvasRT = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        targetScreenPosition = camera.WorldToScreenPoint(targetPosition + offset);
        rt.anchorMax = targetScreenPosition;
        rt.anchorMin = targetScreenPosition;
    }

	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        targetPosition = target.transform.position;
        targetScreenPosition = camera.WorldToScreenPoint(targetPosition + offset);
        rt.anchorMax = targetScreenPosition;
        rt.anchorMin = targetScreenPosition;
    }

    void FixedUpdate()
    {
        
    }
}
