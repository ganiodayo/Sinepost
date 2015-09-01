#region License
//
// Sinepost
// Copyright (c) 2015 Jonathan A. Califa
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

using UnityEngine;

namespace Sinepost.Synthesis {

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
