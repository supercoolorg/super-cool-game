using UnityEngine;
using System.Collections;

public class PlayerBuilding : MonoBehaviour {

    // BOOLs
    // If player is building (show where the block should be placed
    public bool isBuilding = false;

    // Keep track of the 
    public bool isPlaceholderSpanwed = false;

    // GAMEOBJECTS
    // Import placeholder block to show where the block will be placed
    public GameObject pb;

    // Reference to the spawned object
    private GameObject placeholder;

    // VECTOR2
    // The rounded position to the size of the grid which is multiples of blockWidth
    public Vector2 SnappedPosition {
        get {
            return new Vector2(
                Mathf.Ceil(transform.position.x / blockWidth) * blockWidth - blockWidth / 2,
                Mathf.Ceil(transform.position.y / blockWidth) * blockWidth - blockWidth / 2);
        }
    }

    private int blockWidth = BuildingController.blockWidth;

    // Update is called once per frame
    void Update() {
        // Show where the block should be placed
        if (isBuilding) {
            if (!isPlaceholderSpanwed) {
                placeholder = Instantiate(pb, SnappedPosition, Quaternion.identity);
                isPlaceholderSpanwed = true;
            } else {
                placeholder.transform.position = SnappedPosition;
            }
        } else {
            // If we're not building, destroy the placeholder
            if (isPlaceholderSpanwed) {
                Destroy(placeholder);
                isPlaceholderSpanwed = false;
            }
        }
    }
}
