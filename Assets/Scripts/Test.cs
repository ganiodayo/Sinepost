using Sinepost;
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
	public float frequency = 1f, bitDepth = 32f, modFreq = 10f;
	private Oscillator sine, sawtooth, square, triangle, play, mod;
	private Wavetable table;
	private Instrument instrument;
    public Transform position;
    public bool flip;
    float[] scale = new float[5]{0f, 03f, 5f, 7f, 10f};

    IEnumerator Notes(){

        while(true){

            instrument[0]["Frequency"] = 440f * Mathf.Pow(2f, (float)(UnityEngine.Random.Range(2, 3)
                    * 12 + scale[UnityEngine.Random.Range(0, scale.Length)] - 12 * 5 + 9) / 12f);
            instrument[1]["Frequency"] = 440f * Mathf.Pow(2f, (float)(UnityEngine.Random.Range(2, 4)
                    * 12 + scale[UnityEngine.Random.Range(0, scale.Length)] - 12 * 5 + 9) / 12f);
            instrument[2]["Frequency"] = 440f * Mathf.Pow(2f, (float)(UnityEngine.Random.Range(2, 4)
                    * 12 + scale[UnityEngine.Random.Range(0, scale.Length)] - 12 * 5 + 9) / 12f);

            yield return new WaitForSeconds(0.18f);

            instrument[1]["Frequency"] = 440f * Mathf.Pow(2f, (float)(UnityEngine.Random.Range(2, 4)
                    * 12 + scale[UnityEngine.Random.Range(0, scale.Length)] - 12 * 5 + 9) / 12f);

            yield return new WaitForSeconds(0.09f);

            instrument[2]["Frequency"] = 440f * Mathf.Pow(2f, (float)(UnityEngine.Random.Range(2, 4)
                    * 12 + scale[UnityEngine.Random.Range(0, scale.Length)] - 12 * 5 + 9) / 12f);

            yield return new WaitForSeconds(0.09f);

        }

    }

	void Awake(){

		play = new Oscillator(0.3f);
        play.Modulate("Frequency", new Noise(0.1f), new Oscillator(0.005f, 80f));

		instrument = new Instrument(1f, 1f, play, new Oscillator(0.3f), new Oscillator(0.3f));

		Wavetable r1 = Wavetable.Sine;
		Wavetable r2 = ~(++r1);
		Wavetable r3 = ~r2;
		Wavetable r4 = ~r3;

        mod = new Oscillator(r1);
		sine = new Oscillator(r1);
		sawtooth = new Oscillator(r2);
		square = new Oscillator(r3);
		triangle = new Oscillator(r4);

        play.Modulator = mod;

        StartCoroutine(Notes());

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

		sine["Frequency"] = frequency;
        sawtooth["Frequency"] = frequency;
        square["Frequency"] = frequency;
        triangle["Frequency"] = frequency;
		bitDepth = r2 * 31f + 1f;
		Wavetable modulated = ((Wavetable.Sine * r1 + Wavetable.Sawtooth * r2) ^ bitDepth) |
			((Wavetable.Square * r4 + Wavetable.Triangle * r3) ^ (33f - bitDepth));

        mod["Frequency"] = modFreq;

		foreach(Oscillator oscil in instrument){

			oscil.wavetable = modulated;
			wave = oscil.wavetable;

		}

        instrument.host = flip ? -position.position : position.position;
        instrument[1].Pan(position.position);
        instrument[2].Pan(-position.position);

	}

	void OnAudioFilterRead(float[] data, int channels){

		instrument.Stream(ref data);

	}

}