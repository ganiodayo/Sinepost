using Sinepost;
using UnityEngine;
using System.Collections;

public class Synth : MonoBehaviour {

    public AnimationCurve wave;
	public float frequency = 25/72f, bitDepth = 32f, modFreq = 10f;
	private Oscillator sine, sawtooth, square, triangle, play, mod, x;
	private Wavetable table;
	private Instrument instrument;

    float[] scale = new float[8]{0f, 0f, 03f, 5f, 7f, 7f, 10f, 12f};

    IEnumerator Notes(){

        while(true){

            instrument[0]["Frequency"] = scale[UnityEngine.Random.Range(0, scale.Length)] / 2f;
            instrument[1]["Frequency"] = scale[UnityEngine.Random.Range(0, scale.Length)];
            instrument[2]["Frequency"] = scale[UnityEngine.Random.Range(0, scale.Length)];

            yield return new WaitForSeconds(0.36f);

            instrument[1]["Frequency"] = scale[UnityEngine.Random.Range(0, scale.Length)];

            yield return new WaitForSeconds(0.18f);

            instrument[2]["Frequency"] = scale[UnityEngine.Random.Range(0, scale.Length)];

            yield return new WaitForSeconds(0.18f);

        }

    }

	void Awake(){
		
		instrument = new Instrument(0.3f, 1f, new Oscillator(0.3f), new Oscillator(0.3f), new Oscillator(0.3f));

		Wavetable r1 = Wavetable.Sine;
		Wavetable r2 = ~(++r1);
		Wavetable r3 = ~r2;
		Wavetable r4 = ~r3;

        mod = new Oscillator(r1);
		sine = new Oscillator(r1);
		sawtooth = new Oscillator(r2);
		square = new Oscillator(r3);
		triangle = new Oscillator(r4);

        instrument.Modulator = mod;

        x = new Oscillator(Wavetable.Triangle, 0.7f, 25/36f);

        for(int i = 0; i < scale.Length; i++)
            scale[i] = 440f * Mathf.Pow(2f, (scale[i] - 3) / 12f);

        StartCoroutine(Notes());

	}

	void Update(){

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

        float render = x.Render;
        instrument[1].Pan(render);
        instrument[2].Pan(-render);

	}

	void OnAudioFilterRead(float[] data, int channels){

		instrument.Stream(ref data);

	}

}