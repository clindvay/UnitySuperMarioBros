using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBlockScript : MonoBehaviour
{

    public enum BlockType { Singlecoin, Multicoin, Powerup, Oneup, Activated } //Types of things contained in the coinblock.
    public BlockType blockType; //This stores the blocktype variable

    public int multiCoinCounter = 10;

    private Animator animator;



    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();

        //If marked as activated, then change this to the activated state.
        if (blockType == BlockType.Activated) { animator.SetBool("IsActivated", true); }
    }

    
    private void OnCollisionEnter2D(Collision2D col)
    {
        foreach (ContactPoint2D hitPos in col.contacts)
        {
            //Debug.Log(hitPos.normal); // (x and y), (1,0) = Left, (0,1) = Bottom, (-1,0) = Right, (0,-1) = Top;
            if (hitPos.normal.y == 1) //If colliding with bottom of coinblock.
            {
                //Debug.Log(col.gameObject.tag);
                if (col.gameObject.tag == "Player")
                {
                    //Do the coin block reveal thing.
                    ActivateCoinBlock();
                    FindObjectOfType<Game_Controller>().AddScore(100);

                }

            }
        }

    }

    private void ActivateCoinBlock()
    {
        if (blockType == BlockType.Oneup)
        {
            animator.SetBool("IsActivated", true);
        }
        else if (blockType == BlockType.Multicoin)
        {
            if (multiCoinCounter > 0)
            {
                FindObjectOfType<Game_Controller>().AddCoin();
                multiCoinCounter--;
            }
            else
            {
                animator.SetBool("IsActivated", true);
                blockType = BlockType.Activated;
            }
        }
        else if (blockType == BlockType.Powerup)
        {
            animator.SetBool("IsActivated", true);
        }
        else if (blockType == BlockType.Singlecoin)
        {
            FindObjectOfType<Game_Controller>().AddCoin();
            animator.SetBool("IsActivated", true);
            blockType = BlockType.Activated;
        }
        else // Activated already.
        {
            
        }


    }
}
