using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Sinepost {

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
					output += signals[i].Render * this["Amplitude"];
				
				return output;
				
			}
			
		}
		
		public override float Datum{
			
			get{
				
				float sample = 0f;
				
				for(int i = 0; i < signals.Count; i++)
                    sample += signals[i].Datum * panner[check] * this["Amplitude"];

                check = (check + 1u) % channels;

				return sample * this.Modulation;
				
			}
			
		}
		
		public IEnumerator GetEnumerator(){
			
			return signals.GetEnumerator();
			
		}
		
	}

}
