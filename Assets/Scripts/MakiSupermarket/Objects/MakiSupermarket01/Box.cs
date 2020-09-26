using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MakiSupermarket
{
    public class Box : MonoBehaviour, Useable
    {
        public cakeslice.Outline outline;
        public ProgressionController01 progressionController;
        public Transform itemPos;
        public AudioSource audioSource;

        public string symbolName;
        public Sprite symbolSprite;

        bool activated = false;

        Collider coll;

        void Start()
        {
            coll = GetComponent<Collider>();
        }

        void Update()
        {
            outline.enabled = false;
        }

        void Useable.LookingAt()
        {
            if (!activated)
                outline.enabled = true;
        }

        void Useable.Use()
        {
            if (!activated)
            {
                if (progressionController.AddBox(symbolName, symbolSprite))
                {
                    activated = true;
                    StartCoroutine(Use());
                }
            }
        }

        IEnumerator Use()
        {
            coll.enabled = false;
            audioSource.Play();
            yield return Things.PosRotScaleLerp(transform, itemPos, 60);
            gameObject.SetActive(false);
        }

        void OnDisable()
        {
            outline.enabled = false;
        }
    }
}