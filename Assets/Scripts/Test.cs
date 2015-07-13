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

	public AnimationCurve wave, test;
	public float frequency = 440f, bitDepth = 32f;
	private Oscillator oscil, sine, sawtooth, square, triangle;
	private Wavetable table;

	private Instrument instrument;

	void Awake(){

		oscil = new Oscillator(0.5f, 440f, Wavetable.Sine);
		Oscillator o1 = new Oscillator(0.1f, 880f, Wavetable.Sine), o2 = new Oscillator(0.01f, 1f, Wavetable.Triangle);
		instrument = new Instrument(1f, 880f, o1, o2);

		//recursive wavetable interpolation system!


		Wavetable r1 = Wavetable.Sine;
		Wavetable r2 = ~(++r1);
		Wavetable r3 = ~r2;
		Wavetable r4 = ~r3;

		test = r1 + r3;

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

		//signal.sample vs signal.render

	}

	void Update(){

		Monitor();

		float r1 = sine.render, r2 = sawtooth.render, r3 = square.render, r4 = triangle.render;

		sine.frequency = frequency;
		sawtooth.frequency = frequency;
		square.frequency = frequency;
		triangle.frequency = frequency;
		bitDepth = r2 * bitDepth + 1f;
		oscil.wavetable = (Wavetable.Sine * r1 + Wavetable.Sawtooth * r2) ^ bitDepth;// +  Wavetable.Square * r3 + Wavetable.Triangle * r4;

		wave = oscil.wavetable;

	}

	void OnAudioFilterRead(float[] data, int channels){

		instrument.Stream(ref data);

	}

}