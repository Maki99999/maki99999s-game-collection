using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeObject : MonoBehaviour
{
    public bool freezePosX = false;
    public bool freezePosY = false;
    public bool freezePosZ = false;
    [Space(10)]
    public bool freezeRotX = false;
    public bool freezeRotY = false;
    public bool freezeRotZ = false;

    Vector3 startPos;
    Vector3 startRot;

    void Start()
    {
        startPos = transform.position;
        startRot = transform.eulerAngles;
    }

    void Update()
    {
        transform.position = new Vector3(freezePosX ? startPos.x : transform.position.x,
                freezePosY ? startPos.y : transform.position.y,
                freezePosZ ? startPos.z : transform.position.z
                );

        transform.eulerAngles = new Vector3(freezeRotX ? startRot.x : transform.eulerAngles.x,
                freezeRotY ? startRot.y : transform.eulerAngles.y,
                freezeRotZ ? startRot.z : transform.eulerAngles.z
                );
    }
}