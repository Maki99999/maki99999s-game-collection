using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SportsGame {
public class TagNPCMove : MonoBehaviour {

	public float walkSpeed = 1;
	public float maxAngle = 30;
	public float timeBetweenAngles = 10;

	protected Bounds area;
	public float areaTurnDistance;
	public float areaTurnVelocity;

	public GameObject zombiePrefab;

	protected Rigidbody rig;
	protected Animator animator;
	protected GameController gameController;
	protected FakeController fakeController;

	bool isAreaTurning;
	bool areaTurnRight;
	protected bool walkIn = true;

	protected bool isFake = false;

	void Start () {
		rig = GetComponent<Rigidbody>();
		animator = GetComponent<Animator>();
		gameController = GetComponentInParent<GameControllerTag>();
		if(gameController == null) {
			isFake = true;
			fakeController = GetComponentInParent<FakeController>();
		}

		StartCoroutine(ChangeAngleValue());
		StartCoroutine(WalkIn());

		InitArea();
	}

	void FixedUpdate() {
		if(GameControllerTag.paused.Value) return;
		if(walkIn) {
			rig.MovePosition(transform.position + transform.forward * walkSpeed);
			return;
		}
		Walk();
	}

	protected void Walk() {
		Vector3 newPos = transform.position + transform.forward * walkSpeed;
		if((newPos.x < area.min.x || newPos.x > area.max.x || newPos.z < area.min.z || newPos.z > area.max.z)) {
			if(!isAreaTurning) {
				isAreaTurning = true;
				float currentY = transform.eulerAngles.y;
				while(currentY < 0) currentY += 360;
				currentY %= 360;

				if(newPos.x < area.min.x || newPos.x > area.max.x) {
					if(currentY <= 90 || (currentY <= 270 && currentY > 180)) {
						areaTurnRight = false;
					} else {
						areaTurnRight = true;
					}
				} else if(newPos.z < area.min.z || newPos.z > area.max.z) {
					if(currentY < 90 || (currentY < 270 && currentY >= 180)) {
						areaTurnRight = true;
					} else {
						areaTurnRight = false;
					}
				}
			}
		} else {
			isAreaTurning = false;
		}
		if(isAreaTurning) {
			transform.Rotate(areaTurnRight ? transform.up * areaTurnVelocity : transform.up * -areaTurnVelocity);
		}
		rig.MovePosition(newPos);
	}

	protected void InitArea() {
		area = isFake ? fakeController == null ? new Bounds(Vector3.zero, Vector3.zero) : fakeController.tagArea : gameController.area;
		
		Vector3 areaMin = area.min + areaTurnDistance * (Vector3.forward + Vector3.right);
		Vector3 areaMax = area.max - areaTurnDistance * (Vector3.forward + Vector3.right);

		area = new Bounds();
		area.SetMinMax(areaMin, areaMax);
	}

	IEnumerator WalkIn() {
		yield return new WaitUntil(() => area.Contains(transform.position));
		walkIn = false;
		StartCoroutine(ChangeAngle(-Random.value * maxAngle));
	}

	IEnumerator ChangeAngleValue() {
		while(this.enabled) {
			yield return new WaitForSecondsPaused(Random.Range(timeBetweenAngles * 3 / 5, timeBetweenAngles * 7 / 5), GameControllerTag.paused);
			if(!walkIn || !GameControllerTag.paused.Value) {
				StartCoroutine(ChangeAngle((Random.value - 0.5f) * 2 * maxAngle));
			}
		}
	}

	IEnumerator ChangeAngle(float angle) {
		for(float f = 0f; f < 1f; f = f + 0.01f) {
			transform.Rotate(transform.up * angle * 0.01f);
			yield return new WaitForSeconds(1f / 60f);
		}
	}

	public TagChaserMove Infect() {
		TagChaserMove zombie = Instantiate(zombiePrefab, transform.position, transform.rotation, transform.parent).GetComponent<TagChaserMove>();
		transform.GetChild(transform.childCount - 1).SetParent(zombie.transform);
		Destroy(this.gameObject);
		return zombie;
	}

	public void SetAnimation(string animationTrigger) {
		animator.SetTrigger(animationTrigger);
	}
}
}