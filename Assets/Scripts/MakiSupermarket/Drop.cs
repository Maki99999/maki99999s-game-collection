using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MakiSupermarket
{
    public class Drop : MonoBehaviour, Useable
    {
        public cakeslice.Outline[] outlines;
        public PlayerController playerController;
        public Transform itemPosition;
        public Transform newPosition;
        public string itemName;
        public Transform helpTransform;

        bool activated = false;

        void Start()
        {
            newPosition.gameObject.SetActive(false);
        }

        void Update()
        {
            foreach (cakeslice.Outline outline in outlines)
                outline.enabled = false;
        }

        void Useable.LookingAt()
        {
            if (!activated && playerController.CollectedItemsContains(itemName))
                foreach (cakeslice.Outline outline in outlines)
                    outline.enabled = true;
        }

        void Useable.Use()
        {
            if (!activated && playerController.CollectedItemsContains(itemName))
                StartCoroutine(Use());
        }

        IEnumerator Use()
        {
            activated = true;
            newPosition.gameObject.SetActive(true);

            Vector3 newPos = newPosition.position;
            Quaternion newRot = newPosition.rotation;
            for (float f = 0; f < 1; f += 1f / 100f)
            {
                newPosition.position = Vector3.Lerp(itemPosition.position, newPos, f);
                newPosition.rotation = Quaternion.Lerp(itemPosition.rotation, newRot, f);
                yield return new WaitForSeconds(1f / 60f);
            }

            playerController.RemoveCollectedItem(itemName);
        }
    }
}