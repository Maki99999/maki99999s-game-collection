using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MakiSupermarket
{
    public class ExpandingIsland : MonoBehaviour
    {
        public Transform playerTransform;
        public Transform expansions;
        public GameObject[] expansionsPrefabs;
        int expansionCircle = 0;

        //public float StartOffsetZ = -2f;
        public float lengthenMultiplier = .9f;

        float startZ;
        float maxZ;

        int nextZForExpansion = 16;
        int nextZExpansionPlacingLocal = 0;

        void Start()
        {
            startZ = transform.position.z;
            maxZ = startZ - 20 * lengthenMultiplier;
        }

        void LateUpdate()
        {
            transform.position = new Vector3(transform.position.x, transform.position.y,
                    //Mathf.Clamp(startZ + (playerTransform.position.z - StartPoint.position.z) - 
                    //((playerTransform.position.z - StartPoint.position.z) / lengthenMultiplyer), maxZ, startZ));
                    //Mathf.Clamp(startZ - (((startZ + StartOffsetZ) - playerTransform.position.z) * lengthenMultiplier), maxZ, startZ));
                    Mathf.Min(Mathf.SmoothStep(startZ, maxZ, ((20 - playerTransform.position.z - 8) / 20)), transform.position.z));

            if (transform.position.z -.1f < nextZForExpansion)
            {
                nextZForExpansion -= 2;
                Instantiate(expansionsPrefabs[expansionCircle], new Vector3(expansions.position.x, expansions.position.y, expansions.position.z + nextZExpansionPlacingLocal), Quaternion.Euler(0,0,0), expansions);
                expansionCircle = (expansionCircle + 1) % expansionsPrefabs.Length;
                nextZExpansionPlacingLocal += 2;
            }
        }
    }
}