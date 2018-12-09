using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physics_Controller : MonoBehaviour {
    
    /*
     * This adds basic physics to any object in the game.
     *  1. Gravity motion
     *  2. Collisions
     * 
     *
     */


    //Modifiable
    public float minGroundNormalY = 0.65f; //Angle which is considered "grounded". 
    public float gravityModifier = 1f; //Changes Gravity characteristics (Higher = More Gravity)
    public bool isFixed; //If you want object to not move at all. (True = Does not move).


    //Non-Modifiable
    public Vector2 velocity;  //Actual velocity of the gameobject
    public Vector2 targetVelocity; //Used to add horizontal velocity to our rigid body's velocity.
    protected bool grounded; //Is on the ground (or a slope angle that is considered a ground?)
    protected Vector2 groundNormal;
    protected Rigidbody2D rb2d;
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
        
        //Put the x component of our velocity into our Rigid body's velocity.
        velocity.x = targetVelocity.x;

        if (isFixed) { velocity = Vector2.zero; } //If things aren't supposed to move, then remove zero out our move vector.


        grounded = false; 

        
        //Set Movement Vector
        Vector2 deltaPosition = velocity * Time.deltaTime;

        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x); //Direction of movement along the ground.

        
        //Add the resultant vectors
        Vector2 move = moveAlongGround * deltaPosition.x;

        Movement(move, false); //Horizontal movement only, checking for collisions.

        move = Vector2.up * deltaPosition.y;

        Movement(move, true); //Vertical Movement, checking for collisions.




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
                if (currentNormal.y > minGroundNormalY) //If that normal direction is greater than our ground normal value
                {
                    grounded = true; //set grounded to true.
                    if (yMovement) //If this is for vertical movement
                    {
                        groundNormal = currentNormal; //Set the ground normal (the direction perpendicular to the ground pushing upwards)
                        currentNormal.x = 0;  //Blank out any horizontal movement in the current iteration.
                    }
                }

                float projection = Vector2.Dot(velocity, currentNormal); //Calculating directional difference between velocity and currentNormal
                
                if (projection < 0) //If returning a negative value (moving opposite direction from our velocity)
                {

                    velocity = velocity - projection * currentNormal; //Cancel out part of velocity that would be stopped from collision.
                }

                float modifiedDistance = hitBufferList[i].distance - shellRadius; //Subtract shellradius from distance
                distance = modifiedDistance < distance ? modifiedDistance : distance; //Use whichever one is smaller (distance or modified distance) to prevent getting stuck in collision boxes.
            }


        }

        rb2d.position = rb2d.position + move.normalized * distance;  //Move rigid body's position in the direction of move.normalized, the distance calculated above.
        
    }



}
