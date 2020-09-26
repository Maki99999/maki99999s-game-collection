using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallRoll3 {
public class BallMovement : MonoBehaviour {

	public CameraFollow cameraFollowScript;
	public Animator fadeBlack;
    public LayerMask groundDetectMask = -1;
	public AudioSource deathSound;
	public GameObject explodeParticlePrefab;
	public MoveContinuously spike;
	public ObstackleFollow spikeFollow;
	public Boss boss;
	
	public float speed = 15;
	public float maxSpeed = 30;

	public Vector3 forward = Vector3.forward;
	public Vector3 right = Vector3.right;

	Rigidbody rigidBody;
	AudioSource audioSource;

	Vector3 checkpointPosition;
	Vector2 input;
	Animator animator;
	bool dontMove;
	bool onGround;
	float deathHeight = -3;

	[HideInInspector] public bool bossMovement = false;
	[HideInInspector] public float bossZ = 0f;
	[HideInInspector] public bool hackerMode = false;

	void Start() {
		rigidBody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
		animator = GetComponent<Animator>();

		checkpointPosition = transform.position;
		dontMove = false;
	}


	void Update() {
		input = new Vector2(
			Input.GetAxis("Horizontal"),
			Input.GetAxis("Vertical")
		);
		input.Normalize();
		cameraFollowScript.input = input;
	}
	
	void FixedUpdate () {
		if(dontMove) {
			onGround = Physics.CheckCapsule(transform.position, new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z), 0.5f, groundDetectMask);
			float magnitudeA = new Vector3(rigidBody.velocity.x, 0f, rigidBody.velocity.z).magnitude;
			audioSource.volume = onGround ? magnitudeA / (maxSpeed / 4) : 0;
			return;
		}

		Vector3 movement = input.x * right + input.y * forward;
		movement *= speed;
		movement += onGround ? 2f * Vector3.up : -4f * Vector3.up;
		
		rigidBody.AddForce(movement);

		Vector3 velocity = rigidBody.velocity;
		float magnitude = new Vector3(velocity.x, 0f, velocity.z).magnitude;
		
		onGround = Physics.CheckCapsule(transform.position, new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z), 0.5f, groundDetectMask);

		if(magnitude > maxSpeed)
			rigidBody.velocity = velocity.normalized * maxSpeed;

		audioSource.volume = onGround ? magnitude / (maxSpeed / 4) : 0;

		if(bossMovement)  {
			transform.position = new Vector3(transform.position.x, transform.position.y, bossZ);
		}

		if(transform.position.y < deathHeight) {
			dontMove = true;
			StartCoroutine(ToLastCheckpoint());
		}
	}

	IEnumerator ToLastCheckpoint() {
		audioSource.Stop();

		fadeBlack.SetTrigger("Out");
		yield return new WaitForSeconds(1f);

		transform.rotation = Quaternion.Euler(-90, 0, 0);
		rigidBody.velocity = Vector3.zero;
		transform.position = checkpointPosition;
		yield return null;
		transform.rotation = Quaternion.Euler(-90, 0, 0);
		rigidBody.velocity = Vector3.zero;
		transform.position = checkpointPosition;
		yield return null;
		transform.rotation = Quaternion.Euler(-90, 0, 0);
		rigidBody.velocity = Vector3.zero;
		transform.position = checkpointPosition;
		yield return null;
		transform.rotation = Quaternion.Euler(-90, 0, 0);
		rigidBody.velocity = Vector3.zero;
		transform.position = checkpointPosition;
		
		fadeBlack.SetTrigger("Again");

		if(spike != null)
			spike.Reset();
		if(spikeFollow != null)
			spikeFollow.StopAllCoroutines();
		if(boss != null)
			boss.Reset();

		audioSource.Play();
		dontMove = false;
	}

	public void SetCheckpoint(Vector3 position, float newDeathHeight) {
		checkpointPosition = position;
		deathHeight = newDeathHeight;
	}

	void OnCollisionEnter(Collision other) {
		if(!dontMove && other.gameObject.CompareTag("Deadly")) {
			if(hackerMode) {
				return;
			}
			dontMove = true;
			deathSound.Play();
			Instantiate(explodeParticlePrefab, transform.position, Quaternion.identity);
			animator.SetTrigger("Explode");
			StartCoroutine(ToLastCheckpoint());
		}
	}

	public void DontMove(bool dont) {
		dontMove = dont;
	}
}
}