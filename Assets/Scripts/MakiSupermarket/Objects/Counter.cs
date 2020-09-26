using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MakiSupermarket
{
    public class Counter : UseableOutline
    {
        public Animator anim;

        public AudioSource audioSource;

        bool open = false;

        public override void Use()
        {
            open = !open;
            anim.SetBool("Open", open);

            audioSource.pitch = open ? 1f : 0.85f;
            audioSource.PlayDelayed(.1f);
        }
    }
}