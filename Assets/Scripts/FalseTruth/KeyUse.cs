using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FalseTruth {
public class KeyUse : MonoBehaviour {

	public Text text;
	public Image image;
	
	public void Deactivate() {		//Empties the text field
		image.enabled = false;
		text.text = "";
	}

	public void Activate() {		//Sets the text to the interact key
		image.enabled = true;
		text.text = "E"; //CustomKeyBindings.interact;
	}
}
}