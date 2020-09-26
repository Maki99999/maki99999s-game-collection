using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MakiSupermarket
{
    public class Door : MonoBehaviour, Useable
    {
        public Animator anim;
        public Animator animHandle;
        public cakeslice.Outline outline;
        public AudioSource audioSource;
        public AudioClip audioClipOpen;
        public AudioClip audioClipClose;
        [Space(10)]
        public PlayerController playerController;
        public DialogueManager dialogueManager;
        [Space(10)]
        public AudioClip audioClipLocked;
        public bool noReason = false;
        public bool locked = false;
        public string lockedMessage = "§iLocked...";

        bool open = false;

        DialogueNode lockedDialogue;

        void Start()
        {
            lockedDialogue = new DialogueNode();
            lockedDialogue.Add("You", lockedMessage);
        }

        void Update()
        {
            outline.enabled = false;
        }

        void Useable.Use()
        {
            if (noReason)
            {
                if (!lockedMessage.Equals(""))
                    StartCoroutine(LockedDialogue());

            }
            else if (locked)
            {
                animHandle.SetTrigger("Rattle");
                if (!lockedMessage.Equals(""))
                {
                    StartCoroutine(LockedDialogue());
                    audioSource.clip = audioClipLocked;
                    audioSource.Play();
                }
            }
            else
            {
                open = !open;
                anim.SetBool("Open", open);
                animHandle.SetTrigger("Open");

                if (open)
                {
                    audioSource.clip = audioClipOpen;
                    audioSource.Play();
                }
                else
                {
                    audioSource.clip = audioClipClose;
                    audioSource.PlayDelayed(.8f);
                }
            }
        }

        IEnumerator LockedDialogue()
        {
            if (playerController != null)
            {
                playerController.SetCanMove(false);
                yield return dialogueManager.StartDialogue(lockedDialogue);
                playerController.SetCanMove(true);
            }
        }

        void Useable.LookingAt()
        {
            outline.enabled = true;
        }

        void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (playerController != null)
                    playerController.charController.Move(transform.right * Time.deltaTime);
            }
        }
    }
}