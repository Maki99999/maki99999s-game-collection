using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace proj3ct
{
    public class Finish : MonoBehaviour
    {
        public AudioSource winAudio;
        bool triggered = false;
        LevelController levelcontroller;

        void Start() {
            levelcontroller = GameObject.FindWithTag("GameController").GetComponent<LevelController>();    
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && !triggered)
            {
                triggered = true;
                winAudio.Play();
                StartCoroutine(WinAnimation());
            }
        }

        IEnumerator WinAnimation()
        {
            yield return new WaitForSeconds(winAudio.clip.length);
            triggered = false;
            levelcontroller.EndLevel();
        }
    }
}