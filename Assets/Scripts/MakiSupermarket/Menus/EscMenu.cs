using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MakiSupermarket
{
    public class EscMenu : MonoBehaviour
    {
        public Animator fadeAnim;
        public Animator anim;

        public PauseManager pauseManager;

        bool shown = false;
        bool optionsShown = false;

        private void Start()
        {
            //Lock Mouse
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {
            if (Input.GetKeyDown(GlobalSettings.keyEscape) || Input.GetKeyDown(GlobalSettings.keyEscape2))
            {
                if (optionsShown)
                {
                    HideOptions();
                }
                else
                {
                    if (shown)
                    {
                        Continue();
                    }
                    else
                    {
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;
                        pauseManager.Pause();
                        anim.SetBool("Shown", true);
                        shown = true;
                        optionsShown = false;
                    }
                }
            }
        }

        public void Continue()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            anim.SetBool("Shown", false);
            pauseManager.UnPause();
            shown = false;
            optionsShown = false;
        }

        public void ShowOptions()
        {
            anim.SetBool("OptionsShown", true);
            shown = true;
            optionsShown = true;
        }

        public void HideOptions()
        {
            anim.SetBool("OptionsShown", false);
            shown = true;
            optionsShown = false;

        }

        public void MainMenu()
        {
            StartCoroutine(LoadScene("MakiSupermarketMainMenu"));
        }

        IEnumerator LoadScene(string sceneName)
        {
            fadeAnim.SetTrigger("Out");
            yield return new WaitForSeconds(3f);

            SceneManager.LoadScene(sceneName);
        }
    }
}