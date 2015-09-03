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

using System.Collections;
using System.Collections.Generic;

namespace Sinepost.Synthesis {

	public class Instrument : Periodic, IEnumerable {

		private List<Signal> signals = new List<Signal>();
		
		public Instrument(float amplitude = 1f, float frequency = 1f, params Signal[] signals){

            Setup(this, amplitude, frequency);

			for(int i = 0; i < signals.Length; i++)
				this.signals.Add(signals[i]);
			
		}
		
		public Signal this[int index]{
			
			get{
				
				return signals[index];
				
			}
			
		}

        public int Count{
            
            get{

                return signals.Count;

            }

        }
		
		public override float Render{
			
			get{
				
				float output = 0f;
				
				for(int i = 0; i < signals.Count; i++)
					output += signals[i].Render;
				
				return output;
				
			}
			
		}
		
		public override float Datum{
			
			get{
				
				float sample = 0f;
				
				for(int i = 0; i < signals.Count; i++)
                    sample += signals[i].Datum * panner[check];

                check = (check + 1u) % channels;

				return sample * this.Modulation;
				
			}
			
		}
		
		public IEnumerator GetEnumerator(){
			
			return signals.GetEnumerator();
			
		}
		
	}

}
