using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : Object {
    // The type of block, see BlockType
    public int type;

    /* Coordinates of the block, expressed in block-units
     * from the center of the map, for example:
     * . _ _ [] <-- this block has coordinates (3,0)
     * this block too --> [] _ _ .
     * (the dot is the center)
     */
    public Vector2Int position;
    public GameObject referencedGameObject;

    // <summary>
    //  Create a data reference of the placed block (gameobject) by
    //  passing the type, the position, and the spawned gameobject to
    //  a new instance of this class
    // </summary>
    // The Vector2 in global coordinates will be processed into
    // a relative positioning, in block-units, respect to the center of the map.
    public Block(int type, Vector2 position, GameObject reference) {
        this.type = type;
        this.referencedGameObject = reference;
        this.position = GetBlockRelativeCoordinates(position);

        Debug.Log("Instanced a Block with position " + this.position.x + ", " + this.position.y);
    }

    // <summary>
    //  Return a Vector2Int that doesnt represent the actual block coordinates,
    //  but its position in a grid where the cells have width and height of 1.
    // </summary>
    public static Vector2Int GetBlockRelativeCoordinates(Vector2 position) {
        int blockW = BuildingController.blockWidth;
        // Blocks in the grid are in this form
        // ...[-2][-1](center)[1][2]...
        int x = (int)(Mathf.Sign(position.x) * Mathf.Ceil(Mathf.Abs(position.x) / blockW));
        Debug.Log(x);
        return new Vector2Int(
            (int)(Mathf.Sign(position.x) * Mathf.Ceil(Mathf.Abs(position.x) / blockW)),
            (int)(Mathf.Sign(position.y) * Mathf.Floor(Mathf.Abs(position.y) / blockW)));
    }
}