using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Otherwise {

	public abstract class Notated {

		public float tempo = 120f;
		public rhythmic signature;

		public struct rhythmic {
			
			public int numerator;
			public int denominator;
			
		}
		
		public struct cyclic {

			public byte velocity;
			public float frequency;
			public rhythmic duration;
			
		}

		public abstract void Read();
		
	}

	public class Part : Notated {

		rhythmic beat;
		cyclic note;
		public int index;

		public Part(rhythmic beat, cyclic note){



		}

		public static Part operator + (Part input1, Part input2){

			Part output = input1;

			return output;

		}
		
		public override void Read (){
			
			
			
		}

	}

	public class Section : Notated {

		private readonly Part[] parts;

		public Section(params Part[] parts){
			
			this.parts = parts;
			
		}

		public override void Read(){
			
		}

		public Part this[int index]{
			
			get{
				
				return parts[index];
				
			}
			
		}

	}

	public class Score : Notated {

		private readonly Section[] sections;

		public Score(params Section[] sections){

			this.sections = sections;

		}

		public Section this[int index]{
			
			get{
				
				return sections[index];
				
			}
			
		}
		
		public override void Read(){
			
		}

	}

	public interface IAudible {

		//List<T>() : Signal

		void OnTransformChildrenChanged();

	}

	public class Instrument : MonoBehaviour, IAudible {

		//generic list of type signal;

		void Awake(){

		}

		public void OnTransformChildrenChanged(){


		}
		
		public static Instrument operator ++ (Instrument instrument){
			
			return new Instrument();
			
		}
		
	}

	public class Orchestra : MonoBehaviour, IAudible {

		private readonly Instrument[] instrument;

		public Orchestra(params Instrument[] members){

			this.instrument = members;

		}

		void Awake(){
			
		}
		
		public void OnTransformChildrenChanged(){
			
			
		}

		public Instrument this[int index]{
			
			get{
				
				return instrument[index];
				
			}
			
		}

	}

	public class Wavetable {
		
		private readonly float[] waveform;
		private readonly int size;

		//cast to Keyframe + cast to AnimationCurve

		public Wavetable(double[] amp, int size = 8192){

			this.size = size;
			waveform = new float[this.size];
			
			double index = 0f;
			float peak = float.MinValue;
			
			for(int i = 0; i < this.size; i++){
				
				index = i / (this.size - 1f);
				
				for(int j = 0; j < amp.Length; j++){
					
					waveform[i] += (float)(amp[j] * Math.Sin(2f * Math.PI * (j + 1) * index));
					
				}
				
				if(Mathf.Abs(waveform[i]) > peak)
					peak = Mathf.Abs(waveform[i]);
				
			}
			
			for(int i = 0; i < this.size; i++)
				waveform[i] /= peak;
			
		}
		
		public Wavetable(double[] partial, double[] amp, int size = 8192){

			this.size = size;
			waveform = new float[this.size];
			
			double index = 0f;
			float peak = float.MinValue;
			
			for(int i = 0; i < this.size; i++){

				index = i / (this.size - 1f);
				
				for(int j = 0; j < partial.Length; j++){
					
					waveform[i] += (float)(amp[j % amp.Length] * Math.Sin(2f * Math.PI * partial[j] * index));
					
				}
				
				if(Mathf.Abs(waveform[i]) > peak)
					peak = Mathf.Abs(waveform[i]);
				
			}
			
			for(int i = 0; i < this.size; i++)
				waveform[i] /= (float)peak;
			
		}
		
		public Wavetable(double[] partial, double[] amp, double[] phase, int size = 8192){

			this.size = size;
			waveform = new float[this.size];
			
			double index = 0f;
			float peak = float.MinValue;
			
			for(int i = 0; i < this.size; i++){
				
				index = i / (this.size - 1f);
				
				for(int j = 0; j < partial.Length; j++){
					
					waveform[i] += (float)(amp[j % amp.Length] * Math.Sin(2f * Math.PI * partial[j] * index + phase[j % phase.Length]));
					
				}
				
				if(Mathf.Abs(waveform[i]) > peak)
					peak = Mathf.Abs(waveform[i]);
				
			}
			
			for(int i = 0; i < this.size; i++)
				waveform[i] /= (float)peak;
			
		}
		
		public Wavetable(double[] partial, double[] amp, double[] phase, double[] offset, int size = 8192){

			this.size = size;
			waveform = new float[this.size];
			
			double index = 0f;
			float peak = float.MinValue;
			
			for(int i = 0; i < this.size; i++){
				
				index = i / (this.size - 1f);
				
				for(int j = 0; j < partial.Length; j++){
					
					waveform[i] += (float)(amp[j % amp.Length] * Math.Sin(2f * Math.PI * partial[j] * index + phase[j % phase.Length]) + offset[j % offset.Length]);
					
				}
				
				if(Mathf.Abs(waveform[i]) > peak)
					peak = Mathf.Abs(waveform[i]);
				
			}
			
			for(int i = 0; i < this.size; i++)
				waveform[i] /= (float)peak;
			
		}
		
		public float this[int index]{
			
			get{
				
				return waveform[index];
				
			}
			
		}

		public float this[float index]{
			
			get{

				return waveform[Mathf.FloorToInt(index)] * (1f - (index % 1f)) + waveform[Mathf.CeilToInt(index) % waveform.Length] * (index % 1f);
				
			}
			
		}

		public int Size{

			get{

				return this.size; //make public instead?

			}

		}
		
		public static Wavetable Sine{
			
			get{
				
				return new Wavetable(new double[1]{1f});
				
			}
			
		}
		
		public static Wavetable Sawtooth{
			
			get{
				
				return new Wavetable(new double[50]{1d, 2d, 3d, 4d, 5d, 6d, 7d, 8d, 9d, 10d,
									  	   11d, 12d, 13d, 14d, 15d, 16d, 17d, 18d, 19d, 20d,
										   21d, 22d, 23d, 24d, 25d, 26d, 27d, 28d, 29d, 30d,
										   31d, 32d, 33d, 34d, 35d, 36d, 37d, 38d, 39d, 40d,
										   41d, 42d, 43d, 44d, 45d, 46d, 47d, 48d, 49d, 50d},
									 new double[50]{1d, 1/2d, 1/3d, 1/4d, 1/5d, 1/6d, 1/7d, 1/8d, 1/9d, 1/10d,
										 1/11d, 1/12d, 1/13d, 1/14d, 1/15d, 1/16d, 1/17d, 1/18d, 1/19d, 1/20d,
										 1/21d, 1/22d, 1/23d, 1/24d, 1/25d, 1/26d, 1/27d, 1/28d, 1/29d, 1/30d,
										 1/31d, 1/32d, 1/33d, 1/34d, 1/35d, 1/36d, 1/37d, 1/38d, 1/39d, 1/40d,
										 1/41d, 1/42d, 1/43d, 1/44d, 1/45d, 1/46d, 1/47d, 1/48d, 1/49d, 1/50d});
									
			}
			
		}
		
		public static Wavetable Square{
			
			get{
				
				return new Wavetable(new double[25]{1d, 3d, 5d, 7d, 9f,
										 	   11d, 13d, 15d, 17d, 19f,
										 	   21d, 23d, 25d, 27d, 29f,
											   31d, 33d, 35d, 37d, 39f,
										 	   41d, 43d, 45d, 47d, 49f},
									 new double[25]{1d, 1/3d, 1/5d, 1/7d, 1/9d,
									 		 1/11d, 1/13d, 1/15d, 1/17d, 1/19d,
											 1/21d, 1/23d, 1/25d, 1/27d, 1/29d,
											 1/31d, 1/33d, 1/35d, 1/37d, 1/39d,
											 1/41d, 1/43d, 1/45d, 1/47d, 1/49d});
				
			}
			
		}
		
	}

	public abstract class Signal {

		public int check = 0, channels = 2;
		public readonly int sampleRate = AudioSettings.outputSampleRate;
		public float amplitude, duration, sample;
		public float[] panner;

		// vector2 -> equal power panner struct?
		// modulator >> carrier.parameter += lambda(mod.datum * (1 + parameter)

		public abstract float datum{ get; }
		public abstract void Write(ref float[] data);

		public int SpeakerMode(){
			
			switch(AudioSettings.driverCapabilities){
				
			case AudioSpeakerMode.Mono:
				return 1;
				
			case AudioSpeakerMode.Stereo:
				return 2;
				
			case AudioSpeakerMode.Prologic:
				return 2;
				
			case AudioSpeakerMode.Quad:
				return 4;
				
			case AudioSpeakerMode.Surround:
				return 5;
				
			case AudioSpeakerMode.Mode5point1:
				return 6;
				
			case AudioSpeakerMode.Mode7point1:
				return 8;
				
			default:
				return 2;
				
			}
			
		}
		
	}

	public class Oscillator : Signal {

		public float frequency, phasor;
		private readonly Wavetable wavetable;

		//phasor getter
		//frequency modifier

		public Oscillator(float a, float f){

			amplitude = a;
			frequency = f;
			wavetable = Wavetable.Sine;

			channels = SpeakerMode();
			panner = new float[channels];

		}

		public Oscillator(float a, float f, Wavetable w){

			amplitude = a;
			frequency = f;
			wavetable = w;

			channels = SpeakerMode();
			panner = new float[channels];

		}

		public override float datum{
			
			get{
				
				sample = wavetable[phasor] * amplitude;
				
				if(check == 0)
					phasor = (phasor + frequency * wavetable.Size / sampleRate) % wavetable.Size;
				
				check = (check + 1) % channels;
				
				return sample;
				
			}
			
		}

		public override void Write(ref float[] data){

			for(int i = 0; i < data.Length; i++)				
				data[i] = datum;

			//add modulation delegate?

		}
		
	}

	public class Noise : Signal {

		public override float datum{
			
			get{
				
				return sample;
				
			}
			
		}
		
		public override void Write(ref float[] data){
			
			for(int i = 0; i < data.Length; i++)				
				data[i] = datum;
			
		}

	}

}