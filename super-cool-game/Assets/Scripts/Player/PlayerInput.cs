using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
    /* Handles user inputs, to be sent over to PlayerController */

    private PlayerController controller;
    private PlayerUI ui;

    void Start() {
        controller = gameObject.GetComponent<PlayerController>();
        ui = GameObject.Find("/PlayerUICanvas").GetComponent<PlayerUI>();
    }
    
    void Update () {
        // Player wants to move laterally
        controller.Move(Input.GetAxis("Horizontal"));

        // Player wants to jumpy
        if (Input.GetButtonDown("Jump")) {
            controller.Jump();
        }

        // PLAYER UI
        // Scroll left in block selector menu
        if (Input.GetButtonDown("ScrollLeft_BM")) {
            ui.ScrollLeft();
        }
        // Scroll right in block selector menu
        if (Input.GetButtonDown("ScrollRight_BM")) {
            ui.ScrollRight();
        }
	}
}
