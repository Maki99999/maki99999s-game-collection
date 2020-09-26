using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace proj3ct
{
    public class Menu : MonoBehaviour
    {

        public GameObject mainMenu;
        public GameObject settingsMenu;
        public GameObject levelMenu;
        public GameObject helpMenu;
        public GameObject easterEggMenu;

        [Space(20)]

        public Text soundText;
        public Text musicText;

        [Space(20)]

        public AudioSource easterEggSound;
        public AudioMixer mainMixer;

        public Animator fadeBlack;

        int easterEggCount = 0;
        float easterEggLast = 0f;

        void Start()
        {
            mainMenu.SetActive(true);
            settingsMenu.SetActive(false);
            levelMenu.SetActive(false);
            helpMenu.SetActive(false);
            easterEggMenu.SetActive(false);

            mainMixer.SetFloat("fxVolume", 0);
            mainMixer.SetFloat("musicVolume", 0);
        }

        void Update()
        {
            if (Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Q))
            {
                StartCoroutine(LoadScene("MainMenu"));
            }
        }

        public void Play()
        {
            mainMenu.SetActive(false);
            levelMenu.SetActive(true);
        }

        public void Help()
        {
            helpMenu.SetActive(!helpMenu.activeSelf);
        }

        public void Settings()
        {
            settingsMenu.SetActive(!settingsMenu.activeSelf);
        }

        public void Quit()
        {
            StartCoroutine(LoadScene("MainMenu"));
        }

        public void ToggleMusic()
        {
            float volume;
            mainMixer.GetFloat("musicVolume", out volume);
            volume = volume == 0 ? -80 : 0;

            mainMixer.SetFloat("musicVolume", volume);

            musicText.text = volume == 0 ? "Music on" : "Music off";
        }

        public void ToggleSound()
        {
            float volume;
            mainMixer.GetFloat("fxVolume", out volume);
            volume = volume == 0 ? -80 : 0;

            mainMixer.SetFloat("fxVolume", volume);

            soundText.text = volume == 0 ? "Sound on" : "Sound off";
        }

        public void EasterEgg()
        {
            if (Time.time > easterEggLast + 1f)
                easterEggCount = 0;
            easterEggLast = Time.time;
            if (++easterEggCount > 3)
            {
                easterEggCount = 0;
                StartCoroutine(EasterEggAnimation());
            }
        }

        public void Level1()
        {
            StartCoroutine(LoadScene("Proj3ctLevel1"));
        }

        public void Level2()
        {
            StartCoroutine(LoadScene("Proj3ctLevel2"));
        }

        public void Level3()
        {
            StartCoroutine(LoadScene("Proj3ctLevel3"));
        }

        public void Level4()
        {
            StartCoroutine(LoadScene("Proj3ctLevel4"));
        }

        public void Level5()
        {
            StartCoroutine(LoadScene("Proj3ctLevel5"));
        }

        IEnumerator LoadScene(string name)
        {
            fadeBlack.SetTrigger("Out");
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(name);
        }

        IEnumerator EasterEggAnimation()
        {
            easterEggMenu.SetActive(true);
            easterEggSound.Play();
            yield return new WaitForSeconds(2.5f);
            easterEggMenu.SetActive(false);
        }
    }
}