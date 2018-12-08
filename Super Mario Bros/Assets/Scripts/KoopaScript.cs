using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoopaScript : Physics_Controller {

    public float moveSpeed = 1f;
    public float moveDir = -1f;
    public Animator animator;
    public bool facingRight;

    private Collider2D kcollider;
    private float kdistance = 0.2f;

    

	void Start () {
        animator = GetComponent<Animator>();
        kcollider = GetComponent<Collider2D>();
        facingRight = false;

	}

    protected override void ComputeVelocity()
    {
        
        if (CheckCollisions(kcollider, targetVelocity, kdistance))
        {
            moveDir *= -1;
            Flip();
        }
        //Move left...if you hit something collidable, move right.
        targetVelocity.x = moveDir * moveSpeed;

        

                

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

    
    void Flip()
    {
        if (facingRight)
        { facingRight = false; GetComponent<SpriteRenderer>().flipX = false; }
        else if (!facingRight) { facingRight = true; GetComponent<SpriteRenderer>().flipX = true; }
    }

    void Die()
    {

    }


}
