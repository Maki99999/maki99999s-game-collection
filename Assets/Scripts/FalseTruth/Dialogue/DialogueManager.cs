using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace FalseTruth
{
    public class DialogueManager : MonoBehaviour
    {

        public Text nameText;
        public Text dialogueText;
        public Button continueButton;
        public GameObject choiceButtonParent;
        public GameObject choiceButtonObject;

        public Animator animator;

        [HideInInspector]
        public int dialogueOutput;

        FalseTruth.TextElement currentTextElement;
        string currentSentence = "";
        bool unlockedMouse = false;
        bool waitForChoice = false;

        bool stopTyping;

        public GameObject audioSourceObject;
        public int audioSourcesCount = 25;
        public AudioClip[] audioClips;
        int every4thLetter = -1;
        int currentAudioSource = -1;
        List<AudioSource> audioSources;

        List<GameObject> choiceButtons;

        bool finished;

        void Start()
        {
            audioSources = new List<AudioSource>();
            audioSources.Add(audioSourceObject.GetComponent<AudioSource>());
            for (int i = 0; i < audioSourcesCount - 1; i++)
                audioSources.Add(Instantiate(audioSourceObject, audioSourceObject.transform.position,
                    audioSourceObject.transform.rotation, transform).GetComponent<AudioSource>());
        }

        public IEnumerator StartDialogue(string name, string text, float seconds)
        {
            FalseTruth.Dialogue dialogue = new FalseTruth.Dialogue(new FalseTruth.TextElement(name, text));
            StartDialogue(dialogue, false);

            yield return new WaitForSeconds(seconds);
            EndDialogue();
        }

        public void StartDialogue(FalseTruth.Dialogue dialogue)
        {
            StartDialogue(dialogue, true);
        }

        public void StartDialogue(FalseTruth.Dialogue dialogue, bool unlockedMouse)
        {
            this.unlockedMouse = unlockedMouse;
            finished = false;
            dialogueOutput = -1;

            if (unlockedMouse)
            {
                FalseTruth.GameController.UnlockMouse();
            }
            else
            {
                FalseTruth.GameController.LockMouse();
            }

            animator.SetBool("IsOpen", true);

            currentTextElement = dialogue.firstText;

            DisplayNextScentence();
        }

        public void DisplayNextScentence()
        {
            if (currentSentence != "")
            {
                stopTyping = true;
                return;
            }

            if (waitForChoice) return;

            if (currentTextElement == null)
            {
                EndDialogue();
                return;
            }

            if (currentTextElement.italic)
            {
                dialogueText.fontStyle = FontStyle.Italic;
            }
            else
            {
                dialogueText.fontStyle = FontStyle.Normal;
            }
            stopTyping = true;
            StartCoroutine(TypeSentence(currentTextElement.text));
            if (nameText.text != currentTextElement.name)
            {
                StartCoroutine(ChangeName());
            }

            int countNext = currentTextElement.nextTexts.Count;

            if (countNext < 1)
            {
                currentTextElement = null;
            }
            else if (countNext == 1)
            {
                currentTextElement = currentTextElement.nextTexts[0];
            }
            else
            {
                ShowChoices();
                waitForChoice = true;
            }
        }

        public void DisplayNextScentence(string choiceName)
        {
            waitForChoice = false;
            currentTextElement = currentTextElement.nextTexts[currentTextElement.choiceTexts.FindIndex(i => i == choiceName)];
            HideChoices();
            DisplayNextScentence();
        }

        void ShowChoices()
        {
            choiceButtons = new List<GameObject>();
            for (int i = 0; i < currentTextElement.choiceTexts.Count; i++)
            {
                choiceButtons.Add(Instantiate(choiceButtonObject, choiceButtonParent.transform));
                choiceButtons[i].GetComponentInChildren<Text>().text = currentTextElement.choiceTexts[i];
            }

            animator.SetBool("Choices", true);
        }

        void HideChoices()
        {
            animator.SetBool("Choices", false);

            foreach (GameObject button in choiceButtons)
            {
                Destroy(button);
            }
        }

        IEnumerator ChangeName()
        {
            nameText.text = currentTextElement.name;
            nameText.resizeTextForBestFit = true;
            yield return new WaitForSeconds(.2f);
            nameText.resizeTextForBestFit = false;
        }

        IEnumerator TypeSentence(string sentence)
        {
            currentSentence = sentence;
            stopTyping = false;

            dialogueText.text = "";
            for (int i = 0; i < sentence.Length; i++)
            {
                if (sentence[i] == '@')
                {
                    if (sentence[i + 1] == '@')
                    {
                        dialogueText.text += "@";
                        i++;
                        continue;
                    }
                    string command = sentence.Substring(i + 1, 3);

                    string parameterString = Regex.Match(sentence.Substring(i + 4, sentence.Length - i - 4), @"^\d+").Value;
                    int parameterInt;
                    int.TryParse(parameterString, out parameterInt);

                    if (command == "Del")
                    {
                        dialogueText.text = dialogueText.text.Substring(0, i - parameterInt);

                    }
                    else if (command == "Skp")
                    {
                        dialogueText.text += sentence.Substring(i + 4 + parameterString.Length, parameterInt);
                        i += parameterInt;

                    }
                    else if (command == "Jmp")
                    {   //Not used
                        i += parameterInt;

                    }
                    else if (command == "Out")
                    {
                        dialogueOutput = parameterInt;

                    }
                    else if (command == "End")
                    {
                        dialogueOutput = parameterInt;
                        EndDialogue();
                        goto endTyping;
                    }
                    i += 3 + parameterString.Length;
                    continue;
                }

                every4thLetter = (every4thLetter + 1) % 4;
                if (!stopTyping && every4thLetter == 0)
                    PlayRandomSound();

                dialogueText.text += sentence[i];
                if (!stopTyping)
                    yield return new WaitForSeconds(1f / 60f);
            }

        endTyping:
            currentSentence = "";
            stopTyping = false;
        }

        public void EndDialogue()
        {
            finished = true;
            if (unlockedMouse)
            {
                FalseTruth.GameController.LockMouse();
            }
            else
            {
                FalseTruth.GameController.UnlockMouse();
            }
            animator.SetBool("IsOpen", false);
        }

        public bool isFinished()
        {
            return finished;
        }

        void PlayRandomSound()
        {
            for (int i = 0; i < audioSources.Count; i++)
            {
                if (!audioSources[i].isPlaying)
                {
                    currentAudioSource = i;
                    break;
                }
                if (i == audioSources.Count - 1)
                    currentAudioSource = (currentAudioSource + 1) % audioSources.Count;

            }
            AudioSource audioSource = audioSources[currentAudioSource];

            if (!audioSource.isPlaying)
            {
                audioSource.pitch = 1f + Random.Range(-.2f, .2f);
                audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
                audioSource.Play();
            }
        }
    }
}