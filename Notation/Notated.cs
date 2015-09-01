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
using System.Xml;

namespace Sinepost.Notation {

	public abstract class Notated /* : XmlReader, IComparable (priority/probability value) */ {
		
		public float tempo = 120f;
		public rhythmic signature;
		//metronome IEnumerator
		public delegate float Tuning(decimal x);
		public Tuning temperament;
		
		public struct cyclic {
			
			public decimal? note;
			public float? frequency;
			public byte? velocity;
			public rhythmic duration;
			
			public static implicit operator cyclic(decimal note){
				
				cyclic output = new cyclic();
				output.note = note;
				
				return output;
				
			}
			
			public static implicit operator cyclic(rhythmic rest){
				
				cyclic output = new cyclic();
				output.note = null;
				output.frequency = null;
				output.velocity = null;
				
				return output;
				
			}
			
		}
		
		public struct rhythmic {
			
			public int numerator;
			public int denominator;
			
		}
		
		public Tuning EqualTemperament(uint octaveDivision = 12u, float concertPitch = 440f){
			
			return (decimal x) => concertPitch * Mathf.Pow(2f, (float)(x - octaveDivision * 5u + 9u) / octaveDivision);
			
		}
		
	}

}
