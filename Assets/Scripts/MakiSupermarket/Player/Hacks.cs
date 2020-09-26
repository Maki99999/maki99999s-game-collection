using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hacks : MonoBehaviour
{
    float speed = 1f;

    void Update()
    {
        speed = 1f;

        if(Input.GetKey(KeyCode.O)) {
            speed = 5f;
        }

        Time.timeScale = speed;
    }
}
