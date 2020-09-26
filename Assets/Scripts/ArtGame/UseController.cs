using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace artgame
{
    public class UseController : MonoBehaviour
    {

        public float range = 2;
        Transform cam;

        bool isPressing = false;
        bool isRiding = false;
        Rideable lastRide = null;

        void Start()
        {
            cam = transform.GetChild(0);
        }

        void Update()
        {
            //Get Input
            bool useKey = Input.GetKey(GlobalSettings.keyUse);

            //IsRiding & pressing -> standUp
            if (isRiding)
            {
                if (useKey && !isPressing && PlayerController.playerController.canMove)
                {
                    lastRide.Use();
                    isRiding = false;
                }
            }
            else
            {
                //Get useable GameObject and maybe use it
                RaycastHit hit;
                if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
                {
                    GameObject hitObject = hit.collider.gameObject;
                    if (hitObject.CompareTag("Useable"))
                    {
                        Useable useable = hitObject.GetComponent<Useable>();
                        useable.ShowControls();

                        if (useKey && !isPressing && PlayerController.playerController.canMove)
                        {
                            isPressing = true;
                            if (useable is Rideable)
                            {
                                lastRide = (Rideable)useable;
                                isRiding = true;
                            }
                            useable.Use();
                        }
                    }
                }
            }

            if (useKey && !isPressing)
            {
                isPressing = true;
            }
            if (!useKey && isPressing)
            {
                isPressing = false;
            }
        }
    }
}