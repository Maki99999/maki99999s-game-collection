using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MakiSupermarket
{
    public class AutomaticDoorAntiPlayer : MonoBehaviour
    {
        public PlayerController playerController;
        public DialogueManager dialogueManager;
        public Transform pushPosition;

        bool triggered = false;

        private void OnTriggerEnter(Collider other)
        {
            if (!triggered && other.CompareTag("Player"))
            {
                triggered = true;
                StartCoroutine(PushPlayerAway());
            }
        }

        IEnumerator PushPlayerAway()
        {
            playerController.SetCanMove(false);
            yield return dialogueManager.StartDialogue(Dialogue.AutoDoorAntiPlayerDialogue());
            yield return playerController.MovePlayer(pushPosition);
            playerController.SetCanMove(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (triggered && other.CompareTag("Player"))
                triggered = false;
        }
    }
}