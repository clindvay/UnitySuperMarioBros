using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller2 : Physics_Controller {

    public float jumpTakeOffSpeed = 7f;
    public float fallMultiplier = 1.15f;
    public float hSpeed = 1f;
    public float runMax = 14f;
    public float walkMax = 7f;
    public float hMax;
    public float hDecceleration = 0.5f;
    public float jumpDecceleration = 0.5f;
    public float runDecceleration = 0.5f;
    public bool facingRight = false;
    public bool isBigMario = false;
    public bool growOrShrink = false;
    public Animator animator;
    private BoxCollider2D pCollider;
    private SpriteRenderer spriteRenderer;

    // Use this for initialization
    void Awake () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        pCollider = GetComponent<BoxCollider2D>();
        hMax = walkMax;
	}


    protected override void ComputeVelocity()
    {
        if (!isBigMario && growOrShrink)
        {
            MakeBig(true);
            growOrShrink = false;
        }
        else if (isBigMario && growOrShrink)
        {
            MakeBig(false);
            growOrShrink = false;
        }
        

     
        //Get Horizontal Input  Move.x is just the direction (-1 , 0, or 1);
        Vector2 move = Vector2.zero;
        move.x = Input.GetAxis("Horizontal");


        //Get Vertical Input
        Vector2 yInput = Vector2.zero;
        yInput.y = Input.GetAxis("Vertical");

        //Direction Facing
        if (move.x < 0.0f && facingRight == false && grounded) {FlipPlayer();}
        else if (move.x > 0.0f && facingRight == true && grounded) {FlipPlayer();}

        // Walking/Idle transition
        if (move.x != 0) { animator.SetBool("Walking", true); } else { animator.SetBool("Walking", false); }

        //Walking vs Running
        if (Input.GetButton("Fire3") && grounded) { hMax = runMax; } //If on ground and run key, run.
        else if (!Input.GetButton("Fire3") && grounded) {hMax = walkMax; } //If on ground and no run key, walk.

        //Crouching
        if (Input.GetKey("down") && isBigMario)
        {
            animator.SetBool("IsCrouching", true);
            pCollider.size = new Vector2(pCollider.size.x, 0.95f);
            pCollider.offset = new Vector2(pCollider.offset.x, -0.5f);
            move.x = 0;
        }
        else if (!Input.GetKey("down") && isBigMario)
        {
            animator.SetBool("IsCrouching", false);
            pCollider.size = new Vector2(pCollider.size.x, 1.95f);
            pCollider.offset = new Vector2(pCollider.offset.x, 0f);

        }


        //If no movement is detected, then deccelerate.
        if (move.x == 0)
        {
            float slowDown;
            if (!grounded) { slowDown = jumpDecceleration;}
            else if (Input.GetButton("Fire3")) { slowDown = runDecceleration; }
            else { slowDown = hDecceleration; }
            //Debug.Log(slowDown);
            targetVelocity *= slowDown;
        }

        
        targetVelocity.x += move.x * hSpeed;        //The actual command that controls movement.

        targetVelocity.x = Mathf.Clamp(targetVelocity.x, -hMax, hMax); //Clamp the velocity between zero and max.

        if (Mathf.Abs(targetVelocity.x) < 0.001f) { targetVelocity.x = 0; } //When we get to small numbers, stop horizontal movement.


        //Slide Animation
        if (move.x > 0 && velocity.x < 0 && grounded) { animator.SetBool("IsSliding", true); }
        else if (move.x < 0 && velocity.x > 0 && grounded) { animator.SetBool("IsSliding", true);}
        else { animator.SetBool("IsSliding", false); }

        //targetVelocity = move * hSpeed;
        //How do we get it to keep the momentum even if the air?

        //Jumping Controller

        if (Input.GetButtonDown("Jump") && grounded)
        {
            if (Mathf.Abs(move.x) == 1 && Input.GetButton("Fire3")) { velocity.y = jumpTakeOffSpeed * 1.1f; }
            else { velocity.y = jumpTakeOffSpeed; }
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

    private bool CheckCollisions(Collider2D moveCollider, Vector2 direction, float distance)
    {
        if (moveCollider != null)
        {
            RaycastHit2D[] hits = new RaycastHit2D[10];
            ContactFilter2D filter = new ContactFilter2D() { };

            int numHits = moveCollider.Cast(direction, filter, hits, distance);
            for (int i = 0; i < numHits; i++)
            {
                if (!hits[i].collider.isTrigger)
                {
                    return true;
                }
            }


        }

        return false;
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
            pCollider.size = new Vector2(pCollider.size.x, 1.95f);
            pCollider.offset = new Vector2(pCollider.offset.x, 0f);
            
        }

        else if (big == false)
        {
            isBigMario = false;
            animator.SetBool("IsBig", false);
            pCollider.size = new Vector2(pCollider.size.x, 0.95f);
            pCollider.offset = new Vector2(pCollider.offset.x, -0.5f);

        }

        //1. Transition to big animation control set.
        //2. Change Collision box size (or disable/enable correct box).
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        foreach (ContactPoint2D hitPos in col.contacts)
        {
            if (col.gameObject.tag == "Enemy")
            {
                if (hitPos.normal.y == 1) //If colliding with bottom of player.
                {
                    if (Input.GetButton("Jump")) { velocity.y = jumpTakeOffSpeed; }
                    else { velocity.y = jumpTakeOffSpeed * 0.5f; }

                }
                else
                {
                    if (isBigMario)
                    {
                        MakeBig(false);
                    }
                    else
                    {
                        Debug.Log("Player Dies");

                    }
                }
            }

            if (col.gameObject.tag == "Mushroom")
            {
                MakeBig(true);
                Destroy(col.gameObject);
                    
            }

            if (col.gameObject.tag == "Flower")
            {

            }
            
        }
    }

}
