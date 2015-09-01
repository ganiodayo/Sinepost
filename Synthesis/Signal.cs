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
using System.Collections.Generic;

namespace Sinepost.Synthesis {

	public abstract class Signal : Modulatable, IComparable {

        public float pan = 0f;
        public Vector2 host;

        protected readonly int sampleRate = AudioSettings.outputSampleRate;
		protected readonly double init = AudioSettings.dspTime;
		protected uint channels = 2u, check = 0u, instances = 0u;
        protected float[] panner;
        protected Vector2[] reference;
        protected Parameter<Signal> amplitude;
		protected Envelope envelope;

        public static Signal operator ++ (Signal signal){

            signal.instances++;

            return signal;

        }

        public static Signal operator --(Signal signal) {

            signal.instances--;

            return signal;

        }

		public abstract float Render{ get; }
		public abstract float Datum{ get; } //protected in Modulatable? replace with IEnumerator for instances?

		protected static uint SpeakerMode{
			
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

        protected static Vector2[] SpeakerPositions{

            get{

                Vector2[] output = new Vector2[SpeakerMode];

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
                        output[3] = Vector2.zero;
                        output[4] = new Vector2(Mathf.Sin(Mathf.PI * 25/18f), Mathf.Cos(Mathf.PI * 25/18f));
                        output[5] = new Vector2(Mathf.Sin(Mathf.PI * 11/18f), Mathf.Cos(Mathf.PI * 11/18f));
                        break;

                    case AudioSpeakerMode.Mode7point1:
                        output[0] = new Vector2(Mathf.Sin(Mathf.PI * 11/6f), Mathf.Cos(Mathf.PI * 11/6f));
                        output[1] = Vector2.up;
                        output[2] = new Vector2(Mathf.Sin(Mathf.PI / 6f), Mathf.Cos(Mathf.PI / 6f));
                        output[3] = Vector2.zero;
                        output[4] = new Vector2(Mathf.Sin(Mathf.PI * 25/18f), Mathf.Cos(Mathf.PI * 25/18f));
                        output[5] = new Vector2(Mathf.Sin(Mathf.PI * 11/18f), Mathf.Cos(Mathf.PI * 11/18f));
                        output[6] = new Vector2(Mathf.Sin(Mathf.PI * 7/6f), Mathf.Cos(Mathf.PI * 7/6f));
                        output[7] = new Vector2(Mathf.Sin(Mathf.PI * 5/6f), Mathf.Cos(Mathf.PI * 5/6f));
                        break;

                }

                return output;

            }

        }

		public int CompareTo(object o){

			return 0; //can this be useful? probably
			
		}

        public static string[] GetChannelNames(bool abbreviated = false) {

            string[] names = new string[SpeakerMode];

            switch(AudioSettings.driverCapabilities) {

                case AudioSpeakerMode.Mono:
                    names[0] = abbreviated ? "C" : "Centre";
                    break;

                default:
                case AudioSpeakerMode.Stereo:
                case AudioSpeakerMode.Prologic:
                    names[0] = abbreviated ? "L" : "Left";
                    names[1] = abbreviated ? "R" : "Right";
                    break;

                case AudioSpeakerMode.Quad:
                    names[0] = abbreviated ? "L" : "Left";
                    names[1] = abbreviated ? "R" : "Right";
                    names[2] = abbreviated ? "LS" : "Left Surround";
                    names[3] = abbreviated ? "RS" : "Right Surround";
                    break;

                case AudioSpeakerMode.Surround:
                    names[0] = abbreviated ? "L" : "Left";
                    names[1] = abbreviated ? "C" : "Centre";
                    names[2] = abbreviated ? "R" : "Right";
                    names[3] = abbreviated ? "LS" : "Left Surround";
                    names[4] = abbreviated ? "RS" : "Right Surround";
                    break;

                case AudioSpeakerMode.Mode5point1:
                    names[0] = abbreviated ? "L" : "Left";
                    names[1] = abbreviated ? "C" : "Centre";
                    names[2] = abbreviated ? "R" : "Right";
                    names[3] = abbreviated ? "LFE" : "Low Frequency Effects";
                    names[4] = abbreviated ? "LS" : "Left Surround";
                    names[5] = abbreviated ? "RS" : "Right Surround";
                    break;

                case AudioSpeakerMode.Mode7point1:
                    names[0] = abbreviated ? "L" : "Left";
                    names[1] = abbreviated ? "C" : "Centre";
                    names[2] = abbreviated ? "R" : "Right";
                    names[3] = abbreviated ? "LFE" : "Low Frequency Effects";
                    names[4] = abbreviated ? "LS" : "Left Surround";
                    names[5] = abbreviated ? "RS" : "Right Surround";
                    names[6] = abbreviated ? "LB" : "Left Back";
                    names[7] = abbreviated ? "RB" : "Right Back";
                    break;

            }

            return names;

        }

        private void Pan(){

            if(channels == 2u){

                panner[0] = Mathf.Sqrt(1f - (pan + 1f) / 2f);
                panner[1] = Mathf.Sqrt((pan + 1f) / 2f);

            } else
                Pan(new Vector2(pan, 0f));

        }

        public void Pan(float x){

            if(channels == 2u){

                panner[0] = Mathf.Sqrt(1f - (x + 1f) / 2f);
                panner[1] = Mathf.Sqrt((x + 1f) / 2f);

            } else
                Pan(new Vector2(x, 0f));

        }

        public void Pan(Vector2 position){
            
            for(uint i = 0; i < panner.Length; i++)
                panner[i] = Mathf.Sqrt(Mathf.Abs(1f - (reference[i] - position).magnitude));

        }

        protected virtual void Setup(float amplitude){

            channels = SpeakerMode;
            reference = SpeakerPositions;
            panner = new float[channels];
            Pan();
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

            for(int i = 0; i < data.Length; i++)
                data[i] = Datum;

        }
		
	}

}
