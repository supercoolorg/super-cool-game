using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGridController : MonoBehaviour {

    // Get ground to get map size
    public GameObject ground;

    // Set map max height
    public float maxMapHeight = 100;

    // Set block width
    public int blockWidth = 4;

    // Set map width
    [HideInInspector] public float mapWidth;
    [HideInInspector] public Vector2 mapCenter;

    void Start() {

        Vector2 groundSize = ground.transform.GetComponent<SpriteRenderer>().bounds.size;

        // Get width of the sprite rendered as ground.
        mapWidth = groundSize.x;

        mapCenter = new Vector2(ground.transform.position.x,  ground.transform.position.y + (groundSize.y / 2));

        Debug.Log("Map Width: " + mapWidth);

    }

    void DebugDrawGrid () {

        int width = Mathf.FloorToInt(mapWidth / blockWidth);

        // Draw vertical lines
        for(int i = -width/2; i <= width/2; i++) {

            Vector3 start = new Vector3(mapCenter.x - i * blockWidth, mapCenter.y, 1);
            Vector3 end = new Vector3(0, maxMapHeight, 1);
            Debug.DrawRay(start, end, Color.yellow);
        }
        // Draw horizontal lines
        for (int i = 0; i <= maxMapHeight; i++) {

            Vector3 start = new Vector3(mapCenter.x -mapWidth/2, mapCenter.y + i * blockWidth, 1);
            Vector3 end = new Vector3(mapWidth, 0, 1);
            Debug.DrawRay(start, end, Color.yellow);
        }
    }

    private void Update() {
        // Comment this out to dont show grid
        DebugDrawGrid();
    }
}
