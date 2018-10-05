using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : Object {
	// Int from Enum that indicates the type of block
	public int type;
	
	// The position relative to the grid in Block-Units
	// 1 block = 1w x 1h
	public Vector2Int position;
	
	// The associated gameObject (the spawned in-game block)
	public GameObject reference = null;

	public Block(int type, Vector2 position, GameObject reference) {
		// Get the block width from the BuildingController script
		int blockWidth = FindObjectOfType<BuildingController>().blockWidth;

		// Set object's values
		this.type = type;
		this.position = new Vector2Int(
			(int)(Mathf.Sign(position.x) * Mathf.Ceil(Mathf.Abs(position.x / blockWidth))),
			(int)Mathf.Ceil(Mathf.Abs(position.y / blockWidth))
		);
		this.reference = reference;
	}
}
