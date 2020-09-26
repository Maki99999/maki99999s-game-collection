using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

namespace MakiSupermarket
{
    public class OptionsMenu : MonoBehaviour
    {
        public AudioMixer mainMixer;
        public AudioMixer fxMixer;
        public AudioMixer musicMixer;

        public Dropdown resolutionDropdown;
        Resolution[] resolutions;

        void Start()
        {
            resolutions = Screen.resolutions;

            resolutionDropdown.ClearOptions();
            List<string> options = new List<string>();

            bool alreadyHas = PlayerPrefs.HasKey("resolutionHeight") && PlayerPrefs.HasKey("resolutionWidth");
            int prefWidth = 0;
            int prefHeight = 0;
            if (alreadyHas)
            {
                prefWidth = PlayerPrefs.GetInt("resolutionWidth");
                prefHeight = PlayerPrefs.GetInt("resolutionHeight");
            }

            int currentResolution = -1;

        beginLoop:
            for (int i = 0; i < resolutions.Length; i++)
            {
                options.Add(resolutions[i].width + " x " + resolutions[i].height);

                if (alreadyHas)
                {
                    if (resolutions[i].width == prefWidth && resolutions[i].height == prefHeight)
                        currentResolution = i;
                }
                else
                {
                    if (resolutions[i].Equals(Screen.currentResolution))
                        currentResolution = i;
                }
            }
            if (currentResolution == -1)
            {
                if (alreadyHas)
                {
                    alreadyHas = false;
                    goto beginLoop;
                }
                currentResolution = 0;
            }
            PlayerPrefs.SetInt("resolutionWidth", prefWidth);
            PlayerPrefs.SetInt("resolutionHeight", prefHeight);

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolution;
            resolutionDropdown.RefreshShownValue();

            SetPrefValues();
        }

        void SetPrefValues()
        {
            SetMainVolume(PlayerPrefs.GetFloat("masterVolume", 0));
            SetFxVolume(PlayerPrefs.GetFloat("fxVolume", 0));
            SetMusicVolume(PlayerPrefs.GetFloat("musicVolume", 0));

            SetQuality(PlayerPrefs.GetInt("quality", 5));
            SetFullscreenMode(PlayerPrefs.GetInt("fullscreenMode", 0));
        }

        public void SetMainVolume(float volume)
        {
            mainMixer.SetFloat("masterVolume", volume);
            PlayerPrefs.SetFloat("masterVolume", volume);
        }

        public void SetFxVolume(float volume)
        {
            fxMixer.SetFloat("fxVolume", volume);
            PlayerPrefs.SetFloat("fxVolume", volume);
        }

        public void SetMusicVolume(float volume)
        {
            musicMixer.SetFloat("musicVolume", volume);
            PlayerPrefs.SetFloat("musicVolume", volume);
        }

        public void SetQuality(int level)
        {
            QualitySettings.SetQualityLevel(level);
            PlayerPrefs.SetInt("quality", level);
        }

        public void SetFullscreenMode(int index)
        {
            Screen.fullScreen = index != 1;
            PlayerPrefs.SetInt("fullscreenMode", index);
        }

        public void SetResolution(int index)
        {
            Resolution resolution = resolutions[index];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            PlayerPrefs.SetInt("resolutionWidth", resolution.width);
            PlayerPrefs.SetInt("resolutionHeight", resolution.height);
        }
    }
}