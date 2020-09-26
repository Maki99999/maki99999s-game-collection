using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FalseTruth {
public class ChoiceButtonScript : MonoBehaviour {

	public void Click() {
		FindObjectOfType<FalseTruth.DialogueManager>().DisplayNextScentence(GetComponentInChildren<Text>().text);
	}
}
}