using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace artgame
{
    public class UseableBank : Rideable
    {
        public Text controlsText;
        public Transform sitPosition;
        public Transform standPosition;

        bool inUse = false;

        public float timeControlsAreShown = 1.5f;
        float timeWhenControlsAreHiddenAgain = 0f;

        void Update()
        {
            float deltatime = timeWhenControlsAreHiddenAgain - Time.time;
            float fadeValue = inUse ? 0 : (deltatime > timeControlsAreShown / 2) ? 1 : deltatime / (timeControlsAreShown / 2);
            controlsText.color = new Color(1f, 1f, 1f, fadeValue);
        }

        public override void ShowControls()
        {
            timeWhenControlsAreHiddenAgain = Time.time + timeControlsAreShown;
        }

        public override void Use()
        {
            inUse = !inUse;
            if (inUse)
            {
                PlayerController.playerController.Sit(true, sitPosition);
            }
            else
            {
                PlayerController.playerController.Sit(false, standPosition);
            }
        }

        public override void Move(float xRot, float yRot, float axisSneak, float axisHorizontal, float axisVertical) { }
    }
}
