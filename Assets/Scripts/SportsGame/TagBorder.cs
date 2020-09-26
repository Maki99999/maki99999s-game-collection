using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SportsGame {
public class TagBorder : MonoBehaviour {

    public float maxDist;
    
    public Vector3 planeNormal;
    public Transform player;

    float minY;
    float maxY;

    void Start() {
        minY = transform.position.y;
        maxY = -minY;
    }
    
    void FixedUpdate() {
        if(player == null || GameControllerTag.paused.Value) {
            transform.localPosition = new Vector3(transform.localPosition.x, minY, transform.localPosition.z);
            return;
        }
        float distanceToPlayer = Vector3.Project(player.localPosition - transform.localPosition, planeNormal).magnitude;
        
        if(distanceToPlayer < maxDist) {
            transform.localPosition = new Vector3(transform.localPosition.x, maxY - (distanceToPlayer / maxDist) * maxY * 2, transform.localPosition.z);
        } else {
            transform.localPosition = new Vector3(transform.localPosition.x, minY, transform.localPosition.z);
        }
    }
}
}