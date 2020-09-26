using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FalseTruth
{
    public class FootstepSound : MonoBehaviour
    {
        public float sqrDistanceBetweenFootsteps = 1f;
        public AudioSource[] audioSources;
        int currenAudioSource = -1;

        float secondsBetweenChecks = 0.1f;
        float distanceSinceLastFootstep = 0f;
        bool isStanding = true;

        Vector3 oldPos;
        Vector3 newPos;

        void Start()
        {
            oldPos = transform.position;
            newPos = transform.position;
            StartCoroutine(Footsteps());
        }

        IEnumerator Footsteps()
        {
            while (enabled)
            {
                yield return new WaitForSeconds(secondsBetweenChecks);
                oldPos = newPos;
                newPos = transform.position;
                if (oldPos.Equals(newPos))
                {
                    if (!isStanding)
                    {
                        isStanding = true;
                        PlayFootstepSound();
                        distanceSinceLastFootstep = 0f;

                    }
                }
                else
                {
                    isStanding = false;
                    distanceSinceLastFootstep += (oldPos - newPos).sqrMagnitude;
                    if (distanceSinceLastFootstep >= sqrDistanceBetweenFootsteps)
                    {
                        distanceSinceLastFootstep = 0f;
                        PlayFootstepSound();
                    }
                }
            }
        }

        void PlayFootstepSound()
        {
            currenAudioSource = (currenAudioSource + 1) % audioSources.Length;
            audioSources[currenAudioSource].Play();
        }
    }
}