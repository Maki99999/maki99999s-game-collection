using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace artgame
{
    public class UseablePCChair : Rideable
    {
        public Text controlsText;
        public Transform sitPosition;
        public Transform standPosition;

        public float speedMovement;
        public float speedRotation;

        public float maxSpeed;
        public float maxRotate;

        bool inUse = false;

        public float timeControlsAreShown = 1.5f;
        float timeWhenControlsAreHiddenAgain = 0f;

        public float fixFrameRateDependanceMultiplier;

        Rigidbody rigid;

        void Start()
        {
            rigid = GetComponent<Rigidbody>();
            if (fixFrameRateDependanceMultiplier <= 0f)
                Debug.LogError("fixFrameRateDependanceMultiplier can't be 0");
        }

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
                PlayerController.playerController.Ride(transform, sitPosition);
            }
            else
            {
                PlayerController.playerController.Ride(null, standPosition);
            }
        }

        public override void Move(float xRot, float yRot, float axisSneak, float axisHorizontal, float axisVertical)
        {
            Vector3 movement = -transform.right * axisVertical * speedMovement;
            if (rigid.velocity.magnitude < maxSpeed)
                rigid.AddForce(movement * Time.deltaTime * fixFrameRateDependanceMultiplier);
            if (rigid.angularVelocity.magnitude < maxRotate)
                rigid.AddTorque(0f, axisHorizontal * speedRotation * Time.deltaTime * fixFrameRateDependanceMultiplier, 0f);
        }
    }
}
