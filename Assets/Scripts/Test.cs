using Otherwise;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Test : MonoBehaviour {

	public double doubleDifference;
	public float floatDifference, currentFrameRate, averageDeltaTime, deltaTime, deltaTimeTest, unscaledDeltaTime, unscaledDeltaTimeTest;
	public AnimationCurve wave;
	private Oscillator oscil;
	private Wavetable table;

	private double reference;

	void Awake(){

		table = Wavetable.Sine ^ -2;
		wave = table;
		oscil = new Oscillator(0.5f, 440f, table);

	}

	void Update(){

		if(Time.frameCount == 1){

			reference = AudioSettings.dspTime - Time.time;

		}

		floatDifference = (float)AudioSettings.dspTime - Time.time - (float)reference;
		doubleDifference = AudioSettings.dspTime - Time.time - reference;
		unscaledDeltaTime = Time.unscaledDeltaTime;
		unscaledDeltaTimeTest = 1 / Time.unscaledDeltaTime;
		deltaTime = Time.deltaTime;
		deltaTimeTest = 1 / Time.deltaTime;
		currentFrameRate = Time.frameCount / Time.time;
		averageDeltaTime = 1 / currentFrameRate;


	}

	void OnAudioFilterRead(float[] data, int channels){

		oscil.Stream(ref data);

	}

}