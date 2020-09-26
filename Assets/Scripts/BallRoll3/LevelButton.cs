using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BallRoll3 {
public class LevelButton : MonoBehaviour {

	public int level;
	public Text text;

	BallRoll3Menu menu;

	void Start () {
		menu = transform.parent.GetComponentInParent<BallRoll3Menu>();
	}
	
	public void SetLevel(int level) {
		this.level = level;
		text.text = "" + level;
	}

	public void StartLevel() {
		menu.StartLevel(level);
	}
}
}