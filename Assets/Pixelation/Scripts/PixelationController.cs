using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Pixelation.Scripts;

public class PixelationController : MonoBehaviour {

	Pixelation pixelation;
	int pixelationLevel;
	
	void Start() {
		pixelation = GetComponent<Pixelation>();
		pixelation.enabled = false;
		pixelationLevel = 0;
	}

	void Update () {
		if(Input.GetKeyDown(KeyCode.P)) {	//Toggles the menu by pressing P
			TogglePixelation();
		}
	}

	void TogglePixelation() {
		pixelationLevel = (pixelationLevel + 1) % 6;
		switch(pixelationLevel) {
			case 0:
				pixelation.enabled = false;
				break;
			case 1:
				pixelation.enabled = true;
				pixelation.BlockCount = 500;
				break;
			case 2:
				pixelation.enabled = true;
				pixelation.BlockCount = 400;
				break;
			case 3:
				pixelation.enabled = true;
				pixelation.BlockCount = 300;
				break;
			case 4:
				pixelation.enabled = true;
				pixelation.BlockCount = 200;
				break;
			case 5:
				pixelation.enabled = true;
				pixelation.BlockCount = 100;
				break;
		}
	}
}
