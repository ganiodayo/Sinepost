using UnityEngine;
using System;
using System.Collections.Generic;

namespace Sinepost {

	public abstract class Signal : Modulatable, IComparable {

        public Vector2 host;

        protected readonly int sampleRate = AudioSettings.outputSampleRate;
		protected readonly double init = AudioSettings.dspTime;
		protected uint check = 0u, channels = 2u;
        protected float[] panner;
        protected Vector2[] reference;
        protected Parameter<Signal> amplitude;
		protected Envelope envelope;

		public abstract float Render{ get; }
		public abstract float Datum{ get; } //protected in Modulatable?

		protected uint SpeakerMode{
			
			get{
				
				switch(AudioSettings.driverCapabilities){
					
				    case AudioSpeakerMode.Mono:
					    return 1u;
					
                    default:
				    case AudioSpeakerMode.Stereo:
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
					
				}
				
			}
			
		}

        protected Vector2[] SpeakerPositions{

            get{

                Vector2[] output = new Vector2[channels];

                switch(AudioSettings.driverCapabilities){

                    case AudioSpeakerMode.Mono:
                        output[1] = Vector2.zero;
                        break;

                    default:
                    case AudioSpeakerMode.Stereo:
                    case AudioSpeakerMode.Prologic:
                        output[0] = new Vector2(-0.5f, 0f);
                        output[1] = new Vector2(0.5f, 0f);
                        break;

                    case AudioSpeakerMode.Quad:
                        output[0] = new Vector2(-0.5f, 0.5f);
                        output[1] = new Vector2(0.5f, 0.5f);
                        output[2] = new Vector2(-0.5f, -0.5f);
                        output[3] = new Vector2(0.5f, -0.5f);
                        break;

                    case AudioSpeakerMode.Surround:
                        output[0] = new Vector2(Mathf.Sin(Mathf.PI * 11/6f), Mathf.Cos(Mathf.PI * 11/6f));
                        output[1] = Vector2.up;
                        output[2] = new Vector2(Mathf.Sin(Mathf.PI / 6f), Mathf.Cos(Mathf.PI / 6f));
                        output[3] = new Vector2(Mathf.Sin(Mathf.PI * 25/18f), Mathf.Cos(Mathf.PI * 25/18f));
                        output[4] = new Vector2(Mathf.Sin(Mathf.PI * 11/18f), Mathf.Cos(Mathf.PI * 11/18f));
                        break;

                    case AudioSpeakerMode.Mode5point1:
                        output[0] = new Vector2(Mathf.Sin(Mathf.PI * 11/6f), Mathf.Cos(Mathf.PI * 11/6f));
                        output[1] = Vector2.up;
                        output[2] = new Vector2(Mathf.Sin(Mathf.PI / 6f), Mathf.Cos(Mathf.PI / 6f));
                        output[5] = Vector2.zero;
                        output[3] = new Vector2(Mathf.Sin(Mathf.PI * 25/18f), Mathf.Cos(Mathf.PI * 25/18f));
                        output[4] = new Vector2(Mathf.Sin(Mathf.PI * 11/18f), Mathf.Cos(Mathf.PI * 11/18f));
                        break;

                    case AudioSpeakerMode.Mode7point1:
                        output[0] = new Vector2(Mathf.Sin(Mathf.PI * 11/6f), Mathf.Cos(Mathf.PI * 11/6f));
                        output[1] = Vector2.up;
                        output[2] = new Vector2(Mathf.Sin(Mathf.PI / 6f), Mathf.Cos(Mathf.PI / 6f));
                        output[5] = Vector2.zero;
                        output[3] = new Vector2(Mathf.Sin(Mathf.PI * 25/18f), Mathf.Cos(Mathf.PI * 25/18f));
                        output[4] = new Vector2(Mathf.Sin(Mathf.PI * 11/18f), Mathf.Cos(Mathf.PI * 11/18f));
                        output[0] = new Vector2(Mathf.Sin(Mathf.PI * 7/6f), Mathf.Cos(Mathf.PI * 7/6f));
                        output[0] = new Vector2(Mathf.Sin(Mathf.PI * 5/6f), Mathf.Cos(Mathf.PI * 5/6f));
                        break;

                }

                return output;

            }

        }
		
		public int CompareTo(object o){

			return 0; //can this be useful? probably
			
		}

        public void Pan(Vector2 position){

            for(uint i = 0; i < panner.Length; i++)
                panner[i] = Mathf.Sqrt(Mathf.Abs(1f - (reference[i] - position).magnitude));

        }

        protected virtual void Setup(float amplitude){

            channels = SpeakerMode;
            reference = SpeakerPositions;
            panner = new float[channels];
            Pan(Vector2.zero);
            this.amplitude = new Parameter<Signal>("Amplitude", amplitude);
            parameters += this.amplitude;

        }

        protected virtual void Setup(Instrument context, float amplitude){

            channels = SpeakerMode;
            reference = SpeakerPositions;
            panner = new float[channels];
            Pan(Vector2.zero);
            this.amplitude = new Parameter<Signal>(context, "Amplitude", amplitude);
            parameters += this.amplitude;

        }

        public void Stream(ref float[] data){

            Pan(host);

            for(int i = 0; i < data.Length; i++)
                data[i] = Datum;

        }
		
	}

}
