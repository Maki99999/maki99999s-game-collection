using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MakiSupermarket
{
    public class RandomCreepySounds : MonoBehaviour
    {
        public AudioSource audioSource;
        public AudioClip[] audioClips;

        public Vector2 Interval;
        int lastClip = -1;

        void Start()
        {
            StartCoroutine(CreepySounds());
        }

        IEnumerator CreepySounds()
        {
            while (enabled)
            {
                yield return new WaitForSeconds(Random.Range(Interval.x, Interval.y));

                int nextClip = Random.Range(0, audioClips.Length);
                if (nextClip == lastClip)
                    nextClip = (nextClip + 1) % audioClips.Length;
                lastClip = nextClip;

                audioSource.clip = audioClips[nextClip];
                audioSource.pitch = .9f - Random.Range(0f, 0.2f);
                audioSource.Play();
            }
        }
    }
}