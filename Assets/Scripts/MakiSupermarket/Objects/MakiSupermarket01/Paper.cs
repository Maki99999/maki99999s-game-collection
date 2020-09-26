using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MakiSupermarket
{
    public class Paper : MonoBehaviour, Useable
    {
        public cakeslice.Outline outline;
        public PlayerController playerController;
        public ProgressionController01 progressionController;
        public DialogueManager dialogueManager;
        public Transform paperTransform;
        public Transform paperHoldPosition;
        public AudioSource audioSource;

        bool activated = false;
        bool canPutAway = false;

        void Update()
        {
            outline.enabled = false;

            if (canPutAway && activated && (Input.GetKeyDown(GlobalSettings.keyUse) || Input.GetKeyDown(GlobalSettings.keyUse2) || GlobalSettings.Confirm()))
            {
                activated = false;
                canPutAway = false;
                StartCoroutine(MovePaperBack());
            }

        }

        void Useable.LookingAt()
        {
            if (!activated)
                outline.enabled = true;
        }

        void Useable.Use()
        {
            activated = true;
            StartCoroutine(MovePaper());
        }

        IEnumerator MovePaper()
        {
            playerController.SetCanMove(false);
            audioSource.Play();
            yield return Things.PosRotLerp(paperTransform, paperHoldPosition, 40);
            yield return dialogueManager.StartDialogue(Dialogue.OneLineMonologue(progressionController.GetHint()));
            canPutAway = true;
        }

        IEnumerator MovePaperBack()
        {
            yield return Things.PosRotLerp(paperTransform, transform, 40);
            playerController.SetCanMove(true);
        }
    }
}