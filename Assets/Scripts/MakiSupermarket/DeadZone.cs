using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MakiSupermarket
{
    public class DeadZone : MonoBehaviour
    {
        public Animator fadeAnimator;
        public PlayerController playerController;

        public Vector3 resetPosition;
        public Vector3 resetRotation;

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                StartCoroutine(ResetPlayer());
            }
        }

        IEnumerator ResetPlayer()
        {
            fadeAnimator.SetBool("Open", false);
            yield return new WaitForSeconds(1.5f);

            playerController.transform.position = resetPosition;
            playerController.transform.eulerAngles = new Vector3(0, resetRotation.y, 0);
            playerController.camTransform.eulerAngles = new Vector3(resetRotation.x, 0, 0);

            fadeAnimator.SetBool("Open", true);
        }
    }
}