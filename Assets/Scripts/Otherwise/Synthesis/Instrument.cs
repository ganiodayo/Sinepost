using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Otherwise {

	public class Instrument : Periodic, IEnumerable {
		
		public readonly Envelope envelope;
		private List<Signal> signals = new List<Signal>();
		
		public Instrument(float amplitude, float frequency, params Signal[] signals){
			
			this.amplitude = amplitude;
			this.amplitudeModifier = amplitude;
			this.frequency = frequency;
			this.frequencyModifier = frequency;
			
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

}
