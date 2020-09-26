using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace artgame
{
    public class Shiteyanyorun : MonoBehaviour
    {
        public float speed = 0.07f;

        void Update()
        {
            transform.RotateAround(Vector3.zero, Vector3.up, -speed);
        }
    }
}
