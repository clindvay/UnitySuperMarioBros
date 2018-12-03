using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToDo : MonoBehaviour {

/*
 * 1. Player Movement:
 *  a. Walking
 *  b. Running (2x speed, animation speed 2x).
 *      i. Need to conserve speed when not on ground.
 *  c. Sliding when coming to a stop, or turning the other direction.
 *      i. Need a momentum set so that when turning the other direction, the momentum must be overcome before moving in the other direction.
 *  d. Jumping
 *      i. Holding jump button adds jump height to a certain point.
 *      ii. Letting go of jump button immediately starts falling.
 *      iii. Running and Jumping gives a bit more height than walking and jumping.
 *      iv. Horizontal movement is conserved during jump, but switching direction will slow down, but slower than if on ground.
 *  e. Climbing
 *      i. Can only climb on climable objects (vines, ladders).
 *      ii. When climbing, always climb on left or right of object. (Move to specific location when moving up or down.)
 *      iii. Pushing left or right will swing from one side of the climable object, then let go and fall downward.
 *  f. Swimming
 *      i. Jump paddles, which gives a small vertical momentum and adds to horizontal momentum.
 *      ii. Horizontal momentum is maintained as long as direction is maintained.
 *      iii. Horizontal momentum is decreased when opposite direction is pushed.
 *      iv. Horizontal momentum is greatly decreased when opposite direction is pushed and jump is pushed.
 *      v. Pushing down and jumping greatly decreases vertical momentum (smaller vertical jump).
 *      vi. Pushing up and jumping greatly increase vertical momentum (larger vertical jump).
 *      vii. Downward  momentum is continuous without jumping (or paddling0.
 *   g. Crouching
 *      i. If big Mario, pushing down crouches. Collision box is half height.
 *      ii. If underneath a collidable when crouching stops, character is pushed opposite the direction they are facing.
 *      
 * 2. Blocks
 *  a. Solid Blocks
 *  b. Punchable blocks
 *      1. Question Mark Blocks
 *          i. Hitting from below will activate.
 *          ii.  Can only activate a number of times.
 *      2. Bricks
 *          i. Hitting from below will activate.
 *          ii. If big mario, hitting from below will destroy.
 *  c. Climable Blocks
 *  d. Swimmable? (blocks or levels?)
 *  
 * 3. Enemies
 * 4. Misc.
 *  a. Time Limit
 *  b. Coins
 *  c. Screen falling.
 *  d. 
 * 
 */



}
