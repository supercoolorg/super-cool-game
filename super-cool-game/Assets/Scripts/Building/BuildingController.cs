using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour {
    // DEBUG
    public bool showDebugGrid;

    // Set block width
    public int blockWidth = 4;

    // PREFIX "b_" means "Block Unit", where
    // 1 BU = 1 * blockWidth.
    // Set building grid dimensions
    public int b_gridHeight = 30;

    // The space (in block-units) where players of one team can build
    public int b_gridTeamWidth = 3;

    // The space (in block-units) that separate a team building grid
    // from the center of the map.
    public int b_gridTeamGapWidth = 2;

    // Get ground to get map size
    public GameObject ground;

    // Import Prefabs to spawn them in-game
    public GameObject[] blocks;

    [HideInInspector] public float mapWidth;
    [HideInInspector] public Vector2 mapCenter;

    // The grid data reference
    private List<Block> grid = new List<Block>();

    private GameObject gridContainer;
    
    void Start() {
        // Get rendered size of ground
        if (ground == null) {
            ground = GameObject.FindGameObjectWithTag("Ground");
        }
        Vector2 groundSize = ground.transform.GetComponent<SpriteRenderer>().bounds.size;
        // Get width of the sprite rendered as ground.
        mapWidth = groundSize.x;
        mapCenter = new Vector2(ground.transform.position.x,  ground.transform.position.y + (groundSize.y / 2));

        // Spawn grid container game object
        gridContainer = new GameObject();
        gridContainer.name = "Grid";
    }

    // EDITING THE GRID
    public void PlaceBlock(int type, Vector2 position) {
        // Create a new gameObject
        GameObject newBlock = Object.Instantiate(blocks[type]);

        // Create new 'Block' object
        Block newBlockData = new Block(type, position, newBlock);
        grid.Add(newBlockData);

        // Choose where to spawn the block
        // As example, I'll use the player position.
        // Later, it should be the server to choose it.
        newBlock.transform.position = new Vector2(
            newBlockData.position.x * blockWidth - Mathf.Sign(newBlockData.position.x) * blockWidth / 2,
            newBlockData.position.y * blockWidth - blockWidth / 2 + mapCenter.y
        );
        ResizeBlock(newBlock);
        newBlock.transform.parent = gridContainer.transform;
    }

    public void DestroyBlock(Vector2Int gridPosition) {
        // Find the block to destroy
        Block block = grid.Find(b => b.position == gridPosition);
        // If the block exists
        if(block != null) {
            // destroy the relative spawned GameObject
            Destroy(block.reference);
            // and the block from the list
            grid.Remove(block);
        } 
    }

    // Helper function to scale block indipendently from the resolution
    private void ResizeBlock(GameObject block) {
        // Get one dimension (it's a square...) of the block
        float size = block.GetComponent<Renderer>().bounds.size.x;

        // Get its current scale
        Vector2 newScale = block.transform.localScale;

        // Set newScale to the result of a "simple" proportion
        newScale.x = newScale.y = blockWidth * newScale.x / size;

        block.transform.localScale = newScale;
    }

    // DEBUG
    void DebugDrawGrid () {
        int b_gridHalfWidth = b_gridTeamWidth + b_gridTeamGapWidth;

        // Draw vertical lines
        for (int i = -b_gridHalfWidth; i <= b_gridHalfWidth; i++) {
            Vector3 start = new Vector3(i * blockWidth, mapCenter.y, 1);
            Vector3 end   = new Vector3(0, blockWidth * b_gridHeight, 0);
            Debug.DrawRay(start, end, Color.yellow);
        }

        // Draw horizontal lines
        for (int i = 0; i <= b_gridHeight; i++) {
            Vector3 start = new Vector3(-b_gridHalfWidth * blockWidth, mapCenter.y + i * blockWidth, 1);
            Vector3 end   = new Vector3(b_gridHalfWidth * blockWidth * 2, 0, 0);
            Debug.DrawRay(start, end, Color.yellow);
        }
    }
    private void Update() {
        // Show debug grid
        if (showDebugGrid) DebugDrawGrid();
    }
}
