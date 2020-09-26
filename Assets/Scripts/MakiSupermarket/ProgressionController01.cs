using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace MakiSupermarket
{
    public class ProgressionController01 : MonoBehaviour, ItemObserver
    {
        [HideInInspector] public ProgressionState currentState = ProgressionState.GetMop;

        public PlayerController playerController;
        public DialogueManager dialogueManager;
        [Space(10)]
        public int puddleCount = 6;
        int cleanedPuddles = 0;
        public List<string> boxesOnShelves = new List<string>() { "Box", "Circle", "Line", "Star" };
        public int shelfCount = 4;
        int refilledShelves = 0;
        [Space(10)]
        public GameObject bloodyPuddle;
        [Space(10)]
        public Material lightMaterial;
        public Transform lightsTransform;
        Light[] lights;
        float lightIntensity;
        [Space(10)]
        public Collider[] boxes;
        public Collider[] emptyShelves;
        public BoxFallDownTrigger boxFallDownTrigger;
        public Image symbolImage;
        bool isHoldingBox = false;
        [Space(10)]
        public Collider fuseBoxCollider;
        public Collider flashlightCollider;
        public Collider backDoorStopPlayer;
        [Space(10)]
        public Animator phoneAnim;
        public Animator symbolAnim;
        public Animator boxFallAnim;
        [Space(10)]
        public GameObject black;
        public GameObject bones;
        public Collider[] mopDrops;
        [Space(10)]
        public Animator autoDoorDisappearAnim;
        public Transform autoDoorAnim;
        public Transform autoDoorLook;
        public Door lockedBackExit;
        [Space(10)]
        public AudioSource audioSource;
        public AudioClip LightsOutClip;
        public AudioClip lightsOnClip;
        public AudioClip neonClip;
        [Space(10)]
        public Animator fadeAnim;
        public Transform helpTransform;

        void Start()
        {
            fuseBoxCollider.enabled = false;
            flashlightCollider.enabled = false;
            playerController.itemObservers.Add(this);

            lights = lightsTransform.GetComponentsInChildren<Light>();
            lightIntensity = lights[0].intensity;

            foreach (Collider emptyShelf in emptyShelves)
                emptyShelf.enabled = false;
            foreach (Collider box in boxes)
                box.enabled = false;
            foreach (Collider mopDrop in mopDrops)
                mopDrop.enabled = false;
        }

        public void NextLevel()
        {
            if (currentState == ProgressionState.CheckOneOtherDoor)
            {
                currentState = ProgressionState.Exit;
                StartCoroutine(LoadScene("MakiSupermarket02"));
            }
        }

        IEnumerator LoadScene(string sceneName)
        {
            fadeAnim.SetTrigger("Out");
            yield return new WaitForSeconds(3f);

            SceneManager.LoadScene(sceneName);
        }

        public void AutomaticDoorDisappearTrigger()
        {
            if (currentState == ProgressionState.TryToGoOut)
            {
                currentState = ProgressionState.CheckOneOtherDoor;
                lockedBackExit.lockedMessage = "";
                StartCoroutine(AutomaticDoorDisappear());
            }
        }

        IEnumerator AutomaticDoorDisappear()
        {
            playerController.SetCanMove(false);
            autoDoorDisappearAnim.SetTrigger("Disappear");
            StartCoroutine(playerController.RotatePlayer(Quaternion.LookRotation(autoDoorLook.position - playerController.camTransform.position), 50));
            yield return new WaitForSeconds(2f);
            autoDoorAnim.gameObject.SetActive(false);
            yield return dialogueManager.StartDialogue(Dialogue.AutoDoorDisappearDialogue());
            playerController.SetCanMove(true);
        }

        public void BackDoorTrigger()
        {
            if (currentState == ProgressionState.GetFlashlight)
            {
                backDoorStopPlayer.enabled = true;
                StartCoroutine(StopPlayerWithoutFlashlight());
            }
            else if (currentState == ProgressionState.FuseBox)
            {
                backDoorStopPlayer.enabled = false;
            }
            else if (currentState == ProgressionState.BringBackMop1)
            {
                currentState = ProgressionState.InspectFallenBox;
                backDoorStopPlayer.enabled = true;
                StartCoroutine(InspectFallenBox());
            }
            else if (currentState == ProgressionState.InspectFallenBox)
            {
                StartCoroutine(InspectFallenBoxStopPlayer());
            }
            else if (currentState == ProgressionState.BringBackMop2)
            {
                backDoorStopPlayer.enabled = false;
            }
        }

        IEnumerator StopPlayerWithoutFlashlight()
        {
            playerController.SetCanMove(false);
            StartCoroutine(playerController.RotatePlayer(Quaternion.LookRotation(boxFallAnim.transform.position - playerController.transform.position), 50));
            yield return dialogueManager.StartDialogue(Dialogue.OneLineMonologue("It's too dark without a flashlight! I need the one from the counter!"));
            playerController.SetCanMove(true);
        }

        public IEnumerator InspectingFallenBox()
        {
            playerController.SetCanMove(false);
            Vector3 bonesPos = bones.transform.position;
            bonesPos.y = 0;
            StartCoroutine(playerController.RotatePlayer(Quaternion.LookRotation(bonesPos - playerController.camTransform.position), 50));
            yield return dialogueManager.StartDialogue(Dialogue.InspectingBoxDialogue());
            black.SetActive(true);
            bones.SetActive(false);
            boxFallAnim.SetTrigger("Reset");
            audioSource.clip = lightsOnClip;
            audioSource.Play();

            yield return new WaitForSeconds(0.5f);
            black.SetActive(false);
            StartCoroutine(playerController.RotatePlayer(Quaternion.LookRotation(boxFallAnim.transform.position - playerController.transform.position), 50));
            yield return dialogueManager.StartDialogue(Dialogue.InspectingBoxDialogue2());
            playerController.SetCanMove(true);

            currentState = ProgressionState.BringBackMop2;
            foreach (Collider mopDrop in mopDrops)
                mopDrop.enabled = true;
        }

        IEnumerator InspectFallenBox()
        {
            playerController.SetCanMove(false);
            boxFallAnim.SetTrigger("Fall");
            StartCoroutine(playerController.RotatePlayer(Quaternion.LookRotation(boxFallAnim.transform.position - playerController.transform.position), 50));
            yield return new WaitForSecondsPaused(1f, PauseManager.isPaused());
            yield return dialogueManager.StartDialogue(Dialogue.OneLineMonologue("What was that?"));
            playerController.SetCanMove(true);
        }

        IEnumerator InspectFallenBoxStopPlayer()
        {
            playerController.SetCanMove(false);
            StartCoroutine(playerController.RotatePlayer(Quaternion.LookRotation(boxFallAnim.transform.position - playerController.transform.position), 50));
            yield return dialogueManager.StartDialogue(Dialogue.OneLineMonologue("I should inspect that box."));
            playerController.SetCanMove(true);
        }

        public bool AddBox(string symbolName, Sprite symbolSprite)
        {
            if (isHoldingBox)
            {
                StartCoroutine(dialogueManager.StartDialogue(Dialogue.OneLineMonologue("§iI already have a box.")));
                return false;
            }
            else
            {
                boxesOnShelves.Remove(symbolName);
                isHoldingBox = true;
                playerController.AddCollectedItem("Box" + symbolName);
                symbolImage.sprite = symbolSprite;
                symbolAnim.SetBool("Shown", true);
                return true;
            }
        }

        IEnumerator LastShelf()
        {
            currentState = ProgressionState.BringBackMop1;
            playerController.SetCanMove(false);
            yield return dialogueManager.StartDialogue(Dialogue.LastShelfDialogue());
            playerController.SetCanMove(true);
        }

        public bool AddShelf(string symbolName)
        {
            if (!isHoldingBox)
            {
                StartCoroutine(dialogueManager.StartDialogue(Dialogue.OneLineMonologue("§iI need a box first.")));
                return false;
            }
            else if (!playerController.CollectedItemsContains("Box" + symbolName))
            {
                StartCoroutine(dialogueManager.StartDialogue(Dialogue.OneLineMonologue("§iWrong shelf.")));
                return false;
            }
            else
            {
                isHoldingBox = false;
                symbolAnim.SetBool("Shown", false);
                if (++refilledShelves == shelfCount)
                {
                    StartCoroutine(LastShelf());
                }
                else if (refilledShelves == shelfCount - 1)
                {
                    boxFallDownTrigger.enabledTrigger = true;
                    boxFallDownTrigger.box = ((Box)FindObjectOfType(typeof(Box))).transform;
                }
                return true;
            }
        }

        public void AddPuddle()
        {
            if (++cleanedPuddles == puddleCount)
            {
                StartCoroutine(LightsOut());
            }
            if (cleanedPuddles == puddleCount + 1)
            {
                StartCoroutine(FinishedPuddles());
            }
        }

        IEnumerator FinishedPuddles()
        {
            currentState = ProgressionState.RefillShelves;
            foreach (Collider emptyShelf in emptyShelves)
                emptyShelf.enabled = true;
            foreach (Collider box in boxes)
                box.enabled = true;

            playerController.SetCanMove(false);
            yield return dialogueManager.StartDialogue(Dialogue.FinishedPuddlesDialogue());
            playerController.SetCanMove(true);
        }

        IEnumerator LightsOut()
        {
            audioSource.clip = LightsOutClip;
            audioSource.Play();

            lightMaterial.DisableKeyword("_EMISSION");
            phoneAnim.SetBool("Shown", true);
            currentState = ProgressionState.GetFlashlight;
            foreach (Light light in lights)
                light.intensity = 0;

            playerController.SetCanMove(false);
            yield return dialogueManager.StartDialogue(Dialogue.LightsOutDialogue());
            playerController.SetCanMove(true);

            flashlightCollider.enabled = true;
        }

        public IEnumerator LightsOn()
        {
            audioSource.clip = lightsOnClip;
            audioSource.Play();

            lightMaterial.EnableKeyword("_EMISSION");
            playerController.RemoveCollectedItem("Flashlight");
            bloodyPuddle.SetActive(true);
            currentState = ProgressionState.CleanBloodyPuddle;
            foreach (Light light in lights)
                light.intensity = lightIntensity;

            playerController.SetCanMove(false);
            yield return dialogueManager.StartDialogue(Dialogue.LightsOnDialogue());
            playerController.SetCanMove(true);
            flashlightCollider.enabled = true;
        }

        public string GetHint()
        {
            switch (currentState)
            {
                case ProgressionState.GetMop:
                    return "There should be a mop in a locker in the storage room.";
                case ProgressionState.CleanPuddles:
                    if (cleanedPuddles == puddleCount - 1)
                        return "There is still a puddle left.";
                    else
                        return "There are still " + (puddleCount - cleanedPuddles) + " puddles left.";
                case ProgressionState.GetFlashlight:
                    return "There is a flashlight below this counter.";
                case ProgressionState.FuseBox:
                    return "The fuse box is on the opposite side of the lockers.";
                case ProgressionState.CleanBloodyPuddle:
                    return "There is still a puddle left.";
                case ProgressionState.RefillShelves:
                    if (refilledShelves == shelfCount - 1)
                        return "There is still an empty shelf.";
                    else
                        return "There are still " + (shelfCount - refilledShelves) + " empty shelves left.";
                case ProgressionState.BringBackMop1:
                case ProgressionState.BringBackMop2:
                    return "Everything is done, I should bring back the Mop.";
                case ProgressionState.InspectFallenBox:
                    return "I should inspect the box on the floor.";
                case ProgressionState.TryToGoOut:
                    return "Time to go out.";
                case ProgressionState.CheckOneOtherDoor:
                    return "There should be another exit in the back.";
                default:
                    Debug.Log("Hier fehlt ein Tipp!");
                    return "...";
            }
        }

        void ItemObserver.UpdateItemStatus(string itemName, bool status)
        {
            if (itemName.Equals("Mop") && status == true)
            {
                currentState = ProgressionState.CleanPuddles;
            }
            else if (itemName.Equals("Flashlight") && status == true)
            {
                currentState = ProgressionState.FuseBox;
                phoneAnim.SetBool("Shown", false);
                fuseBoxCollider.enabled = true;
            }
            if (itemName.Equals("Mop") && status == false)
            {
                currentState = ProgressionState.TryToGoOut;
            }
        }

        void OnApplicationQuit()
        {
            lightMaterial.EnableKeyword("_EMISSION");
        }
    }

    public enum ProgressionState
    {
        GetMop,
        CleanPuddles,
        GetFlashlight,
        FuseBox,
        CleanBloodyPuddle,
        RefillShelves,
        BringBackMop1,
        InspectFallenBox,
        BringBackMop2,
        TryToGoOut,
        CheckOneOtherDoor,
        Exit
    }
}