using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

    [HideInInspector]
    public double doubleDifference, reference;
    [HideInInspector]
    public float floatDifference, currentFrameRate, averageDeltaTime, deltaTime, deltaTimeTest, unscaledDeltaTime, unscaledDeltaTimeTest;

    void Monitor(){

        if(Time.frameCount == 1)
            reference = AudioSettings.dspTime - Time.time;

        floatDifference = (float)AudioSettings.dspTime - Time.time - (float)reference;
        doubleDifference = AudioSettings.dspTime - Time.time - reference;
        unscaledDeltaTime = Time.unscaledDeltaTime;
        unscaledDeltaTimeTest = 1 / Time.unscaledDeltaTime;
        deltaTime = Time.deltaTime;
        deltaTimeTest = 1 / Time.deltaTime;
        currentFrameRate = Time.frameCount / Time.time;
        averageDeltaTime = 1 / currentFrameRate;

    }

    void Update(){

        Monitor();

    }

}
