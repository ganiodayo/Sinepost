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

namespace Sinepost.Synthesis {

    public abstract partial class Modulatable {

        protected abstract class Parameter : Modulatable {

            public string name;
            protected float actual;
            protected Instrument context;

            public static implicit operator float(Parameter value){

                return value.Value;

            }

            public abstract float Value{ get; set; }

        }

        protected class Parameter<T> : Parameter where T : Modulatable {

            protected readonly float modifier;

            public Parameter(string name, float f = 1f){

                this.name = name;
                actual = f;
                modifier = f;

            }

            public Parameter(Instrument context, string name, float f = 1f){

                this.context = context;
                this.name = name;
                actual = f;
                modifier = f;

            }

            public override float Value{

                get{

                    return actual * this.Modulation;

                } set{

                    actual = value * modifier;

                    if(context != null){

                        foreach(T signal in context)
                            signal[name] = actual;
                            

                    }

                }

            }

        }

    }

}
