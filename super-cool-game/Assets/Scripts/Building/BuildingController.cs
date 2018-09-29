using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
// Each kind of block has a numerical id
// that is used to keep track of disposition
// of the block in the grid that is shared between
// client and server.
// <summary>
// Return a numerical value for each kind of block.
// </summary>
enum BlockType {
    Spawn = 0,
    Heal = 1
}

public class BuildingController : MonoBehaviour {
    // Get ground to get map size
    public GameObject ground;

    // Set map max height
    public int maxMapHeight = 40;

    // Building distance from center of the map expressed in blocks
    public int buildingDistance = 2;

    // Max building width
    public int maxBuildingWidth = 3;

    // Set block width
    public int blockWidth = 4;

    // Debug Tools
    public bool showDebugGrid = false;

    // Assign gameObjects to be addressable by code
    public GameObject[] blocks;

    // HIDDEN IN INSPECTOR
    // Set map width
    [HideInInspector] public float mapWidth;
    
    // Coordinates of the center of the map, on the top surface of it.
    [HideInInspector] public Vector2 mapCenter;

    // Reference grid of block gameobjects
    [HideInInspector] public List<Block> team1Grid = new List<Block>();
    [HideInInspector] public List<Block> team2Grid = new List<Block>();

    void Start() {
        Vector2 groundSize = ground.transform.GetComponent<SpriteRenderer>().bounds.size;

        // Get width of the sprite rendered as ground.
        mapWidth = groundSize.x;

        // Calculate center of the map
        mapCenter = new Vector2(ground.transform.position.x,  ground.transform.position.y + (groundSize.y / 2));

        PlaceBlock((int)BlockType.Spawn, new Vector2(-5, 0));
    }
    // <summary>
    // This function place a block of the specified type in the scene,
    // at the coordinates of the Vector2 that are passed to it.
    // </summary>
    public void PlaceBlock (int type, Vector2 position) {
        // Avoid unintentional placement of block in wrong places
        position.x = Mathf.Floor(position.x / blockWidth) * 4;
        if (position.x > 0) {
            position.x += blockWidth / 2;
        } else {
            position.x -= blockWidth / 2;
        }
        position.y = Mathf.Floor(position.y) + blockWidth/2;

        // Spawn the block
        Instantiate(blocks[type], position, Quaternion.identity);
    }

    // DEBUG UTILITIES
    void DebugDrawGrid () {
        int width = Mathf.FloorToInt(mapWidth / blockWidth);

        // Draw vertical lines
        for (int i = -width/2; i <= width/2; i++) {
            Vector3 start = new Vector3(mapCenter.x - i * blockWidth, mapCenter.y, 1);
            Vector3 end = new Vector3(0, maxMapHeight, 1);
            Debug.DrawRay(start, end, Color.yellow);
        }

        // Draw horizontal lines
        for (int i = 0; i <= maxMapHeight / blockWidth; i++) {
            Vector3 start = new Vector3(mapCenter.x -mapWidth/2, mapCenter.y + i * blockWidth, 1);
            Vector3 end = new Vector3(mapWidth, 0, 1);
            Debug.DrawRay(start, end, Color.yellow);
        }
    }

    private void Update() {
        if (showDebugGrid) {
            DebugDrawGrid();
        }
    }
}