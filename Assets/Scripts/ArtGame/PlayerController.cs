using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace artgame
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController playerController;
        public static float rotationY;

        public float mouseSensitivityX = 2f;
        public float mouseSensitivityY = 2f;

        public float speedNormal = 5f;
        public float speedSneaking = 2f;
        float speedCurrent = 0f;
        public float speedFalling = 10f;

        public float heightNormal = 1.8f;
        public float heightSneaking = 1.4f;
        public float camUpperOffset = 0.2f;

        public bool canMove = true;
        public bool isSneaking = false;
        public bool isSitting = false;

        public float fixFrameRateDependanceMultiplier;

        CharacterController charController;
        Transform cam;

        Rideable currentRide = null;
        bool isRiding = false;

        void Start()
        {
            cam = transform.GetChild(0);
            cam.localPosition = new Vector3(0f, (heightNormal / 2) - camUpperOffset, 0f);

            charController = GetComponent<CharacterController>();

            speedCurrent = speedNormal;

            playerController = this;

            //Lock Mouse
			Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {
            //Do nothing when Player isn't allowed to move
            if (!canMove)
                return;

            //Get Inputs
            float xRot = Input.GetAxis("Mouse Y") * mouseSensitivityY;
            float yRot = Input.GetAxis("Mouse X") * mouseSensitivityX;
            float axisSneak = Input.GetAxisRaw("Sneak");
            float axisHorizontal = GlobalSettings.usingMouse ? Input.GetAxisRaw("Horizontal") : Input.GetAxis("Horizontal");
            float axisVertical = GlobalSettings.usingMouse ? Input.GetAxisRaw("Vertical") : Input.GetAxis("Vertical");

            //Calculate and set Rotations
            Quaternion characterTargetRot = transform.localRotation;
            Quaternion cameraTargetRot = cam.localRotation;

            characterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
            cameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

            cameraTargetRot = ClampRotationAroundXAxis(cameraTargetRot);
            rotationY = characterTargetRot.eulerAngles.y;

            transform.localRotation = characterTargetRot;
            cam.localRotation = cameraTargetRot;

            //Don't move when sitting or while riding, use Move-Method of the rideable-Object
            if (isSitting)
            {
                if (isRiding)
                    currentRide.Move(xRot, yRot, axisSneak, axisHorizontal, axisVertical);

                return;
            }

            //Changes camera height and speed while sneaking
            if (axisSneak > 0)
            {
                if (!isSneaking)
                {
                    StartCoroutine(Sneak(true));
                }
            }
            else
            {
                if (isSneaking)
                {
                    StartCoroutine(Sneak(false));
                }
            }
            speedCurrent = isSneaking ? speedSneaking : speedNormal;

            //Normalize input and add speed
            Vector2 input = new Vector2(axisHorizontal, axisVertical);
            input.Normalize();
            input *= speedCurrent;

            //Create movement vector and move Player
            Vector3 movement = transform.forward * input.y + transform.right * input.x;
            if (!charController.isGrounded)
                movement -= transform.up * speedFalling;

            charController.Move(movement * fixFrameRateDependanceMultiplier * Time.deltaTime);
        }

        IEnumerator Sneak(bool willSneak)
        {
            isSneaking = willSneak;

            Vector3 oldCamPos = cam.localPosition;
            float newHeight = willSneak ? (heightSneaking / 2) - camUpperOffset : (heightNormal / 2) - camUpperOffset;

            for (float i = 0; i < 1; i = i + 0.2f)
            {
                cam.localPosition = Vector3.Lerp(oldCamPos, new Vector3(0f, newHeight, 0f), i);
                if (isSneaking == willSneak)
                {
                    yield return new WaitForSeconds(1f / 60f);
                }
                else
                {
                    break;
                }
            }
            if (isSneaking == willSneak)
                cam.localPosition = new Vector3(0f, newHeight, 0f);
        }

        public void Sit(bool activate, Transform newPosition)
        {
            isSitting = activate;
            charController.enabled = !activate;
            StartCoroutine(MovePlayer(newPosition));
        }

        public void Ride(Transform rideable, Transform newPosition)
        {
            StartCoroutine(RideEnumerator(rideable, newPosition));
        }

        private IEnumerator RideEnumerator(Transform rideable, Transform newPosition)
        {
            transform.SetParent(rideable);

            if (currentRide == null)
            {
                currentRide = rideable.GetComponent<Rideable>();
                isSitting = true;
                isRiding = true;
                charController.enabled = false;
                yield return MovePlayer(newPosition);
            }
            else
            {
                yield return MovePlayer(newPosition);
                currentRide = null;
                isSitting = false;
                isRiding = false;
                charController.enabled = true;
            }
        }

        IEnumerator MovePlayer(Transform newPosition)
        {
            canMove = false;

            Vector3 positionOld = transform.position;
            Quaternion rotationPlayerOld = transform.rotation;
            Quaternion rotationCameraOld = cam.localRotation;

            Vector3 positionNew = newPosition.position;
            Quaternion rotationPlayerNew = Quaternion.Euler(0f, newPosition.eulerAngles.y, 0f);
            Quaternion rotationCameraNew = Quaternion.Euler(newPosition.localEulerAngles.x, 0f, 0f);

            for (float f = 0; f <= 1; f += 0.01f)
            {
                float fSmooth = Mathf.SmoothStep(0f, 1f, f);
                transform.position = Vector3.Lerp(positionOld, positionNew, fSmooth);
                transform.rotation = Quaternion.Lerp(rotationPlayerOld, rotationPlayerNew, fSmooth);
                cam.localRotation = Quaternion.Lerp(rotationCameraOld, rotationCameraNew, fSmooth);

                yield return new WaitForSeconds(1f / 60f);
            }

            transform.position = positionNew;
            transform.rotation = rotationPlayerNew;
            cam.localRotation = rotationCameraNew;

            canMove = true;
        }

        Quaternion ClampRotationAroundXAxis(Quaternion q)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

            angleX = Mathf.Clamp(angleX, -90, 90);

            q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

            return q;
        }
    }
}
