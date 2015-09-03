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

using System.Collections.Generic;

namespace Sinepost.Synthesis {

	public abstract partial class Modulatable {

        protected float offset = 1f;
        private List<Signal> modulators = new List<Signal>();
        protected ParameterList parameters = new ParameterList();
        //abstract enum parameters?

        public static Modulatable operator + (Modulatable modulatable, float offset){

            modulatable.offset += offset;

            return modulatable;

        }

        public float this[string name]{

            get{

                return parameters[name]; //try catch exposed parameter throw error

            } set{

                parameters[name].Value = value;

            }

        }

        protected float Modulation{

            get{

                float output = offset;

                foreach(Signal signal in modulators)
                    output += signal.Datum;

                return output;

            }

        }

        public Signal Modulator{

            set{

                modulators.Add(value);

            }

        }

        public void Modulate(string parameter = "Ring", params Signal[] modulators){

            if(parameter != "Ring"){

                for(int i = 0; i < modulators.Length; i++)
                    parameters[parameter].Modulator = modulators[i];

            } else{

                for(int i = 0; i < modulators.Length; i++)
                    Modulator = modulators[i];

            }

        }


    }

}
