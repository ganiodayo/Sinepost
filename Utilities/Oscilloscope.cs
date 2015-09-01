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
using Sinepost.Synthesis;

namespace Sinepost.Utilities
{

	public class Oscilloscope : MonoBehaviour {

        private int size;
        private string[] channels;
        private Camera view;
        private LineRenderer[] signal;
        private float[][] buffer;

        public void Awake(){

            size = AudioSettings.GetConfiguration().dspBufferSize;
            channels = Signal.GetChannelNames();
            buffer = new float[channels.Length][];
            signal = new LineRenderer[channels.Length];

            for(int i = 0; i < channels.Length; i++){

                buffer[i] = new float[size];
                GameObject channel = new GameObject(channels[i], typeof(LineRenderer));
                channel.transform.parent = transform;
                signal[i] = channel.GetComponent<LineRenderer>();
                signal[i].SetWidth(0.01f, 0.011f);
                signal[i].SetVertexCount(size);

            }

            view = FindObjectOfType<Camera>();

        }

        void Update(){

            for(int c = 0; c < channels.Length; c++){

                AudioListener.GetOutputData(buffer[c], c);

                for(int i = 0; i < size; i++)
                    signal[c].SetPosition(i, view.ViewportToWorldPoint(
                        new Vector3(i / (float)size, (buffer[c][i] / 2f + 1) / 2f, 10f)));

            }

        }

    }

}