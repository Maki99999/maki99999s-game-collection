using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace artgame
{
    public class ExtendingRoom : MonoBehaviour
    {
        Transform player;

        bool playerInside = false;

        private void Update()
        {
            if (player == null)
                player = PlayerController.playerController.transform;

            if (playerInside)
            {
                transform.localScale = new Vector3(1f, 1f, Mathf.Max((player.position.z - transform.position.z + 4) / 4, 1));
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInside = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInside = false;
            }
        }
    }
}