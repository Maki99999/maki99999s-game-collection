using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MakiSupermarket
{
    public class BoxFallDown2 : MonoBehaviour
    {
        public ProgressionController01 progressionController;

        bool activated = false;

        void OnTriggerEnter(Collider other)
        {
            if (progressionController.currentState == ProgressionState.InspectFallenBox && !activated && other.CompareTag("Player"))
            {
                activated = true;
                progressionController.StartCoroutine(progressionController.InspectingFallenBox());
            }
        }
    }
}