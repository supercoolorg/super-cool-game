using UnityEngine;

// This is gonna be used to send multiplayer commands too.
public class PlayerController : MonoBehaviour {
    /* Translates inputs from PlayerInput into move commands
     * to be sento to PlayerMovement 
     */

    private PlayerMovement pm;
    private float speed = 4.0f;
    private float jumpHeight = 7.0f;

    private float velX = 0;

    void Start () {
        pm = gameObject.GetComponent<PlayerMovement>();
	}

    public void Move(float moveX) {
        var velX = Mathf.Max(-1, Mathf.Min(1, moveX)) * speed;
        if (this.velX != velX) {
            pm.Move(velX);
            this.velX = velX;
            // TODO: Send to network
        }
    }

    public void Jump(float jumpMult = 1) {
        var velY = jumpHeight * jumpMult;
        pm.Jump(velY);
        // TODO: Send to network
    }
}
