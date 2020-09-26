using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MakiSupermarket
{
    public class Puddle : MonoBehaviour, Useable
    {
        public cakeslice.Outline outline;
        public PlayerController playerController;
        public ProgressionController01 progressionController;
        public Animator mopAnimator;
        public Transform helpTransform;
        public Transform center;

        bool activated = false;

        void Update()
        {
            outline.enabled = false;
        }

        void Useable.LookingAt()
        {
            if (!activated && playerController.CollectedItemsContains("Mop"))
                outline.enabled = true;
        }

        void Useable.Use()
        {
            if (!activated && playerController.CollectedItemsContains("Mop"))
                StartCoroutine(MopAnimation());
        }

        IEnumerator MopAnimation()
        {
            activated = true;
            yield return new WaitUntil(() => playerController.charController.isGrounded);
            playerController.SetCanMove(false);

            helpTransform.position = center.position;
            Vector3 playerNoY = playerController.transform.position;
            playerNoY.y = 0f;
            helpTransform.LookAt(playerNoY);
            helpTransform.position += helpTransform.forward;
            helpTransform.position = new Vector3(helpTransform.position.x, playerController.camTransform.position.y, helpTransform.position.z);
            helpTransform.LookAt(center);
            helpTransform.position = new Vector3(helpTransform.position.x, playerController.transform.position.y, helpTransform.position.z);

            yield return playerController.MovePlayer(helpTransform);
            mopAnimator.SetTrigger("Mop");
            yield return new WaitForSeconds(1f);

            Vector3 scale = transform.localScale;
            Vector3 newScale = Vector3.zero;
            for (float f = 0; f < 1; f += 1f / (60 * 3f))
            {
                transform.localScale = Vector3.Lerp(scale, newScale, f);
                yield return new WaitForSeconds(1f / 60f);
            }
            transform.localScale = newScale;

            yield return new WaitForSeconds(.5f);
            playerController.SetCanMove(true);
            progressionController.AddPuddle();
            gameObject.SetActive(false);
        }
    }
}