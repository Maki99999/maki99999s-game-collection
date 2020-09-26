using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MakiSupermarket
{
    public class AutomaticDoorDisappearTrigger : MonoBehaviour
    {
        public ProgressionController01 progressionController;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                progressionController.AutomaticDoorDisappearTrigger();
        }
    }
}