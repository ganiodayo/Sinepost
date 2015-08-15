using UnityEngine;
using System.Collections;

namespace Otherwise {

	public class SampleHold : Periodic, IEnumerator {

		float jitter, slew;

		public SampleHold(){}
		
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
				
				return 0f;
				
			}
			
		}
		
		public override float Datum{
			
			get{
				
				return 0f;
				
			}
			
		}
		
		public override void Stream(ref float[] data){
			
			for(int i = 0; i < data.Length; i++)
				data[i] = Datum;
			
		}
		
		public override void Pan(Vector2 position){}

		public object Current{

			get{

				return 0f;

			}

		}

		public bool MoveNext(){

			return true;

		}

		public void Reset(){}

	}

}
