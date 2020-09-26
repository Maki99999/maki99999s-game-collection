using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace artgame
{
    public class SceneController : MonoBehaviour
    {
        public Animator fadeBlack;

        void Update()
        {
            if (Input.GetKey(GlobalSettings.keyEscape) || Input.GetKey(KeyCode.Q))
            {
                StartCoroutine(LoadScene("MainMenu"));
            }
        }

        IEnumerator LoadScene(string name)
        {
            fadeBlack.SetTrigger("Out");
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(name);
        }
    }
}