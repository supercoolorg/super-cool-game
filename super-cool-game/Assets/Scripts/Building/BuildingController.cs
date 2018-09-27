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

    // Assign gameObjects to be addressable by code
    public GameObject[] blocks;

    // HIDDEN IN INSPECTOR
    // Set map width
    [HideInInspector] public float mapWidth;

    // Coordinates of the center of the map, on the top surface of it.
    [HideInInspector] public Vector2 mapCenter;

    // Reference grid of block gameobjects
    [HideInInspector] public List<GameObject> team1PlacedBlocks = new List<GameObject>();
    [HideInInspector] public List<GameObject> team2PlacedBlocks = new List<GameObject>();

    // Container for blocks in world space, in order to have relative positioning
    [HideInInspector] public GameObject team1grid;
    [HideInInspector] public GameObject team2grid;

    void Start() {
        Vector2 groundSize = ground.transform.GetComponent<SpriteRenderer>().bounds.size;

        // Get width of the sprite rendered as ground.
        mapWidth = groundSize.x;

        // Calculate center of the map
        mapCenter = new Vector2(ground.transform.position.x, ground.transform.position.y + (groundSize.y / 2));

        // Spawn two empty gameObjects.
        // Blocks must be children of these two.
        team1grid = Instantiate(new GameObject(), new Vector2(mapCenter.x - blockWidth * 2, mapCenter.y), Quaternion.identity);
        team2grid = Instantiate(new GameObject(), new Vector2(mapCenter.x + blockWidth * 2, mapCenter.y), Quaternion.identity);
    }

    // <summary>
    // Create a singleton to call functions without instancing the class directly
    // </summary>
    private static BuildingController _instance = null;

    public static BuildingController instance {
        get {
            if (_instance == null) {
                // Find an already setted instance of BC
                _instance = FindObjectOfType(typeof(BuildingController)) as BuildingController;
            } else {
                // Remind that you should never destroy GameManager gameObject.
                Debug.Log("There isn't an instance of BuildingController! Has the GameManager object been deleted by mistake?");
            }

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
        // Spawn the block
        GameObject go = Instantiate(blocks[type], position, Quaternion.identity);
        go.transform.parent = GetTeamGrid(1).transform;

        GetTeamBlocks(1).Add(go);
    }

    // Return the list of placed blocks
    public List<GameObject> GetTeamBlocks(int teamId) {
        if (teamId == 1) {
            return team1PlacedBlocks;
        } else if (teamId == 2) {
            return team2PlacedBlocks;
        }
        return null;
    }

    // Return the root gameobject of 
    public GameObject GetTeamGrid(int teamId) {
        if (teamId == 1) {
            return team1grid;
        } else if (teamId == 2) {
            return team2grid;
        }
        return null;
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