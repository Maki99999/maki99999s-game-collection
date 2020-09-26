using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace SportsGame
{
    public class MainController : MonoBehaviour
    {

        public Animator door;
        public Transform menuCam;
        public Transform camMainPos;
        public Transform camOptsPos;
        public Transform camPausePos;
        public Transform[] camGamePos;
        public FakeController fakeController;
        public Animator canvasAnimator;
        public Text playGameButtonText;
        public Text playGameTitleText;
        public Text playGameDescriptionText;
        public GameObject playGameLeaderboardObject;

        [Space(20)]
        public GameControllerTag gameControllerTag;
        public Transform camTagDoorPos;

        public GameControllerDodge gameControllerDodge;

        public GameControllerParkour gameControllerParkour;

        public GameControllerThrow gameControllerThrow;
        public Transform camThrowDoorPos;

        [Space(20)]
        public Text scoreTimeText;
        public Text scoreCustomText;
        public Text nameInputText;

        Text[] playGameLeaderboardTexts;

        GameController currentGame;
        int gameCount;
        int selectedGame = 0;
        bool camMoving = false;
        bool stopCamMovement = false;
        bool inPause = false;
        bool inMenu = false;

        float scoreTimePassed;
        float scoreTimeStart;
        float finalScoreTime = -1f;
        bool isScoreTimeRunning = false;

        float scoreCustom;
        string scoreCustomUnit;

        LinkedList<string>[] scoreNameList;
        LinkedList<float>[] scoreValueList;
        LinkedList<float> scoreCustomValueList;

        readonly string[] prefStrings = new string[] { "SportsGameScoreTag", "SportsGameScoreDodge", "SportsGameScoreParkour", "SportsGameScoreThrow" };

        const char splitScoresSymbol = ';';
        const char splitScoreNameSymbol = ':';

        float pressMaxDelay = 0.5f;
        float pressTimer = 0f;
        int pressCount = 0;

        void Awake()
        {
            GetScoreValues();
        }

        void Start()
        {
            playGameLeaderboardTexts = playGameLeaderboardObject.transform.GetComponentsInChildren<Text>();
            gameCount = camGamePos.Length;
            StartCoroutine(BeginningScene());
        }

        void Update()
        {
            if (inMenu && Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else if (!inMenu && Cursor.lockState != CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            if (isScoreTimeRunning)
                UpdateScoreTimeText();

            if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9))
            {
                pressCount++;
                pressTimer = Time.time;
            }
            if (pressTimer + pressMaxDelay < Time.time)
            {
                pressCount = 0;
                pressTimer = 0;
            }
            if (pressCount == 5)
            {
                pressCount = 0;
                pressTimer = 0;
                currentGame.Hack();
            }
        }

        void GetScoreValues()
        {
            scoreValueList = new LinkedList<float>[4];
            scoreNameList = new LinkedList<string>[4];
            scoreCustomValueList = new LinkedList<float>();

            for (int i = 0; i < prefStrings.Length; i++)
            {
                string scoreListString = PlayerPrefs.GetString(prefStrings[i], "");
                scoreValueList[i] = new LinkedList<float>();
                scoreNameList[i] = new LinkedList<string>();
                if (scoreListString.Length > 2)
                {
                    foreach (string scoreItem in scoreListString.Split(splitScoresSymbol))
                    {
                        string[] scoreItemSplit = scoreItem.Split(splitScoreNameSymbol);

                        scoreNameList[i].AddLast(scoreItemSplit[0]);

                        if (scoreItemSplit[1][scoreItemSplit[1].Length - 1] == 'm')
                            scoreCustomValueList.AddLast(float.Parse(scoreItemSplit[1].Remove(scoreItemSplit[1].Length - 1)));
                        else
                            scoreValueList[i].AddLast(float.Parse(scoreItem.Split(splitScoreNameSymbol)[1]));
                    }
                }
            }
        }

        void SaveScoreValues()
        {
            for (int i = 0; i < prefStrings.Length; i++)
            {
                string prefString = "";
                LinkedListNode<string> currentNameNode = scoreNameList[i].First;
                LinkedListNode<float> currentValueNode = scoreValueList[i].First;

                for (int j = 0; j < scoreValueList[i].Count; j++)
                {
                    prefString += currentNameNode.Value + splitScoreNameSymbol + currentValueNode.Value + splitScoresSymbol;

                    currentNameNode = currentNameNode.Next;
                    currentValueNode = currentValueNode.Next;
                }

                if (i == 2)
                {
                    foreach (float customValue in scoreCustomValueList)
                    {
                        prefString += currentNameNode.Value + splitScoreNameSymbol + customValue + 'm' + splitScoresSymbol;

                        currentNameNode = currentNameNode.Next;
                    }
                }

                if (prefString.Length > 0)
                    prefString = prefString.Remove(prefString.Length - 1);

                PlayerPrefs.SetString(prefStrings[i], prefString);
            }
        }

        void DisplayLeaderboard(int id)
        {
            LinkedListNode<string> currentNameNode = scoreNameList[id].First;
            LinkedListNode<float> currentValueNode = scoreValueList[id].First;
            LinkedListNode<float> currentCustomValueNode = scoreCustomValueList.First;

            int min = Mathf.Min(scoreValueList[id].Count, 10);

            for (int i = 1; i <= 10; i++)
            {
                if (i <= min)
                {
                    playGameLeaderboardTexts[i].text = currentNameNode.Value + " - " + currentValueNode.Value;
                    currentNameNode = currentNameNode.Next;
                    currentValueNode = currentValueNode.Next;
                }
                else
                {
                    if (id == 2 && currentNameNode != null)
                    {
                        playGameLeaderboardTexts[i].text = currentNameNode.Value + " - " + currentCustomValueNode.Value + 'm';
                        currentNameNode = currentNameNode.Next;
                        currentCustomValueNode = currentCustomValueNode.Next;
                    }
                    else
                    {
                        playGameLeaderboardTexts[i].text = "";
                    }
                }
            }
        }

        void UpdateScoreTimeText()
        {
            float secondsPassed = scoreTimePassed + (Time.time - scoreTimeStart);
            scoreTimeText.text = Mathf.Floor(secondsPassed / 60f) + ":" + (secondsPassed % 60).ToString("0.00");
        }

        public void StartScoreTime()
        {
            scoreTimePassed = 0f;
            scoreTimeStart = Time.time;
            isScoreTimeRunning = true;
        }

        public void EndScoreTime(bool timeValid = true)
        {
            isScoreTimeRunning = false;
            finalScoreTime = timeValid ? scoreTimePassed + (Time.time - scoreTimeStart) : -1f;
            UpdateScoreTimeText();
        }

        void PauseScoreTime()
        {
            isScoreTimeRunning = false;
            scoreTimePassed = scoreTimePassed + (Time.time - scoreTimeStart);
        }

        void UnpauseScoreTime()
        {
            scoreTimeStart = Time.time;
            isScoreTimeRunning = true;
        }

        public void SetCustomScore(float newScore, string unit = "")
        {
            scoreCustom = newScore;
            scoreCustomUnit = unit;
            scoreCustomText.text = newScore + " " + unit;
        }

        bool NewScore()
        {
            switch (currentGame.type)
            {
                case GameType.Parkour:  //Higher distance is better or if completed lower time
                    if (scoreValueList[selectedGame].Count + scoreCustomValueList.Count < 10) return true;
                    if (finalScoreTime < 0)
                        return (scoreCustomValueList.Count == 0 && scoreValueList[selectedGame].Count < 10) || scoreCustom > scoreCustomValueList.Last.Value;
                    else
                        return scoreValueList[selectedGame].Count < 10 || finalScoreTime < scoreValueList[selectedGame].Last.Value;
                case GameType.Throw:    //Lower time is better
                    return scoreValueList[selectedGame].Count < 10 || finalScoreTime < scoreValueList[selectedGame].Last.Value;
                default:                //Higher time is better
                    return scoreValueList[selectedGame].Count < 10 || finalScoreTime > scoreValueList[selectedGame].Last.Value;
            }
        }

        public void SubmitLeaderboardEntry()
        {
            string name = nameInputText.text.Length > 0 ? nameInputText.text : "Anonymous";

            LinkedListNode<float> currentValueNode = scoreValueList[selectedGame].First;
            LinkedListNode<string> currentNameNode = scoreNameList[selectedGame].First;

            switch (currentGame.type)
            {
                case GameType.Parkour:
                    if (finalScoreTime < 0)
                    {
                        currentValueNode = scoreCustomValueList.First;
                        for (int i = 0; i < scoreValueList[selectedGame].Count; i++)
                            currentNameNode = currentNameNode.Next;

                        while (currentValueNode != null && currentValueNode.Next != null && scoreCustom <= currentValueNode.Next.Value)
                        {
                            currentValueNode = currentValueNode.Next;
                            currentNameNode = currentNameNode.Next;
                        }

                        if (currentValueNode == null)
                        {
                            scoreCustomValueList.AddFirst(scoreCustom);

                            if (scoreNameList[selectedGame].First == null)
                                scoreNameList[selectedGame].AddFirst(name);
                            else
                                scoreNameList[selectedGame].AddAfter(currentNameNode, name);
                        }
                        else if (scoreCustom > currentValueNode.Value)
                        {
                            scoreCustomValueList.AddBefore(currentValueNode, scoreCustom);
                            scoreNameList[selectedGame].AddBefore(currentNameNode, name);
                        }
                        else
                        {
                            scoreCustomValueList.AddAfter(currentValueNode, scoreCustom);
                            scoreNameList[selectedGame].AddAfter(currentNameNode, name);
                        }
                    }
                    else
                    {
                        while (currentValueNode != null && currentValueNode.Next != null && finalScoreTime >= currentValueNode.Next.Value)
                        {
                            currentValueNode = currentValueNode.Next;
                            currentNameNode = currentNameNode.Next;
                        }

                        if (currentValueNode == null)
                        {
                            scoreValueList[selectedGame].AddFirst(finalScoreTime);
                            scoreNameList[selectedGame].AddFirst(name);
                        }
                        else if (finalScoreTime < currentValueNode.Value)
                        {
                            scoreValueList[selectedGame].AddBefore(currentValueNode, finalScoreTime);
                            scoreNameList[selectedGame].AddBefore(currentNameNode, name);
                        }
                        else
                        {
                            scoreValueList[selectedGame].AddAfter(currentValueNode, finalScoreTime);
                            scoreNameList[selectedGame].AddAfter(currentNameNode, name);
                        }
                    }

                    if (scoreValueList[selectedGame].Count + scoreCustomValueList.Count > 10)
                    {
                        scoreNameList[selectedGame].RemoveLast();

                        if (scoreCustomValueList.Count > 0)
                            scoreCustomValueList.RemoveLast();
                        else
                            scoreValueList[selectedGame].RemoveLast();
                    }
                    break;

                case GameType.Throw:
                    while (currentValueNode != null && currentValueNode.Next != null && finalScoreTime >= currentValueNode.Next.Value)
                    {
                        currentValueNode = currentValueNode.Next;
                        currentNameNode = currentNameNode.Next;
                    }

                    if (currentValueNode == null)
                    {
                        scoreValueList[selectedGame].AddFirst(finalScoreTime);
                        scoreNameList[selectedGame].AddFirst(name);
                    }
                    else if (finalScoreTime < currentValueNode.Value)
                    {
                        scoreValueList[selectedGame].AddBefore(currentValueNode, finalScoreTime);
                        scoreNameList[selectedGame].AddBefore(currentNameNode, name);
                    }
                    else
                    {
                        scoreValueList[selectedGame].AddAfter(currentValueNode, finalScoreTime);
                        scoreNameList[selectedGame].AddAfter(currentNameNode, name);
                    }

                    if (scoreValueList[selectedGame].Count > 10)
                    {
                        scoreValueList[selectedGame].RemoveLast();
                        scoreNameList[selectedGame].RemoveLast();
                    }
                    break;

                default:
                    while (currentValueNode != null && currentValueNode.Next != null && finalScoreTime <= currentValueNode.Next.Value)
                    {
                        currentValueNode = currentValueNode.Next;
                        currentNameNode = currentNameNode.Next;
                    }

                    if (currentValueNode == null)
                    {
                        scoreValueList[selectedGame].AddFirst(finalScoreTime);
                        scoreNameList[selectedGame].AddFirst(name);
                    }
                    else if (finalScoreTime > currentValueNode.Value)
                    {
                        scoreValueList[selectedGame].AddBefore(currentValueNode, finalScoreTime);
                        scoreNameList[selectedGame].AddBefore(currentNameNode, name);
                    }
                    else
                    {
                        scoreValueList[selectedGame].AddAfter(currentValueNode, finalScoreTime);
                        scoreNameList[selectedGame].AddAfter(currentNameNode, name);
                    }

                    if (scoreValueList[selectedGame].Count > 10)
                    {
                        scoreValueList[selectedGame].RemoveLast();
                        scoreNameList[selectedGame].RemoveLast();
                    }
                    break;
            }

            SaveScoreValues();
            DisplayLeaderboard(selectedGame);
            canvasAnimator.SetBool("Leaderboard", false);
        }

        IEnumerator BeginningScene()
        {
            inMenu = false;
            door.SetBool("Open", true);
            yield return new WaitForSeconds(2.5f);
            StartCoroutine(MoveCam(camMainPos, 60));
            yield return new WaitForSeconds(1.3f);
            canvasAnimator.SetTrigger("MainMenu");
            door.SetBool("Open", false);
            inMenu = true;
        }

        IEnumerator MoveCam(Transform newPosRot, int frameCount)
        {
            yield return MoveCam(newPosRot.position, newPosRot.rotation, frameCount);
        }

        IEnumerator MoveCam(Vector3 newPosition, Quaternion newRotation, int frameCount)
        {
            if (camMoving)
            {
                stopCamMovement = true;
                yield return new WaitUntil(() => !camMoving);
                stopCamMovement = false;
            }
            camMoving = true;
            Vector3 oldPosition = menuCam.position;
            Quaternion oldRotation = menuCam.rotation;
            for (float i = 0f; i <= 1; i += 1f / frameCount)
            {
                menuCam.position = Vector3.Lerp(oldPosition, newPosition, Mathf.SmoothStep(0f, 1f, i));
                menuCam.rotation = Quaternion.Lerp(oldRotation, newRotation, Mathf.SmoothStep(0f, 1f, i));
                if (stopCamMovement)
                {
                    break;
                }
                yield return new WaitForSeconds(1f / 60f);
            }
            camMoving = false;
        }

        public static IEnumerator MoveObject(Transform obj, Transform newPosRot, int frameCount)
        {
            yield return MoveObject(obj, newPosRot.position, newPosRot.rotation, frameCount);
        }

        public static IEnumerator MoveObject(Transform obj, Vector3 newPosition, Quaternion newRotation, int frameCount)
        {
            Vector3 oldPosition = obj.position;
            Quaternion oldRotation = obj.rotation;
            for (float i = 0f; i <= 1; i += 1f / frameCount)
            {
                obj.position = Vector3.Lerp(oldPosition, newPosition, Mathf.SmoothStep(0f, 1f, i));
                obj.rotation = Quaternion.Lerp(oldRotation, newRotation, Mathf.SmoothStep(0f, 1f, i));
                yield return new WaitForSeconds(1f / 60f);
            }
        }

        public void BackToMainMenu()
        {
            StartCoroutine(LoadScene("MainMenu"));
        }

        public void Options()
        {
            StartCoroutine(MoveCam(camOptsPos, 60));
            canvasAnimator.SetTrigger("Options");
        }

        public void Back()
        {
            StartCoroutine(MoveCam(camMainPos, 60));
            canvasAnimator.SetBool("Game", false);
            canvasAnimator.SetTrigger("MainMenu");
        }

        public void Play()
        {
            canvasAnimator.SetBool("Game", true);
            ChangeGame(selectedGame);
        }

        public void PlayGame()
        {
            inMenu = false;
            scoreTimeText.text = "0:00";
            scoreCustomText.text = "";
            canvasAnimator.SetTrigger("Play");
            StartCoroutine(GamesStart());
        }

        public void Next()
        {
            ChangeGame((selectedGame + 1) % gameCount);
        }

        public void Prev()
        {
            ChangeGame(selectedGame == 0 ? gameCount - 1 : selectedGame - 1);
        }

        void ChangeGame(int id)
        {
            selectedGame = id;
            StartCoroutine(MoveCam(camGamePos[id], 60));
            DisplayLeaderboard(id);
            string title = "";
            string description = "";
            switch (id)
            {
                case 0:
                    title += "Tag";
                    description = "Avoid everything red.";
                    break;
                case 1:
                    title += "Dodge";
                    description = "Dodge the balls.";
                    break;
                case 2:
                    title += "Parkour";
                    description = "Jump and Run.";
                    break;
                case 3:
                    title += "Throw";
                    description = "Hit Everyone.";
                    break;
                default:
                    title += "???";
                    description = "???";
                    break;
            }
            StartCoroutine(fakeController.ShowAll(id != 3));
            playGameButtonText.text = "Play " + title;
            playGameTitleText.text = title;
            playGameDescriptionText.text = description;
        }

        public IEnumerator WinAnimation(Transform playerCam)
        {
            menuCam.position = playerCam.position;
            menuCam.rotation = playerCam.rotation;
            menuCam.gameObject.SetActive(true);
            playerCam.parent.gameObject.SetActive(false);
            yield return new WaitForSeconds(1f);

            for (float i = 0f; i <= 1; i += 1f / 60)
            {
                menuCam.position = Vector3.Lerp(playerCam.position, playerCam.position + Vector3.forward, Mathf.SmoothStep(0f, 1f, i));
                menuCam.rotation = Quaternion.Lerp(playerCam.rotation, Quaternion.Euler(0, -180, 0), Mathf.SmoothStep(0f, 1f, i));
                yield return new WaitForSeconds(1f / 60f);
            }

            float circleSize = 1f;
            float circleGrowSpeed = 0.01f;

            canvasAnimator.SetBool("Game", false);
            canvasAnimator.SetTrigger("Win");

            for (int i = 0; i < 600; i++)
            {
                float xPos = Mathf.Sin(0.02f * i) * circleSize;
                float yPos = circleSize - 1f;
                float zPos = Mathf.Cos(0.02f * i) * circleSize;

                if (circleSize < 10)
                    circleSize += circleGrowSpeed;

                menuCam.position = playerCam.position + new Vector3(xPos, yPos, zPos);
                menuCam.LookAt(playerCam.position);

                yield return new WaitForSeconds(1f / 60f);
            }

            yield return fakeController.ShowAll(selectedGame != 3);
            if (NewScore())
                canvasAnimator.SetBool("Leaderboard", true);
            else
                canvasAnimator.SetBool("Leaderboard", false);
            canvasAnimator.SetBool("Game", true);
            inMenu = true;
            yield return MoveCam(camGamePos[selectedGame], 60);
        }

        public IEnumerator LooseAnimation(Transform playerCam)
        {
            menuCam.position = playerCam.position;
            menuCam.rotation = playerCam.rotation;

            menuCam.gameObject.SetActive(true);
            playerCam.parent.gameObject.SetActive(false);

            yield return MoveCam(playerCam.position + Vector3.up * 4, Quaternion.Euler(89, 0, 0), 60);
            canvasAnimator.SetBool("Game", false);
            canvasAnimator.SetTrigger("Loose");

            float endTime = Time.time + 7f;
            while (Time.time < endTime)
            {
                menuCam.Rotate(Vector3.forward * Time.deltaTime * 60);
                yield return null;
            }

            StartCoroutine(MoveCam(camGamePos[selectedGame], 60));
            StartCoroutine(fakeController.ShowAll(selectedGame != 3));
            if (NewScore())
                canvasAnimator.SetBool("Leaderboard", true);
            else
                canvasAnimator.SetBool("Leaderboard", false);
            canvasAnimator.SetBool("Game", true);
            inMenu = true;
        }

        IEnumerator GamesStart()
        {
            switch (selectedGame)
            {
                case 0:
                    currentGame = gameControllerTag;
                    yield return fakeController.Hide(currentGame.type);
                    currentGame.player.SetActive(false);
                    yield return MoveCam(camTagDoorPos, 60);
                    currentGame.Init();
                    yield return new WaitForSeconds(gameControllerTag.npcSpawner.delayBetweenSpawns * (gameControllerTag.npcSpawner.countOfAllNpcs / 2));
                    break;
                case 1:
                    currentGame = gameControllerDodge;
                    yield return fakeController.Hide(currentGame.type);
                    currentGame.player.SetActive(false);
                    currentGame.Init();
                    break;
                case 2:
                    currentGame = gameControllerParkour;
                    yield return fakeController.Hide(currentGame.type);
                    currentGame.player.SetActive(false);
                    currentGame.Init();
                    break;
                case 3:
                    currentGame = gameControllerThrow;
                    yield return fakeController.Hide(currentGame.type);
                    currentGame.player.SetActive(false);
                    yield return MoveCam(camThrowDoorPos, 60);
                    currentGame.Init();
                    yield return new WaitForSeconds(gameControllerTag.npcSpawner.delayBetweenSpawns * (gameControllerTag.npcSpawner.countOfAllNpcs / 2));
                    break;
                default:
                    break;
            }
            yield return MoveCam(currentGame.playerCam, 60);
            currentGame.player.SetActive(true);
            menuCam.gameObject.SetActive(false);
            currentGame.inGame = true;
        }

        public IEnumerator Pause()
        {
            PauseScoreTime();
            inMenu = true;
            canvasAnimator.SetBool("Game", false);
            inPause = true;
            menuCam.position = currentGame.playerCam.position;
            menuCam.rotation = currentGame.playerCam.rotation;
            menuCam.gameObject.SetActive(true);
            currentGame.player.SetActive(false);

            yield return MoveCam(camPausePos, 60);
            canvasAnimator.SetBool("Paused", true);

            while (inPause)
            {
                menuCam.RotateAround(menuCam.position, Vector3.up, 0.2f);
                yield return new WaitForSeconds(1f / 60f);
            }
        }

        public void Resume()
        {
            StartCoroutine(Unpause());
        }

        IEnumerator Unpause()
        {
            UnpauseScoreTime();
            inMenu = false;
            canvasAnimator.SetBool("Game", false);
            canvasAnimator.SetBool("Paused", false);
            yield return MoveCam(currentGame.playerCam, 60);
            currentGame.player.SetActive(true);
            menuCam.gameObject.SetActive(false);
            currentGame.getPaused().Value = false;
            inPause = false;
        }

        public void BackToMenuFromGame()
        {
            StartCoroutine(BackFromGameToMenu());
        }

        IEnumerator BackFromGameToMenu()
        {
            inPause = false;
            currentGame.Reset();

            canvasAnimator.SetBool("Game", true);
            canvasAnimator.SetBool("Paused", false);
            yield return fakeController.ShowAll(selectedGame != 3);

            ChangeGame(selectedGame);
        }

        IEnumerator LoadScene(string name)
        {
            canvasAnimator.SetTrigger("Out");
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(name);
        }
    }
}