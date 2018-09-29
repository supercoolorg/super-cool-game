using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
    /* Handles user inputs, to be sent over to PlayerController */

    private PlayerController controller;

    void Start() {
        controller = gameObject.GetComponent<PlayerController>();
    }
    
    void Update () {
        // Player wants to move laterally
        controller.Move(Input.GetAxis("Horizontal"));

        // Player wants to jumpy
        if (Input.GetButtonDown("Jump")) {
            controller.Jump();
        }
	}
}
