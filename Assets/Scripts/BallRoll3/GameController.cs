using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace BallRoll3 {
public class GameController : MonoBehaviour {

	public int level;
	public int requiredPieceCount;

	public GameObject exit;

	public Text currentPieceCountText;
	public GameObject levelWinningTextObject;

	public Transform ball;
	public Transform fakeBall;
	public Transform cameraObject;

	public AudioSource winnigMusic;
	public AudioSource toSkyFx;

	[HideInInspector] public bool hasEnoughPieces = false;

	TextMesh requiredPieceCountText;
	Text levelWinningText;

	Animator requiredPieceCountAnimator;
	Animator levelWinningTextAnimator;
	Animator fakeBallAnimator;

	SphereCollider ballCollider;
	Rigidbody ballRigidbody;
	BallMovement ballMovement;
	AudioSource backgroundMusic;
	CameraFollow cameraFollow;
	EscMenu escMenu;

	int currentPieceCount;

	float pressMaxDelay = 0.5f;
	float pressTimer = 0f;
	int pressCount = 0;

	void Start () {
		currentPieceCount = 0;

		requiredPieceCountText = exit.GetComponentInChildren<TextMesh>();
		requiredPieceCountAnimator = exit.GetComponent<Animator>();
		levelWinningTextAnimator = levelWinningTextObject.GetComponent<Animator>();
		levelWinningText = levelWinningTextObject.GetComponentInChildren<Text>();
		cameraFollow = cameraObject.GetComponent<CameraFollow>();
		fakeBallAnimator = fakeBall.GetComponent<Animator>();
		ballCollider = ball.GetComponent<SphereCollider>();
		ballRigidbody = ball.GetComponent<Rigidbody>();
		ballMovement = ball.GetComponent<BallMovement>();
		backgroundMusic = GetComponent<AudioSource>();
		
		if(backgroundMusic.clip != null) backgroundMusic.Play();

		if(level == -1) {
			string sceneLevel = Regex.Match(SceneManager.GetActiveScene().name, @"[0-9]+").Value;
			int.TryParse(sceneLevel, out level);
		}

		levelWinningText.text = "Level " + level;
		requiredPieceCountText.text = "" + requiredPieceCount;

		escMenu = FindObjectOfType<EscMenu>();
	}

	void Update() {
		if(Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9)) {
			pressCount++;
			pressTimer = Time.time;
		}
		if(pressTimer + pressMaxDelay < Time.time) {
			pressCount = 0;
			pressTimer = 0;
		}
		if(pressCount == 5) {
			pressCount = 0;
			pressTimer = 0;
			StartCoroutine(SkipLevel());
		}
	}
	
	public void AddPieces(int count) {
		currentPieceCount += count;

		currentPieceCountText.text = "" + currentPieceCount;

		if(currentPieceCount >= requiredPieceCount) {
			hasEnoughPieces = true;

			currentPieceCountText.color = new Color(0.1960784f, 0.6f, 0.1960784f);

			requiredPieceCountAnimator.SetTrigger("Open");
			requiredPieceCountText.color = new Color(0.4666667f, 1, 0.4666667f);
		}
	}

	public void NextLevel() {
		if(currentPieceCount < requiredPieceCount) {
			hasEnoughPieces = false;
		} else {
			if(PlayerPrefs.GetInt("BallRollProgress", -1) < level) {
				PlayerPrefs.SetInt("BallRollProgress", level);
			}
			StartCoroutine(WinningCutscene());
		}
	}

	IEnumerator WinningCutscene() {
		StartCoroutine(WinningMusicChange());

		cameraFollow.stopMoving = true;
		cameraFollow.StartCoroutine(cameraFollow.JustLookAt(fakeBall));

		levelWinningText.text = "You Won";
		levelWinningTextAnimator.SetTrigger("Won");

		ballCollider.enabled = false;
		ballRigidbody.isKinematic = true;

		for(float i = 0; i <= 1; i += 0.01f) {
			ball.position = Vector3.Lerp(ball.position, fakeBall.position, i);
			ball.rotation = Quaternion.Lerp(ball.rotation, fakeBall.rotation, i);
			yield return new WaitForSeconds(1f / 60f);
		}
		fakeBall.gameObject.SetActive(true);
		ball.localScale = new Vector3(0.01f, 0.01f, 0.01f);
		ball.position = ball.position + 100 * Vector3.up;

		fakeBallAnimator.SetTrigger("Won");

		yield return new WaitForSeconds(3f);
		if(level >= 30) {
			StartCoroutine(escMenu.ToScene("BallRoll3Credits"));
		} else {
			StartCoroutine(escMenu.ToScene("BRLevel" + (level + 1)));
		}
	}

	IEnumerator WinningMusicChange() {
		winnigMusic.Play();
		for(float f = 0; f <= 1f; f += 0.05f) {
			winnigMusic.volume = f;
			backgroundMusic.volume = 1f - f;
			yield return new WaitForSeconds(1f / 60f);
		}
		backgroundMusic.Stop();

		toSkyFx.Play();
	}

	IEnumerator SkipLevel() {
		
		if(!ballMovement.hackerMode) {
			ballMovement.hackerMode = true;
			currentPieceCount += int.MaxValue - 1000;		
			currentPieceCountText.text = "Hackerman: " + currentPieceCount;
			currentPieceCountText.color = Color.magenta;
		} else {
			ballMovement.hackerMode = false;
			currentPieceCount -= int.MaxValue - 1000;		
			currentPieceCountText.text = "" + currentPieceCount;
		}

		if(level != 30) {
			yield return null;

			if(PlayerPrefs.GetInt("BallRollProgress", -1) < level) {
				PlayerPrefs.SetInt("BallRollProgress", level);
			}
			StartCoroutine(WinningMusicChange());

			cameraFollow.stopMoving = true;
			cameraFollow.StartCoroutine(cameraFollow.JustLookAt(fakeBall));

			levelWinningText.text = "Wow you won, you are so good! Keep up the good work! I'm really proud of you!";
			levelWinningTextAnimator.SetTrigger("Won");

			ballCollider.enabled = false;
			ballRigidbody.isKinematic = true;

			fakeBall.gameObject.SetActive(true);
			ball.localScale = new Vector3(0.01f, 0.01f, 0.01f);
			ball.position = ball.position + 100 * Vector3.up;

			fakeBallAnimator.SetTrigger("Won");

			yield return new WaitForSeconds(3.5f);

			StartCoroutine(escMenu.ToScene("BRLevel" + (level + 1)));
		}
	}
}
}