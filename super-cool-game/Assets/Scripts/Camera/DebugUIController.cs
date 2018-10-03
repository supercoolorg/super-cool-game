using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugUIController : MonoBehaviour {
	BuildingController bc;
	GameObject player;
	
	void Start() {
		// Find the first gameobject with the BC script
		bc = FindObjectOfType<BuildingController>();
		// TODO: This one will have to be changed in multiplayer test
		// Here it will the the first player in the hierarchy, in SP test
		// there will be just one.
		player = GameObject.FindGameObjectWithTag("Player");
	}

	public void PlaceBlock() {
		bc.PlaceBlock(0, player.transform.position);
	}
}
