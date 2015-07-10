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
			public float frequency; //nullable?
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

		public delegate double Recurse(ref double x);

		private readonly float[] waveform;
		private readonly uint size;

		public Wavetable(double[] amplitude, uint size = 8192u){
			
			this.size = size;
			waveform = new float[this.size];
			
			double index = 0d;
			float peak = float.Epsilon;
			
			for(int i = 0; i < this.size; i++){
				
				index = i / (this.size - 1d);
				
				for(int j = 0; j < amplitude.Length; j++)
					waveform[i] += (float)(amplitude[j % amplitude.Length] * Math.Sin(2d * Math.PI * (j + 1) * index));
				
				if(Mathf.Abs(waveform[i]) > peak)
					peak = Mathf.Abs(waveform[i]);
				
			}
			
			for(int i = 0; i < this.size; i++)
				waveform[i] /= peak;
			
		}

		public Wavetable(Recurse amplitude, uint iterations = 1u, uint size = 8192u){
			
			this.size = size;
			waveform = new float[this.size];
			
			double index = 0d, a;
			float peak = float.Epsilon;
			
			for(int i = 0; i < this.size; i++){
				
				a = 1d;
				index = i / (this.size - 1f);
				
				for(uint j = 0u; j < iterations; j++)
					waveform[i] += (float)(1/a * Math.Sin(2d * Math.PI * (j + 1) * index));
				
				if(Mathf.Abs(waveform[i]) > peak)
					peak = Mathf.Abs(waveform[i]);
				
			}
			
			for(int i = 0; i < this.size; i++)
				waveform[i] /= peak;
			
		}

		public Wavetable(double[] partial, double[] amplitude, uint size = 8192u){

			this.size = size;
			waveform = new float[this.size];
			
			double index = 0d;
			float peak = float.Epsilon;
			
			for(int i = 0; i < this.size; i++){

				index = i / (this.size - 1d);
				
				for(int j = 0; j < partial.Length; j++)
					waveform[i] += (float)(amplitude[j % amplitude.Length] * Math.Sin(2d * Math.PI * partial[j] * index));
				
				if(Mathf.Abs(waveform[i]) > peak)
					peak = Mathf.Abs(waveform[i]);
				
			}
			
			for(int i = 0; i < this.size; i++)
				waveform[i] /= peak;
			
		}

		public Wavetable(Recurse partial, Recurse amplitude, uint iterations = 1u, uint size = 8192u){
			
			this.size = size;
			waveform = new float[this.size];
			
			double index = 0d, h, a;
			float peak = float.Epsilon;
			
			for(int i = 0; i < this.size; i++){
				
				h = 1d;
				a = 1d;
				index = i / (this.size - 1d);
				
				for(uint j = 0; j < iterations; j++)
					waveform[i] += (float)(amplitude(ref a) * Math.Sin(2d * Math.PI * partial(ref h) * index));
				
				if(Mathf.Abs(waveform[i]) > peak)
					peak = Mathf.Abs(waveform[i]);
				
			}
			
			for(int i = 0; i < this.size; i++)
				waveform[i] /= peak;
			
		}
		
		public Wavetable(double[] partial, double[] amplitude, double[] phase, uint size = 8192u){

			this.size = size;
			waveform = new float[this.size];
			
			double index = 0d;
			float peak = float.Epsilon;
			
			for(int i = 0; i < this.size; i++){
				
				index = i / (this.size - 1d);
				
				for(int j = 0; j < partial.Length; j++)
					waveform[i] += (float)(amplitude[j % amplitude.Length] * Math.Sin(2d * Math.PI * partial[j] * index + phase[j % phase.Length]));
				
				if(Mathf.Abs(waveform[i]) > peak)
					peak = Mathf.Abs(waveform[i]);
				
			}
			
			for(int i = 0; i < this.size; i++)
				waveform[i] /= peak;
			
		}

		public Wavetable(Recurse partial, Recurse amplitude, Recurse phase, uint iterations = 1u, uint size = 8192u){
			
			this.size = size;
			waveform = new float[this.size];
			
			double index = 0d, h, a, p;
			float peak = float.Epsilon;
			
			for(int i = 0; i < this.size; i++){
				
				h = 1d;
				a = 1d;
				p = 0d;
				index = i / (this.size - 1d);
				
				for(uint j = 0; j < iterations; j++)
					waveform[i] += (float)(amplitude(ref a) * Math.Sin(2d * Math.PI * partial(ref h) * index + phase(ref p)));
				
				if(Mathf.Abs(waveform[i]) > peak)
					peak = Mathf.Abs(waveform[i]);
				
			}
			
			for(int i = 0; i < this.size; i++)
				waveform[i] /= peak;
			
		}
		
		public Wavetable(double[] partial, double[] amplitude, double[] phase, double[] offset, uint size = 8192u){

			this.size = size;
			waveform = new float[this.size];
			
			double index = 0d;
			float peak = float.Epsilon;
			
			for(int i = 0; i < this.size; i++){
				
				index = i / (this.size - 1d);
				
				for(int j = 0; j < partial.Length; j++)
					waveform[i] += (float)(amplitude[j % amplitude.Length] * Math.Sin(2d * Math.PI * partial[j] * index + phase[j % phase.Length]) + offset[j % offset.Length]);
				
				if(Mathf.Abs(waveform[i]) > peak)
					peak = Mathf.Abs(waveform[i]);
				
			}
			
			for(int i = 0; i < this.size; i++)
				waveform[i] /= peak;
			
		}

		public Wavetable(Recurse partial, Recurse amplitude, Recurse phase, Recurse offset, uint iterations = 1u, uint size = 8192u){
			
			this.size = size;
			waveform = new float[this.size];
			
			double index = 0d, h, a, p, o;
			float peak = float.MinValue;
			
			for(int i = 0; i < this.size; i++){
				
				h = 1d;
				a = 1d;
				p = 0d;
				o = 0d;
				index = i / (this.size - 1d);
				
				for(uint j = 0; j < iterations; j++)
					waveform[i] += (float)(amplitude(ref a) * Math.Sin(2d * Math.PI * partial(ref h) * index + phase(ref p)) + offset(ref o));
				
				if(Mathf.Abs(waveform[i]) > peak)
					peak = Mathf.Abs(waveform[i]);
				
			}
			
			for(int i = 0; i < this.size; i++)
				waveform[i] /= peak;
			
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

		public static implicit operator AnimationCurve(Wavetable wave){

			Keyframe[] map = new Keyframe[wave.Size];

			for(int i = 0; i < map.Length; i++)
				map[i] = new Keyframe((float)i / map.Length, wave[i]);

			return new AnimationCurve(map);

		}

		public uint Size{

			get{

				return this.size;

			}

		}
		
		public static Wavetable Sine{
			
			get{
				
				return new Wavetable((ref double x) => 1d);
				
			}
			
		}
		
		public static Wavetable Sawtooth{
			
			get{

				return new Wavetable((ref double x) => 1d / x++, 50u);
									
			}
			
		}
		
		public static Wavetable Square{
			
			get{

				return new Wavetable((ref double x) => { x += x % 2 != 1 ? 1 : 0; return x++; }, (ref double x) => { x += x % 2 != 1 ? 1 : 0; return 1d / x++; }, 50u);
				
			}
			
		}
		
	}

	public abstract class Signal {

		public uint check = 0, channels = 2;
		public readonly int sampleRate = AudioSettings.outputSampleRate;
		public float amplitude, duration, sample;
		public float[] panner;

		// vector2 -> equal power panner struct?

		public abstract float datum{ get; }
		public abstract void Stream(ref float[] data);

		public uint SpeakerMode(){
			
			switch(AudioSettings.driverCapabilities){
				
			case AudioSpeakerMode.Mono:
				return 1u;
				
			case AudioSpeakerMode.Stereo:
				return 2u;
				
			case AudioSpeakerMode.Prologic:
				return 2u;
				
			case AudioSpeakerMode.Quad:
				return 4u;
				
			case AudioSpeakerMode.Surround:
				return 5u;
				
			case AudioSpeakerMode.Mode5point1:
				return 6u;
				
			case AudioSpeakerMode.Mode7point1:
				return 8u;
				
			default:
				return 2u;
				
			}
			
		}
		
	}

	public class Oscillator : Signal {

		private float frequency, phasor;
		private readonly Wavetable wavetable;

		//phasor getter
		//frequency modifier for inclusion in instrument?

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
				
				if(check == 0u)
					phasor = (phasor + frequency * wavetable.Size / sampleRate) % wavetable.Size;
				
				check = (check + 1u) % channels;
				
				return sample;
				
			}
			
		}

		public override void Stream(ref float[] data){

			for(int i = 0; i < data.Length; i++)
				data[i] = datum;
				
		}
		
	}

	public class Noise : Signal {

		public override float datum{
			
			get{
				
				return sample;
				
			}
			
		}
		
		public override void Stream(ref float[] data){
			
			for(int i = 0; i < data.Length; i++)
				data[i] = datum;
			
		}

	}

}