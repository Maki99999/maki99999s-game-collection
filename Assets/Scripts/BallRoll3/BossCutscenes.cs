using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallRoll3 {
public class BossCutscenes : MonoBehaviour {

	public Transform ball;
	BallMovement ballMovement;
	Rigidbody ballRigid;
	public AudioSource backgroundMusic;
	public AudioClip bossMusic0;
	public AudioClip bossMusic1;
	public AudioClip bossMusic2;
	public Boss bossScript;
	public GameObject topPlatform;
	public Transform[] movingPartsOfTheTopPlatform;
	public GameObject bossArea;
	public Transform ballCameraObject;
	Camera ballCamera;
	CameraFollow ballCameraFollow;
	public Color darkBackground;
	public BossRing bossRing;

	public GameObject part1Child;
	Transform part1;
	public GameObject spikeBall1;
	Transform spikeBall1Transform;
	Animator spikeBall1Animator;
	AudioSource spikeBall1AudioSource;
	public Transform button1;
	public GameObject part2;
	public Animator cryFireBallAnimator;
	FireBallSpawner[] part2FireBalls;
	
	public GameObject spikeBall2;
	Animator spikeBall2Animator;
	AudioSource spikeBall2AudioSource;
	public Transform button2;
	public GameObject part3;

	public GameObject spikeBall3;
	Animator spikeBall3Animator;
	AudioSource spikeBall3AudioSource;
	public GameObject end;
	public ParticleSystem endBallParticle;
	public Camera endCamera;
	public EscMenu escMenu;

	bool activated = false;

	void Start () {
		bossArea.SetActive(false);
		ballMovement = ball.GetComponent<BallMovement>();
		ballRigid = ball.GetComponent<Rigidbody>();
		ballCamera = ballCameraObject.GetComponent<Camera>();
		ballCameraFollow = ballCameraObject.GetComponent<CameraFollow>();

		part1 = part1Child.transform.parent;
		spikeBall1.SetActive(false);
		spikeBall1Transform = spikeBall1.transform;
		spikeBall1Animator = spikeBall1.GetComponent<Animator>();
		spikeBall1AudioSource = spikeBall1.GetComponent<AudioSource>();
		spikeBall1Transform.localScale = Vector3.zero;
		part2.SetActive(false);

		spikeBall2.SetActive(false);
		spikeBall2Animator = spikeBall2.GetComponent<Animator>();
		spikeBall2AudioSource = spikeBall2.GetComponent<AudioSource>();
		part3.SetActive(false);
		
		spikeBall3.SetActive(false);
		spikeBall3Animator = spikeBall3.GetComponent<Animator>();
		spikeBall3AudioSource = spikeBall3.GetComponent<AudioSource>();
		end.SetActive(false);
	}

	void OnTriggerEnter (Collider other) {
		if(!activated && other.CompareTag("Player")) {
			activated = true;
			StartCoroutine(Cutscene1());
		}
	}
	
	public void PhaseEnd2() {
		StartCoroutine(CutsceneEnd2());
	}

	IEnumerator CutsceneEnd2() {
		yield return new WaitForSeconds(1.5f);

		spikeBall3.SetActive(true);
		spikeBall3Animator.SetTrigger("Throw");
		end.SetActive(true);
		ball.gameObject.SetActive(false);
		ballCameraObject.gameObject.SetActive(false);
		bossRing.StartCoroutine(bossRing.ResetRotation());

		yield return new WaitForSeconds(0.5f);

		spikeBall3AudioSource.Play();

		yield return new WaitForSeconds(2f);

		bossScript.Death();
		bossRing.ChangeSpeed(0f);
		spikeBall3.SetActive(false);
		StartCoroutine(ChangeMusic(null, 1f));

		yield return new WaitForSeconds(8f); 	//10:30

		endBallParticle.Stop();
		StartCoroutine(ChangeBackgroundColorAgain());
		StartCoroutine(HideBoss());

		yield return new WaitForSeconds(8.5f);	//19:00

		escMenu.StartCoroutine(escMenu.ToScene("BallRoll3Credits"));
	}

	IEnumerator HideBoss() {
		Vector3 oldPos = bossArea.transform.position;
		Vector3 newPos = bossArea.transform.position + 100f * Vector3.forward;
		for(float f = 0; f <= 1f; f += 0.01f) {
			bossArea.transform.position = Vector3.Lerp(oldPos, newPos, f * f);
			yield return new WaitForSeconds(1f / 60f);
		}
		bossArea.SetActive(false);
	}

	public void PhaseEnd() {
		StartCoroutine(CutsceneEnd());
	}

	IEnumerator CutsceneEnd() {
		PlayerPrefs.SetInt("BallRollProgress", 30);
		ballMovement.DontMove(true);
		ballRigid.velocity = Vector3.zero;
		ballRigid.angularVelocity = Vector3.zero;
		bossScript.dontMove = true;
		Quaternion oldBallRotation = ball.rotation;
		Quaternion newBallRotation = Quaternion.Euler(-90f, 0f, 0f);
		for(float f = 0; f <= 1f; f += 0.01f) {
			ball.position = Vector3.Lerp(new Vector3(-181f, ball.position.y, ball.position.z), new Vector3(-181f, ball.position.y, -100f), f);
			ball.rotation = Quaternion.Lerp(oldBallRotation, newBallRotation, f);
			yield return new WaitForSeconds(1f / 60f);
		}
		Vector3 oldPos = bossScript.transform.position;
		Vector3 newPos = new Vector3(-181f, bossScript.transform.position.y, -70f);
		for(float f = 0; f <= 1f; f += 0.01f) {
			bossScript.transform.position = Vector3.Lerp(oldPos, newPos, f);
			yield return new WaitForSeconds(1f / 60f);
		}
	}
	
	public void Phase3b() {
		StartCoroutine(Cutscene3b());
	}
	
	IEnumerator Cutscene3b() {
		ballMovement.right = Vector3.zero;
		bossScript.endMovement = true;
		
		float oldx = ball.position.x;
		for(float f = 0; f <= 1f; f += 0.01f) {
			ball.position = Vector3.Lerp(new Vector3(oldx, ball.position.y, ball.position.z), new Vector3(-181f, ball.position.y, ball.position.z), f);
			yield return new WaitForSeconds(1f / 60f);
		}
	}

	public void Phase3() {
		StartCoroutine(Cutscene3());
	}
	
	IEnumerator Cutscene3() {
		ballMovement.DontMove(true);
		ballRigid.velocity = Vector3.zero;
		foreach(FireBallSpawner fireBallSpawner in part2FireBalls) {
			Destroy(fireBallSpawner);
		}
		Vector3 oldPosition = ball.position;
		Vector3 newPosition = new Vector3(-180.56f, 81.28438f, 11.5f);
		Vector3 scaleOld = Vector3.one;
		Vector3 scaleNew = new Vector3(0.02f, 0.02f, 0.02f);
		for(float f = 0; f <= 1f; f += 0.01f) {
			ball.position = Vector3.Lerp(new Vector3(oldPosition.x, ball.position.y, oldPosition.z), new Vector3(newPosition.x, ball.position.y, newPosition.z), f);
			part2.transform.localScale = Vector3.Lerp(scaleOld, scaleNew, Mathf.SmoothStep(0f, 1f, f));
			yield return new WaitForSeconds(1f / 60f);
		}
		part2.SetActive(false);
		spikeBall2.SetActive(true);
		spikeBall2Animator.SetTrigger("Throw");

		yield return new WaitForSeconds(0.5f);

		spikeBall2AudioSource.Play();
		part3.SetActive(true);
		oldPosition = button2.position;
		newPosition = new Vector3(button2.position.x, -3f, button2.position.z);
		for(float f = 0; f <= 1f; f += 0.01f) {
			button2.position = Vector3.Lerp(oldPosition, newPosition, f * f);
			yield return new WaitForSeconds(1f / 60f);
		}
		button2.gameObject.SetActive(false);
		
		yield return new WaitForSeconds(0.3f);

		bossScript.Hit();
		spikeBall2.SetActive(false);
		
		yield return new WaitForSeconds(3f);

		bossScript.Cry();
		StartCoroutine(ChangeCameraAngleAgain());

		yield return new WaitForSeconds(2f);

		ballMovement.bossMovement = false;
		ballMovement.forward = Vector3.forward;
		bossScript.moveTowardsPlayer = true;
		ballMovement.DontMove(false);
		for(float f = 0; f <= 1f; f += 0.01f) {
			bossScript.transform.position = Vector3.Lerp(bossScript.transform.position, new Vector3(-181f, bossScript.transform.position.y, bossScript.transform.position.z), f);
			yield return new WaitForSeconds(1f / 60f);
		}
	}

	IEnumerator ChangeCameraAngleAgain() {
		Vector3 newOffset = new Vector3(0f, 4f, -8f);
		float newMaxTilt = 0f;

		for(float f = 0; f <= 0.3f; f += 0.001f) {
			ballCameraFollow.offset = Vector3.Lerp(ballCameraFollow.offset, newOffset, f);
			ballCameraFollow.maxTilt = Mathf.Lerp(ballCameraFollow.maxTilt, newMaxTilt, f);
			yield return new WaitForSeconds(1f / 60f);
		}
	}
	
	IEnumerator ChangeBackgroundColorAgain() {
		Color oldBackground = new Color(0.8156863f, 0.9497069f, 1f, 0f);
		for(float f = 0; f <= 1f; f += 0.01f) {
			endCamera.backgroundColor = Color.Lerp(endCamera.backgroundColor, oldBackground, f);
			yield return new WaitForSeconds(1f / 60f);
		}
	}

	public void Phase2() {
		StartCoroutine(Cutscene2());
	}
	
	IEnumerator Cutscene2() {
		ballMovement.DontMove(true);
		ballRigid.velocity = Vector3.zero;

		Vector3 oldPosition = ball.position;
		Vector3 newPosition = new Vector3(-66f, 92.09438f, 11.5f);
		Vector3 scaleOld = Vector3.one;
		Vector3 scaleNew = new Vector3(0.02f, 0.02f, 0.02f);
		for(float f = 0; f <= 1f; f += 0.01f) {
			ball.position = Vector3.Lerp(new Vector3(oldPosition.x, ball.position.y, oldPosition.z), new Vector3(newPosition.x, ball.position.y, newPosition.z), f);
			part1.localScale = Vector3.Lerp(scaleOld, scaleNew, Mathf.SmoothStep(0f, 1f, f));
			yield return new WaitForSeconds(1f / 60f);
		}

		yield return new WaitForSeconds(0.5f);

		spikeBall1.SetActive(true);
		for(float f = 0; f <= 1f; f += 0.01f) {
			spikeBall1Transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, f * f);
			yield return new WaitForSeconds(1f / 60f);
		}
		part1Child.SetActive(false);

		yield return new WaitForSeconds(0.5f);

		spikeBall1Animator.SetTrigger("Throw");
		spikeBall1AudioSource.Play();
		part2.SetActive(true);
		oldPosition = button1.position;
		newPosition = new Vector3(button1.position.x, -3f, button1.position.z);
		for(float f = 0; f <= 1f; f += 0.01f) {
			button1.position = Vector3.Lerp(oldPosition, newPosition, f * f);
			yield return new WaitForSeconds(1f / 60f);
		}
		button1.gameObject.SetActive(false);
		
		yield return new WaitForSeconds(0.3f);

		bossScript.Hit();
		spikeBall1.SetActive(false);

		yield return new WaitForSeconds(3f);

		StartCoroutine(ChangeMusic(bossMusic2, 10f));
		bossScript.Cry();

		yield return new WaitForSeconds(1.4f);

		cryFireBallAnimator.gameObject.SetActive(true);
		cryFireBallAnimator.SetTrigger("Again");
		part2FireBalls = FindObjectsOfType<FireBallSpawner>();
		foreach(FireBallSpawner fireBallSpawner in part2FireBalls) {
			fireBallSpawner.StartSpawning();
		}

		yield return new WaitForSeconds(2f);

		ballMovement.DontMove(false);
	}

	IEnumerator Cutscene1() {
		bossArea.SetActive(true);
		StartCoroutine(MoveTheMovingPartsOfTheTopPlatform());
		bossRing.StartRotating();

		ballMovement.DontMove(true);
		ballRigid.velocity = Vector3.zero;

		Vector3 oldBallPosition = ball.position;
		Vector3 newBallPosition = new Vector3(0f, ball.position.y - 2f, 11.5f);
		Quaternion oldBallRotation = ball.rotation;
		Quaternion newBallRotation = Quaternion.Euler(-90f, 0f, 0f);
		for(float f = 0; f <= 1f; f += 0.05f) {
			ball.position = Vector3.Lerp(oldBallPosition, newBallPosition, f * f);
			ball.rotation = Quaternion.Lerp(oldBallRotation, newBallRotation, f * f);
			yield return new WaitForSeconds(1f / 60f);
		}

		StartCoroutine(ChangeMusic(bossMusic0, 0.5f));
		StartCoroutine(ChangeBackgroundColor());
		StartCoroutine(ChangeCameraAngle());

		yield return new WaitForSeconds(1.2f);

		bossScript.Cry();

		yield return new WaitForSeconds(5f);

		topPlatform.SetActive(false);
		ballMovement.forward = Vector3.zero;
		ballMovement.bossMovement = true;
		ballMovement.bossZ = ball.position.z;
		ballMovement.DontMove(false);
		StartCoroutine(ChangeMusic(bossMusic1, 10f));
	}

	IEnumerator MoveTheMovingPartsOfTheTopPlatform() {
		Vector3[] newPositions = {
			movingPartsOfTheTopPlatform[0].position + new Vector3(0f, 0f, -4f),
			movingPartsOfTheTopPlatform[1].position + new Vector3(0f, 0f, 4f),
			movingPartsOfTheTopPlatform[2].position + new Vector3(0f, 0f, 5f),
			movingPartsOfTheTopPlatform[3].position + new Vector3(-4f, 0f, 0f),
			movingPartsOfTheTopPlatform[4].position + new Vector3(-5f, 0f, 0f),
			movingPartsOfTheTopPlatform[5].position + new Vector3(4f, 0f, 0f),
			movingPartsOfTheTopPlatform[6].position + new Vector3(5f, 0f, 0f)
		};
		
		for(float f = 0; f <= 1f; f += 0.05f) {
			for(int i = 0; i < movingPartsOfTheTopPlatform.Length; i++) {
				movingPartsOfTheTopPlatform[i].position = Vector3.Lerp(movingPartsOfTheTopPlatform[i].position, newPositions[i], f);
			}
			yield return new WaitForSeconds(1f / 60f);
		}
	}

	IEnumerator ChangeCameraAngle() {
		Vector3 newOffset = new Vector3(0f, 1f, -16f);
		float newMaxTilt = 0f;

		for(float f = 0; f <= 0.3f; f += 0.001f) {
			ballCameraFollow.offset = Vector3.Lerp(ballCameraFollow.offset, newOffset, f);
			ballCameraFollow.maxTilt = Mathf.Lerp(ballCameraFollow.maxTilt, newMaxTilt, f);
			yield return new WaitForSeconds(1f / 60f);
		}
	}

	IEnumerator ChangeBackgroundColor() {
		for(float f = 0; f <= 1f; f += 0.01f) {
			ballCamera.backgroundColor = Color.Lerp(ballCamera.backgroundColor, darkBackground, f);
			yield return new WaitForSeconds(1f / 60f);
		}
	}

	IEnumerator ChangeMusic(AudioClip newAudioClip, float speedModifier) {
		for(float f = 0; f <= 1f; f += 0.05f * speedModifier) {
			backgroundMusic.volume = 1f - f;
			yield return new WaitForSeconds(1f / 60f);
		}
		backgroundMusic.Stop();
		backgroundMusic.clip = newAudioClip;
		backgroundMusic.Play();
		for(float f = 0; f <= 1f; f += 0.05f * speedModifier) {
			backgroundMusic.volume = f;
			yield return new WaitForSeconds(1f / 60f);
		}
	}
}
}