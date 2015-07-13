using Otherwise;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Test : MonoBehaviour {

	[HideInInspector]
	public double doubleDifference, reference;
	[HideInInspector]
	public float floatDifference, currentFrameRate, averageDeltaTime, deltaTime, deltaTimeTest, unscaledDeltaTime, unscaledDeltaTimeTest;

	public AnimationCurve wave;
	public float frequency = 1f, bitDepth = 32f;
	private Oscillator sine, sawtooth, square, triangle, play;
	private Wavetable table;

	private Instrument instrument;

	void Awake(){

		play = new Oscillator(0.1f, 1f);

		instrument = new Instrument(1f, 1f, play);
		instrument.Frequency = 350f;
		//sinusoidal interpolation system 

		Wavetable r1 = Wavetable.Sine;
		Wavetable r2 = ~(++r1);
		Wavetable r3 = ~r2;
		Wavetable r4 = ~r3;

		sine = new Oscillator(1f, frequency, r1);
		sawtooth = new Oscillator(1f, frequency, r2);
		square = new Oscillator(1f, frequency, r3);
		triangle = new Oscillator(1f, frequency, r4);

	}

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

		//signal.sample vs signal.Render

	}

	void Update(){

		Monitor();

		float r1 = sine.Render, r2 = sawtooth.Render, r3 = square.Render, r4 = triangle.Render;

		sine.Frequency = frequency;
		sawtooth.Frequency = frequency;
		square.Frequency = frequency;
		triangle.Frequency = frequency;
		bitDepth = r2 * bitDepth + 1f;
		Wavetable modulated = ((Wavetable.Square * r3 + Wavetable.Triangle * r4) ^ (bitDepth) * r1) +
			((Wavetable.Sine * r1 + Wavetable.Sawtooth * r2) ^ (33f - bitDepth) * r3) |
			((Wavetable.Square * r3 + Wavetable.Triangle * r4) ^ (bitDepth) * r4) +
			((Wavetable.Sine * r1 + Wavetable.Sawtooth * r2) ^ (33f - bitDepth) * r2);

		foreach(Oscillator oscil in instrument){

			oscil.wavetable = modulated;

			wave = oscil.wavetable;

		}

	}

	void OnAudioFilterRead(float[] data, int channels){

		play.Stream(ref data);

	}

}