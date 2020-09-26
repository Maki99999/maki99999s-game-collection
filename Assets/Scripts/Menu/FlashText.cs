using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashText : MonoBehaviour {

	public int seconds;
	public float minScale;
	public float maxScale = 1f;

	void Start () {
		StartCoroutine(Flash());
	}

	IEnumerator Flash() {
		while(enabled) {
			for(float f = minScale; f <= maxScale; f += 1f / (seconds * 60f)) {
				transform.localScale = new Vector3(f, f, f);
				yield return new WaitForSeconds(1f / 60f);
			}
			for(float f = maxScale; f >= minScale; f -= 1f / (seconds * 60f)) {
				transform.localScale = new Vector3(f, f, f);
				yield return new WaitForSeconds(1f / 60f);
			}
		}
	}
}
