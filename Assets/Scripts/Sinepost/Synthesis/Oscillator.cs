using UnityEngine;

namespace Sinepost {

	public class Oscillator : Periodic {
		
		private float phasor;
		public Wavetable wavetable;
		
		public Oscillator(float amplitude = 1f, float frequency = 1f){

            Setup(amplitude, frequency);
			this.wavetable = Wavetable.Sine;
			
		}
		
		public Oscillator(Wavetable wavetable, float amplitude = 1f, float frequency = 1f){

            Setup(amplitude, frequency);
			this.wavetable = wavetable;
			
		}
		
		public override float Render{
			
			get{

                return wavetable[((float)(AudioSettings.dspTime - init) * this["Frequency"] * wavetable.Size) % wavetable.Size] *
                    this["Amplitude"] * this.Modulation;
				
			}
			
		}
		
		public override float Datum{
			
			get{

                float sample = wavetable[phasor] * panner[check] * this["Amplitude"];    
				
				if(check == 0u)
                    phasor = (phasor + this["Frequency"] * wavetable.Size / sampleRate) % wavetable.Size;
				
				check = (check + 1u) % channels;

                return sample * this.Modulation;
				
			}
			
		}
		
	}

}
