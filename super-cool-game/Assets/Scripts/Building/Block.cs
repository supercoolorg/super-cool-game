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
    public int x;
    public int y;
}