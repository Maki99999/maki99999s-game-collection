using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForSecondsPaused : CustomYieldInstruction {

    float seconds;
    float timer;
    BooleanWrapper paused;

    public override bool keepWaiting {
        get {
            if(paused.Value) return true;
            if(timer > seconds) return false;
            
           timer += Time.deltaTime;
           return true;
        }
    }

    public WaitForSecondsPaused(float seconds, BooleanWrapper paused) {
        timer = 0f;
        this.seconds = seconds;
        this.paused = paused;
    }
}
