﻿using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    /* Translates commands from PlayerController into movement */

    private Rigidbody2D rb;
    private BoxCollider2D groundTrigger;

    private float friction = 0.4f;
    private float velX = 0, velY = 0;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        if (!IsGrounded) {
            // Apply gravity
            velY -= 9.81f * 1.5f * Time.fixedDeltaTime;
        }
        rb.velocity = new Vector2(velX, velY);
    }

    public void Jump(float velY) {
        if (IsGrounded)
            this.velY = velY;
    }

    // Moves the player on the X axis
    public void MoveX(float velX) {
        this.velX = velX;
    }

    public void MoveY(float velY) {
        this.velY = velY;
    }

    public bool IsGrounded {
        get { return touchingColliders > 0; }
        private set { }
    }

    private float ApplyFriction(float vel) {
        if (!IsGrounded) return vel;

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
