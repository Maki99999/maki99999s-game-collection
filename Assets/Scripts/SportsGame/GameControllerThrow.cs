using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SportsGame {
public class GameControllerThrow : GameController {
    
    public Collider[] ballColliderToIgnore;
    public Collider floorCollider;
    public Transform npcs;
    public GameObject dodgerPrefab;
    public Animator hallDoor;
    public GameObject fakePrefab;

    public int enemyCount;
    int enemysHit = 0;
    public int ballsLeft;
    
    List<Dodger> dodger;
    public List<GameObject> thrownBalls;

    public void AddEnemysHit() {
        if(++enemysHit == enemyCount) {
            Win();
        }
    }

    public bool RemoveBall() {
        if(--ballsLeft == 0) {
            StartCoroutine(WaitForBalls());
            return false;
        }
        return true;
    }

    IEnumerator WaitForBalls() {
        yield return new WaitUntil(() => thrownBalls.Count == 0 || thrownBalls[thrownBalls.Count -1].CompareTag("Untagged"));
        Loose();
    }
    
    void Start() {
        mainController = GameObject.FindWithTag("GameController").GetComponent<MainController>();
        thrownBalls = new List<GameObject>();
    }

    void Update() {
        if(inGame && !paused.Value && (Input.GetAxisRaw("Cancel") > 0 || Input.GetKeyDown(KeyCode.Q))) {
            paused.Value = true;
            mainController.StartCoroutine(mainController.Pause());
            thrownBalls.ForEach(t => t.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll);
        }
        else if(inGame) {
            thrownBalls.ForEach(t => t.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None);
        }
    }

    public override void Init() {
        inGame = true;
        dodger = new List<Dodger>();
        StartCoroutine(InitAnim());
    }

    IEnumerator InitAnim() {
        hallDoor.SetBool("Open", true);
        yield return new WaitForSeconds(2.5f);
        SpawnNpcs();
        for(int i = 1; i < 10; i++) {
            Instantiate(fakePrefab, transform.position, transform.rotation, transform);
            yield return new WaitForSecondsPaused(1f, paused);
        }
        mainController.StartScoreTime();
        hallDoor.SetBool("Open", false);
    }

    void SpawnNpcs() {
		for(int i = 0; i < enemyCount; i++) {
            dodger.Add(Instantiate(dodgerPrefab, FakeController.RandomPointInBounds(area, 3f), Quaternion.Euler(0, Random.Range(0f, 360f), 0), npcs).GetComponent<Dodger>());
		}
    }

    public override void Reset() {
        StartCoroutine(ShortReset());
    }

    public override void Loose() {
        if(inGame) {
            mainController.EndScoreTime();
            inGame = false;
            paused.Value = true;
            dodger.ForEach((d) => d.SetAnimation("Win"));
            mainController.StartCoroutine(mainController.LooseAnimation(playerCam));
            StartCoroutine(LongReset());
        }
    }

    void Win() {
        if(inGame) {
            mainController.EndScoreTime();
            inGame = false;
            paused.Value = true;
            dodger.ForEach((d) => d.SetAnimation("Loose"));
            mainController.StartCoroutine(mainController.WinAnimation(playerCam));
            StartCoroutine(LongReset());
        }
    }

    IEnumerator ShortReset() {
        inGame = false;
        player.transform.position = playerInitPos;
        player.transform.rotation = playerInitRot;
		yield return MainController.MoveObject(transform, transform.position - Vector3.up * 10, transform.rotation, 60);
        thrownBalls.ForEach((d) => Destroy(d));
        dodger.ForEach((d) => Destroy(d));
        hallDoor.SetBool("Open", false);
        transform.position = Vector3.zero;
        StopAllCoroutines();
        paused.Value = false;
        StopAllCoroutines();
    }

    IEnumerator LongReset() {
        inGame = false;
        player.transform.position = playerInitPos;
        player.transform.rotation = playerInitRot;
        yield return new WaitForSeconds(9f);
		yield return MainController.MoveObject(transform, transform.position - Vector3.up * 10, transform.rotation, 60);
        yield return new WaitForSeconds(1f);
        thrownBalls.ForEach((d) => Destroy(d));
        dodger.ForEach((d) => Destroy(d));
        hallDoor.SetBool("Open", false);
        transform.position = Vector3.zero;
        StopAllCoroutines();
        paused.Value = false;
        StopAllCoroutines();
    }

    public override void Hack() {
        Win();
    }
}
}