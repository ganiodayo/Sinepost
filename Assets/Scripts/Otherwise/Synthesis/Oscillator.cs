using UnityEngine;

namespace Otherwise {

	public class Oscillator : Periodic {
		
		private float phasor;
		public Wavetable wavetable;
		
		public Oscillator(float amplitudeModifier, float frequencyModifier){
			
			this.amplitude = amplitudeModifier;
			this.amplitudeModifier = amplitudeModifier;
			this.frequency = frequencyModifier;
			this.frequencyModifier = frequencyModifier;
			this.wavetable = Wavetable.Sine;
			
			channels = SpeakerMode;
			panner = new float[channels];
			
		}
		
		public Oscillator(float amplitudeModifier, float frequencyModifier, Wavetable wavetable){
			
			this.amplitude = amplitudeModifier;
			this.amplitudeModifier = amplitudeModifier;
			this.frequency = frequencyModifier;
			this.frequencyModifier = frequencyModifier;;
			this.wavetable = wavetable;
			
			channels = SpeakerMode;
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
		
		public override void Pan(Vector2 position){}
		
	}

}
