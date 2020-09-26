using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MakiSupermarket
{
    public class CogWheel : UseableOutline, ItemObserver
    {
        public PlayerController playerController;
        public DialogueManager dialogueManager;
        public Animator cogWheelAnim;
        public AudioSource audioSource;
        public Transform itemPositionStart;
        public Transform item;
        public Collider itemCollider;

        [Space(10)]
        public GameObject expandingIsland;
        public GameObject expandingIslandLamp;
        public Transform lever;
        public AudioClip lampAudio;
        public AudioClip leverAudio;
        public AudioClip CogWheelsAudio;

        bool firstTime = true;
        bool playerHasLever = false;
        bool leverAttached = false;

        public override void Use()
        {
            if (firstTime)
            {
                firstTime = false;
                playerController.itemObservers.Add(this);

                StartCoroutine(LookToLever());
            }
            else if (playerHasLever)
            {
                playerController.RemoveCollectedItem("CogWheelLever");
                StartCoroutine(AddLever());
            }
            else if (leverAttached)
            {
                leverAttached = false;

                foreach (cakeslice.Outline outline in outlines)
                    outline.enabled = false;
                outlines = new List<cakeslice.Outline>();

                StartCoroutine(TurnWheel());
            }
        }

        public IEnumerator TurnWheel()
        {
            playerController.SetCanMove(false);

            audioSource.clip = CogWheelsAudio;
            audioSource.loop = true;
            cogWheelAnim.SetTrigger("Turn");
            audioSource.Play();

            yield return new WaitForSeconds(10f);
            audioSource.Stop();
            playerController.SetCanMove(true);
        }
        public IEnumerator LookToLever()
        {
            playerController.SetCanMove(false);

            yield return dialogueManager.StartDialogue(Dialogue.OneLineMonologue("I need something to turn this..."));

            Transform expandingIslandTransform = expandingIsland.transform;
            expandingIslandTransform.position = expandingIslandTransform.position - Vector3.up * 10;
            expandingIsland.SetActive(true);

            StartCoroutine(playerController.ForceLookPlayer(lever, 100));

            Vector3 oldPos = expandingIslandTransform.position;
            Vector3 newPos = oldPos - Vector3.up * oldPos.y;

            for (float f = 0; f < 1f; f += 1f / 100)
            {
                expandingIslandTransform.position = Vector3.Lerp(oldPos, newPos, Mathf.SmoothStep(0, 1, f));
                yield return new WaitForSeconds(1f / 60f);
            }
            expandingIslandTransform.position = newPos;

            yield return new WaitForSeconds(.3f);
            expandingIslandLamp.SetActive(true);
            audioSource.clip = lampAudio;
            audioSource.Play();

            playerController.SetCanMove(true);
        }

        void ItemObserver.UpdateItemStatus(string itemName, bool status)
        {
            if (itemName.Equals("CogWheelLever"))
                playerHasLever = status;
        }

        IEnumerator AddLever()
        {
            Vector3 origPos = item.position;
            Quaternion origRot = item.rotation;

            item.position = itemPositionStart.position;
            item.rotation = itemPositionStart.rotation;
            item.gameObject.SetActive(true);

            bool playedAudio = false;
            for (float f = 0; f < 1; f += 1f / 40f)
            {
                item.position = Vector3.Lerp(itemPositionStart.position, origPos, f);
                item.rotation = Quaternion.Lerp(itemPositionStart.rotation, origRot, f);
                if (!playedAudio && f > 0.8f)
                {
                    playedAudio = true;
                    audioSource.clip = leverAudio;
                    audioSource.Play();
                }
                yield return new WaitForSeconds(1f / 60f);
            }

            item.position = origPos;
            item.rotation = origRot;

            itemCollider.enabled = true;
            leverAttached = true;
        }
    }
}