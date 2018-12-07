using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParrallaxScrolling : MonoBehaviour {

    public Transform[] backgrounds;     //Array of all layers to be parallaxed.
    private float[] parrallaxScales;     //The proportion of the camera's movement to move the backgrounds.
    public float smoothing = 1f;        //How smooth.  Must be above 0.

    private Transform cam;              //Ref to camera's transform.
    private Vector3 previousCamPos;

    //Called before Start, but after all objects are loaded.
    private void Awake()
    {
        cam = Camera.main.transform;
    }

    void Start ()
    {
        previousCamPos = cam.position;
        parrallaxScales = new float[backgrounds.Length];

        for (int i = 0; i < backgrounds.Length; i++)
        {
            parrallaxScales[i] = backgrounds[i].position.z * -1;  //Assign the scale to be inversely proportional to it's Z position.
        }
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < backgrounds.Length; i++)
        {
            float parallax = (previousCamPos.x - cam.position.x) * parrallaxScales[i]; //change in camera * parallax scale.
            float backgroundTargetPosX = backgrounds[i].position.x + parallax; //Target x position + parralax.
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);

        }

        previousCamPos = cam.position;

	}
}
