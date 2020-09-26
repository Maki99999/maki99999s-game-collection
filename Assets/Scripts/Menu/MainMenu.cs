using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{

    public GameObject levelSelectMenu;
    public GameObject optionsMenu;
    public GameObject DeletePlayerPrefsMenu;

    public AudioMixer mainMixer;

    AudioSource audioSource;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        audioSource = GetComponent<AudioSource>();

        SetPrefValues();

        DiscordIntegration.UpdateActivity("Main Menu");
    }

    public void DeletePlayerPrefsConfirm()
    {
        audioSource.Play();
        PlayerPrefs.DeleteAll();
        optionsMenu.SetActive(true);
        DeletePlayerPrefsMenu.SetActive(false);
    }

    public void DeletePlayerPrefsCancel()
    {
        audioSource.Play();
        optionsMenu.SetActive(true);
        DeletePlayerPrefsMenu.SetActive(false);
    }

    public void GameSelect()
    {
        audioSource.Play();
        levelSelectMenu.SetActive(true);
    }

    public void OpenOptions()
    {
        audioSource.Play();
        optionsMenu.SetActive(true);
    }

    public void CloseOptions()
    {
        audioSource.Play();
        optionsMenu.SetActive(false);
    }

    public void DeletePlayerPrefs()
    {
        audioSource.Play();
        DeletePlayerPrefsMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void SetPrefValues()
    {
        SetMainVolume(PlayerPrefs.GetFloat("masterVolume", 0));
        SetFxVolume(PlayerPrefs.GetFloat("fxVolume", 0));
        SetMusicVolume(PlayerPrefs.GetFloat("musicVolume", 0));
    }

    public void SetMainVolume(float volume)
    {
        mainMixer.SetFloat("masterVolume", volume);
        PlayerPrefs.SetFloat("masterVolume", volume);
    }

    public void SetFxVolume(float volume)
    {
        mainMixer.SetFloat("fxVolume", volume);
        PlayerPrefs.SetFloat("fxVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        mainMixer.SetFloat("musicVolume", volume);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }
}
