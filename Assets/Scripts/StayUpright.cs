using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayUpright : MonoBehaviour {

	void FixedUpdate () {
		transform.rotation = Quaternion.identity;
	}
}
