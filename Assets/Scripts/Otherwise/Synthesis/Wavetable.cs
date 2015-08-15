using UnityEngine;
using UnityEditor; //use this
using System;

namespace Otherwise {

	public sealed class Wavetable {
		
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

}
