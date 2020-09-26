using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace artgame
{
    public class ThunderBehavior : MonoBehaviour
    {

        public AudioSource audioSource;

        void Start()
        {
            StartCoroutine(Behavior());
            Destroy(transform.GetChild(0).gameObject, 10f);
            Destroy(gameObject, 10f);
        }

        IEnumerator Behavior()
        {
            yield return new WaitForSeconds(1f);
            audioSource.Play();
            StartCoroutine(WeatherController.weatherController.Lightning());
        }
    }
}