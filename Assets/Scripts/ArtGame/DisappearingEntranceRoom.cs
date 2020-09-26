using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace artgame
{
    public class DisappearingEntranceRoom : MonoBehaviour
    {
        public GameObject innerBox;
        public Text innerBoxText;
        public Text outerBoxText;
        public Text outerBoxStartText;
        public string startText;
        public TextAsset textAsset;
        string[] text;

        State currentState = State.Idle;
        int currentText = -1;
        float rotationOffset;
        float rotL; //270
        float rotR; //90

        void Start()
        {
            ChangeCurrentState(State.Idle);
            text = textAsset.ToString().Split(new[] { "\r\n", "\r", "\n" }, System.StringSplitOptions.None);
            rotationOffset = (transform.eulerAngles.y + 180f) % 360f;
            rotL = (270f + rotationOffset) % 360;
            rotR = (90f + rotationOffset) % 360;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && currentState == State.Idle && BetweenDegrees(PlayerController.rotationY, rotL, rotR))
            {
                ChangeCurrentState(State.TextBack);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") && currentState != State.Idle)
            {
                ChangeCurrentState(State.Idle);
            }
        }

        void Update()
        {
            if (currentState == State.Idle)
                return;
            if (currentState == State.TextFront && BetweenDegrees(PlayerController.rotationY, rotL, rotR))
            {
                ChangeCurrentState(State.TextBack);
            }
            else if (currentState == State.TextBack && BetweenDegrees(PlayerController.rotationY, rotR, rotL))
            {
                ChangeCurrentState(State.TextFront);
            }
        }

        bool BetweenDegrees(float value, float from, float to)
        {
            if (to >= from)
                return value > from && value < to;
            else
                return value < to || value > from;
        }

        void ChangeCurrentState(State newState)
        {
            currentState = newState;
            switch (newState)
            {
                case State.Idle:
                    innerBox.SetActive(false);
                    currentText = -1;
                    outerBoxStartText.text = startText;
                    outerBoxStartText.gameObject.SetActive(true);
                    outerBoxText.gameObject.SetActive(false);
                    break;

                case State.TextBack:
                    innerBox.SetActive(true);

                    currentText++;
                    if (currentText >= text.Length)
                        innerBox.SetActive(false);
                    else
                        innerBoxText.text = text[currentText];
                    break;

                case State.TextFront:
                    outerBoxStartText.gameObject.SetActive(false);
                    outerBoxText.gameObject.SetActive(true);

                    currentText++;
                    if (currentText >= text.Length)
                        innerBox.SetActive(false);
                    else
                        outerBoxText.text = text[currentText];
                    break;
            }
        }

        enum State
        {
            Idle,
            TextBack,
            TextFront
        }
    }
}