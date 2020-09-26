using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace artgame
{
    public class UseableSwitch : Useable
    {
        public Text controlsText;

        public float timeControlsAreShown = 1.5f;
        float timeWhenControlsAreHiddenAgain = 0f;

        public GameObject[] gameObjectsToDisable;

        bool isInAnimation = false;
        Animator animator;

        bool on = true;

        void Start()
        {
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            float deltatime = timeWhenControlsAreHiddenAgain - Time.time;
            float fadeValue = isInAnimation ? 0 : (deltatime > timeControlsAreShown / 2) ? 1 : deltatime / (timeControlsAreShown / 2);
            controlsText.color = new Color(1f, 1f, 1f, fadeValue);
        }

        public override void ShowControls()
        {
            if (!isInAnimation)
                timeWhenControlsAreHiddenAgain = Time.time + timeControlsAreShown;
        }

        public override void Use()
        {
            if (!isInAnimation)
            {
                StartCoroutine(Animation());
            }
        }

        private IEnumerator Animation()
        {
            isInAnimation = true;

            on = !on;
            animator.SetBool("On", on);

            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

            foreach (GameObject gameObject in gameObjectsToDisable)
            {
                gameObject.SetActive(on);
            }
            isInAnimation = false;
        }
    }
}