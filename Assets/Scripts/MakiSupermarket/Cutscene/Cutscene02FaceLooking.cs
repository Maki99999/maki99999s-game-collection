using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MakiSupermarket
{
    public class Cutscene02FaceLooking : MonoBehaviour, Useable
    {
        public Cutscene02Face cutscene;

        bool activated = false;

        void Useable.LookingAt()
        {
            if (!activated)
            {
                activated = true;
                cutscene.StartAfterLooking();
            }
        }

        void Useable.Use() { }
    }
}