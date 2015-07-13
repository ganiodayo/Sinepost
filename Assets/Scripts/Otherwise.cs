using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Otherwise {

	public abstract class Notated {

		public float tempo = 120f;
		public rhythmic signature;

		public delegate float Tuning(decimal x);
		public Tuning temperament;

		public struct cyclic {

			public decimal? note;
			public float? frequency;
			public byte? velocity;
			public rhythmic duration;

			public static implicit operator cyclic(decimal note){

				cyclic output = new cyclic();
				output.note = note;

				return output;

			}

			public static implicit operator cyclic(rhythmic rest){
				
				cyclic output = new cyclic();
				output.note = null;
				output.frequency = null;
				output.velocity = null;
				
				return output;
				
			}
			
		}

		public struct rhythmic {
			
			public int numerator;
			public int denominator;
			
		}

		public abstract void Read();

		public Tuning EqualTemperament(uint octaveDivision = 12u, float concertPitch = 440f){

			return (decimal x) => concertPitch * Mathf.Pow(2f, (float)(x - octaveDivision * 5u + 9u) / octaveDivision);

		}
		
	}

	public class Part : Notated {

		rhythmic beat;
		cyclic note;
		public int index;

		public Part(rhythmic beat, cyclic note){
			
			this.temperament = EqualTemperament(12u, 440f);
			
		}

		public Part(rhythmic beat, cyclic note, Tuning temperament){

			this.temperament = temperament;

		}

		public static Part operator + (Part input1, Part input2){

			Part output = input1;

			return output;

		}
		
		public override void Read(){
			
			
			
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
		
		//transition message system;

		void OnEnable();

		void OnTransformChildrenChanged();

	}

	public sealed class Instrumentalist : MonoBehaviour, IAudible {

		//generic list of type signal;

		void Awake(){

		}

		public void OnEnable(){}

		public void OnTransformChildrenChanged(){}
		
	}

	public sealed class Orchestra : MonoBehaviour, IAudible {

		private readonly Instrumentalist[] members;

		public Orchestra(params Instrumentalist[] members){

			this.members = members;

		}

		void Awake(){
			
		}

		public void OnEnable(){

		}
		
		public void OnTransformChildrenChanged(){
			
			
		}

		public Instrumentalist this[int index]{
			
			get{
				
				return members[index];
				
			}
			
		}

	}

	public class Envelope {

		//final key = release;

	}

	public sealed class Wavetable {

		//own gui

		public delegate double Recurse(ref double x);

		private readonly float[] waveform;
		private readonly uint size;
		private float ratio = 1f;

		public Wavetable(double[] amplitude, uint size = 4096u){
			
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

		public Wavetable(Recurse amplitude, uint iterations = 64u, uint size = 4096u){
			
			this.size = size;
			waveform = new float[this.size];
			
			double index = 0d, a;
			float peak = float.Epsilon;
			
			for(int i = 0; i < this.size; i++){
				
				a = 1d;
				index = i / (this.size - 1f);
				
				for(uint j = 0u; j < iterations; j++)
					waveform[i] += (float)(amplitude(ref a) * Math.Sin(2d * Math.PI * (j + 1) * index));
				
				if(Mathf.Abs(waveform[i]) > peak)
					peak = Mathf.Abs(waveform[i]);
				
			}
			
			for(int i = 0; i < this.size; i++)
				waveform[i] /= peak;
			
		}

		public Wavetable(double[] partial, double[] amplitude, uint size = 4096u){

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

		public Wavetable(Recurse partial, Recurse amplitude, uint iterations = 64u, uint size = 4096u){
			
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
		
		public Wavetable(double[] partial, double[] amplitude, double[] phase, uint size = 4096u){

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

		public Wavetable(Recurse partial, Recurse amplitude, Recurse phase, uint iterations = 64u, uint size = 4096u){
			
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
		
		public Wavetable(double[] partial, double[] amplitude, double[] phase, double[] offset, uint size = 4096u){

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

		public Wavetable(Recurse partial, Recurse amplitude, Recurse phase, Recurse offset, uint iterations = 64u, uint size = 4096u){
			
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

		public Wavetable(Markov chain, bool mirror = false, uint size = 4096u){

			//fix this

			this.size = size;
			waveform = new float[this.size];

			float peak = float.MinValue;
			
			for(int i = 0; i < waveform.Length; i++){

				waveform[i] = mirror && i >= waveform.Length / 2 ? -waveform[waveform.Length / 2 - i] : chain.Next();

				if(Mathf.Abs(this.waveform[i]) > peak)
					peak = Mathf.Abs(this.waveform[i]);
				
			}
			
			for(int i = 0; i < this.size; i++)
				this.waveform[i] /= peak;

		}

		private Wavetable(float[] waveform){

			this.waveform = waveform;
			this.size = (uint)waveform.Length;
			float peak = float.MinValue;

			for(int i = 0; i < waveform.Length; i++){

				if(Mathf.Abs(this.waveform[i]) > peak)
					peak = Mathf.Abs(this.waveform[i]);

			}

			for(int i = 0; i < this.size; i++)
				this.waveform[i] /= peak;

		}

		public static Wavetable operator ~ (Wavetable wave){
			
			return wave >> (int)wave.Size / 4;
			
		}

		public static Wavetable operator ! (Wavetable wave){

			float offset = 0;
			float[] waveform = new float[wave.Size];

			for(int i = 0; i < waveform.Length; i++)
				offset += wave[i];

			offset /= waveform.Length;

			for(int i = 0; i < waveform.Length; i++)
				waveform[i] = wave[i] - offset;
			
			return new Wavetable(waveform);
			
		}

		public static Wavetable operator + (Wavetable wave){
			
			return new Wavetable(wave);
			
		}

		public static Wavetable operator - (Wavetable wave){
			
			float[] waveform = new float[wave.Size];
			
			for(int i = 0; i < waveform.Length; i++)
				waveform[i] = -wave[i];
			
			return new Wavetable(waveform);
			
		}

		public static Wavetable operator ++ (Wavetable wave){
			
			float[] waveform = new float[wave.Size];
			
			for(int i = 0; i < waveform.Length; i++)
				waveform[i] = wave[i] + 1f;
			
			return new Wavetable(waveform);
			
		}

		public static Wavetable operator -- (Wavetable wave){
			
			float[] waveform = new float[wave.Size];
			
			for(int i = 0; i < waveform.Length; i++)
				waveform[i] = wave[i] - 1f;
			
			return new Wavetable(waveform);
			
		}

		public static Wavetable operator + (Wavetable w1, Wavetable w2){
			
			float[] waveform = new float[4096];
			
			for(int i = 0; i < 4096; i++)
				waveform[i] = w1.ratio * w1[i * w1.Size / 4096f] + w2.ratio * w2[i * w2.Size / 4096f];
			
			return new Wavetable(waveform);
			
		}

		public static Wavetable operator - (Wavetable w1, Wavetable w2){
			
			float[] waveform = new float[4096];
			
			for(int i = 0; i < 4096; i++)
				waveform[i] = w1.ratio * w1[i * w1.Size / 4096f] - w2.ratio * w2[i * w2.Size / 4096f];
			
			return new Wavetable(waveform);
			
		}

		public static Wavetable operator * (Wavetable w1, Wavetable w2){
			
			float[] waveform = new float[4096];
			
			for(int i = 0; i < 4096; i++)
				waveform[i] = w1[i * w1.Size / 4096f] * w2[i * w2.Size / 4096f];
			
			return new Wavetable(waveform);
			
		}

		public static Wavetable operator / (Wavetable w1, Wavetable w2){
			
			float[] waveform = new float[4096];
			
			for(int i = 0; i < 4096; i++)
				waveform[i] = w1[i * w1.Size / 4096f] / w2[i * w2.Size / 4096f];
			
			return new Wavetable(waveform);
			
		}

		public static Wavetable operator ^ (Wavetable wave, float bitDepth){
			
			float[] waveform = new float[wave.Size];
			bitDepth = Mathf.Pow(2f, Mathf.Abs(bitDepth));

			for(int i = 0; i < waveform.Length; i++)
				waveform[i] = Mathf.Round(wave[i] * bitDepth) / bitDepth;

			return new Wavetable(waveform);
			
		}
		
		public static Wavetable operator | (Wavetable w1, Wavetable w2){

			w1.ratio /= w1.ratio + w2.ratio;
			w2.ratio = 1f - w1.ratio;
			float[] waveform = new float[4096];

			for(int i = 0; i < 4096; i++)
				waveform[i] = i <= w1.ratio * 4096f ? w1[i * w1.Size / 4096f] : w2[i * w2.Size / 4096f];

			return new Wavetable(waveform);

		}

		public static Wavetable operator >> (Wavetable wave, int phase){
			
			float[] waveform = new float[wave.Size];
			
			for(int i = 0; i < waveform.Length; i++)
				waveform[i] = wave[(i + phase) % waveform.Length];
			
			return new Wavetable(waveform);
			
		}

		public static Wavetable operator << (Wavetable wave, int phase){
			
			float[] waveform = new float[wave.Size];
			
			for(int i = 0; i < waveform.Length; i++)
				waveform[i] = wave[(i + waveform.Length - phase) % waveform.Length];
			
			return new Wavetable(waveform);
			
		}

		public static Wavetable operator * (Wavetable wave, float f){
			
			wave.ratio = f;
			
			return wave;
			
		}

		public float this[int index]{
			
			get{
				
				return waveform[index];
				
			}
			
		}

		public float this[float index]{
			
			get{

				return waveform[Mathf.FloorToInt(index) % waveform.Length] * (1f - (index % 1f)) + waveform[Mathf.CeilToInt(index) % waveform.Length] * (index % 1f);
				
			}
			
		}

		public static implicit operator Markov(Wavetable wave){

			Markov chain = new Markov((int)wave.Size);
			chain.Stream(wave.waveform);

			return chain;

		}

		public static implicit operator AnimationCurve(Wavetable wave){

			Keyframe[] period = new Keyframe[wave.Size];

			for(int i = 0; i < period.Length; i++)
				period[i] = new Keyframe((float)i / period.Length, wave[i]);

			return new AnimationCurve(period);

		}

		public static implicit operator Wavetable(AnimationCurve curve){
			
			float[] waveform = new float[4096];

			for(int i = 0; i < 4096; i++)
				waveform[i] = curve.Evaluate(i / 4096f);

			return new Wavetable(waveform);
			
		}

		public uint Size{

			get{

				return this.size;

			}

		}
		
		public static Wavetable Sine{
			
			get{
				
				return new Wavetable(new double[1]{1d});
				
			}
			
		}
		
		public static Wavetable Sawtooth{
			
			get{

				return new Wavetable((ref double x) => x++, (ref double x) => 1d / x++,
				                     (ref double x) => x += x > Math.PI ? Math.PI : 2d * Math.PI, 64u);
									
			}
			
		}
		
		public static Wavetable Square{
			
			get{

				return new Wavetable((ref double x) => { x += x % 2 != 1 ? 1 : 0; return x++; }, (ref double x) => { x += x % 2 != 1 ? 1 : 0; return 1d / x++; }, 64u);
				
			}
			
		}

		public static Wavetable Triangle{
			
			get{
				
				return new Wavetable((ref double x) => { x += x % 2 != 1 ? 1 : 0; return x++; }, (ref double x) => { x += x % 2 != 1 ? 1 : 0; return 1d / (x * x++); },
							(ref double x) => x += x > Math.PI ? Math.PI : 2d * Math.PI, 64u);
				
			}
			
		}
		
	}

	public abstract class Signal : IComparable {

		protected readonly int sampleRate = AudioSettings.outputSampleRate;
		protected readonly double init = AudioSettings.dspTime;
		protected uint check = 0, channels = 2;
		protected float amplitude, amplitudeModifier = 1f, duration, sample;
		protected float[] panner;

		// vector2 -> equal power panner struct?

		public abstract float Amplitude{ get; set; }
		public abstract float Render{ get; }
		public abstract float Datum{ get; }
		public abstract void Stream(ref float[] data);
		public abstract void Pan(Vector2 position);

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

		public int CompareTo(object o){

			return 0;

		}
		
	}

	public abstract class Periodic : Signal {

		protected float frequency, frequencyModifier = 1f;

		public abstract float Frequency{ get; set; }

	}

	public class Oscillator : Periodic {

		private float phasor;
		public Wavetable wavetable;

		public Oscillator(float amplitudeModifier, float frequencyModifier){

			this.amplitudeModifier = amplitudeModifier;
			this.frequencyModifier = frequencyModifier;
			this.wavetable = Wavetable.Sine;

			channels = SpeakerMode();
			panner = new float[channels];

		}

		public Oscillator(float amplitudeModifier, float frequencyModifier, Wavetable wavetable){

			this.amplitudeModifier = amplitudeModifier;
			this.frequencyModifier = frequencyModifier;
			this.wavetable = wavetable;

			channels = SpeakerMode();
			panner = new float[channels];

		}

		public override float Amplitude{
			
			get{
				
				return amplitude;
				
			} set{
				
				amplitude = value * this.amplitudeModifier;
				
			}
			
		}

		public override float Frequency{

			get{

				return frequency;

			} set{

				frequency = value * this.frequencyModifier;

			}

		}

		public override float Render{
			
			get{

				return wavetable[((float)(AudioSettings.dspTime - init) * frequency * wavetable.Size) % wavetable.Size] * amplitude;
				
			}
			
		}

		public override float Datum{
			
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
				data[i] = Datum;
				
		}

		public override void Pan (Vector2 position){}
		
	}

	public class Noise : Signal {

		public override float Amplitude{
			
			get{
				
				return amplitude;
				
			} set{
				
				amplitude = value * this.amplitudeModifier;
				
			}
			
		}

		public override float Render{
			
			get{
				
				return sample;
				
			}
			
		}

		public override float Datum{
			
			get{
				
				return sample;
				
			}
			
		}
		
		public override void Stream(ref float[] data){
			
			for(int i = 0; i < data.Length; i++)
				data[i] = Datum;
			
		}

		public override void Pan (Vector2 position){}

	}

	public class Instrument : Periodic, IEnumerable {

		public readonly Envelope envelope;
		private List<Signal> signals = new List<Signal>();

		public Instrument(float amplitudeModifier, float frequencyModifier, params Signal[] signals){

			this.amplitudeModifier = amplitudeModifier;
			this.frequencyModifier = frequencyModifier;

			for(int i = 0; i < signals.Length; i++)
				this.signals.Add(signals[i]);

		}
		
		public Signal this[int index]{
			
			get{
				
				return signals[index];
				
			}
			
		}

		public override float Amplitude{
			
			get{
				
				return amplitude;
				
			} set{
				
				amplitude = value * this.amplitudeModifier;
			
				foreach(Signal audible in signals)
					audible.Amplitude = amplitude;

			}
			
		}

		public override float Frequency{
			
			get{
				
				return frequency;
				
			} set{
				
				frequency = value * this.frequencyModifier;

				foreach(Periodic periodic in signals)
						periodic.Frequency = frequency;
				
			}
			
		}

		public override float Render{
			
			get{

				float output = 0f;

				for(int i = 0; i < signals.Count; i++)
					output += signals[i].Render * this.amplitude;

				return output;
				
			}
			
		}

		public override float Datum{
			
			get{

				sample = 0f;

				for(int i = 0; i < signals.Count; i++)
					sample += signals[i].Datum * this.amplitude;

				return sample;
				
			}
			
		}
		
		public override void Stream(ref float[] data){
			
			for(int i = 0; i < data.Length; i++)
				data[i] = Datum;
			
		}

		public override void Pan (Vector2 position){}

		public IEnumerator GetEnumerator(){

			return signals.GetEnumerator();

		}

	}

	public class Markov {

		//comparable list search with lambdas

		class State {
			
			public float[] past;
			public List<float> future = new List<float>();
			
			public State(float[] past){
				
				this.past = past;
				
			}
			
			public float Select(){
				
				return future[UnityEngine.Random.Range(0, future.Count)];
				
			}
			
		}
		
		public bool init;
		
		private int order;
		private float[] status;
		private List<State> states = new List<State>();
		
		public Markov(int order){
			
			this.order = order < 1 ? 1 : order;
			
		}
		
		public void Stream(List<float> data){
			
			float[] temp = new float[order];
			
			if(data.Count > order){
				
				init = true;
				
				for(int i = 0; i < data.Count; i++){
					
					for(int j = 0; j < this.order; j++)
						temp[j] = data[(i + j) % data.Count];
					
					if(states.Count > 0){
						
						bool test = false;
						
						for(int k = 0; k < states.Count; k++){

							int match = 0;

							for(int l = 0; l < order; l++){

								if(Mathf.Approximately(temp[l], states[k].past[l]))
									match++;
								else
									break;

							}

							if(match == order){

								states[k].future.Add(data[(i + this.order) % data.Count]);
								break;

							}

						}
						
						if(!test)
							states.Add(new State(temp));
						
					} else
						states.Add(new State(temp));
					
				}
				
			}
			
		}
		
		public void Stream(params float[] data){
			
			float[] temp = new float[order];
			
			if(data.Length > order){
				
				init = true;
				
				for(int i = 0; i < data.Length; i++){
					
					for(int j = 0; j < this.order; j++)
						temp[j] = data[(i + j) % data.Length];
					
					if(states.Count > 0){
						
						bool test = false;
						
						for(int k = 0; k < states.Count; k++){
							
							int match = 0;
							
							for(int l = 0; l < order; l++){
								
								if(Mathf.Approximately(temp[l], states[k].past[l]))
									match++;
								else
									break;
								
							}
							
							if(match == order){
								
								states[k].future.Add(data[(i + this.order) % data.Length]);
								break;
								
							}
							
						}
						
						if(!test)
							states.Add(new State(temp));
						
					} else
						states.Add(new State(temp));
					
				}
				
			}
			
		}
		
		public float Next(){
			
			if(init){
				
				if(states.Count > 0 && status == null)
					status = states[UnityEngine.Random.Range(0, states.Count)].past;
				
				float current = status[status.Length - 1];
				State state;

				for(int i = 0; i < states.Count; i++){

					int match = 0;
					
					for(int j = 0; j < order; j++){
						
						if(Mathf.Approximately(status[j], states[i].past[j]))
							match++;
						else
							break;
						
					}
					
					if(match == order){
						
						state = states[i];
						current = state.Select();
						
						for(i = 0; i < status.Length; i++)
							status[i] = i == status.Length - 1 ? current : status[i + 1];

						break;
						
					}
					
				}
				
				return current;
				
			} else
				return 0f;
			
		}
		
	}

}