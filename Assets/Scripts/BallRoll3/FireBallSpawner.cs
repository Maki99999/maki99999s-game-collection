using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallRoll3 {
public class FireBallSpawner : MonoBehaviour {

	public GameObject fireBallPrefab;
	public float startDelay;
	public float delayBetweenFireBalls;
	public AudioSource audioSource;
	public AudioClip audioClip;

	public void StartSpawning() {
		StartCoroutine(Spawning());
	}
	
	IEnumerator Spawning() {
		yield return new WaitForSeconds(startDelay);
		while(enabled) {
			Instantiate(fireBallPrefab, transform.position, transform.rotation, transform);
			yield return new WaitForSeconds(delayBetweenFireBalls);
		}
	}

	public void ExlplodeSound() {
		audioSource.PlayOneShot(audioClip);
	}
}
}