using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugUIController : MonoBehaviour {

    public GameObject player;

    // TEXT
    public Text startStopBuildingBtnText;
    public Text messageLabel;

    private PlayerBuilding pb;
    private BuildingController bc;

    private void Start() {
        pb = player.GetComponent<PlayerBuilding>();
        bc = FindObjectOfType<BuildingController>();
    }
    public void PlaceBlock() {
        if (pb.isBuilding) {
            bc.PlaceBlock((int)BlockType.Spawn, pb.SnappedPosition);
        }
    }

    public void ToggleBuilding() {
        if (pb.isBuilding) {
            startStopBuildingBtnText.text = "Start Building";
            pb.isBuilding = false;
        } else {
            startStopBuildingBtnText.text = "Stop Building";
            pb.isBuilding = true;
        }
    }
}
