using Otherwise;
using UnityEngine;
using System;
using System.Collections;

public class Test : MonoBehaviour {

	public AnimationCurve wave;
	private Oscillator oscil;
	private Wavetable table;

	void Awake(){

		table = Wavetable.Triangle;
		wave = table;
		oscil = new Oscillator(0.5f, 440f, table);

	}

	void OnAudioFilterRead(float[] data, int channels){

		oscil.Stream(ref data);

	}

}