using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SportsGame {
public class NPCSpawner : MonoBehaviour {

    public GameObject tagNpcPrefab;
    public GameObject tagChaserPrefab;
    public int countOfAllNpcs;
    public float delayBetweenSpawns;
    public GameObject infectParticlePrefab;

    List<TagNPCMove> npcs;
    List<TagChaserMove> chaser;
    List<TagChaserMove> zombies;

    public void Init() {
        npcs = new List<TagNPCMove>();
        chaser = new List<TagChaserMove>();
        zombies = new List<TagChaserMove>();
        StartCoroutine(Spawning());
    }

    IEnumerator Spawning() {
		for(int i = 0; i < countOfAllNpcs - 1; i++) {
            npcs.Add(Instantiate(tagNpcPrefab, transform.position, transform.rotation, transform).GetComponent<TagNPCMove>());
			yield return new WaitForSecondsPaused(delayBetweenSpawns, GameControllerTag.paused);
		}
		chaser.Add(Instantiate(tagChaserPrefab, transform.position, transform.rotation, transform).GetComponent<TagChaserMove>());
	}

    public bool InfectOne() {
        if(npcs.Count <= 0) {
            return false;
        }
        TagNPCMove victim = npcs[Random.Range(0, npcs.Count - 1)];
        npcs.Remove(victim);
        
        StartCoroutine(InfectAnimation(victim));

        return true;
    }

    public void SetSpeedZombie(float speed) {
        zombies.ForEach((z) => z.walkSpeed = speed);
    }

    public void SetSpeedChaser(float speed) {
        chaser.ForEach((c) => c.walkSpeed = speed);
    }

    public bool ZombieToChaser() {
        if(zombies.Count <= 0) {
            return false;
        }
        TagChaserMove next = zombies[Random.Range(0, zombies.Count - 1)];
        zombies.Remove(next);

        StartCoroutine(ZombieToChaser(next));

        return true;
    }
    
    IEnumerator InfectAnimation(TagNPCMove victim) {
        Instantiate(infectParticlePrefab, victim.transform.position, Quaternion.Euler(-90, 0, 0), victim.transform);
        for(int i = 10; i >= 1; i--) {
            victim.walkSpeed = (0.008f) * i;
           yield return new WaitForSeconds(1.7f / 10);
        }
        zombies.Add(victim.Infect());
    }

    IEnumerator ZombieToChaser(TagChaserMove next) {
        Instantiate(infectParticlePrefab, next.transform.position, Quaternion.Euler(-90, 0, 0), next.transform);
        yield return new WaitForSeconds(1.7f);

        chaser.Add(Instantiate(chaser[0], next.transform.position, next.transform.rotation, transform).GetComponent<TagChaserMove>());
		next.transform.GetChild(next.transform.childCount - 1).SetParent(chaser[chaser.Count -  1].transform);
        Destroy(next.gameObject);
    }

    public void Animation(string triggerName) {
        npcs.ForEach(z => z.SetAnimation(triggerName));
        chaser.ForEach(z => z.SetAnimation(triggerName));
        zombies.ForEach(z => z.SetAnimation(triggerName));
    }

    public void Reset() {
        StopAllCoroutines();
        npcs.ForEach(z => Destroy(z.gameObject));
        chaser.ForEach(z => Destroy(z.gameObject));
        zombies.ForEach(z => Destroy(z.gameObject));
    }
}
}