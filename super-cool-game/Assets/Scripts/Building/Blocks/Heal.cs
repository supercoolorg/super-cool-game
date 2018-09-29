using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D col) {
        // If the collided object is a player
        if (col.gameObject.tag == "Player") {
            // Heal it
            // TODO: Script it better
            PlayerStats ps = col.gameObject.GetComponent<PlayerStats>();
            ps.currentHealth = 100;
        }
    }
}
