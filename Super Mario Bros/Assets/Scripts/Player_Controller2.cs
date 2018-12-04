using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller2 : Physics_Controller {

    public float jumpTakeOffSpeed = 7f;
    public float fallMultiplier = 1.15f;
    public float walkSpeed = 1f;
    public float runSpeed = 2f;
    public float hSpeed;
    public float runMax = 14f;
    public float walkMax = 7f;
    public float hMax;
    public float hDecceleration = 0.5f;
    public float jumpDecceleration = 0.5f;
    public bool facingRight = false;
    public bool isBigMario = false;
    public Animator animator;
    private SpriteRenderer spriteRenderer;

    // Use this for initialization
    void Awake () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        hSpeed = walkSpeed;
        hMax = walkMax;
	}

    protected override void ComputeVelocity()
    {
        //Get Horizontal Input  Move.x is just the direction (-1 , 0, or 1);
        Vector2 move = Vector2.zero;
        move.x = Input.GetAxis("Horizontal");
        Debug.Log(move.x);


        //Get Vertical Input
        Vector2 yInput = Vector2.zero;
        yInput.y = Input.GetAxis("Vertical");

        //Direction Facing
        if (move.x < 0.0f && facingRight == false && grounded) {FlipPlayer();}
        else if (move.x > 0.0f && facingRight == true && grounded) {FlipPlayer();}

        // Walking/Idle transition
        if (move.x != 0) { animator.SetBool("Walking", true); } else { animator.SetBool("Walking", false); }

        //Walking vs Running
        if (Input.GetButton("Fire3") && grounded) { hSpeed = runSpeed;  hMax = runMax; } //If on ground and run key, run.
        else if (!Input.GetButton("Fire3") && grounded) {hSpeed = walkSpeed; hMax = walkMax; } //If on ground and no run key, walk.


        //Crouching
        if (yInput.y < 0 && isBigMario)
        {
            animator.SetBool("IsCrouching", true);
            move.x = 0;
            //targetVelocity.x *= hDecceleration;
            //need a way to decrease h-velocity till come to a stop.
        }
        else { animator.SetBool("IsCrouching", false); }

        //If no movement is detected, then deccelerate.

        if (move.x == 0 && !grounded) { targetVelocity.x *= jumpDecceleration; } //Decelerate slower in the air
        else if (move.x == 0 && grounded) { targetVelocity.x *=  hDecceleration; } //faster on the ground.
        else
        {

            //The actual command that controls movement.
            targetVelocity.x += move.x * hSpeed;
        }

        //Acceleration
        // velocity = initial velocity + ( accelleration * time )
        // velocity.x = velocity.x + accel * time.deltaTime;
        // Decelleration 
        // velocity.x = velocity.x + (-accel) * time.deltaTime;


        targetVelocity.x = Mathf.Clamp(targetVelocity.x, -hMax, hMax);

        if (Mathf.Abs(targetVelocity.x) < 0.001f) { targetVelocity.x = 0;} //When we get to small numbers, stop horizontal movement.
        
        //Slide Animation
        if (move.x > 0 && velocity.x < 0 && grounded) { animator.SetBool("IsSliding", true); }
        else if (move.x < 0 && velocity.x > 0 && grounded) { animator.SetBool("IsSliding", true);}
        else { animator.SetBool("IsSliding", false); }

        //targetVelocity = move * hSpeed;
        //How do we get it to keep the momentum even if the air?



        if (Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y = jumpTakeOffSpeed;
            animator.SetBool("Jumping", true);
        }
        else if (Input.GetButtonUp("Jump"))
        {
            if(velocity.y > 0){velocity.y = velocity.y * 0.5f;}
        }

        //Makes the falling portion of the jump a bit faster than jumping portion (set fallMultiplier to adjust).
        if (velocity.y < 0) { velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1); }
        
        if (velocity.y == 0 && grounded){animator.SetBool("Jumping", false);}




    }

    public void FlipPlayer()
    {
        facingRight = !facingRight;
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }

    public void MakeBig(bool big)
    {
        if (big == true)
        {
            isBigMario = true;
            animator.SetBool("IsBig", true);


        }

        else if (big == false)
        {
            isBigMario = false;
            animator.SetBool("IsBig", false);
         }

        //1. Transition to big animation control set.
        //2. Change Collision box size (or disable/enable correct box).
    }

}
