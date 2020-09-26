using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SportsGame {
public class GameControllerParkour : GameController {

    public List<ParkourCheckpoint> checkpoints;
    int totalCheckpointCount;
    public int currentCheckpointCount;
    public GameObject barriers;
    public Transform savePlayerPosition;
    public Collider floor;
    public ParkourGhost parkourGhost;
    float parkourGhostInitSpeed;
    int currentGhostCheckpointCount;
    float currentDistance;

    CharacterController playerController;

    void Start() {
        mainController = GameObject.FindWithTag("GameController").GetComponent<MainController>();
        totalCheckpointCount = checkpoints.Count;
        playerController = player.GetComponent<CharacterController>();
        Physics.IgnoreCollision(player.GetComponent<CharacterController>(), floor);
        parkourGhostInitSpeed = parkourGhost.moveSpeed;
        playerInitPos = player.transform.position;
        playerInitRot = player.transform.rotation;
    }

    void Update() {
        if(inGame) {
            if(!paused.Value && (Input.GetAxisRaw("Cancel") > 0 || Input.GetKeyDown(KeyCode.Q))) {
                paused.Value = true;
                mainController.StartCoroutine(mainController.Pause());
            }

            UpdateDistance();
        }
    }

    void LateUpdate() {
        if(playerController.isGrounded) {
            savePlayerPosition.position = player.transform.position;
            savePlayerPosition.rotation = player.transform.rotation;
        }
        if((parkourGhost.transform.position - player.transform.position).magnitude > 5f) {
            parkourGhost.moveSpeed = parkourGhostInitSpeed * 1.3f;
        } else {
            parkourGhost.moveSpeed = parkourGhostInitSpeed;
        }
    }

    void UpdateDistance() {
        if(currentCheckpointCount > 0)
            mainController.SetCustomScore(currentDistance + 
                    (checkpoints[currentCheckpointCount - 1].transform.position - player.transform.position).magnitude, "m");
    }

    public void AddCp() {
        currentCheckpointCount++;
        if(currentCheckpointCount > 1)
            currentDistance += (checkpoints[currentCheckpointCount - 1].transform.position - checkpoints[currentCheckpointCount - 2].transform.position).magnitude;
    }

    public void AddGhostCp() {
        currentGhostCheckpointCount++;
        if(inGame && currentGhostCheckpointCount > currentCheckpointCount) {
            Loose();
        }
    }

    public override void Init() {
        mainController.StartScoreTime();
        barriers.SetActive(true);
        parkourGhost.StartWalk();
        player.transform.position = playerInitPos;
        player.transform.rotation = playerInitRot;
        checkpoints.ForEach(c => c.activated = false);
        checkpoints.ForEach(c => c.ghostActivated = false);
        currentCheckpointCount = 0;
        currentGhostCheckpointCount = 0;
        inGame = true;
        currentDistance = 0f;
    }

    public void CorrectIllegalPosition() {
        player.transform.position = savePlayerPosition.position;
        player.transform.rotation = savePlayerPosition.rotation;
    }

    public override void Reset() {
        inGame = false;
        paused.Value = true;
        parkourGhost.StopWalk();
        paused.Value = false;
        barriers.SetActive(false);
    }

    void Win() {
        if(inGame) {
            mainController.EndScoreTime();
            Reset();
            mainController.StartCoroutine(mainController.WinAnimation(playerCam));
        }
    }

    public void hasWon() {
        if(currentCheckpointCount >= totalCheckpointCount) {
            Win();
        }
    }

    public override void Loose() {
        if(inGame) {
            mainController.SetCustomScore(currentDistance + 
                    (checkpoints[currentCheckpointCount - 1].transform.position - player.transform.position).magnitude, "m");
            mainController.EndScoreTime(false);
            Reset();
            mainController.StartCoroutine(mainController.LooseAnimation(playerCam));
        }
    }

    public override void Hack() {
        Win();
    }

    void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Deadly")) {
            CorrectIllegalPosition();
        }
    }
}
}