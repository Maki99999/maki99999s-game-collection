using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAtHeight : MonoBehaviour
{
    public float height;

    void Update()
    {
        if (transform.position.y <= height)
            Destroy(gameObject);
    }
}
