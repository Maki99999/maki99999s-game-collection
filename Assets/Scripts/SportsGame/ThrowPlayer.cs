using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SportsGame {
public class ThrowPlayer : MonoBehaviour {

    public GameControllerThrow gameControllerThrow;
    public GameObject ballPrefab;
    public Transform ballPos0;
    public Transform ballPos1;
    public Transform npcs;
    public Transform cam;
    public float reloadTime;

    LineRenderer lineRenderer;
    bool isShooting = false;
    bool isHoldingBall = false;
    float throwForce = 0f;
    GameObject ballInHand;

    Vector3 initBallPos0;
    Vector3 initBallPos1;
    
    void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        initBallPos1 = ballPos1.localPosition;
        initBallPos0 = ballPos0.localPosition;
        StartCoroutine(Reload());
    }

    void Update() {
        if(!isShooting && isHoldingBall && Input.GetAxis("Primary Fire") > 0) {
            isShooting = true;
            StartCoroutine(Throw());
        }
    }

    void FixedUpdate() {
        if(isShooting) {
            Vector3 last_pos = ballPos1.position;
            Vector3 velocity = cam.forward * (6 + 6 * throwForce) + cam.up * 5 * throwForce;

            lineRenderer.positionCount = 1;
            lineRenderer.SetPosition(0, ballPos1.position);

            for(int i = 1; i < 150; i++) {
                velocity += Physics.gravity * Time.fixedDeltaTime;
                RaycastHit hit;
                if(Physics.Linecast(last_pos, (last_pos + (velocity * Time.fixedDeltaTime)), out hit)){
                    if(hit.collider.CompareTag("Solid")) break;
                }
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(i, last_pos);
                last_pos += velocity * Time.fixedDeltaTime;
            }
        } else {
            lineRenderer.positionCount = 1;
        }
    }

    public IEnumerator Reload() {
        if(gameControllerThrow.RemoveBall()) {
            ballPos1.localPosition = initBallPos0;
            ballInHand = Instantiate(ballPrefab, ballPos1.position, Quaternion.Euler(0, 0, 0), ballPos1);
            gameControllerThrow.thrownBalls.Add(ballInHand);

            for(float f = 0f; f <= 1f; f += 1 / (60f * reloadTime)) {
                ballPos1.localPosition = Vector3.Lerp(initBallPos0, initBallPos1, f);
                yield return new WaitForSeconds(1f / 60f);
            }
            isHoldingBall = true;
        }
    }

    IEnumerator Throw() {
        GameObject ball = ballInHand;

        Collider ballCollider = ball.GetComponent<Collider>();
        foreach (Collider collider in gameControllerThrow.ballColliderToIgnore) {
            Physics.IgnoreCollision(ballCollider, collider);
        }

        throwForce = 0f;
        while(Input.GetAxis("Primary Fire") > 0) {
            throwForce = Mathf.Min(1f, throwForce + 0.01f);
            ballPos1.localPosition = initBallPos1 + throwForce * Vector3.up * 0.5f;
            yield return new WaitForSeconds(1f / 60f);
        }
        ball.transform.parent = npcs;
        Rigidbody ballRigid = ball.GetComponent<Rigidbody>();
        ballRigid.isKinematic = false;
        ballRigid.useGravity = true;
        
        Vector3 force = cam.forward * (6 + 6 * throwForce) + cam.up * 5 * throwForce;
        ballRigid.velocity = force;

        isShooting = false;
        isHoldingBall = false;
        StartCoroutine(Reload());
        ballPos1.localPosition = initBallPos1;
        
        yield return new WaitForSecondsPaused(1f, GameControllerDodge.paused);
        bool isIn = true;
        for(int i = 0; i < 20; i++) {
            yield return new WaitForSecondsPaused(0.5f, GameControllerDodge.paused);
            if(isIn && ball != null && !gameControllerThrow.area.Contains(ball.transform.position)) {
                isIn = false;
                ball.tag = "Untagged";
                StartCoroutine(ChangeBallColor(ball));
            }
        }
        if(ball != null) {
            Physics.IgnoreCollision(ballCollider, gameControllerThrow.floorCollider);
            yield return new WaitUntil(() => ball.transform.position.y < -0.5f);
            gameControllerThrow.thrownBalls.Remove(ball);
            Destroy(ball);
        }
    }

    IEnumerator ChangeBallColor(GameObject ball) {
        Renderer ballRenderer = ball.GetComponent<Renderer>();
        Color startColor = ballRenderer.material.color;
        Color endColor = new Color(0.3018868f, 0.3018868f, 0.3018868f);
        for(float f = 0; f <= 1f && ball != null; f += 1 / 60f) {
            ballRenderer.material.color = Color.Lerp(startColor, endColor, f);
            yield return new WaitForSeconds(1f / 60f);
        }
    }
}
}