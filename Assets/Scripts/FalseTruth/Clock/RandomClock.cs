using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FalseTruth {
public class RandomClock : MonoBehaviour {
	
	public FalseTruth.MasterClock clock;		//the master clock
	
	float normalSecondsPerSecond;	//normal speed of the master clock
	
	void Start () {
		clock.SetTime(Random.Range(0, 11), Random.Range(0,59));		//Starts the clock with a random time
		normalSecondsPerSecond = clock.secondsPerSecond;			//Get the normal speed of the master clock
	}

	void Update() {
		if(Random.value < 0.01) {		//With a probability of 0.01%
			if(clock.secondsPerSecond != normalSecondsPerSecond) {		//Resets the clocks speed when not normal
				clock.secondsPerSecond = normalSecondsPerSecond;		//
			} else {													//Or else sets the clock speed to a random value
				clock.secondsPerSecond = Random.Range(-666f, 3000);		//
			}
		}
	}
}
}