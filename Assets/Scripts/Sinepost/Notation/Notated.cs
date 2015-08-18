using UnityEngine;

namespace Sinepost {

	public abstract class Notated /* : IComparable */ {
		
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
		
		public abstract void Read();
		
		public Tuning EqualTemperament(uint octaveDivision = 12u, float concertPitch = 440f){
			
			return (decimal x) => concertPitch * Mathf.Pow(2f, (float)(x - octaveDivision * 5u + 9u) / octaveDivision);
			
		}
		
	}

}
