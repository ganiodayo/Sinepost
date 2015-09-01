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
using System;
using System.Collections;

namespace Sinepost.Synthesis {

	public class Noise : Signal {
		
		double rate;
		float reflection;
		public float[,] node = new float[128, 4];
        System.Random random = new System.Random();
		
		//noise node struct with phasorlike read
		//buffersize
		//
		//amp = Math.Pow(3/4d, i/2d);
		
		class Node : SampleHold {

			//delegates?

		}
		
		public Noise(float amplitude = 1f, double alpha = 0d){

			//rate = 1d / Math.Pow(sampleRate, alpha);

            Setup(amplitude);

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
			
			//Debug.Log (a + ", " + b + ", " + c);
			
		}
		
		public override float Render{
			
			get{
				
				return (float)(1 - 2 * random.NextDouble()) * this["Amplitude"];
				
			}
			
		}
		
		public override float Datum{
			
			get{

                return (float)(1 - 2 * random.NextDouble()) * this["Amplitude"];
				
			}
			
		}
		
	}

}
