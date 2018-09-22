using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
    // Player Input handles the control of the player's character
    // on the client machine.

    // This one is imported from PlayerMovement.cs, so dont touch it.
    private PlayerMovement pm;

    void Start() {
        // Get speed from PlayerMovement.cs
        pm = gameObject.GetComponent<PlayerMovement>();
    }
    // Update is called once per frame
    void Update () {

        // Player wants to move laterally
        pm.Move(Input.GetAxis("Horizontal"));

        // Player wants to jumpy
        if(Input.GetButtonDown("Jump")) {
            pm.Jump();
        }
	}
}
