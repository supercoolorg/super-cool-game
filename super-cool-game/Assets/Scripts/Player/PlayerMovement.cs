using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    /* Translates commands from PlayerController into movement */

    private Rigidbody2D rb;
    private BoxCollider2D groundTrigger;

    private float friction = 0.4f;
    private float velX = 0;
    
    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void FixedUpdate() {
        float computedVel = velX;

        // Always apply friction when grounded
        if (IsGrounded) {
            computedVel = ApplyFriction(velX, friction);
        }

        // If starting from still, apply some linear drag
        if (velX >= 0 && rb.velocity.x >= 0 && rb.velocity.x < computedVel) {
            computedVel = rb.velocity.x + friction;
        }
        if (velX < 0 && rb.velocity.x <= 0 && rb.velocity.x > computedVel) {
            computedVel = rb.velocity.x - friction;
        }

        // If the command is to stop, slow down with friction
        if (velX == 0 && rb.velocity.x != 0) {
            computedVel = ApplyFriction(rb.velocity.x, friction);
        }
        
        // If changing direction, slow down to 0 before accelerating
        if ((rb.velocity.x > 0 && velX < 0) || (rb.velocity.x < 0 && velX > 0)) {
            computedVel = ApplyFriction(rb.velocity.x, 3*friction);
        }

        rb.velocity = new Vector2(computedVel, rb.velocity.y);
    }

    public void Jump (float velY) {
        if(IsGrounded)
            rb.velocity = new Vector2(rb.velocity.x, velY);
    }

    // Moves the player on the X axis
    public void Move (float velX) {
        this.velX = velX;
    }
    
    public bool IsGrounded {
        get { return touchingColliders > 0;  }
        private set { }
    }
    
    private float ApplyFriction(float vel, float friction) {
        if (vel > 0)
            vel = Mathf.Max(0, vel - friction);
        else if (vel < 0)
            vel = Mathf.Min(0, vel + friction);

        return vel;
    }

    private int touchingColliders = 0;
    private void OnTriggerEnter2D(Collider2D other) {
        touchingColliders++;
    }
    private void OnTriggerExit2D(Collider2D other) {
        touchingColliders--;
    }
}
