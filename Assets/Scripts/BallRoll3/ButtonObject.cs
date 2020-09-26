using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallRoll3 {
public class ButtonObject : MonoBehaviour {

	public BossCutscenes bossCutscenes;
	public int nextPhase = 2;

	AudioSource audioSource;
	Animator animator;
	bool activated = false;

	void Start () {
		audioSource = GetComponent<AudioSource>();
		animator = GetComponentInParent<Animator>();
	}
	
	void OnTriggerEnter (Collider other) {
		if(!activated && other.CompareTag("Player")) {
			activated = true;
			if(animator != null) animator.SetTrigger("Press");
			if(audioSource != null) audioSource.PlayDelayed(1f);
			switch(nextPhase) {
				case 2:
					bossCutscenes.Phase2();
					break;
				case 3:
					bossCutscenes.Phase3();
					break;
				case 4:
					bossCutscenes.Phase3b();
					break;
				case 5:
					bossCutscenes.PhaseEnd();
					break;
				case 6:
					bossCutscenes.PhaseEnd2();
					break;
			}
		}
	}
}
}