using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PosRotSca : System.Object
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 eulerAngles;
    public Vector3 scale;

    public PosRotSca(Transform transform)
    {
        position = transform.position;
        rotation = transform.rotation;
        eulerAngles = rotation.eulerAngles;
        scale = Vector3.one;
    }

    public PosRotSca(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        this.position = position;
        this.rotation = rotation;
        eulerAngles = rotation.eulerAngles;
        this.scale = scale;
    }

    public PosRotSca(Vector3 position, Quaternion rotation)
    {
        this.position = position;
        this.rotation = rotation;
        eulerAngles = rotation.eulerAngles;
        this.scale = Vector3.one;
    }

    public PosRotSca(Vector3 position)
    {
        this.position = position;
        this.rotation = Quaternion.Euler(0, 0, 0);
        eulerAngles = Vector3.zero;
        this.scale = Vector3.one;
    }

    public override string ToString() {
        return position + " " + eulerAngles + " " + scale;
    }
}
