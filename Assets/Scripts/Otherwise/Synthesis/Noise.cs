using UnityEngine;
using System;
using System.Collections;

namespace Otherwise {

	public class Noise : Signal {
		
		double rate;
		float reflection;
		public float[,] node = new float[128, 4];
		
		//noise node struct with phasorlike read
		//buffersize
		//
		//amp = Math.Pow(3/4d, i/2d);
		
		class Node : SampleHold {

			//delegates?

		}
		
		public Noise(double alpha = 0d){

			//rate = 1d / Math.Pow(sampleRate, alpha);
			
			for(int i = 0; i < 128; i++){
				
				node[i, 3] = sampleRate / Mathf.Pow(2f, (139f - i) / 12f);
				node[i, 2] = Mathf.Pow(3/4f, i/2f);
				node[i, 1] = UnityEngine.Random.Range(-node[i, 2], node[i, 2]);
				node[i, 0] = UnityEngine.Random.Range(-node[i, 2], node[i, 2]);
				
			}
			
			float a = 0f, b = 0f, c = 0f;
			
			for(int i = 0; i < 127; i++){
				//Debug.Log(node[i, 0] + ", " + node[i, 1] + ", " + node[i, 2] + ", " + node[i, 3]);
				a += node[i, 0];
				b += node[i, 1];
				c += node[i, 2];
			}
			
			Debug.Log (a + ", " + b + ", " + c);
			
		}
		
		public override float Amplitude{
			
			get{
				
				return amplitude;
				
			} set{
				
				amplitude = value * this.amplitudeModifier;
				
			}
			
		}
		
		public override float Render{
			
			get{
				
				return UnityEngine.Random.value * amplitude;
				
			}
			
		}
		
		public override float Datum{
			
			get{

                return UnityEngine.Random.value * amplitude;
				
			}
			
		}
		
		public override void Stream(ref float[] data){
			
			for(int i = 0; i < data.Length; i++)
				data[i] = Datum;
			
		}
		
		public override void Pan(Vector2 position){}
		
	}

}
