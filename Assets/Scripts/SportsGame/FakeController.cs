using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SportsGame {
public class FakeController : MonoBehaviour {

    public GameObject fakeTag;
    public GameObject fakeDodge;
    public GameObject fakeParkour;
    public GameObject fakeThrow;
    
    [Space(20)]
    public Bounds tagArea;
    public GameObject tagNpcPrefab;
    public GameObject tagChaserPrefab;
    public GameObject zombiePrefab;
    public int tagNpcCount;
    public int tagChaserCount;
    public int zombieCount;
    
    [Space(20)]
    public GameControllerDodge gameControllerDodge;
    
    [Space(20)]
    public ParkourGhost fakeParkourPlayer;

    [Space(20)]
    public Bounds throwArea;
    public GameObject dodgerPrefab;
    public int dodgerCount;
    public Thrower fakeThrowPlayer;
    bool fakeThrowDirection = false;
    
    void Start() {
        SpawningTagNpcs();
        SpawningThrowNpcs();
        fakeThrow.SetActive(false);
    }

    void OnEnable() {
        StartCoroutine(FakeDodgeGame());
        StartCoroutine(FakeThrowGame());
        fakeParkourPlayer.StartWalk();
    }

    public IEnumerator Hide(GameType game) {
        GameObject go = null;
        switch(game) {
            case GameType.Tag:
                go = fakeTag;
                break;
            case GameType.Dodge:
                go = fakeDodge;
                break;
            case GameType.Parkour:
                go = fakeParkour;
                fakeParkourPlayer.StopWalk();
                break;
            case GameType.Throw:
                go = fakeThrow;
                break;
        }
        if(go.activeSelf) {
		    yield return MainController.MoveObject(go.transform, go.transform.position - Vector3.up * 5, go.transform.rotation, 60);
		    go.SetActive(false);
        }
    }

    IEnumerator HideDodge(bool unhide) {
        GameObject go = gameControllerDodge.gameObject;
        if(!unhide) {
		    yield return MainController.MoveObject(go.transform, go.transform.position - Vector3.up * 5, go.transform.rotation, 60);
		    go.SetActive(false);
        } else {
		    go.SetActive(true);
		    yield return MainController.MoveObject(go.transform, Vector3.zero, go.transform.rotation, 60);
        }
    }

    public IEnumerator ShowAll(bool dodgeOverThrow = true) {
        if(!fakeTag.activeSelf) {
		    fakeTag.SetActive(true);
		    yield return MainController.MoveObject(fakeTag.transform, Vector3.zero, fakeTag.transform.rotation, 60);
        }
        if(!fakeParkour.activeSelf) {
		    fakeParkour.SetActive(true);
            fakeParkourPlayer.StartWalk();
		    yield return MainController.MoveObject(fakeParkour.transform, Vector3.zero, fakeParkour.transform.rotation, 60);
        }
        if(dodgeOverThrow) {
            if(fakeThrow.activeSelf) {
                yield return Hide(GameType.Throw);
            }
            if(!fakeDodge.activeSelf) {
                StartCoroutine(HideDodge(true));
                fakeDodge.SetActive(true);
                yield return MainController.MoveObject(fakeDodge.transform, Vector3.zero, fakeDodge.transform.rotation, 60);
            }
        } else {
            if(fakeDodge.activeSelf) {
                StartCoroutine(HideDodge(false));
                yield return Hide(GameType.Dodge);
            }
            if(!fakeThrow.activeSelf) {
                fakeThrow.SetActive(true);
                yield return MainController.MoveObject(fakeThrow.transform, Vector3.zero, fakeThrow.transform.rotation, 60);
            }
        }
    }

    void SpawningTagNpcs() {
		for(int i = 0; i < tagNpcCount; i++) {
            Instantiate(tagNpcPrefab, RandomPointInBounds(tagArea, 3f), Quaternion.Euler(0, Random.Range(0f, 360f), 0), fakeTag.transform).GetComponent<TagNPCMove>();
		}
        for(int i = 0; i < tagChaserCount; i++) {
            Instantiate(tagChaserPrefab, RandomPointInBounds(tagArea, 3f), Quaternion.Euler(0, Random.Range(0f, 360f), 0), fakeTag.transform).GetComponent<TagNPCMove>();
		}
        for(int i = 0; i < zombieCount; i++) {
            Instantiate(zombiePrefab, RandomPointInBounds(tagArea, 3f), Quaternion.Euler(0, Random.Range(0f, 360f), 0), fakeTag.transform).GetComponent<TagNPCMove>();
		}
	}
    
    void SpawningThrowNpcs() {
		for(int i = 0; i < dodgerCount; i++) {
            Instantiate(dodgerPrefab, RandomPointInBounds(throwArea, 3f), Quaternion.Euler(0, Random.Range(0f, 360f), 0), fakeThrow.transform).GetComponent<TagNPCMove>();
		}
        fakeThrow.transform.position -= Vector3.up * 5;
    }

    IEnumerator FakeDodgeGame() {
        while(true) {
            yield return new WaitForSeconds(Random.Range(2, 5));
            if(!gameControllerDodge.inGame && fakeDodge.activeSelf) {
                gameControllerDodge.ThrowABall();
            }
        }
    }

    IEnumerator FakeThrowGame() {
        StartCoroutine(FakeThrowGamePlayer());
        while(true) {
            yield return new WaitForSeconds(Random.Range(2f, 5));
            if(fakeThrow.activeSelf) {
                fakeThrowPlayer.Throw();
            }
            fakeThrowDirection = Random.value > 0.5f;
        }
    }

    IEnumerator FakeThrowGamePlayer() {
        while(true) {
            if(fakeThrowDirection) {
                fakeThrowPlayer.transform.position = Vector3.MoveTowards(fakeThrowPlayer.transform.position, new Vector3(fakeThrowPlayer.transform.position.x, fakeThrowPlayer.transform.position.y, -46f), 1f * Time.deltaTime);
            } else {
                fakeThrowPlayer.transform.position = Vector3.MoveTowards(fakeThrowPlayer.transform.position, new Vector3(fakeThrowPlayer.transform.position.x, fakeThrowPlayer.transform.position.y, -29f), 1f * Time.deltaTime);
            }
            yield return null;
        }
    }

    public static Vector3 RandomPointInBounds(Bounds bounds, float scope) {
        return new Vector3(
            Random.Range(bounds.min.x + scope, bounds.max.x - scope),
            0,
            Random.Range(bounds.min.z + scope, bounds.max.z - scope)
        );
    }
}
}