using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToggleItem : MonoBehaviour {

	public GameSelect levelSelectScript;
	public int id = 0;

	public void ValueChange(bool active) {
		if(active) {
			levelSelectScript.SetActiveToggle(id, true);
		} else {
			levelSelectScript.SetActiveToggle(id, false);
		}
	}

	public virtual void OnPointerClick(PointerEventData eventData) {
		Debug.Log("Test");
        if(eventData.clickCount == 2) {
			levelSelectScript.SelectLevel();
        }
    }
}
