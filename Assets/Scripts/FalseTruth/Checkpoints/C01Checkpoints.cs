using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FalseTruth {
public class C01Checkpoints : FalseTruth.Checkpoints {

	public Transform C01Checkpoint01;
	public GameObject eyes;

	int currentCheckpoint = -1;

	FalseTruth.FirstPersonControllerExtended fpsController;
	Transform player;

	void Start () {
		PlayerPrefs.SetInt("Scene", 1);
		LoadCheckpoint();
	}

	public override void LoadCheckpoint() {
		int checkpoint = PlayerPrefs.GetInt("Checkpoint", -1);
		currentCheckpoint = checkpoint;

		if(currentCheckpoint < 0 || currentCheckpoint > 1) {
			return;
		}
		StartCoroutine(Case0());
	}

	IEnumerator Case0() {
		yield return null;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		C01Start cutscene = FindObjectOfType<C01Start>();
		cutscene.enabled = false;
		cutscene.StopAllCoroutines();
		cutscene.girlChairAnimator.SetTrigger("Move");
		cutscene.paperAnimator.SetBool("Hold", true);

		cutscene.playerAnimator.enabled = false;
		cutscene.ghost.SetActive(false);
		cutscene.girlAnimator.gameObject.SetActive(false);
		BoxCollider[] collider = cutscene.deskNChair.GetComponents<BoxCollider>();
		collider[0].enabled = true;
		collider[1].enabled = true;
		collider[2].enabled = false;
		player = GameObject.FindWithTag("Player").transform;
		cutscene.deskNChair.GetComponent<FalseTruth.SitInChair>().isSitting = false;
		Transform pos00 = cutscene.deskNChair.GetComponent<FalseTruth.SitInChair>().standPosition;
		player.position = pos00.position;
		player.rotation = Quaternion.Euler(pos00.eulerAngles.y * Vector3.up);
		player.GetChild(0).transform.localRotation = Quaternion.Euler(pos00.eulerAngles.x * Vector3.right);
		fpsController = cutscene.fpsController;
		fpsController.SetMovement(FalseTruth.FirstPersonControllerExtended.FULL_MOVEMENT);
		fpsController.SetMovement(FalseTruth.FirstPersonControllerExtended.FULL_MOVEMENT);

		if(currentCheckpoint == 1) {
			StartCoroutine(Case1());
		}
		yield return null;
		cutscene.paperAnimator.SetBool("Hold", false);
	}

	IEnumerator Case1() {
		PlayerPrefs.SetFloat("OccultLevel", -1);
		yield return null;
		C01OccultClub cutscene = FindObjectOfType<C01OccultClub>();
		cutscene.StopAllCoroutines();
		cutscene.alreadyUsed = true;	
		player.position = C01Checkpoint01.position;
		player.rotation = Quaternion.Euler(C01Checkpoint01.eulerAngles.y * Vector3.up);
		player.GetChild(0).transform.localRotation = Quaternion.Euler(C01Checkpoint01.eulerAngles.x * Vector3.right);
		fpsController.SetMovement(FalseTruth.FirstPersonControllerExtended.FULL_MOVEMENT);
		fpsController.SetMovement(FalseTruth.FirstPersonControllerExtended.FULL_MOVEMENT);
	}
}
}