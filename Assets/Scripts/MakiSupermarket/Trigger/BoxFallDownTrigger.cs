using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MakiSupermarket
{
    public class BoxFallDownTrigger : MonoBehaviour
    {
        public Transform fallingBox;
        [HideInInspector] public Transform box;
        public Animator anim;

        public Transform helpTransform;

        public bool enabledTrigger = false;
        bool activated = false;

        void OnTriggerEnter(Collider other)
        {
            if (enabledTrigger && !activated && other.CompareTag("Player"))
            {
                activated = true;
                StartCoroutine(WaitForAnimation());
            }
        }

        IEnumerator WaitForAnimation()
        {
            anim.SetTrigger("Fall");
            yield return new WaitForSeconds(1.5f);
            Things.CopyTransform(helpTransform, box);
            Things.CopyTransform(box, fallingBox);
            Things.CopyTransform(fallingBox, helpTransform);
        }
    }
}