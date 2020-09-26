﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MakiSupermarket
{
    public class PlayerController : MonoBehaviour
    {
        public float mouseSensitivityX = 2f;
        public float mouseSensitivityY = 2f;
        [Space(10)]
        public float speedNormal = 5f;
        public float speedSneaking = 2f;
        public float speedSprinting = 8f;
        public float jumpForce = 1f;
        float speedCurrent = 0f;
        public float gravity = 10f;
        public float airControl = 1f;
        Vector3 moveDirection = Vector3.zero;
        [Space(10)]
        public float heightNormal = 1.8f;
        public float heightSneaking = 1.4f;
        public float camOffsetHeight = 0.2f;
        public float camOffsetX = 0f;
        public float camOffsetY = 0f;
        public float fovNormal = 60f;
        public float fovSprinting = 80f;
        [Space(10)]
        bool canMove = true;
        float distanceSinceLastFootstepSqr = 0f;
        public float distanceBetweenFootstepsSqr = 1.5f;
        public bool isSneaking = false;
        public bool isSprinting = false;
        [Space(10)]
        public Animator crossAnimator;
        public AudioSource audioSource;
        public AudioClip[] audioClips;

        [HideInInspector] public CharacterController charController;
        [HideInInspector] public Transform camTransform;
        Transform heightOffsetTransform;
        Camera cam;

        [HideInInspector] public Rideable currentRide = null;
        private List<string> collectedItems = new List<string>();
        [HideInInspector] public List<ItemObserver> itemObservers = new List<ItemObserver>();

        void Start()
        {
            heightOffsetTransform = transform.GetChild(0);
            camTransform = heightOffsetTransform.GetChild(0);
            heightOffsetTransform.localPosition = new Vector3(0f, (heightNormal / 2) - camOffsetHeight, 0f);
            cam = camTransform.GetComponent<Camera>();

            charController = GetComponent<CharacterController>();

            speedCurrent = speedNormal;
        }

        void Update()
        {
            //Apply Camera Effects
            CameraEffects();

            //Do nothing when Player isn't allowed to move
            if (!canMove || PauseManager.isPaused().Value)
                return;

            //Get Inputs
            MoveData inputs = new MoveData()
            {
                xRot = Input.GetAxis("Mouse Y") * mouseSensitivityY,
                yRot = Input.GetAxis("Mouse X") * mouseSensitivityX,
                axisHorizontal = GlobalSettings.usingMouse ? Input.GetAxisRaw("Horizontal") : Input.GetAxis("Horizontal"),
                axisVertical = GlobalSettings.usingMouse ? Input.GetAxisRaw("Vertical") : Input.GetAxis("Vertical"),
                axisSneak = Input.GetAxisRaw("Sneak"),
                axisSprint = Input.GetAxisRaw("Sprint"),
                axisJump = Input.GetAxis("Jump")
            };

            //Don't move when sitting or while riding, use Move-Method of the rideable-Object
            if (currentRide != null)
            {
                if (currentRide.Move(inputs))
                    Rotate(inputs.xRot, inputs.yRot);

                return;
            }
            else
            {
                Rotate(inputs.xRot, inputs.yRot);
                Move(inputs);
            }
        }

        void Rotate(float xRot, float yRot)
        {
            Quaternion characterTargetRot = transform.localRotation;
            Quaternion cameraTargetRot = camTransform.localRotation;

            characterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
            cameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

            cameraTargetRot = ClampRotationAroundXAxis(cameraTargetRot);

            transform.localRotation = characterTargetRot;
            camTransform.localRotation = cameraTargetRot;
        }

        void Move(MoveData inputs)
        {
            //Changes camera height and speed while sneaking
            if (inputs.axisSneak > 0)
            {
                if (!isSneaking)
                    StartCoroutine(Sneak(true));

                if (isSprinting)
                    StartCoroutine(Sprint(false));
            }
            else
            {
                if (isSneaking)
                    StartCoroutine(Sneak(false));

                //Changes camera FOV and speed while sprinting
                if (inputs.axisSprint > 0 && inputs.axisVertical > 0)
                {
                    if (!isSprinting)
                        StartCoroutine(Sprint(true));
                }
                else
                {
                    if (isSprinting)
                        StartCoroutine(Sprint(false));
                }
            }
            speedCurrent = isSneaking ? speedSneaking : isSprinting ? speedSprinting : speedNormal;

            //Normalize input and add speed
            Vector2 input = new Vector2(inputs.axisHorizontal, inputs.axisVertical);
            input.Normalize();
            input *= speedCurrent;

            //Jump and Gravity
            if (charController.isGrounded)
            {
                moveDirection = transform.forward * input.y + transform.right * input.x;
                if (inputs.axisJump > 0)
                {
                    moveDirection.y = jumpForce;
                }
            }
            else
            {
                input *= airControl;
                moveDirection = transform.forward * input.y + transform.right * input.x + transform.up * moveDirection.y;
            }

            moveDirection.y -= gravity * (Time.deltaTime / 2);

            Vector3 oldPos = transform.position;
            charController.Move(moveDirection * Time.deltaTime);
            Vector3 newPos = transform.position;

            Sound(oldPos, newPos);

            moveDirection.y -= gravity * (Time.deltaTime / 2);
        }

        void CameraEffects()
        {
            if (camOffsetX != 0f || camOffsetY != 0f)
                camTransform.localPosition = new Vector3(camOffsetX, camOffsetY, 0f);
        }

        void Sound(Vector3 oldPos, Vector3 newPos)
        {
            oldPos.y = 0;
            newPos.y = 0;

            distanceSinceLastFootstepSqr += isSprinting ? (oldPos - newPos).sqrMagnitude / 2 : (oldPos - newPos).sqrMagnitude;

            if (distanceSinceLastFootstepSqr > distanceBetweenFootstepsSqr && charController.isGrounded)
            {
                distanceSinceLastFootstepSqr = 0;
                audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
                audioSource.pitch = 1f - Random.Range(-.1f, .1f);
                audioSource.Play();
            }
        }

        IEnumerator Sneak(bool willSneak)
        {
            isSneaking = willSneak;

            Vector3 oldCamPos = heightOffsetTransform.localPosition;
            float newHeight = willSneak ? (heightSneaking / 2) - camOffsetHeight : (heightNormal / 2) - camOffsetHeight;

            for (float i = 0; i < 1; i = i + 0.2f)
            {
                heightOffsetTransform.localPosition = Vector3.Lerp(oldCamPos, new Vector3(0f, newHeight, 0f), i);
                if (isSneaking == willSneak)
                    yield return new WaitForSeconds(1f / 60f);
                else
                    break;
            }
            if (isSneaking == willSneak)
                heightOffsetTransform.localPosition = new Vector3(0f, newHeight, 0f);
        }

        IEnumerator Sprint(bool willSprint)
        {
            isSprinting = willSprint;

            float oldFov = willSprint ? fovNormal : fovSprinting;
            float newFov = willSprint ? fovSprinting : fovNormal;

            for (float i = 0; i < 1; i = i + 0.2f)
            {
                cam.fieldOfView = Mathf.Lerp(oldFov, newFov, i);
                if (isSprinting == willSprint)
                    yield return new WaitForSeconds(1f / 60f);
                else
                    break;
            }
            if (isSprinting == willSprint)
                cam.fieldOfView = newFov;
        }

        public void Ride(Transform rideable, Transform newPosition) { StartCoroutine(RideEnumerator(rideable, newPosition)); }

        private IEnumerator RideEnumerator(Transform rideable, Transform newPosition)
        {
            SetCanMove(false);
            Ride(rideable);
            yield return MovePlayer(newPosition);
            SetCanMove(true);
        }

        public void Ride(Transform rideable)
        {
            transform.SetParent(rideable);
            currentRide = rideable.GetComponent<Rideable>();
            charController.enabled = false;
        }

        public void UnRide(Transform newPosition) { StartCoroutine(UnRideEnumerator(newPosition)); }

        private IEnumerator UnRideEnumerator(Transform newPosition)
        {
            SetCanMove(false);
            yield return MovePlayer(newPosition);
            UnRide();
            SetCanMove(true);
        }

        public void UnRide()
        {
            transform.SetParent(null);
            currentRide = null;
            charController.enabled = true;
        }

        public bool GetCanMove() { return canMove; }

        public void SetCanMove(bool canMove)
        {
            if (this.canMove != canMove)
            {
                this.canMove = canMove;
                crossAnimator.SetBool("On", canMove);
            }
        }

        public bool CollectedItemsContains(string itemName) { return collectedItems.Contains(itemName); }

        public void AddCollectedItem(string itemName)
        {
            collectedItems.Add(itemName);
            UpdateItemObservers(itemName, true);
        }

        public void RemoveCollectedItem(string itemName)
        {
            collectedItems.Remove(itemName);
            UpdateItemObservers(itemName, false);
        }

        private void UpdateItemObservers(string itemName, bool status)
        {
            foreach (ItemObserver itemObserver in itemObservers)
            {
                itemObserver.UpdateItemStatus(itemName, status);
            }
        }

        public IEnumerator MovePlayer(Transform newPosition, int frameCount = 100)
        {
            if (isSprinting)
                StartCoroutine(Sprint(false));

            Vector3 positionOld = transform.position;
            Quaternion rotationPlayerOld = transform.rotation;
            Quaternion rotationCameraOld = camTransform.localRotation;

            Vector3 positionNew = newPosition.position;
            Quaternion rotationPlayerNew = Quaternion.Euler(0f, newPosition.eulerAngles.y, 0f);
            Quaternion rotationCameraNew = Quaternion.Euler(newPosition.localEulerAngles.x, 0f, 0f);

            float fSmooth;
            for (float f = 0; f <= 1; f += 1f / frameCount)
            {
                fSmooth = Mathf.SmoothStep(0f, 1f, f);
                transform.position = Vector3.Lerp(positionOld, positionNew, fSmooth);
                transform.rotation = Quaternion.Lerp(rotationPlayerOld, rotationPlayerNew, fSmooth);
                camTransform.localRotation = Quaternion.Lerp(rotationCameraOld, rotationCameraNew, fSmooth);

                yield return new WaitForSeconds(1f / 60f);
            }

            transform.position = positionNew;
            transform.rotation = rotationPlayerNew;
            camTransform.localRotation = rotationCameraNew;
        }

        public IEnumerator RotatePlayer(Quaternion newRotation, int frameCount = 100)
        {
            if (isSprinting)
                StartCoroutine(Sprint(false));

            Quaternion rotationPlayerOld = transform.rotation;
            Quaternion rotationCameraOld = camTransform.localRotation;

            Quaternion rotationPlayerNew = Quaternion.Euler(0f, newRotation.eulerAngles.y, 0f);
            Quaternion rotationCameraNew = Quaternion.Euler(newRotation.eulerAngles.x, 0f, 0f);

            float fSmooth;
            for (float f = 0; f <= 1; f += 1f / frameCount)
            {
                fSmooth = Mathf.SmoothStep(0f, 1f, f);
                camTransform.localRotation = Quaternion.Lerp(rotationCameraOld, rotationCameraNew, fSmooth);
                transform.rotation = Quaternion.Lerp(rotationPlayerOld, rotationPlayerNew, fSmooth);

                yield return new WaitForSeconds(1f / 60f);
            }

            camTransform.localRotation = rotationCameraNew;
            transform.rotation = rotationPlayerNew;
        }

        public IEnumerator ForceLookPlayer(Transform lookAt, int frameCount = 100)
        {
            yield return RotatePlayer(Quaternion.LookRotation(lookAt.position - camTransform.position), frameCount);
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
