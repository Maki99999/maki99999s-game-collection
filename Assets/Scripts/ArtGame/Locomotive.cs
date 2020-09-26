using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace artgame
{
    public class Locomotive : MonoBehaviour
    {
        public Animator animWheels;
        public AudioSource audioTrainDrive;
        public AudioSource audioTrainWhistle;

        public void FinishMovementToStation()
        {
            animWheels.enabled = false;
            audioTrainDrive.Pause();
        }

        public void FinishMovementIdle()
        {
            animWheels.enabled = true;
            audioTrainDrive.Play();
        }

        public void FinishMovementRandom() {
            audioTrainWhistle.Play();
        }

        IEnumerator MoveTo(Vector3 positionNew, Quaternion rotationNew)
        {
            Vector3 positionOld = transform.position;
            Quaternion rotationPlayerOld = transform.rotation;

            for (float f = 0; f <= 1; f += 0.01f)
            {
                float fSmooth = Mathf.SmoothStep(0f, 1f, f);
                transform.position = Vector3.Lerp(positionOld, positionNew, fSmooth);
                transform.rotation = Quaternion.Lerp(rotationPlayerOld, rotationNew, fSmooth);

                yield return new WaitForSeconds(1f / 60f);
            }

            transform.position = positionNew;
            transform.rotation = rotationNew;
        }
    }
}