using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MakiSupermarket
{
    public class MainMenu : MonoBehaviour
    {
        public Animator fadeAnim;
        public Animator optionsAnim;

        public void Play()
        {
            StartCoroutine(LoadScene("MakiSupermarket01"));
        }

        public void Options()
        {
            optionsAnim.SetBool("OptionsShown", true);
        }

        public void HideOptions()
        {
            optionsAnim.SetBool("OptionsShown", false);
        }

        public void Exit()
        {
            StartCoroutine(LoadScene("MainMenu"));
        }

        IEnumerator LoadScene(string sceneName)
        {
            fadeAnim.SetTrigger("Out");
            yield return new WaitForSeconds(3f);

            SceneManager.LoadScene(sceneName);
        }
    }
}