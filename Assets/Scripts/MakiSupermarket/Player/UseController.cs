using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MakiSupermarket
{
    public class UseController : MonoBehaviour
    {
        public PlayerController playerController;
        public float range = 2.5f;
        public LayerMask mask;
        Transform cam;

        void LateUpdate()
        {
            if(cam == null)
                cam = playerController.camTransform;

            if (PauseManager.isPaused().Value)
                return;

            //Get Input
            bool useKey = Input.GetKeyDown(GlobalSettings.keyUse) || Input.GetKeyDown(GlobalSettings.keyUse2);

            //!(IsRiding & pressing)
            if (!(useKey && playerController.currentRide != null))
            {
                //Get useable GameObject and maybe use it
                RaycastHit hit;
                if (Physics.Raycast(cam.position, cam.forward, out hit, range, mask))
                {
                    GameObject hitObject = hit.collider.gameObject;
                    if (hitObject.CompareTag("Useable"))
                    {
                        Useable[] useables = hitObject.GetComponents<Useable>();

                        if (useables == null)
                            return;

                        if (playerController.GetCanMove())
                        {
                            foreach (Useable useable in useables)
                            {
                                useable.LookingAt();

                                if (useKey)
                                    useable.Use();
                            }
                        }
                    }
                }
            }
        }
    }
}