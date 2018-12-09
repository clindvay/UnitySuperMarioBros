using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomScript : Physics_Controller {

    public float moveSpeed = 3f;
    public int moveDir;

    protected override void ComputeVelocity()
    {
        targetVelocity.x = moveSpeed * moveDir;
    }

    public void SetDir(string direction)
    {
        if (direction == "left")
        {
            moveDir = -1;

        }
        if (direction == "right")
        {
            moveDir = 1;
        }
    }



	
}
