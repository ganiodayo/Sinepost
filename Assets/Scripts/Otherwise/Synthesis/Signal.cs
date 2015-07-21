using UnityEngine;
using System;

namespace Otherwise {

	public abstract class Signal : IComparable {
		
		protected readonly int sampleRate = AudioSettings.outputSampleRate;
		protected readonly double init = AudioSettings.dspTime;
		protected uint check = 0, channels = 2;
		protected float amplitude, amplitudeModifier = 1f, duration, sample;
		protected float[] panner;
		
		// vector2 -> equal power panner struct?
		
		public abstract float Amplitude{ get; set; }
		public abstract float Render{ get; }
		public abstract float Datum{ get; }
		public abstract void Stream(ref float[] data);
		public abstract void Pan(Vector2 position);
		
		public uint SpeakerMode{
			
			get{
				
				switch(AudioSettings.driverCapabilities){
					
				case AudioSpeakerMode.Mono:
					return 1u;
					
				case AudioSpeakerMode.Stereo:
					return 2u;
					
				case AudioSpeakerMode.Prologic:
					return 2u;
					
				case AudioSpeakerMode.Quad:
					return 4u;
					
				case AudioSpeakerMode.Surround:
					return 5u;
					
				case AudioSpeakerMode.Mode5point1:
					return 6u;
					
				case AudioSpeakerMode.Mode7point1:
					return 8u;
					
				default:
					return 2u;
					
				}
				
			}
			
		}
		
		public int CompareTo(object o){
			
			return 0;
			
		}
		
	}

}
