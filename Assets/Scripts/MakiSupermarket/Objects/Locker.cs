using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MakiSupermarket
{
    public class Locker : MonoBehaviour, Useable
    {
        public Animator anim;
        public cakeslice.Outline outline;

        public AudioSource audioSourceOpen;
        public AudioSource audioSourceClose;

        bool open = false;

        void Update()
        {
            outline.enabled = false;
        }

        void Useable.LookingAt()
        {
            outline.enabled = true;
        }

        void Useable.Use()
        {
            open = !open;
            anim.SetBool("Open", open);

            if (open)
                audioSourceOpen.Play();
            else
                audioSourceClose.Play();
        }
    }
}