using Otherwise;
using UnityEngine;
using System.Collections;

public class Sine : MonoBehaviour {

	public Oscillator oscil;

	void Awake(){

		oscil = new Oscillator(0.1f, 440f, Wavetable.Square);

	}

	void OnAudioFilterRead(float[] data, int channels){

		//THIS?
		oscil.Write(ref data);

		//OR THIS?
		for(int i = 0; i < data.Length; i++)
			data[i] = oscil.datum;

	}

}