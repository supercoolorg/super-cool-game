using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    /* Translates commands from PlayerController into movement */

    private Rigidbody2D rb;
    private float checkGroundDistance = 1f;
    private float velX = 0;

    // Use this for initialization
    void Start() {
        // Get Rigidbody from the GameObject
        rb = gameObject.GetComponent<Rigidbody2D>();
    }
    
    void FixedUpdate() {
        rb.velocity = new Vector2(velX, rb.velocity.y);
    }

    public void Jump (float velY) {
        if(IsGrounded)
            rb.velocity = new Vector2(rb.velocity.x, velY);
    }

    // Moves the player on the X axis
    public void Move (float velX) {
        this.velX = velX;
    }

    // Uses a raycast to check if player is grounded
    public bool IsGrounded {
        get {
            Vector2 position = transform.position;
            Vector2 direction = Vector2.down;

            RaycastHit2D hit = Physics2D.Raycast(position, direction, checkGroundDistance);
            if (hit.collider != null)
                return true;
            return false;
        }
    }
}
