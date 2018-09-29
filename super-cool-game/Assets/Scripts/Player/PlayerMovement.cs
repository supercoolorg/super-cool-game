using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    /* Translates commands from PlayerController into movement */

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private float velX = 0;

    // Use this for initialization
    void Start() {
        // Get Rigidbody from the GameObject
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
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
            Vector2 origin = transform.position + Vector3.down * boxCollider.size.y;
            Vector2 direction = Vector2.down;

            RaycastHit2D hit = Physics2D.Raycast(origin, direction, 0.1f);
            return hit.collider != null;
        }
    }
}
