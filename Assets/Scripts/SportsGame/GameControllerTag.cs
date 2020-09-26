using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SportsGame {
public class GameControllerTag : GameController {

    public Animator hallDoor;

    [Space(10)]
    
    public NPCSpawner npcSpawner;
    public float minInfectTime;
    public float maxInfectTime;

    TimeBank timeBank;
    
    
    void Start() {
        mainController = GameObject.FindWithTag("GameController").GetComponent<MainController>();
    }

    void Update() {
        if(inGame && !paused.Value && (Input.GetAxisRaw("Cancel") > 0 || Input.GetKeyDown(KeyCode.Q))) {
            paused.Value = true;
            mainController.StartCoroutine(mainController.Pause());
        }
    }

    public override void Init() {
        playerInitPos = player.transform.position;
        playerInitRot = player.transform.rotation;
        timeBank = new TimeBank(2 * npcSpawner.countOfAllNpcs - 1 + 10, 600f, minInfectTime, maxInfectTime);
        StartCoroutine(Game());
    }

    IEnumerator Game() {
        //Open Door
        hallDoor.SetBool("Open", true);
        yield return new WaitForSeconds(2.5f);
        npcSpawner.Init();
        yield return new WaitForSecondsPaused(npcSpawner.delayBetweenSpawns * npcSpawner.countOfAllNpcs, paused);
        mainController.StartScoreTime();
        yield return new WaitForSecondsPaused(5f, paused);
        hallDoor.SetBool("Open", false);

        //Infect People
        do {
            yield return new WaitForSecondsPaused(timeBank.GetRandomTime(), paused);
        } while(npcSpawner.InfectOne());

        //Make Zombies faster
        for(float i = 0.015f; i <= 0.05; i += 0.005f) {
            yield return new WaitForSecondsPaused(3f, paused);
            npcSpawner.SetSpeedZombie(i);
        }

        //Make Chaser faster
        yield return new WaitForSecondsPaused(timeBank.GetRandomTime(), paused);
        npcSpawner.SetSpeedChaser(0.1f);

        //Turn Zombies into Chaser
        do {
            yield return new WaitForSecondsPaused(timeBank.GetRandomTime(), paused);
        } while(npcSpawner.ZombieToChaser());

        //Even faster
        for(int i = 0; i < 10; i++) {
            yield return new WaitForSecondsPaused(timeBank.GetRandomTime(), paused);
            npcSpawner.SetSpeedChaser(0.1f + ((i + 1) / 100));
        }

        Win();
    }
    
    void Win() {
        if(inGame) {
            mainController.EndScoreTime();
            inGame = false;
            paused.Value = true;
            npcSpawner.Animation("Loose");
            mainController.StartCoroutine(mainController.WinAnimation(playerCam));
            StartCoroutine(LongReset());
        }
    }

    public override void Loose() {
        if(inGame) {
            mainController.EndScoreTime();
            inGame = false;
            paused.Value = true;
            npcSpawner.Animation("Win");
            mainController.StartCoroutine(mainController.LooseAnimation(playerCam));
            StartCoroutine(LongReset());
        }
    }

    public override void Reset() {
        StartCoroutine(ShortReset());
    }

    IEnumerator ShortReset() {
        inGame = false;
        player.transform.position = playerInitPos;
        player.transform.rotation = playerInitRot;
		yield return MainController.MoveObject(transform, transform.position - Vector3.up * 10, transform.rotation, 60);
        npcSpawner.Reset();
        hallDoor.SetBool("Open", false);
        transform.position = Vector3.zero;
        StopAllCoroutines();
        paused.Value = false;
    }

    IEnumerator LongReset() {
        player.transform.position = playerInitPos;
        player.transform.rotation = playerInitRot;
        yield return new WaitForSeconds(9f);
		yield return MainController.MoveObject(transform, transform.position - Vector3.up * 10, transform.rotation, 60);
        yield return new WaitForSeconds(1f);
        npcSpawner.Reset();
        hallDoor.SetBool("Open", false);
        transform.position = Vector3.zero;
        StopAllCoroutines();
        paused.Value = false;
    }

    public override void Hack() {
        Win();
    }
}
}