using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using System;

namespace FriendsInDreams
{
    public class FriendsInDreamsDummy : MonoBehaviour
    {
        public Animator fadeBlack;

        public void ToMainMenu()
        {
            StartCoroutine(LoadScene("MainMenu"));
        }

        public void LaunchGame()
        {
            Process.Start(Application.streamingAssetsPath + "\\FriendsInDreams\\friends-in-dreams.exe");
        }

        IEnumerator LoadScene(string name)
        {
            fadeBlack.SetTrigger("Out");
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(name);
        }
    }
}