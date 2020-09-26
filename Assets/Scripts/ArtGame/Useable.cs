using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace artgame
{
    public abstract class Useable : MonoBehaviour
    {
        public abstract void ShowControls();

        public abstract void Use();
    }
}
