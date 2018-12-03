using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physics_Controller : MonoBehaviour {
        
    //Modifiable
    public float minGroundNormalY = 0.65f;
    public float gravityModifier = 1f;

    //Non-Modifiable
    public Vector2 targetVelocity;
    protected bool grounded; //Is on the ground (or a slope angle that is considered a ground?)
    protected Vector2 groundNormal;
    protected Rigidbody2D rb2d;
    public Vector2 velocity;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);
    protected const float minMoveDistanceY = 0.001f; //Distance to check for movement.
    protected const float shellRadius = 0.01f; //Padding to collision checker
    
    void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Start ()
    {
        //Setting up contactFilter for collision detections
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;

	}
	
	void Update () {

        //targetVelocity = Vector2.zero;
        ComputeVelocity();
        
	}

    protected virtual void ComputeVelocity()
    {

    }

    void FixedUpdate()
    {
        //Set Gravity
        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        //Pull out the x component of our velocity.
        velocity.x = targetVelocity.x;

        grounded = false;

        
        //Set Movement Vector
        Vector2 deltaPosition = velocity * Time.deltaTime;

        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x); //Direction of movement along the ground.

        //Add the resultant vectors
        Vector2 move = moveAlongGround * deltaPosition.x;

        Movement(move, false); //Horizontal movement only.

        move = Vector2.up * deltaPosition.y;

        Movement(move, true); //Vertical Movement
    }

    void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude; //The distance we plan to move

        if (distance > minMoveDistanceY) //If less than minimal check distance, then don't bother checking.
        {
            int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + shellRadius); //Cast for collisions and store them.
            hitBufferList.Clear(); //Clear the hit buffer list.
            for (int i = 0; i < count; i++) //For each hit, add it to our list.
            {
                hitBufferList.Add(hitBuffer[i]);
            }

            for (int i = 0; i < hitBufferList.Count; i++) //For each hit in our list, find the normal direction of that hit.
            {
                Vector2 currentNormal = hitBufferList[i].normal;
                if (currentNormal.y > minGroundNormalY) //If taht normal direction is greater than our ground normal value
                {
                    grounded = true; //set grounded to true.
                    if (yMovement) //If this is for vertical movement
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0; 
                    }
                }

                float projection = Vector2.Dot(velocity, currentNormal);
                if (projection < 0)
                {
                    velocity = velocity - projection * currentNormal;
                }

                float modifiedDistance = hitBufferList[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }


        }

        rb2d.position = rb2d.position + move.normalized * distance;
    }



}
