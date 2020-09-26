using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MakiSupermarket
{
    public class Cutscene02Start : MonoBehaviour, Rideable, Pausing
    {
        public PlayerController playerController;
        public Animator crossAnimator;
        public AudioSource audioSource;
        public Animator animator;

        public DialogueManager dialogueManager;
        public Vector3 playerStartPosition;

        Transform playerTransform;

        void Start()
        {
            playerTransform = playerController.transform;
            playerTransform.position = playerStartPosition;
            playerTransform.eulerAngles = Vector3.zero;
            StartCoroutine(StartCutscene());
        }

        IEnumerator StartCutscene()
        {
            yield return null;
            animator.SetTrigger("Go");
            playerController.Ride(transform);

            yield return new WaitForSecondsPaused(1f, PauseManager.isPaused());
            audioSource.Play();
            yield return new WaitForSecondsPaused(5f, PauseManager.isPaused());

            crossAnimator.SetBool("On", false);
            yield return dialogueManager.StartDialogue(Dialogue.Cutscene02StartDialogue());
            crossAnimator.SetBool("On", true);

            playerController.UnRide();
        }

        bool Rideable.Move(MoveData inputs)
        {
            return false;
        }

        void Pausing.Pause()
        {
            animator.enabled = false;
        }

        void Pausing.UnPause()
        {
            animator.enabled = true;
        }
    }
}