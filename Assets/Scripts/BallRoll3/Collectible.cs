using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallRoll3 {
public class Collectible : MonoBehaviour {

	public int value;
	public GameObject deathParticlePrefab;

	GameController gameController;
	Vector3 randomRotation;

	float defaultHeight;

	void Start () {
		gameController = FindObjectOfType<GameController>();

		defaultHeight = transform.position.y;
		StartCoroutine(UpAndDown());

		randomRotation = Random.insideUnitSphere * 0.6f;
		transform.rotation = Random.rotation;
	}

	void Update () {
		transform.rotation *= Quaternion.Euler(randomRotation);
	}

	IEnumerator UpAndDown() {
		while(true) {
			for(float i = 0; i <= 1; i += 0.01f) {
				transform.position = new Vector3(transform.position.x, Mathf.SmoothStep(defaultHeight - 0.2f, defaultHeight + 0.2f, i), transform.position.z);
				yield return new WaitForSeconds(1f / 60f);
			}
			
			for(float i = 1; i >= 0; i -= 0.01f) {
				transform.position = new Vector3(transform.position.x, Mathf.SmoothStep(defaultHeight - 0.2f, defaultHeight + 0.2f, i), transform.position.z);
				yield return new WaitForSeconds(1f / 60f);
			}
		}
	}

	public void Sync(Vector3 rotation) {
		randomRotation = rotation;
		transform.rotation = Quaternion.identity;
	}
	
	void OnTriggerEnter(Collider other) {
		if(other.CompareTag("Player")) {
			gameController.AddPieces(value);
			Instantiate(deathParticlePrefab, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
	}
}
}