using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace TicTacToe
{
    public class TTTGame : MonoBehaviour
    {

        public Button[] buttons;
        public Text winText;
        public Image player1ActiveImage;
        public Image player2ActiveImage;
        public Animator fadeBlack;
        public Image clickBlocker;

        [HideInInspector] public bool player1IsActive = true;
        bool player1isCPU;
        bool player2isCPU;

        AudioSource audioSource;
        RectTransform rect;
        float oldWidth;
        int[,] field;

        bool hasWon = false;

        void Start()
        {
            rect = GetComponent<RectTransform>();
            audioSource = GetComponent<AudioSource>();
            field = new int[3, 3];

            oldWidth = Mathf.Min(rect.rect.width, rect.rect.height);
            UpdateButtonSize();

            if (player1isCPU)
            {
                StartCoroutine(CPUsTurn());
            }
        }

        void LateUpdate()
        {
            float newWidth = Mathf.Min(rect.rect.width, rect.rect.height);
            if (oldWidth != newWidth)
            {
                UpdateButtonSize();
            }
            oldWidth = newWidth;
        }

        public void SetPlayer1Bot(bool isBot)
        {
            player1isCPU = isBot;

            if (player1IsActive && !hasWon)
                StartCoroutine(CPUsTurn());
        }

        public void SetPlayer2Bot(bool isBot)
        {
            player2isCPU = isBot;

            if (!player1IsActive && !hasWon)
                StartCoroutine(CPUsTurn());
        }

        public void ToMainMenu()
        {
            StartCoroutine(LoadScene("MainMenu"));
        }

        IEnumerator LoadScene(string name)
        {
            fadeBlack.SetTrigger("Out");
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(name);
        }

        public void Reset()
        {
            field = new int[3, 3];
            foreach (Button button in buttons)
            {
                button.GetComponent<Image>().sprite = null;
                button.interactable = true;
            }
            player1IsActive = true;
            player1ActiveImage.enabled = true;
            player2ActiveImage.enabled = false;
            winText.text = "";
            hasWon = false;

            if (player1isCPU)
                StartCoroutine(CPUsTurn());
        }

        public void UpdateButtonSize()
        {
            float width = Mathf.Min(rect.rect.width, rect.rect.height);
            Vector2 size = new Vector2(width, width) / 3;

            foreach (Button button in buttons)
            {
                RectTransform rectT = button.GetComponent<RectTransform>();
                rectT.sizeDelta = size;
            }
        }

        public void ClickedButton(int row, int column)
        {
            audioSource.Play();
            field[row, column] = player1IsActive ? 1 : 2;

            int hasWon = CheckIfWon(field);
            UpdateWinText(CheckIfWon(field));

            if (hasWon != 0)
            {
                clickBlocker.enabled = true;
                this.hasWon = true;
                return;
            }

            player1IsActive = !player1IsActive;
            UpdateActivePlayerImage();

            if (player1IsActive && player1isCPU)
            {
                clickBlocker.enabled = true;
                StartCoroutine(CPUsTurn());
            }
            else if (!player1IsActive && player2isCPU)
            {
                clickBlocker.enabled = true;
                StartCoroutine(CPUsTurn());
            }
            else
            {
                clickBlocker.enabled = false;
            }
        }

        void UpdateWinText(int won)
        {
            switch (won)
            {
                case -1:
                    winText.text = "Draw.";
                    break;
                case 0:
                    winText.text = "";
                    break;
                case 1:
                    winText.text = "X won.";
                    break;
                case 2:
                    winText.text = "O won.";
                    break;
            }
        }

        void UpdateActivePlayerImage()
        {
            if (player1IsActive)
            {
                player2ActiveImage.enabled = false;
                player1ActiveImage.enabled = true;
            }
            else
            {
                player1ActiveImage.enabled = false;
                player2ActiveImage.enabled = true;
            }
        }

        int CheckIfWon(int[,] field)
        {
            int won = 0;

            if (field[1, 1] == 1)
            {
                if ((field[0, 0] == 1 && field[2, 2] == 1) || (field[2, 0] == 1 && field[0, 2] == 1) || (field[0, 1] == 1 && field[2, 1] == 1) || (field[1, 0] == 1 && field[1, 2] == 1))
                    won = 1;
            }
            if (field[0, 0] == 1)
            {
                if ((field[1, 0] == 1 && field[2, 0] == 1) || (field[0, 1] == 1 && field[0, 2] == 1))
                    won = 1;
            }
            if (field[2, 2] == 1)
            {
                if ((field[1, 2] == 1 && field[0, 2] == 1) || (field[2, 1] == 1 && field[2, 0] == 1))
                    won = 1;
            }

            if (field[1, 1] == 2)
            {
                if ((field[0, 0] == 2 && field[2, 2] == 2) || (field[2, 0] == 2 && field[0, 2] == 2) || (field[0, 1] == 2 && field[2, 1] == 2) || (field[1, 0] == 2 && field[1, 2] == 2))
                    won = 2;
            }
            if (field[0, 0] == 2)
            {
                if ((field[1, 0] == 2 && field[2, 0] == 2) || (field[0, 1] == 2 && field[0, 2] == 2))
                    won = 2;
            }
            if (field[2, 2] == 2)
            {
                if ((field[1, 2] == 2 && field[0, 2] == 2) || (field[2, 1] == 2 && field[2, 0] == 2))
                    won = 2;
            }

            bool hasEmptyField = false;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (field[i, j] == 0)
                    {
                        hasEmptyField = true;
                        goto loops;
                    }
                }
            }
            if (!hasEmptyField && won <= 0)
            {
                won = -1;
            }
        loops:
            return won;
        }

        IEnumerator CPUsTurn()
        {
            yield return new WaitForSeconds(1f);

            int bestRow = -1;
            int bestColumn = -1;
            int bestValue = -1;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (field[i, j] == 0)
                    {
                        int[,] fieldCopy = (int[,])field.Clone();

                        fieldCopy[i, j] = player1IsActive ? 1 : 2;
                        int newValue = OneCPUTurn(fieldCopy, !player1IsActive);
                        if ((newValue < bestValue || bestRow == -1) || (newValue == bestValue && Random.value >= 0.5f))
                        {
                            bestRow = i;
                            bestColumn = j;
                            bestValue = newValue;
                        }
                    }
                }
            }
            buttons[bestRow * 3 + bestColumn].GetComponent<TTTField>().Select();
        }

        int OneCPUTurn(int[,] field, bool player1IsActive)
        {
            int winStatus = CheckIfWon(field);
            if (winStatus == 1)
            {
                return player1IsActive ? 1 : -1;
            }
            if (winStatus == 2)
            {
                return !player1IsActive ? 1 : -1;
            }
            if (winStatus == -1)
            {
                return 0;
            }

            int bestValue = 0;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (field[i, j] == 0)
                    {
                        int[,] fieldCopy = (int[,])field.Clone();

                        fieldCopy[i, j] = player1IsActive ? 1 : 2;
                        int newValue = -OneCPUTurn(fieldCopy, !player1IsActive);
                        bestValue += newValue;
                    }
                }
            }
            return bestValue;
        }
    }
}