using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    /* Handles the movement of the player.
     * This script is always attached to the player's character game object,
     * while PlayerInput is attached only when the character's owner is the local player.
     */
    
    // These ones can be edited in the inspector
    public float speed = 3.0f;
    public float maxSpeed = 9.0f;
    public float jumpHeight = 6.0f;
    public float checkGroundDistance = 1.0f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private float moveX;

    // Use this for initialization
    void Start() {
        // Get Rigidbody from the GameObject
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Fixed update is for Physics-based update
    void FixedUpdate () {
        ComputeMovement();
    }

    void ComputeMovement () {
        

        // Resultant of movement
        rb.velocity = new Vector2(moveX, rb.velocity.y);

        // Add force if speed is below max threshold
        if(moveX * rb.velocity.x < maxSpeed) {
            rb.AddForce(new Vector2(moveX * speed, 0.0f), ForceMode2D.Impulse);
        }
        // Cap speed
        if(Mathf.Abs(rb.velocity.x) > maxSpeed) {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
        }

        // TODO: Flip if moveX < 0
    }

    public void Jump () {
        // if player hasnt already jumped
        if(IsGrounded()) {
            rb.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
        } 
    }

    // Moves the player on the X axis
    public void Move (float axisX) {
        moveX = axisX;
    }
    // Uses a raycast to check if player is grounded
    public bool IsGrounded () {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        
        // Draw a vertical line for debug
        Debug.DrawRay(position, direction, Color.green);

        RaycastHit2D hit = Physics2D.Raycast(position, direction, checkGroundDistance, groundLayer);
        if (hit.collider != null) {
            Debug.DrawRay(position, direction, Color.red);
            // The raycast hit something (= the ground)
            return true;
        }
        return false;
    }
}
