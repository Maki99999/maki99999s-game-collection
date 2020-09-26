using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallRoll3 {
public class Spring : MonoBehaviour {

	public float strength;

	Animator animator;
	AudioSource audioSource;

	void Start () {
		animator = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
	}

	void OnTriggerEnter (Collider other) {
		if(other.CompareTag("Player")) {
			Rigidbody rigid = other.GetComponent<Rigidbody>();
			rigid.velocity = transform.up * strength;
			animator.SetTrigger("Jump");
			audioSource.Play();
		}
	}
}
}