using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace proj3ct
{
    public class PlayerController : MonoBehaviour
    {
        Transform cam;
        public float jumpForce = 640f;
        public float speed = 20f;
        public float maxSpeed = 500f;
        bool onGround = false;
        bool jumpFrame = false;

        float hackMultiplier = 1f;

        float axisHorizontal = 0f;
        float axisVertical = 0f;
        bool axisHackPos = false;
        bool axisHackNeg = false;
        bool isPressingHackButton = false;

        Rigidbody2D rigid;
        Animator anim;
        AnimationState animState = AnimationState.Idle;

        public AudioSource jumpAudio;

        void Start()
        {
            rigid = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();

            cam = GameObject.FindWithTag("MainCamera").transform;
        }

        void Update()
        {
            //Get Inputs
            axisHorizontal = Input.GetAxisRaw("Horizontal");
            axisVertical = Input.GetAxisRaw("Vertical") + Input.GetAxisRaw("Jump");
            axisHackPos = Input.GetKey(KeyCode.P);
            axisHackNeg = Input.GetKey(KeyCode.O);

            //Update Cam Position
            cam.position = new Vector3(transform.position.x, transform.position.y, cam.position.z);

            //Update animation
            if (rigid.velocity.x > 0.1f)
            {
                if (animState != AnimationState.Right)
                {
                    anim.SetTrigger("Right");
                    animState = AnimationState.Right;
                }
            }
            else if (rigid.velocity.x < -0.1f)
            {
                if (animState != AnimationState.Left)
                {
                    anim.SetTrigger("Left");
                    animState = AnimationState.Left;
                }
            }
            else
            {
                if (Mathf.Abs(rigid.velocity.y) > 0.1f)
                {
                    if (animState != AnimationState.Jump)
                    {
                        anim.SetTrigger("Jump");
                        animState = AnimationState.Jump;
                    }
                }
                else
                {
                    if (animState != AnimationState.Idle)
                    {
                        anim.SetTrigger("Idle");
                        animState = AnimationState.Idle;
                    }
                }

            }
        }

        void FixedUpdate()
        {
            Vector2 movement = Vector2.zero;

            //Hack
            if (axisHackNeg && !isPressingHackButton)
            {
                hackMultiplier += 1;
                isPressingHackButton = true;
            }
            if (axisHackPos && !isPressingHackButton)
            {
                hackMultiplier -= 1;
                isPressingHackButton = true;
            }
            if (!axisHackNeg && !axisHackPos)
                isPressingHackButton = false;

            //Jump
            jumpFrame = false;
            if (axisVertical > 0 && onGround && rigid.velocity.y < 10f)
            {
                movement += Vector2.up * jumpForce * hackMultiplier;
                onGround = false;
                jumpFrame = true;
                jumpAudio.Play();
            }

            //Walk
            movement += Vector2.right * axisHorizontal * speed * hackMultiplier * (Mathf.Abs(rigid.velocity.x) > maxSpeed ? 0 : 1);

            rigid.AddForce(movement);
        }

        void OnCollisionStay2D(Collision2D other)
        {
            Vector3 normal = other.contacts[0].normal;

            if (Vector2.Angle(Vector2.up, normal) < 20f && !jumpFrame)
            {
                onGround = true;
            }
        }

        enum AnimationState
        {
            Left, Right, Idle, Jump
        }
    }
}