using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FalseTruth {
public class Clock : MonoBehaviour {

	public Transform big;		//big hand
	public Transform small;		//small hand

	FalseTruth.MasterClock master;			//The master clock
	
	void Start() {
		master = GameObject.FindWithTag("MasterClock").GetComponent<FalseTruth.MasterClock>();	//Get the components
	}

	void Update () {
		big.localEulerAngles = master.big.localEulerAngles;			//Sets own rotation to the rotation of the master clock
		small.localEulerAngles = master.small.localEulerAngles;		//
	}
}
}