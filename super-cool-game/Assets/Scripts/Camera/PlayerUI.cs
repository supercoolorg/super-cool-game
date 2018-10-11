using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

	public Image iconStructural;
	public Image iconPowerUps;
	public Image iconSpecial;

	public GameObject panel;
	public GameObject blockImage;

	private List<GameObject> blockImages = new List<GameObject>();

	// DEBUG
	public int blocks;

	// Keep track of the selected block to place
	int selectedElem = 0;

	void Start () {
		// Shall we forget to assing the panel, Unity'll try to find it.
		if(panel == null) {
			panel = GameObject.Find("/PlayerUICanvas/Canvas/Panel");
		}

		Init();
	}

	public void Init() {
		for (int i = 0; i < blocks; i++) {
			GameObject newImage = Instantiate(blockImage, panel.transform);
			newImage.transform.position += new Vector3(45 * i, 0, 0);

			blockImages.Add(newImage);
		}
		Destroy(blockImage);
	}

	public void ScrollLeft() {
		// translate every element to the left
		if(selectedElem < blockImages.Count - 1) {
			// move each block to the left
			foreach (var block in blockImages){
				Vector3 startPos = block.transform.position;
				block.transform.position = startPos + new Vector3(-45, 0, 0);
			}
			selectedElem += 1;
			ScaleBlocks();
		}
	}

	public void ScrollRight() {
		// translate every element to the right
		if(selectedElem > 0) {
			// move each block to the right
			foreach (var block in blockImages){
				Vector3 startPos = block.transform.position;
				block.transform.position = startPos + new Vector3(45, 0, 0);
			}
			selectedElem -= 1;
			ScaleBlocks();
		}
	}

	private void ScaleBlocks() {
		for (int i = 0; i < blockImages.Count; i++) {
			// If it's not the central block, scale it down
			if(selectedElem != i) {
				blockImages[i].transform.localScale = new Vector3(1,1,0);
			} else {
				// Scale it up by 0.1
				blockImages[i].transform.localScale += new Vector3(0.1f, 0.1f, 0);
			}
		}
	}
}
