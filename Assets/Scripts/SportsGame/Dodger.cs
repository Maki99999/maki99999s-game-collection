using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SportsGame {
public class Dodger : MonoBehaviour {

	public float walkSpeed = 0.1f;
	public float maxAngle = 60;
	public float timeBetweenAngles = 3;

	protected Bounds area;
	public float areaTurnDistance;
	public float areaTurnVelocity;
    
	bool isAreaTurning;
	bool areaTurnRight;
    bool isFake = false;
    /*public Vector3 lookAheadLeft;
    public Vector3 lookAheadRight;
    Vector3 center;
    public float maxRad = 10f; */

	Rigidbody rig;
	Animator animator;
	GameControllerThrow gameControllerThrow;
    GameController gameController;
	FakeController fakeController;
    public Transform skeleton;
    bool gotHit = false;

	void Start () {
		rig = GetComponent<Rigidbody>();
		animator = GetComponent<Animator>();
		gameControllerThrow = GetComponentInParent<GameControllerThrow>();

        //center = gameControllerThrow != null ? gameControllerThrow.area.center : new Vector3(-20, 0, -37.5f);
        gameController = gameControllerThrow;
		if(gameController == null) {
			isFake = true;
			fakeController = GetComponentInParent<FakeController>();
		}

		StartCoroutine(ChangeAngleValue());

		InitArea();
	}

	void FixedUpdate() {
		if(GameControllerThrow.paused.Value || gotHit) return;
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
		area = isFake ? fakeController.tagArea : gameController.area;
		
		Vector3 areaMin = area.min + areaTurnDistance * (Vector3.forward + Vector3.right);
		Vector3 areaMax = area.max - areaTurnDistance * (Vector3.forward + Vector3.right);

		area = new Bounds();
		area.SetMinMax(areaMin, areaMax);
	}

	/*protected void Walk() {
		Vector3 newPos = transform.position + transform.forward * walkSpeed;

        float distLeft = (center - PosLocalToGlobal(lookAheadLeft)).magnitude;
        float distRight = (center - PosLocalToGlobal(lookAheadRight)).magnitude;
        
        if(distLeft > maxRad) {
            while((center - PosLocalToGlobal(lookAheadLeft)).magnitude > maxRad) {
                transform.Rotate(Vector3.up * 1f);
            }
        } else if(distRight > maxRad) {
            while((center - PosLocalToGlobal(lookAheadRight)).magnitude > maxRad) {
                transform.Rotate(-Vector3.up * 1f);
            }
        } 
        
		rig.MovePosition(newPos);
	}

    Vector3 PosLocalToGlobal(Vector3 posFromTransform) {
        return transform.position + transform.forward * posFromTransform.z + transform.right * posFromTransform.x;
    } */

	IEnumerator ChangeAngleValue() {
		while(this.enabled) {
			yield return new WaitForSecondsPaused(Random.Range(timeBetweenAngles * 3 / 5, timeBetweenAngles * 7 / 5), GameControllerThrow.paused);
			if(!GameControllerThrow.paused.Value && !gotHit) {
				StartCoroutine(ChangeAngle((Random.value - 0.5f) * 2 * maxAngle));
			}
		}
	}

	IEnumerator ChangeAngle(float angle) {
		for(float f = 0f; f < 1f; f += 0.01f) {
			transform.Rotate(transform.up * angle * 0.01f);
			yield return new WaitForSeconds(1f / 60f);
		}
	}

	public void SetAnimation(string animationTrigger) {
		animator.SetTrigger(animationTrigger);
	}

    void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Projectile") && !gotHit && !isFake) {
            gotHit = true;
            gameControllerThrow.AddEnemysHit();
            other.tag = "Untagged";
            Rigidbody ballRigid = other.GetComponent<Rigidbody>();
            ballRigid.isKinematic = true;
            ballRigid.useGravity = false;
            ballRigid.velocity = Vector3.zero;
            other.transform.parent = skeleton;
            SetAnimation("Hit");
            StartCoroutine(DestroyAfter(other.gameObject));
        }
    }

    IEnumerator DestroyAfter(GameObject ball) {
        yield return new WaitForSeconds(3f);
        gameControllerThrow.thrownBalls.Remove(ball);
        Destroy(gameObject);
    }
}
}