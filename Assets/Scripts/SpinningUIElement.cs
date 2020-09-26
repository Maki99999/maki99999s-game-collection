using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningUIElement : MonoBehaviour {

    public float speed;
    public Vector3 rotation;
    public bool random;

    RectTransform rect;
    
    void Start() {
        rect = GetComponent<RectTransform>();
        if(random)
            rotation = Random.onUnitSphere;
    }

    void Update() {
        rect.Rotate(rotation * speed);
    }
}
