using UnityEngine;

public class NetworkPlayer : MonoBehaviour {
    /*
     * This script handles the movement for the player objects
     * that are not controlled by this client.
     * Sets the rigidbody position, and lerps the mesh to it
     */

    public float lerpAmount = 0.1f;
    private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
        rb = gameObject.GetComponentInChildren<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        // Save rigidbody absolute position
        var tmp = rb.gameObject.transform.position;
        // Compute lerping
        gameObject.transform.position = Vector2.Lerp(gameObject.transform.position, rb.position, lerpAmount);
        // Restore rigidbody position
        rb.gameObject.transform.position = tmp;
	}

    public void Move(Vector2 pos, Vector2 vel) {
        rb.position = pos;
        rb.velocity = vel;
    }
}
