using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace MakiSupermarket
{
    public class Cutscene02End : UseableOutline
    {
        public Collider coll;
        public AudioSource audioSource;
        public AudioSource audioSourceMagic;
        public float jitterXMax = .04f;
        public float jitterYMax = .01f;
        [Space(10)]
        public DialogueManager dialogueManager;
        public Animator fadeAnim;
        public Image fadeWhite;
        public PlayerController playerController;
        public Transform holdPosition;
        public CameraBlurJitterEffect canOldEffects;

        bool used = false;
        bool camShake = false;
        int fadeSpeedMult = 1;

        public override void Use()
        {
            if (!used)
            {
                used = true;
                coll.enabled = false;

                playerController.SetCanMove(false);
                StartCoroutine(Cutscene());
            }
        }

        void Update()
        {
            foreach (cakeslice.Outline outline in outlines)
                outline.enabled = false;

            float jitterXCurrentMax = !camShake ? 0f : jitterXMax;
            float jitterYCurrentMax = !camShake ? 0f : jitterYMax;

            playerController.camOffsetX = Random.Range(-jitterXCurrentMax, jitterXCurrentMax);
            playerController.camOffsetY = Random.Range(-jitterYCurrentMax, jitterYCurrentMax);
        }

        IEnumerator Cutscene()
        {
            canOldEffects.enabled = false;
            StartCoroutine(playerController.ForceLookPlayer(transform, 40));
            yield return dialogueManager.StartDialogue(Dialogue.Cutscene02End01());

            audioSource.Play();
            yield return Things.PosRotLerp(transform, holdPosition, 20);
            yield return new WaitForSeconds(1f);
            yield return dialogueManager.StartDialogue(Dialogue.Cutscene02End02());

            camShake = true;
            audioSourceMagic.Play();
            StartCoroutine(FadeWhiteSlow());
            yield return new WaitForSeconds(3f);
            yield return dialogueManager.StartDialogue(Dialogue.Cutscene02End03());

            fadeSpeedMult = 3;
            StartCoroutine(LoadScene("MakiSupermarket02Infinite"));

            float startVolume = audioSourceMagic.volume;

            while (audioSourceMagic.volume > 0)
            {
                audioSourceMagic.volume -= startVolume * Time.deltaTime / 1f;
                yield return null;
            }

            audioSourceMagic.volume = 0f;
            audioSourceMagic.Stop();
        }

        IEnumerator FadeWhiteSlow()
        {
            fadeWhite.gameObject.SetActive(true);
            for (int i = 0; i < 255; i += fadeSpeedMult)
            {
                fadeWhite.color = new Color(1f, 1f, 1f, i / 255f);
                yield return new WaitForSeconds(0.1f);
            }
        }

        IEnumerator LoadScene(string sceneName)
        {
            fadeAnim.SetTrigger("Out");
            yield return new WaitForSeconds(3f);

            SceneManager.LoadScene(sceneName);
        }
    }
}