using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controller : MonoBehaviour {

    public GameObject player;
    public float xMin;
    public float xMax;
    public float yMin;
    public float yMax;
    public bool enableXMax;
    public bool enableYMax;

	// Use this for initialization
	void Start ()
    {
    }
	
	// Update is called once per frame
	void LateUpdate ()
    {
        if (!enableXMax) { xMax = Mathf.Infinity;}
        if (!enableYMax) { yMax = Mathf.Infinity;}
        float x = Mathf.Clamp(player.transform.position.x, xMin, xMax);
        float y = Mathf.Clamp(player.transform.position.y, yMin, yMax);
        gameObject.transform.position = new Vector3(x, y, gameObject.transform.position.z);
    }
}
