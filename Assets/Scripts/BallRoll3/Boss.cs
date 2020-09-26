using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallRoll3 {
public class Boss : MonoBehaviour {

	public Transform ball;
	public AudioClip cry;
	public AudioClip hit;
	public AudioClip death;
	public bool moveTowardsPlayer = false;
	public float moveTowardsPlayerSpeed = 1f;
	public Transform part3Platform;
	public MoveThingsDown moveThingsDown;
	public bool endMovement = false;
	public bool dontMove = false;

	Animator animator;
	AudioSource audioSource;
	Rigidbody ballRigidbody;
	float distanceBetweenBossAndBall = 70f - 11.5f;

	void Start () {
		animator = GetComponentInChildren<Animator>();
		audioSource = GetComponent<AudioSource>();
		ballRigidbody = ball.GetComponent<Rigidbody>();
	}

	void FixedUpdate () {
		if(dontMove) {
			part3Platform.position -= Vector3.up;
			if(part3Platform.position.y < 0) {
				part3Platform.gameObject.SetActive(false);
			}
			return;
		}
		if(moveTowardsPlayer) {
			if(ballRigidbody.velocity.z < -0.5f) {
				if(endMovement) {
					distanceBetweenBossAndBall -= (distanceBetweenBossAndBall - 24.5f) * 0.005f;
				} else {
					distanceBetweenBossAndBall -= 0.01f * moveTowardsPlayerSpeed;
				}
				transform.position = new Vector3(transform.position.x, transform.position.y, ball.position.z + distanceBetweenBossAndBall);
			} else {
				distanceBetweenBossAndBall -= 0.1f;
				transform.position = new Vector3(transform.position.x, transform.position.y, ball.position.z + distanceBetweenBossAndBall);
			}
			part3Platform.position = new Vector3(-181f, 60f, ball.position.z - 41.5f);
		} else {
			transform.position = new Vector3(ball.position.x, transform.position.y, transform.position.z);
		}
	}

	public void Reset() {
		distanceBetweenBossAndBall = 70f - 11.5f;
		transform.position = new Vector3(-181f, transform.position.y, ball.position.z + distanceBetweenBossAndBall);
		moveTowardsPlayerSpeed = 1f;
		moveThingsDown.Reset();
	}

	public void Cry() {
		animator.SetTrigger("Cry");
		StartCoroutine(PlayAudioIn(cry, 1.4f));
	}

	IEnumerator PlayAudioIn(AudioClip audioClip, float seconds) {
		yield return new WaitForSeconds(seconds);
		audioSource.clip = audioClip;
		audioSource.Play();
	}
	
	public void Hit() {
		animator.SetTrigger("Hit");
		StartCoroutine(PlayAudioIn(hit, 0.1f));
	}

	public void Death() {
		animator.SetTrigger("Death");
		StartCoroutine(PlayAudioIn(death, 0.1f));
	}
}
}