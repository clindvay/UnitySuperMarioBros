using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_ControllerBig : Player_Controller2 {

    public bool isBig;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        if (isBig)
        {
            animator.runtimeAnimatorController = Resources.Load("Assets/Resources/Animation/PlayerBig.controller") as RuntimeAnimatorController;
            //animator.SetBool("isBig", true);


            Vector2 yInput;
            yInput.y = Input.GetAxis("Vertical");
            if (yInput.y > 0) {animator.SetBool("IsCrouching", true);}
            else {animator.SetBool("IsCrouching", false);}

        }

	}
}
