using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Controller : MonoBehaviour {

    public Animator animator;
    public float playerWalkSpeed = 10f;
    public float playerRunSpeed = 20f;
    private bool facingRight = false;
    public float playerJumpPower = 1250f;
    private float moveX;
    public bool isGrounded;
    public float speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        CheckInput();
        CheckIsGrounded();
    }

    public void CheckInput()
    {
        //Controls
        moveX = Input.GetAxis("Horizontal");

        if (moveX != 0) { animator.SetBool("Walking", true); } else { animator.SetBool("Walking", false); }

        if (Input.GetButtonDown("Jump") && isGrounded) { Jump(); }
        //Movement
        if (moveX < 0.0f && facingRight == false)
        {
            FlipPlayer();
        }
        else if (moveX > 0.0f && facingRight == true)
        {
            FlipPlayer();
        }


        //Animation
        //Physics

        //Horizontal Movement
        
        if (Input.GetButton("Fire3") && isGrounded) { speed = playerRunSpeed; } else if (!Input.GetButton("Fire3") && isGrounded) { speed = playerWalkSpeed; }
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(moveX * speed, gameObject.GetComponent<Rigidbody2D>().velocity.y);
        
        //If you fall below the lowest position of the dungeon.
        if (gameObject.GetComponent<Transform>().position.y < -10)
        {
            StartCoroutine(playerDie());
        }

    }

    public void CheckIsGrounded()
    {
        if (CheckCollisions(gameObject.GetComponent<CapsuleCollider2D>(), Vector2.down, 0.1f))
        { isGrounded = true; animator.SetBool("Jumping", false); }
        else { isGrounded = false; animator.SetBool("Jumping", true); }
        
    }


    public bool CheckCollisions(Collider2D moveCollider, Vector2 direction, float distance)
    {
        if (moveCollider != null)
        {
            RaycastHit2D[] hits = new RaycastHit2D[10];
            ContactFilter2D filter = new ContactFilter2D() {};

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

    
    public void Jump()
    {
        animator.SetBool("Jumping", true);
        GetComponent<Rigidbody2D>().AddForce (Vector2.up * playerJumpPower);
    }

    public void FlipPlayer()
    {
        facingRight = !facingRight;
        Vector2 localScale = gameObject.transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    IEnumerator playerDie()
    {
        animator.SetBool("IsDie", true);
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * 1250);
        yield return new WaitForSecondsRealtime(2);
        FindObjectOfType<Game_Controller>().Restart();
        //animator.SetBool("IsDie", false);
    }



}


