using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MakiSupermarket
{
    public class Flashlight : MonoBehaviour, ItemObserver
    {
        public AudioSource audioSource;
        public Animator flashlightAnimator;
        public PlayerController playerController;

        bool isPressing = false;
        bool active = false;

        void Start()
        {
            playerController.itemObservers.Add(this);
        }

        void Update()
        {
            if (active)
            {
                if (isPressing && Input.GetAxis("Primary Fire") <= 0)
                    isPressing = false;

                if (!isPressing && Input.GetAxis("Primary Fire") > 0)
                {
                    audioSource.Play();
                    flashlightAnimator.SetTrigger("Click");
                    isPressing = true;
                }
            }
        }

        void ItemObserver.UpdateItemStatus(string itemName, bool status)
        {
            if (itemName.Equals("Flashlight"))
            {
                this.active = status;
                flashlightAnimator.SetBool("Shown", active);
                if (status)
                    audioSource.PlayDelayed(1f);
            }
        }
    }
}