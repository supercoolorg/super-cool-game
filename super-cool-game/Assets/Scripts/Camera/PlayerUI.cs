using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

	
	// Player to follow
	public GameObject player;

	// Get sprite for circular container of picker UI
	public GameObject ui_circ;

	// Get image prototype to replicate
	public GameObject ui_icon;

	// Set sprite offset relative to player
	public Vector3 offset = new Vector3(0, 20, 0);

	[HideInInspector] public bool isPickerUIActive;

	// List of block images to show on UI
	private List<GameObject> icons = new List<GameObject>();

	// The canvas container
	private GameObject canvas;

	// Used for Lerp
	// How many seconds to complete the animation
	private float ui_circSpeed = 0.1f;

	// Goes from 0 to 1, where 1 is ui_circSpeed [seconds]
	private float ui_circT = 0; 

	private float ui_circRadius;

	void Start () {
		// Shall we forget to assing the panel, Unity'll try to find it.
		if(canvas == null) {
			canvas = GameObject.Find("/PlayerUICanvas/Canvas");
		}

		// Same as before, find the object if not assigned.
		if(ui_circ == null) {
			ui_circ = GameObject.Find("/PlayerUICanvas/Canvas/CircularBackground");
		}

		// Hide it
		ui_circ.transform.localScale = Vector3.zero;

		// Calculate the radius
		ui_circRadius = Vector3.Distance(ui_icon.transform.position, ui_circ.transform.position);

		for (int i = 0; i < 8; i++) {
			GameObject icon = Instantiate(ui_icon, ui_circ.transform);
			icon.name = "Icon_" + i;
			icons.Add(icon);
		}
		Debug.Log(ui_circRadius);
		Destroy(ui_icon);
	}

	// Show circular container
	public void ShowPickerUI() {
		// Increase the % of the animation
		ui_circT += Time.deltaTime;

		// Show the circular background
		ui_circ.transform.localScale = Vector2.Lerp(
			Vector2.zero,
			new Vector2(1, 1),
			ui_circT / ui_circSpeed
		);

		// Set its position on UI based to player position in world.
		ui_circ.transform.position = Camera.main.WorldToScreenPoint(player.transform.position + offset);

		UpdatePickerUIElements();
	}

	// Hide circular container
	public void HidePickerUI() {
		// Reset the progress on the animation
		ui_circT = 0;
		// Show the circular background
		ui_circ.transform.localScale = Vector2.zero;
	}

	public void UpdatePickerUIElements() {
		for (int i = 0; i < 8; i++) {
				float angle = Mathf.PI/4 * i;
				float posX = Mathf.Cos(angle) * ui_circRadius;
				float posY = Mathf.Sin(angle) * ui_circRadius;
				
				icons[i].transform.position = ui_circ.transform.position 
					+ new Vector3(posX, posY, 0);
		}
	}

	void Update() {
		if(isPickerUIActive) {
			ShowPickerUI();
		} else {
			HidePickerUI();
		}
	}
}
