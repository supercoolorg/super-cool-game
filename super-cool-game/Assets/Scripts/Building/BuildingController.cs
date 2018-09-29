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
    public static int blockWidth = 4;

    // Debug Tools
    public bool showDebugGrid = false;

    // Assign gameObjects in EDITOR
    // to be used in code.
    public GameObject[] blocks;

    // HIDDEN IN INSPECTOR
    // Set map width
    [HideInInspector] public float mapWidth;

    // Coordinates of the center of the map, on the top surface of it.
    [HideInInspector] public Vector2 mapCenter;

    // Reference grid of block gameobjects
    // this is DATA
    [HideInInspector] public List<Block> placedBlocks = new List<Block>();

    // Container for blocks in world space, in order to have relative positioning
    // this is GAMEOBJECTS
    [HideInInspector] public GameObject grid;

    void Start() {
        Vector2 groundSize = ground.transform.GetComponent<SpriteRenderer>().bounds.size;

        // Get width of the sprite rendered as ground.
        mapWidth = groundSize.x;

        // Calculate center of the map
        mapCenter = new Vector2(ground.transform.position.x, ground.transform.position.y + (groundSize.y / 2));

        // Spawn two empty gameObjects.
        // Blocks must be children of these two.

        // TODO: Find why unity spawns two gameobjects, Block Grid and Block Grid (clone).
        grid = new GameObject();
        grid.name = "Block Grid";
        grid = Instantiate(grid, mapCenter, Quaternion.identity);
    }

    // <summary>
    // Create a singleton to call functions without instancing the class directly
    // </summary>
    private static BuildingController _instance = null;

    public static BuildingController instance {
        get {
            // If there is no instance, find one.
            if (_instance == null) {
                // Find an already setted instance of BC
                _instance = FindObjectOfType(typeof(BuildingController)) as BuildingController;
            }
            // In theory, if there isnt one you should create it, but since it's right there in the
            // editor, we can avoid to do that.

            return _instance;
        }
    }

    // <summary>
    // This function place a block of the specified type in the scene,
    // at the coordinates of the Vector2 that are passed to it.
    // </summary>
    // It should be the server to decide where and when place the block.
    public void PlaceBlock(int type, Vector2 position) {

        // TODO: Check if there isnt already a block there.
        // If there's no block in that position, create a new one.
        if (!placedBlocks.Exists(
            x => x.position == Block.GetBlockRelativeCoordinates(position)
            )) {
            // Spawn the block
            GameObject go = Instantiate(blocks[type], position, Quaternion.identity);
            go.transform.parent = grid.transform;



            // Add the block to the list for checks and reference.
            placedBlocks.Add(new Block(type, position, go));
        } else {
            Debug.Log("A block is already present in that position, can't place a block there!");
        }
    }

    // DEBUG UTILITIES
    void DebugDrawGrid() {
        int width = Mathf.FloorToInt(mapWidth / blockWidth);

        // Draw vertical lines
        for (int i = -width / 2; i <= width / 2; i++) {
            Vector3 start = new Vector3(mapCenter.x - i * blockWidth, mapCenter.y, 1);
            Vector3 end = new Vector3(0, maxMapHeight, 1);
            Debug.DrawRay(start, end, Color.yellow);
        }

        // Draw horizontal lines
        for (int i = 0; i <= maxMapHeight / blockWidth; i++) {
            Vector3 start = new Vector3(mapCenter.x - mapWidth / 2, mapCenter.y + i * blockWidth, 1);
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