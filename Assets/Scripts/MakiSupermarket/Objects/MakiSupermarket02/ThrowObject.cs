using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MakiSupermarket
{
    public class ThrowObject : MonoBehaviour
    {
        public float deathHeight = 2f;
        public float fadeAnimLength = 20;
        public float scaleValue = 1f;

        bool dead = false;

        void Start()
        {
            StartCoroutine(FadeIn());
        }

        IEnumerator FadeIn()
        {
            for (float f = 0; f < 1f; f += 1f / fadeAnimLength)
            {
                transform.localScale = Vector3.one * Mathf.SmoothStep(0f, scaleValue, f);
                yield return new WaitForSeconds(1f / 60f);
            }
        }

        IEnumerator FadeOut()
        {
            for (float f = 0; f < 1f; f += 1f / (fadeAnimLength / 2))
            {
                transform.localScale = Vector3.one * Mathf.SmoothStep(scaleValue, 0f, f);
                yield return new WaitForSeconds(1f / 60f);
            }
            Destroy(gameObject);
        }

        void Update()
        {
            if (!dead && transform.position.y <= deathHeight)
            {
                dead = true;
                StartCoroutine(FadeOut());
            }
        }
    }
}