using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MakiSupermarket
{
    public class Cutscene01Start : MonoBehaviour, Rideable, Pausing
    {
        public PlayerController playerController;
        public Animator personCutsceneAnimator;
        public Animator personAnimator;
        public AutomaticDoor autoDoor;
        public Animator crossAnimator;

        public AudioSource footstepAudio;
        public AudioClip[] footstepAudioClips;
        public AudioSource carAudio;

        public DialogueManager dialogueManager;
        public Vector3 playerStartPosition;
        public Transform paperLook;

        bool camFollowGuy = true;

        Transform playerTransform;
        Transform personTransform;

        void Start()
        {
            playerTransform = playerController.transform;
            playerTransform.position = playerStartPosition;
            personTransform = personAnimator.transform;
            StartCoroutine(StartCutscene());
        }

        IEnumerator StartCutscene()
        {
            yield return null;
            camFollowGuy = true;
            crossAnimator.SetBool("On", false);
            playerController.Ride(transform);
            personCutsceneAnimator.Play("CutsceneStartPerson", 0, .25f);
            yield return WaitAndMakeFootstepSounds(10f * .75f);
            personAnimator.SetBool("Stand", true);

            //Dialogue
            yield return dialogueManager.StartDialogue(Dialogue.Cutscene01StartDialogue());

            //GoAwayPerson
            personAnimator.SetBool("Stand", false);
            personCutsceneAnimator.SetTrigger("GoAway");
            yield return WaitAndMakeFootstepSounds(1.5f);
            autoDoor.ForceOpen(true);
            yield return WaitAndMakeFootstepSounds(1.5f);
            autoDoor.ForceOpen(false);
            yield return new WaitForSecondsPaused(3f, PauseManager.isPaused());
            carAudio.Play();
            camFollowGuy = false;
            yield return playerController.RotatePlayer(Quaternion.LookRotation(paperLook.position - playerController.camTransform.position), 50);

            crossAnimator.SetBool("On", true);
            playerController.UnRide();
        }

        IEnumerator WaitAndMakeFootstepSounds(float seconds)
        {
            float timeBetweenSteps = 0.775f;
            
            for(float f = 0; f < seconds; f += timeBetweenSteps)
            {
                yield return new WaitForSecondsPaused(timeBetweenSteps, PauseManager.isPaused());
                footstepAudio.clip = footstepAudioClips[Random.Range(0, footstepAudioClips.Length)];
                footstepAudio.pitch = 1f - Random.Range(-.1f, .1f);
                footstepAudio.Play();
            }
        }

        bool Rideable.Move(MoveData inputs)
        {
            if(camFollowGuy) {
                playerTransform.LookAt(personTransform);
                playerTransform.rotation = Quaternion.Euler(0, playerTransform.eulerAngles.y, 0);
            }
            return false;
        }

        void Pausing.Pause()
        {
            personCutsceneAnimator.enabled = false;
            personAnimator.enabled = false;
            autoDoor.anim.enabled = false;
        }

        void Pausing.UnPause()
        {
            personCutsceneAnimator.enabled = true;
            personAnimator.enabled = true;
            autoDoor.anim.enabled = true;
        }
    }
}