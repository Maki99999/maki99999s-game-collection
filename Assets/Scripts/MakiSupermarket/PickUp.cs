using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MakiSupermarket
{
    public class PickUp : MonoBehaviour, Useable
    {
        public cakeslice.Outline[] outlines;
        public Collider coll;
        public AudioSource itemPickUpSound;
        public PlayerController playerController;
        public Transform itemPosition;
        public string itemName;

        bool activated = false;

        void Update()
        {
            foreach (cakeslice.Outline outline in outlines)
                outline.enabled = false;
        }

        void Useable.LookingAt()
        {
            if (!activated)
                foreach (cakeslice.Outline outline in outlines)
                    outline.enabled = true;
        }

        void Useable.Use()
        {
            if (!activated)
                StartCoroutine(Use());
        }

        IEnumerator Use()
        {
            activated = true;
            coll.enabled = false;
            itemPickUpSound.Play();

            Vector3 oldPos = transform.position;
            Quaternion oldRot = transform.rotation;
            for (float f = 0; f < 1; f += 1f / 40f)
            {
                transform.position = Vector3.Lerp(oldPos, itemPosition.position, f);
                transform.rotation = Quaternion.Lerp(oldRot, itemPosition.rotation, f);
                yield return new WaitForSeconds(1f / 60f);
            }

            playerController.AddCollectedItem(itemName);
            gameObject.SetActive(false);
        }
    }
}