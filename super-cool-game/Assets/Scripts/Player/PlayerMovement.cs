using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    /* Translates commands from PlayerController into movement */

    private Rigidbody2D rb;

    public float ERROR_THRESHOLD = 0.2f;
    private float velX = 0;
    private float velY = 0;

    private int excludePlayerMask;

    void Start() {
        rb = gameObject.GetComponent<Rigidbody2D>();
        excludePlayerMask = ~(1 << LayerMask.NameToLayer("Player"));
    }

    void FixedUpdate() {
        // Apply gravity
        if (!IsGrounded) {
            velY -= 9.81f * 1.5f * Time.fixedDeltaTime;
        } else if (velY < 0) {
            velY = 0;
        }

        // Prevent pushing other players
        if (IsTouchingSides)
            velX = 0;

        rb.velocity = new Vector2(velX, velY);
    }

    public void Jump(float inputY) {
        if (IsGrounded)
            this.velY = inputY;
    }

    // Moves the player on the X axis
    public void MoveX(float inputX) {
        this.velX = inputX;
    }

    public void AuthoritativeMove(Vector2 pos, Vector2 vel) {
        var delta = (rb.position - pos).magnitude;
        if (delta > ERROR_THRESHOLD) {
            rb.position = pos;
            rb.velocity = vel;
            velX = vel.x;
            velY = vel.y;
        }
    }

    public bool IsGrounded {
        get { return touchingColliders > 0; }
    }

    public bool IsTouchingSides {
        get {
            RaycastHit2D leftHit = Physics2D.Raycast(transform.position, Vector2.left, 0.32f, excludePlayerMask);
            RaycastHit2D rightHit = Physics2D.Raycast(transform.position, Vector2.right, 0.32f, excludePlayerMask);
            if ((velX < 0 && leftHit.collider != null) ||
                 (velX > 0 && rightHit.collider != null)) {
                return true;
            }
            return false;
        }
    }

    private int touchingColliders = 0;
    private void OnTriggerEnter2D(Collider2D other) {
        touchingColliders++;
    }
    private void OnTriggerExit2D(Collider2D other) {
        touchingColliders--;
    }
}
