using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SportsGame {
[System.Serializable]
public class TimeBank : System.Object {
    
    int totalAccessCount;
    float time;

    float timeLeft;
    float minTime;
    float maxTime;

    int accessCount;
    float average;
    float desiredAverage;

    public TimeBank(int tac, float t, float minT, float maxT) {
        totalAccessCount = tac;
        time = t;

        minTime = minT;
        maxTime = maxT;

        timeLeft = t;
        accessCount = 0;
        desiredAverage = time / totalAccessCount;
    }

    public float GetRandomTime() {
        if(accessCount >= totalAccessCount) return -1;
        if(timeLeft < 0) return 0;
        if(++accessCount == totalAccessCount) {
            float val = timeLeft;
            timeLeft = 0f;
            return val;
        }

        float max = (totalAccessCount - accessCount) * maxTime;
        
        float randMin = average < desiredAverage ? average : minTime;
        float randMax = average > desiredAverage ? average : maxTime;
        if(timeLeft - max >= minTime)
            randMin = timeLeft - max;
        if(maxTime + (totalAccessCount - accessCount) * minTime > timeLeft)
            randMax = timeLeft - (totalAccessCount - accessCount + 1) * minTime;

        float value = Random.Range(randMin, randMax);
        timeLeft -= value;
        average = (time - timeLeft) / accessCount;
        return value;
    }
}
}