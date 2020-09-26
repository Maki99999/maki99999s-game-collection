using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallRoll3 {
public class MoveThingsDown : MonoBehaviour {

	List<GameObject> movedObjects = new List<GameObject>();

	int resetId = 0;

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.name.Contains("Spike")) {
			StartCoroutine(MoveDown(other.transform));
		}
	}

	public void Reset() {
		resetId = (resetId + 1) % 3;
		foreach(GameObject gameObject in movedObjects) {
			gameObject.SetActive(true);
		}
		movedObjects = new List<GameObject>();
	}

	IEnumerator MoveDown(Transform objectTransform) {
		int ObjectResetId = resetId;
		movedObjects.Add(objectTransform.gameObject);
		Vector3 oldPosition = objectTransform.position;
		Vector3 newPosition = new Vector3(objectTransform.position.x, 0f, objectTransform.position.z);
		for(float f = 0; f <= 1f; f += 0.005f) {
			objectTransform.position = Vector3.Lerp(oldPosition, newPosition, f * f);
			if(ObjectResetId != resetId) {
				goto end;
			}
			yield return new WaitForSeconds(1f / 60f);
		}
		objectTransform.gameObject.SetActive(false);
		end: objectTransform.position = oldPosition;
	}
}
}