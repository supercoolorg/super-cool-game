using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    /* Translates commands from PlayerController into movement */

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    private float friction = 0.4f;
    private float velX = 0;
    
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    
    void FixedUpdate() {
        rb.velocity = new Vector2(ApplyFriction(velX), rb.velocity.y);
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
    
    private float ApplyFriction(float vel) {
        if (!IsGrounded) return vel;

        if (vel > 0)
            vel = Mathf.Max(0, vel - friction);
        else if (vel < 0)
            vel = Mathf.Min(0, vel + friction);

        return vel;
    }
}
