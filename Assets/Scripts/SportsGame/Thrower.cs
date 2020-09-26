using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SportsGame {
public class Thrower : MonoBehaviour {

    public GameObject ballPrefab;
    public Transform handBone;
    public Material[] ballMaterials;

    Animator animator;
    GameControllerDodge gameControllerDodge;
    public GameControllerDodge gameControllerDodgeFake;
    Transform player;
    Vector3 offset = new Vector3(-0.16f, 0.0f, 0);
    bool isThrowing = false;
    bool inFreeze = false;

    List<GameObject> thrownBalls;

    void Start() {
        animator = GetComponent<Animator>();
        gameControllerDodge = GetComponentInParent<GameControllerDodge>();
        if(gameControllerDodge == null) gameControllerDodge = gameControllerDodgeFake;
        player = gameControllerDodge.player.transform;
        thrownBalls = new List<GameObject>();
    }

    public void Throw() {
        StartCoroutine(ThrowABall());
    }

    public void FixedUpdate() {
        if(!inFreeze)
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
    }

    IEnumerator ThrowABall() {
        yield return new WaitUntil(() => !isThrowing);
        isThrowing = true;

        int ballType = Random.Range(0, 3);

        animator.SetBool("HandUp", true);
        yield return new WaitForSecondsPaused(25 / 60f, GameControllerDodge.paused);

        GameObject ball = Instantiate(ballPrefab, handBone.position + offset, Quaternion.Euler(0, 0, 0), handBone);
        thrownBalls.Add(ball);

        MeshRenderer meshRenderer = ball.GetComponent<MeshRenderer>();
        meshRenderer.material = ballMaterials[ballType];

        Collider ballCollider = ball.GetComponent<Collider>();
        foreach (Collider collider in gameControllerDodge.ballColliderToIgnore) {
            Physics.IgnoreCollision(ballCollider, collider);
        }

        yield return new WaitForSecondsPaused(Random.Range(3f, 6f), GameControllerDodge.paused);
        animator.SetBool("HandUp", false);

        yield return new WaitForSecondsPaused(0.15f, GameControllerDodge.paused);
        ball.transform.parent = transform.parent;
        Rigidbody ballRigid = ball.GetComponent<Rigidbody>();
        ballRigid.isKinematic = false;
        ballRigid.useGravity = true;
        
        float accuracy = 0;
        switch(ballType) {
            case 0:
                accuracy = 0.5f;
                break;
            case 1:
                accuracy = 0.7f;
                break;
            case 2:
                accuracy = 1f;
                break;
        }
        Vector3 force = BallisticVel(ball.transform, accuracy);
        ballRigid.AddForce(force, ForceMode.Impulse);
        isThrowing = false;
        
        for(int i = 0; i < 10; i++) {
            bool isIn = true;
            yield return new WaitForSecondsPaused(1f, GameControllerDodge.paused);
            if(isIn && !gameControllerDodge.area.Contains(ball.transform.position)) {
                isIn = false;
                ball.tag = "Untagged";
                StartCoroutine(ChangeBallColor(ball));
            }
        }
        
        Physics.IgnoreCollision(ballCollider, gameControllerDodge.floorCollider);
        yield return new WaitUntil(() => ball.transform.position.y < -0.5f);
        thrownBalls.Remove(ball);
        Destroy(ball);
    }

    IEnumerator ChangeBallColor(GameObject ball) {
        Renderer ballRenderer = ball.GetComponent<Renderer>();
        Color startColor = ballRenderer.material.color;
        Color endColor = GetComponentInChildren<Renderer>().material.color;
        for(float f = 0; f <= 1f && ball != null; f += 1 / 60f) {
            ballRenderer.material.color = Color.Lerp(startColor, endColor, f);
            yield return new WaitForSeconds(1f / 60f);
        }
    }

    Vector3 BallisticVel(Transform ball, float accuracy) {
        Vector3 dir = player.position - ball.position;

        float areaMinX = Mathf.Min(player.position.x, (ball.position + accuracy * dir).x);
        float areaMaxX = Mathf.Max(player.position.x, (ball.position + accuracy * dir).x);
        float areaMinZ = Mathf.Min(player.position.z, (ball.position + accuracy * dir).z);
        float areaMaxZ = Mathf.Max(player.position.z, (ball.position + accuracy * dir).z);
        Vector3 targetPoint = new Vector3(Random.Range(areaMinX, areaMaxX), 1f, Random.Range(areaMinZ, areaMaxZ));
        
        dir = targetPoint - ball.position;
        float h = dir.y;
        dir.y = 0;

        float dist = dir.magnitude;
        float a = 45 * Mathf.Deg2Rad; 
        dir.y = dist * Mathf.Tan(a);
        dist += h / Mathf.Tan(a);

        return (Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * a))) * dir.normalized;
    }

    public void Reset() {
        StopAllCoroutines();
        animator.SetBool("HandUp", false);
        animator.SetTrigger("Reset");
        thrownBalls.ForEach(b => Destroy(b));
        thrownBalls = new List<GameObject>();
    }

    public void Freeze() {
        if(inFreeze) return;
        thrownBalls.ForEach(t => t.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll);
    }

    public void Unfreeze() {
        if(!inFreeze) return;
        thrownBalls.ForEach(t => t.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None);
    }

    public void Animation(string triggerName) {
        animator.SetTrigger(triggerName);
    }
}
}