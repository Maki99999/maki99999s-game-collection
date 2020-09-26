using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MakiSupermarket
{
    public class RandomRoom : MonoBehaviour
    {
        public Transform[] connections;

        public Collider[] colliders;

        public List<Bounds> GetBounds()
        {
            List<Bounds> bounds = new List<Bounds>();

            foreach (Collider collider in colliders)
            {
                bounds.Add(collider.bounds);
            }

            return bounds;
        }
    }
}