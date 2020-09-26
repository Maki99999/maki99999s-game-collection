using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallRoll3 {
public class Fireball : MonoBehaviour {
	
	public Transform particleTransform;
	public GameObject fireball;
	public float speed = 1;

	SphereCollider sphereCollider;
	Rigidbody rigid;
	ParticleSystem particles;
	AudioSource audioSource;

	void Start() {
		sphereCollider = GetComponent<SphereCollider>();
		rigid = GetComponent<Rigidbody>();
		particles = GetComponentInChildren<ParticleSystem>();
		audioSource = GetComponent<AudioSource>();
		rigid.AddForce(-transform.up * speed);
	}

	void OnCollisionEnter(Collision other) {
		fireball.SetActive(false);
		sphereCollider.enabled = false;
		rigid.velocity = Vector3.zero;
		rigid.angularVelocity = Vector3.zero;
		particleTransform.localScale = Vector3.one * 3;
		audioSource.Stop();
		FireBallSpawner fireBallSpawner = GetComponentInParent<FireBallSpawner>();
		if(fireBallSpawner != null)
			fireBallSpawner.ExlplodeSound();
		particles.Stop();
		Destroy(gameObject, 5f);
	}
}
}