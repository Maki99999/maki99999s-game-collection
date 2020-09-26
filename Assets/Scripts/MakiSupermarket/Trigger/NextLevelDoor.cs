using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MakiSupermarket
{
    public class NextLevelDoor : MonoBehaviour, Useable
    {
        public ProgressionController01 progressionController;

        void Useable.LookingAt() { }

        void Useable.Use()
        {
            progressionController.NextLevel();
        }
    }
}