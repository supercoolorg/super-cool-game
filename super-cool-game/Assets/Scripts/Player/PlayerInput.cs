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

        // Player wants to jump
        if (Input.GetButtonDown("Jump")) {
            controller.Jump();
        }

        // PLAYER UI
        // Use GetButton as it doesnt fire only the first frame the button is pressed.
        if (Input.GetButton("UI_Picker_Show")) {
            // show ui picker
            ui.isPickerUIActive = true;
        } else {
            ui.isPickerUIActive = false;
        }
	}
}
