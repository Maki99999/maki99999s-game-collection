using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MakiSupermarket
{
    public class FuseBox : MonoBehaviour, Useable
    {
        public Animator anim;
        public cakeslice.Outline outline;
        public ProgressionController01 progressionController;
        public AudioSource audioSourceOpen;
        public AudioSource audioSourceClose;

        bool open = false;
        bool activated = false;

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
            if (activated) return;

            if (open)
            {
                anim.SetTrigger("Switch");
                activated = true;
                progressionController.StartCoroutine(progressionController.LightsOn());

                audioSourceClose.PlayDelayed(1f);
            }
            else
                audioSourceOpen.Play();

            open = !open;
            anim.SetBool("Open", open);
        }
    }
}