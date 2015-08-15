using UnityEngine;
using System;

namespace Otherwise {

	public abstract class Signal : Modulatable, IComparable {

		protected readonly int sampleRate = AudioSettings.outputSampleRate;
		protected readonly double init = AudioSettings.dspTime;
		protected uint check = 0, channels = 2;
		protected float amplitude, amplitudeModifier = 1f;
		protected float[] panner;

		protected Envelope envelope;
		//Duration parameter;

        protected class Parameter : Modulatable {

            private float actual, modifier;

            private Parameter(float f){

                actual = f;
                modifier = f;

            }

            public static implicit operator Parameter(float f){

                return new Parameter(f);

            }

            public static implicit operator Parameter(double d){

                return new Parameter((float)d);

            }

            public static implicit operator float(Parameter value){

                return value.actual;

            }

            public float Value{

                get{

                    return actual * this.Modulate;
                
                } set{

                    actual = value * modifier;

                }

            }

        }

		public abstract float Amplitude{ get; set; }
		public abstract float Render{ get; }
		public abstract float Datum{ get; }
		public abstract void Stream(ref float[] data);
		public abstract void Pan(Vector2 position);

        //kill modulation with envelope release.

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

			return 0; //can this be useful?
			
		}
		
	}

}
