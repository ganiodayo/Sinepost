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
	private Oscillator sine, sawtooth, square, triangle, play, mod;
	private Wavetable table;

	private Instrument instrument;

	void Awake(){

		play = new Oscillator(0.1f, 1f);

		instrument = new Instrument(1f, 1f, play);
		instrument.Frequency = 350f;
		instrument.Amplitude = 1f;

		Wavetable r1 = Wavetable.Sine;
		Wavetable r2 = ~(++r1);
		Wavetable r3 = ~r2;
		Wavetable r4 = ~r3;

        mod = new Oscillator(1f, 10f, r1);
		sine = new Oscillator(1f, frequency, r1);
		sawtooth = new Oscillator(1f, frequency, r2);
		square = new Oscillator(1f, frequency, r3);
		triangle = new Oscillator(1f, frequency, r4);
		sine.Amplitude = 1f;
		sawtooth.Amplitude = 1f;
		square.Amplitude = 1f;
		triangle.Amplitude = 1f;

        play.Modulator = mod;

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

	}

	void Update(){

		Monitor();

		float r1 = sine.Render, r2 = sawtooth.Render, r3 = square.Render, r4 = triangle.Render;

		sine.Frequency = frequency;
		sawtooth.Frequency = frequency;
		square.Frequency = frequency;
		triangle.Frequency = frequency;
		bitDepth = r2 * 31f + 1f;
		Wavetable modulated = ((Wavetable.Sine * r1 + Wavetable.Sawtooth * r2) ^ bitDepth) |
			((Wavetable.Square * r4 + Wavetable.Triangle * r3) ^ (33f - bitDepth));

		foreach(Oscillator oscil in instrument){

			oscil.wavetable = modulated;

			wave = oscil.wavetable;

		}

	}

	void OnAudioFilterRead(float[] data, int channels){

		instrument.Stream(ref data);

	}

}