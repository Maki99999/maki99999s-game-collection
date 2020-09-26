using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SportsGame {
public class GameControllerDodge : GameController {

    public GameObject throwerPrefab;
    public Transform npcs;
    public Collider[] ballColliderToIgnore;
    public Collider floorCollider;

    [Space(10)]
    public float npcOnOneSiteCount;
    public float maxRandomOffset;
    public float spaceFromCorner;
    public float spaceFromEdge;

    [Space(10)]
    public float minTimeBetwThrow;
    public float maxTimeBetwThrow;

    List<Thrower> throwers;
    
    void Start() {
        mainController = GameObject.FindWithTag("GameController").GetComponent<MainController>();
        throwers = new List<Thrower>();
        SpawnNpcs();
    }

    void Update() {
        if(inGame && !paused.Value && (Input.GetAxisRaw("Cancel") > 0 || Input.GetKeyDown(KeyCode.Q))) {
            paused.Value = true;
            mainController.StartCoroutine(mainController.Pause());
            throwers.ForEach(t => t.Freeze());
        }
        else if(inGame) {
            throwers.ForEach(t => t.Unfreeze());
        }
    }
    
    void SpawnNpcs() {
        float distBetwNpcs = (area.size.x - spaceFromCorner * 2) / npcOnOneSiteCount;
        float remains = distBetwNpcs - (int) distBetwNpcs;
        distBetwNpcs -= remains;
        remains += spaceFromCorner;

        for(float f = area.min.x + remains / 2; f <= area.max.x - remains / 2; f += distBetwNpcs) {
            throwers.Add(Instantiate(throwerPrefab, 
                new Vector3(f + Random.Range(-maxRandomOffset, maxRandomOffset), 0, area.max.z + spaceFromEdge), 
                Quaternion.Euler(0, 180, 0), npcs).GetComponent<Thrower>());

            throwers.Add(Instantiate(throwerPrefab, 
                new Vector3(f + Random.Range(-maxRandomOffset, maxRandomOffset), 0, area.min.z - spaceFromEdge), 
                Quaternion.Euler(0, 0, 0),   npcs).GetComponent<Thrower>());
        }

        for(float f = area.min.z + remains / 2; f <= area.max.z - remains / 2; f += distBetwNpcs) {
            throwers.Add(Instantiate(throwerPrefab, 
                new Vector3(area.max.x + spaceFromEdge, 0, f + Random.Range(-maxRandomOffset, maxRandomOffset)), 
                Quaternion.Euler(0, 270, 0), npcs).GetComponent<Thrower>());
            throwers.Add(Instantiate(throwerPrefab, 
                new Vector3(area.min.x - spaceFromEdge, 0, f + Random.Range(-maxRandomOffset, maxRandomOffset)), 
                Quaternion.Euler(0, 90, 0),  npcs).GetComponent<Thrower>());
        }
    }
    
    IEnumerator Game() {
        yield return new WaitForSeconds(5f);
        inGame = true;
        mainController.StartScoreTime();

        for(int i = 0; i < 100; i++) {
            throwers[Random.Range(0, throwers.Count - 1)].Throw();
            float pow = Mathf.Pow(1.028f, i);
            yield return new WaitForSecondsPaused(Random.Range(minTimeBetwThrow / pow, maxTimeBetwThrow / pow), paused);
        }

        Win();
    }

    public void ThrowABall() {
        throwers[Random.Range(0, throwers.Count - 1)].Throw();
    }

    public override void Init() {
        playerInitPos = player.transform.position;
        playerInitRot = player.transform.rotation;
        throwers.ForEach(t => t.Reset());
        StartCoroutine(Game());
    }

    public override void Reset() {
        inGame = false;
        player.transform.position = playerInitPos;
        player.transform.rotation = playerInitRot;
        throwers.ForEach(t => t.Reset());
        StopAllCoroutines();
        paused.Value = false;
    }

    void Win() {
        if(inGame) {
            mainController.EndScoreTime();
            inGame = false;
            paused.Value = true;
            throwers.ForEach(t => t.Animation("Loose"));
            mainController.StartCoroutine(mainController.WinAnimation(playerCam));
            StartCoroutine(LongReset());
        }
    }
    
    public override void Loose() {
        if(inGame) {
            mainController.EndScoreTime();
            inGame = false;
            paused.Value = true;
            throwers.ForEach(t => t.Animation("Win"));
            mainController.StartCoroutine(mainController.LooseAnimation(playerCam));
            StartCoroutine(LongReset());
        }
    }

    public override void Hack() {
        Win();
    }

    IEnumerator LongReset() {
        yield return new WaitForSeconds(9f);
        Reset();
    }
}
}