using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MakiSupermarket
{
    public class EmptyShelf : MonoBehaviour, Useable
    {
        public cakeslice.Outline[] outlines;
        public ProgressionController01 progressionController;
        public Transform wares;
        public Transform itemPos;
        public Transform[] signs;
        public AudioSource audioSource;

        public string symbolName;

        bool activated = false;

        void Start()
        {
            wares.gameObject.SetActive(false);
        }

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
            {
                if (progressionController.AddShelf(symbolName))
                {
                    activated = true;
                    wares.gameObject.SetActive(true);
                    audioSource.Play();
                    wares.position = itemPos.position;
                    wares.localScale = Vector3.zero;
                    StartCoroutine(Things.PosRotScaleLerp(wares, transform, 60));
                    foreach (Transform sign in signs)
                    {
                        StartCoroutine(Things.ScaleLerp(sign, Vector3.zero, 40));
                    }
                }
            }
        }

        void OnDisable()
        {
            foreach (cakeslice.Outline outline in outlines)
                outline.enabled = false;
        }
    }
}